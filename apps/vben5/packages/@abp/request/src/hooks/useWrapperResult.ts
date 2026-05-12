import type { AxiosResponse } from 'axios';

import { useErrorFormat } from './useErrorFormat';

export function useWrapperResult(response: AxiosResponse) {
  const {
    hasError: hasAbpError,
    getErrorMessage: getAbpErrorMessage,
    throwIfError: throwIfAbpError,
  } = useErrorFormat(response);
  const _defaultWrapperHeaderKey: string = '_abpwrapresult';
  const { data, headers } = response;

  /** 是否请求错误 */
  function hasError(): boolean {
    if (hasAbpError()) return true;
    return Reflect.has(data, 'code') && data.code !== '0';
  }

  /** 是否已包装结果 */
  function hasWrapResult(): boolean {
    if (
      typeof headers.has === 'function' &&
      headers.has(_defaultWrapperHeaderKey)
    ) {
      return String(headers[_defaultWrapperHeaderKey]).includes('true');
    }
    return hasAbpError();
  }

  /** 获取包装结果 */
  function getData(): any {
    throwIfError();
    return data.result;
  }

  /** 如果请求错误,抛出异常 */
  function throwIfError(): void {
    throwIfAbpError();
    if (hasError()) {
      const content = data.details || data.message;
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

  /** 获取错误消息 */
  function getErrorMessage(): string | undefined {
    if (!hasWrapResult()) return undefined;
    const errorMessage = getAbpErrorMessage();
    if (errorMessage) return errorMessage;
    if (!hasError()) return undefined;
    return data.message;
  }

  return {
    getData,
    getErrorMessage,
    hasError,
    hasWrapResult,
  };
}
