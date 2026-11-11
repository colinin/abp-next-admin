import type { CrudRequester } from './printDesignerCrudFetcher';

import { useRequest } from '@abp/request';

import { createPrintDesignerCrudFetcher } from './printDesignerCrudFetcher';

export function usePrintDotFetcher() {
  const { request } = useRequest();

  // 设计器的 upsert 固定发 POST，这里交给适配器按“是否存在”改写为 POST/PUT。
  return createPrintDesignerCrudFetcher(request as unknown as CrudRequester);
}
