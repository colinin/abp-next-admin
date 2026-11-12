import { useRequest } from '@abp/request';

export interface PrintDesignerFetcher {
  (input: RequestInfo | URL, init?: RequestInit): Promise<Response>;
  waitForTemplateDetail(id: string): Promise<void>;
}

export function usePrintDotFetcher(): PrintDesignerFetcher {
  const { request } = useRequest();
  const detailWaiters = new Map<string, Set<() => void>>();

  const resolveDetailWaiters = (templateId: string) => {
    const waiters = detailWaiters.get(templateId);
    if (!waiters) {
      return;
    }
    detailWaiters.delete(templateId);
    waiters.forEach((resolve) => resolve());
  };

  const fetcher = (async (
    input: RequestInfo | URL,
    init?: RequestInit,
  ): Promise<Response> => {
    let rawUrl: string;
    if (typeof input === 'string') {
      rawUrl = input;
    } else if (input instanceof URL) {
      rawUrl = input.toString();
    } else {
      rawUrl = input.url;
    }

    const method = (
      init?.method ?? (input instanceof Request ? input.method : 'GET')
    ).toUpperCase();

    const u = new URL(rawUrl, window.location.origin);
    const params: Record<string, string> = {};
    u.searchParams.forEach((v, k) => (params[k] = v));

    const detailMatch = u.pathname.match(/\/templates\/([^/]+)$/);
    const detailId = detailMatch?.[1];
    const templateId =
      method === 'GET' && detailId && detailId !== 'all'
        ? decodeURIComponent(detailId)
        : undefined;

    let data: unknown = init?.body;
    if (typeof data === 'string') {
      try {
        data = JSON.parse(data);
      } catch {
        // ignore
      }
    }

    try {
      const result = await request<unknown>(u.pathname, {
        method: method as any,
        data: method === 'GET' || method === 'DELETE' ? undefined : data,
        params: Object.keys(params).length > 0 ? params : undefined,
      });
      if (templateId) {
        // 让 loadTemplate 先完成 res.json()/applyTemplateDataFields，再唤醒等待者。
        setTimeout(() => resolveDetailWaiters(templateId), 0);
      }
      return toResponse(result, 200);
    } catch (error: any) {
      if (templateId) {
        setTimeout(() => resolveDetailWaiters(templateId), 0);
      }
      const status = error?.response?.status ?? error?.status ?? 500;
      const body = error?.response?.data ??
        error?.data ?? {
          error: { message: error?.message ?? 'Request failed' },
        };
      return toResponse(body, status);
    }
  }) as PrintDesignerFetcher;

  fetcher.waitForTemplateDetail = (templateId: string) => {
    return new Promise<void>((resolve) => {
      let finished = false;
      let timer: ReturnType<typeof setTimeout>;

      const finish = () => {
        if (finished) {
          return;
        }
        finished = true;
        clearTimeout(timer);
        resolve();
      };

      const waiters = detailWaiters.get(templateId) ?? new Set<() => void>();
      waiters.add(finish);
      detailWaiters.set(templateId, waiters);

      timer = setTimeout(() => {
        waiters.delete(finish);
        if (waiters.size === 0) {
          detailWaiters.delete(templateId);
        }
        finish();
      }, 5000);
    });
  };

  return fetcher;
}

function toResponse(data: unknown, status: number): Response {
  if (data === undefined || data === null) {
    return new Response(null, {
      status,
      headers: { 'Content-Type': 'application/json' },
    });
  }
  if (typeof data === 'object' && 'items' in (data as any)) {
    data = (data as any).items;
  }
  const body = typeof data === 'string' ? data : JSON.stringify(data);
  return new Response(body, {
    status,
    headers: { 'Content-Type': 'application/json' },
  });
}
