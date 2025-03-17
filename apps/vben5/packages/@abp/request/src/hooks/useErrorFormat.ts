import type { RemoteServiceErrorInfo } from '@abp/core';
import type { AxiosResponse } from 'axios';

export function useErrorFormat(response: AxiosResponse) {
  const _defaultErrorHeaderKey: string = '_abperrorformat';
  const { data, headers } = response;
  /** 是否请求错误 */
  function hasError(): boolean {
    return headers[_defaultErrorHeaderKey] === 'true';
  }
  /** 如果请求错误,抛出异常 */
  function throwIfError(): void {
    if (!hasError()) return;
    const errorJson = data.error as RemoteServiceErrorInfo;
    let errorMessage = errorJson.message;
    if (errorJson.validationErrors) {
      errorMessage += errorJson.validationErrors
        .map((error) => error.message)
        .join('\n');
    }
    throw Object.assign({}, response, { message: errorMessage, response });
  }

  return {
    hasError,
    throwIfError,
  };
}
