import { authenticateResponseInterceptor, errorMessageResponseInterceptor, RequestClient } from "@/request-client";
import useLocaleStore from "@/store/localeI18nStore";
import useUserStore from "@/store/userStore";
import userStore from "@/store/userStore";
import { mapLocaleToAbpLanguageFormat } from "@/utils";
import { toast } from "sonner";
import { refreshToken } from "./account/token";
import { wrapperResult } from "@/utils/abp/request";
import { handleOAuthError } from "@/utils/abp/handleOAuthError";

const requestClient = new RequestClient({
	baseURL: import.meta.env.VITE_APP_BASE_API,
});

/**
 * 重新认证逻辑
 */
async function doReAuthenticate() {
	console.warn("Access token or refresh token is invalid or expired. ");
	//直接登出
	userStore.getState().actions.clearUserInfoAndToken();
}

/**
 * 刷新token逻辑
 */
async function doRefreshToken() {
	console.debug("try -> Refresh token");

	const { userToken } = useUserStore.getState();
	if (!userToken.refreshToken) {
		console.warn("No refresh token available.");
		return "";
	}

	try {
		const res = await refreshToken({ refreshToken: userToken.refreshToken });

		const { tokenType, accessToken, refreshToken: newRefreshToken } = res;

		if (accessToken) {
			// 更新 userStore，保存新 token
			useUserStore.getState().actions.setUserToken({
				accessToken: `${tokenType} ${accessToken}`,
				refreshToken: newRefreshToken,
			});
			console.debug("Token refreshed successfully.");
			return `${tokenType} ${accessToken}`; // 返回新 token 供拦截器使用
		}

		console.error("Failed to refresh token: No access token returned.");
		return "";
	} catch (error) {
		console.error("Error refreshing token:", error);
		return ""; // 返回空字符串，触发重登录逻辑
	}
}

function formatToken(token: null | string) {
	return token ? token : null; //有个tokenType的获取值
}

// 请求头处理
requestClient.addRequestInterceptor({
	fulfilled: async (config) => {
		const { userToken } = useUserStore.getState();
		if (userToken.accessToken) {
			config.headers.Authorization = `${userToken.accessToken}`;
		}
		const { locale } = useLocaleStore.getState();
		config.headers["Accept-Language"] = mapLocaleToAbpLanguageFormat(locale);
		config.headers["X-Request-From"] = "slash-admin";
		return config;
	},
});

// response数据解构
requestClient.addResponseInterceptor<any>({
	fulfilled: (response) => {
		const { data, status } = response;
		const { hasWrapResult, getData } = wrapperResult(response);

		if (hasWrapResult()) {
			return getData();
		}

		if (status >= 200 && status < 400) {
			return data;
		}

		throw Object.assign({}, response, { response });
	},
});

// token过期的处理
requestClient.addResponseInterceptor(
	authenticateResponseInterceptor({
		client: requestClient,
		doReAuthenticate,
		doRefreshToken,
		enableRefreshToken: true,
		formatToken,
	}),
);

// 通用的错误处理,如果没有进入上面的错误处理逻辑，就会进入这里
requestClient.addResponseInterceptor(
	errorMessageResponseInterceptor((msg: string, error) => {
		// 这里可以根据业务进行定制,你可以拿到 error 内的信息进行定制化处理，根据不同的 code 做不同的提示，而不是直接使用 message.error 提示 msg
		// 当前mock接口返回的错误字段是 error 或者 message
		const responseData = error?.response?.data ?? {};
		if (responseData?.error_description) {
			const { formatError } = handleOAuthError();
			toast.error(formatError(responseData) || msg, {
				position: "top-center",
			});
			return;
		}
		const errorMessage = responseData?.error ?? responseData?.message ?? "";
		// 如果没有错误信息，则会根据状态码进行提示
		toast.error(errorMessage || msg, {
			position: "top-center",
		});
	}),
);

export default requestClient;
