import type { CrudRequestConfig } from './printDesignerCrudFetcher';

import { describe, expect, it } from 'vitest';

import {
  collectEntityIds,
  createPrintDesignerCrudFetcher,
  findDetailResource,
  PRINT_DESIGNER_COLLECTION_PATHS,
} from './printDesignerCrudFetcher';

const templates = PRINT_DESIGNER_COLLECTION_PATHS.templates;
const customElements = PRINT_DESIGNER_COLLECTION_PATHS.customElements;

const ID_A = '11111111-1111-4111-8111-111111111111';
const ID_NEW = '22222222-2222-4222-8222-222222222222';
const ID_SERVER = '33333333-3333-4333-8333-333333333333';

type Handler = (url: string, config: CrudRequestConfig) => Promise<unknown>;

const setupFetcher = (handler: Handler) => {
  const calls: Array<{ config: CrudRequestConfig; url: string }> = [];
  const fetcher = createPrintDesignerCrudFetcher(async (url, config) => {
    calls.push({ config, url });
    return handler(url, config);
  });
  return { calls, fetcher };
};

const postTemplate = (fetcher: ReturnType<typeof setupFetcher>['fetcher']) =>
  fetcher(templates, {
    body: JSON.stringify({ data: {}, id: ID_A, name: 'A' }),
    method: 'POST',
  });

describe('printDesignerCrudFetcher', () => {
  it('collects ids from paged lists, arrays and details', () => {
    expect(collectEntityIds({ items: [{ id: 'a' }, { id: 'b' }] })).toEqual([
      'a',
      'b',
    ]);
    expect(collectEntityIds([{ id: 'a' }, { name: 'no-id' }])).toEqual(['a']);
    expect(collectEntityIds({ id: 'a', name: 'detail' })).toEqual(['a']);
    expect(collectEntityIds({ customElements: [{ id: 'c' }] })).toEqual(['c']);
    expect(collectEntityIds('not-an-object')).toEqual([]);
  });

  it('matches detail paths without confusing nested paths', () => {
    const resources = [
      {
        collectionPath: templates,
        key: 'templates' as const,
        knownIds: new Set<string>(),
      },
    ];
    expect(findDetailResource(`${templates}/${ID_A}`, resources)?.id).toBe(
      ID_A,
    );
    expect(findDetailResource(`${templates}/${ID_A}/extra`, resources)).toBe(
      undefined,
    );
    expect(findDetailResource(`${templates}s`, resources)).toBe(undefined);
  });

  it('unwraps PagedResultDto for the designer and remembers listed ids', async () => {
    const { calls, fetcher } = setupFetcher(async (url, config) => {
      if (url === templates && config.method === 'GET') {
        return { items: [{ id: ID_A, name: 'A' }], totalCount: 1 };
      }
      if (url === `${templates}/${ID_A}` && config.method === 'PUT') {
        return { id: ID_A, name: 'A2' };
      }
      throw new Error(`unexpected ${config.method} ${url}`);
    });

    const response = await fetcher(templates, { method: 'GET' });
    await expect(response.json()).resolves.toEqual([{ id: ID_A, name: 'A' }]);

    const saveResponse = await postTemplate(fetcher);
    expect(saveResponse.status).toBe(200);

    const putCall = calls.find((call) => call.config.method === 'PUT');
    expect(putCall?.url).toBe(`${templates}/${ID_A}`);
    expect(putCall?.config.data).toMatchObject({ id: ID_A, name: 'A' });
  });

  it('creates with POST when the client-side id does not exist', async () => {
    const { calls, fetcher } = setupFetcher(async (url, config) => {
      if (url === `${templates}/${ID_NEW}` && config.method === 'GET') {
        throw { response: { status: 404 } };
      }
      if (url === templates && config.method === 'POST') {
        return { id: ID_SERVER, name: 'New' };
      }
      if (url === `${templates}/${ID_SERVER}` && config.method === 'PUT') {
        return { id: ID_SERVER, name: 'Renamed' };
      }
      throw new Error(`unexpected ${config.method} ${url}`);
    });

    await fetcher(templates, {
      body: JSON.stringify({ id: ID_NEW, name: 'New' }),
      method: 'POST',
    });
    expect(calls.map((call) => `${call.config.method} ${call.url}`)).toEqual([
      `GET ${templates}/${ID_NEW}`,
      `POST ${templates}`,
    ]);

    // 服务端返回的新 id 会被记住，下一次保存直接走 PUT。
    await fetcher(templates, {
      body: JSON.stringify({ id: ID_SERVER, name: 'Renamed' }),
      method: 'POST',
    });
    const lastCall = calls.at(-1);
    expect(lastCall?.config.method).toBe('PUT');
    expect(lastCall?.url).toBe(`${templates}/${ID_SERVER}`);
  });

  it('treats an ABP error body with code 404 as not existing', async () => {
    const { calls, fetcher } = setupFetcher(async (url, config) => {
      if (url === `${templates}/${ID_NEW}` && config.method === 'GET') {
        // ABP 的异常过滤器会把错误体以 HTTP 200 返回，前端拦截器最终抛出 body。
        throw { code: '404', message: '不存在实体' };
      }
      if (url === templates && config.method === 'POST') {
        return { id: ID_SERVER, name: 'New' };
      }
      throw new Error(`unexpected ${config.method} ${url}`);
    });

    await fetcher(templates, {
      body: JSON.stringify({ id: ID_NEW, name: 'New' }),
      method: 'POST',
    });

    expect(calls.map((call) => `${call.config.method} ${call.url}`)).toEqual([
      `GET ${templates}/${ID_NEW}`,
      `POST ${templates}`,
    ]);
  });

  it('applies the same create/update routing to custom elements', async () => {
    const { calls, fetcher } = setupFetcher(async (url, config) => {
      if (url === customElements && config.method === 'GET') {
        return { items: [{ id: ID_A }], totalCount: 1 };
      }
      if (url === `${customElements}/${ID_A}` && config.method === 'PUT') {
        return { id: ID_A };
      }
      throw new Error(`unexpected ${config.method} ${url}`);
    });

    await fetcher(customElements, { method: 'GET' });
    await fetcher(customElements, {
      body: JSON.stringify({ element: {}, id: ID_A, name: 'el' }),
      method: 'POST',
    });

    expect(calls.at(-1)?.config.method).toBe('PUT');
    expect(calls.at(-1)?.url).toBe(`${customElements}/${ID_A}`);
  });

  it('does not create when the existence probe fails for another reason', async () => {
    const { calls, fetcher } = setupFetcher(async (url, config) => {
      if (url === `${templates}/${ID_NEW}` && config.method === 'GET') {
        throw { response: { status: 500 } };
      }
      throw new Error(`unexpected ${config.method} ${url}`);
    });

    await expect(
      fetcher(templates, {
        body: JSON.stringify({ id: ID_NEW, name: 'New' }),
        method: 'POST',
      }),
    ).rejects.toMatchObject({ response: { status: 500 } });

    expect(calls.some((call) => call.config.method === 'POST')).toBe(false);
  });
});
