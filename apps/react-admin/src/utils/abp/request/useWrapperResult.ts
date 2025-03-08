import type { AxiosResponse } from "axios";

import { errorFormat } from "./useErrorFormat";

export function wrapperResult(response: AxiosResponse) {
	const { hasError, throwIfError: throwIfAbpError } = errorFormat(response);
	const _defaultWrapperHeaderKey: string = "_abpwrapresult";
	const { data, headers } = response;
	/** 是否已包装结果 */
	function hasWrapResult(): boolean {
		return headers[_defaultWrapperHeaderKey] === "true" || hasError();
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
		const hasSuccess = data && Reflect.has(data, "code") && code === "0";
		if (!hasSuccess) {
			const content = details || message;
			throw Object.assign({}, response, { message: content, response });
		}
	}

	return {
		getData,
		hasWrapResult,
	};
}
