/* eslint-disable unicorn/no-nested-ternary */
/**
 * vue-print-designer 的 CRUD 协议里，新增和编辑共用一个 `upsert` 端点，
 * 且设计器固定使用 POST（端点未显式配置 method 时）。标准 ABP CRUD 则是
 * POST 新增、PUT /{id} 编辑。
 *
 * 这个 fetcher 在不修改设计器源码的前提下，把设计器的 upsert 请求翻译成
 * 标准 CRUD 请求：
 * - 已知存在于服务端的 id：POST 改写为 PUT {collection}/{id}
 * - 未知 id：先 GET {collection}/{id} 探测（404 视为新增，其余错误不吞掉）
 * - 新增：保持 POST {collection}，服务端生成的 id 会从响应里记下来
 *
 * 之所以需要探测而不是只看 `id`，是因为设计器对新增实体也会先生成一个
 * 客户端 uuid 放进请求体（见 upsertTemplate / addCustomElement），请求体和
 * URL 上都无法直接区分新增与编辑。
 */

export const PRINT_DESIGNER_COLLECTION_PATHS = {
  customElements: '/api/print-designer/admin/custom-elements',
  templates: '/api/print-designer/admin/templates',
} as const;

export type CrudResourceKey = keyof typeof PRINT_DESIGNER_COLLECTION_PATHS;

export interface CrudResource {
  collectionPath: string;
  key: CrudResourceKey;
  /** 已确认存在于服务端的实体 id */
  knownIds: Set<string>;
}

export interface CrudRequestConfig {
  data?: unknown;
  method: string;
  params?: Record<string, string>;
}

export type CrudRequester = (
  url: string,
  config: CrudRequestConfig,
) => Promise<unknown>;

const GUID_PATTERN =
  /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i;

const normalizePath = (path: string) => path.replace(/\/+$/, '') || '/';

const readEntityId = (value: unknown): string | undefined => {
  if (!value || typeof value !== 'object') {
    return undefined;
  }
  const id = (value as { id?: unknown }).id;
  return typeof id === 'string' && id.length > 0 ? id : undefined;
};

export const buildDetailPath = (collectionPath: string, id: string) =>
  `${normalizePath(collectionPath)}/${encodeURIComponent(id)}`;

export const findCollectionResource = (
  path: string,
  resources: CrudResource[],
): CrudResource | undefined => {
  const current = normalizePath(path);
  return resources.find(
    (resource) => normalizePath(resource.collectionPath) === current,
  );
};

export const findDetailResource = (
  path: string,
  resources: CrudResource[],
): undefined | { id: string; resource: CrudResource } => {
  const current = normalizePath(path);
  for (const resource of resources) {
    const base = normalizePath(resource.collectionPath);
    if (!current.startsWith(`${base}/`)) {
      continue;
    }
    const rest = current.slice(base.length + 1);
    if (!rest || rest.includes('/')) {
      continue;
    }
    return { id: decodeURIComponent(rest), resource };
  }
  return undefined;
};

/**
 * 从列表（PagedResultDto / 数组 / 包装对象）或实体详情中收集 id。
 */
export const collectEntityIds = (payload: unknown): string[] => {
  if (Array.isArray(payload)) {
    return payload.map((item) => readEntityId(item) ?? '');
  }
  if (!payload || typeof payload !== 'object') {
    return [];
  }
  const record = payload as Record<string, unknown>;
  for (const key of ['items', 'templates', 'customElements'] as const) {
    if (Array.isArray(record[key])) {
      return collectEntityIds(record[key]);
    }
  }
  const id = readEntityId(payload);
  return id ? [id] : [];
};

const readErrorStatus = (error: unknown): number | undefined => {
  const candidate = error as
    | undefined
    | { code?: unknown; response?: { status?: number }; status?: number };
  const status = candidate?.response?.status ?? candidate?.status;
  if (typeof status === 'number') {
    return status;
  }
  // ABP 的异常过滤器会把错误体当作 200 返回，响应拦截器抛出的是
  // `{ code: "404", message, ... }` 这样的 body，需要从 code 里取状态。
  const code = candidate?.code;
  if (typeof code === 'number') {
    return code;
  }
  if (typeof code === 'string' && /^\d+$/.test(code)) {
    return Number(code);
  }
  return undefined;
};

