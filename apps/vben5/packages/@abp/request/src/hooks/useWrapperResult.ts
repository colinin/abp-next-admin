import type { AxiosResponse } from 'axios';

import { useErrorFormat } from './useErrorFormat';

export function useWrapperResult(response: AxiosResponse) {
  const { hasError, throwIfError: throwIfAbpError } = useErrorFormat(response);
  const _defaultWrapperHeaderKey: string = '_abpwrapresult';
  const { data, headers } = response;
  /** 是否已包装结果 */
  function hasWrapResult(): boolean {
    const wrapperHeader = headers[_defaultWrapperHeaderKey];
    if (!wrapperHeader) {
      return false;
    }
    return String(wrapperHeader).includes('true') || hasError();
  }

  /** 获取包装结果 */
  function getData(): any {
    throwIfError();
    return data.result;
  }

  /** 如果请求错误,抛出异常 */
  function throwIfError(): void {
    throwIfAbpError();
    const { code, details, message } = data;
    const hasSuccess = data && Reflect.has(data, 'code') && code === '0';
    if (!hasSuccess) {
      const content = details || message;
      throw Object.assign({}, response, {
        response: {
          ...response,
          data: {
            ...response.data,
            message: content,
          },
        },
      });
    }
  }

  return {
    getData,
    hasWrapResult,
  };
}