const toResponse = (data: unknown, status: number): Response => {
  if (data === undefined || data === null) {
    return new Response(null, {
      headers: { 'Content-Type': 'application/json' },
      status,
    });
  }
  // PagedResultDto -> 设计器只认识数组
  if (
    typeof data === 'object' &&
    'items' in (data as Record<string, unknown>) &&
    Array.isArray((data as { items?: unknown }).items)
  ) {
    data = (data as { items: unknown[] }).items;
  }
  const body = typeof data === 'string' ? data : JSON.stringify(data);
  return new Response(body, {
    headers: { 'Content-Type': 'application/json' },
    status,
  });
};

export const createPrintDesignerCrudFetcher = (
  request: CrudRequester,
  collectionPaths: Partial<Record<CrudResourceKey, string>> = {},
): ((input: RequestInfo | URL, init?: RequestInit) => Promise<Response>) => {
  const resources: CrudResource[] = (
    Object.keys(PRINT_DESIGNER_COLLECTION_PATHS) as CrudResourceKey[]
  ).map((key) => ({
    collectionPath:
      collectionPaths[key] ?? PRINT_DESIGNER_COLLECTION_PATHS[key],
    key,
    knownIds: new Set<string>(),
  }));

  const remember = (resource: CrudResource, payload: unknown) => {
    for (const id of collectEntityIds(payload)) {
      resource.knownIds.add(id);
    }
  };

  /**
   * 只有服务端明确返回 404 才判定为“不存在”（走新增）。
   * 其它错误（401/403/500/网络）继续抛出，避免误判成新增而产生重复数据。
   */
  const existsOnServer = async (resource: CrudResource, id: string) => {
    try {
      const detail = await request(
        buildDetailPath(resource.collectionPath, id),
        { method: 'GET' },
      );
      const detailStatus = readErrorStatus(detail);
      if (detailStatus && detailStatus >= 400) {
        if (detailStatus === 404) {
          return false;
        }
        throw detail;
      }
      remember(resource, detail);
      return true;
    } catch (error: unknown) {
      if (readErrorStatus(error) === 404) {
        return false;
      }
      throw error;
    }
  };

  return async function fetcher(
    input: RequestInfo | URL,
    init?: RequestInit,
  ): Promise<Response> {
    const rawUrl =
      typeof input === 'string'
        ? input
        : input instanceof URL
          ? input.toString()
          : input.url;
    const url = new URL(rawUrl, window.location.origin);
    let path = url.pathname;

    const params: Record<string, string> = {};
    url.searchParams.forEach((value, key) => {
      params[key] = value;
    });

    let data: unknown = init?.body;
    if (typeof data === 'string') {
      try {
        data = JSON.parse(data);
      } catch {
        // 非 JSON 请求体按原样透传
      }
    }

    const method = (
      init?.method ?? (input instanceof Request ? input.method : 'GET')
    ).toUpperCase();

    const collectionMatch = findCollectionResource(path, resources);
    const detailMatch = findDetailResource(path, resources);
    const bodyId = readEntityId(data);

    let requestMethod = method;
    if (method === 'POST' && collectionMatch && bodyId) {
      const exists =
        collectionMatch.knownIds.has(bodyId) ||
        (GUID_PATTERN.test(bodyId)
          ? await existsOnServer(collectionMatch, bodyId)
          : false);
      if (exists) {
        requestMethod = 'PUT';
        path = buildDetailPath(collectionMatch.collectionPath, bodyId);
      }
    }

    const result = await request(path, {
      data:
        requestMethod === 'GET' || requestMethod === 'DELETE'
          ? undefined
          : data,
      method: requestMethod,
      params: Object.keys(params).length > 0 ? params : undefined,
    });

    if (requestMethod === 'DELETE') {
      if (detailMatch) {
        detailMatch.resource.knownIds.delete(detailMatch.id);
      }
    } else if (requestMethod === 'GET') {
      if (detailMatch) {
        remember(detailMatch.resource, result);
        detailMatch.resource.knownIds.add(detailMatch.id);
      } else if (collectionMatch) {
        remember(collectionMatch, result);
      }
    } else {
      const target = detailMatch?.resource ?? collectionMatch;
      if (target) {
        remember(target, result);
      }
    }

    return toResponse(result, 200);
  };
};
