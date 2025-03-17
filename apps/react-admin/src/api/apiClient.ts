import axios, { type AxiosRequestConfig, type AxiosError, type AxiosResponse } from "axios";

import { t } from "@/locales/i18n";
import userStore, { useUserToken } from "@/store/userStore";

import { toast } from "sonner";
//TODO rm
// 创建 axios 实例
const axiosInstance = axios.create({
	baseURL: import.meta.env.VITE_APP_BASE_API,
	timeout: 50000,
	headers: { "Content-Type": "application/json;charset=utf-8" },
});

// 请求拦截
axiosInstance.interceptors.request.use(
	(config) => {
		// 在请求被发送之前做些什么
		const { accessToken } = useUserToken();
		config.headers.Authorization = `${accessToken}`;
		return config;
	},
	(error) => {
		// 请求错误时做些什么
		return Promise.reject(error);
	},
);

// 响应拦截
axiosInstance.interceptors.response.use(
	(res: AxiosResponse<any>) => {
		const { data, status, headers } = res;

		if (headers._abpwrapresult === "true") {
			const { code, result, message, details } = data;
			const hasSuccess = data && Reflect.has(data, "code") && code === "0";
			if (hasSuccess) {
				return result;
			}
			const content = details || message;

			throw new Error(content);
		}

		if (status >= 200 && status < 400) {
			return data;
		}

		// 业务请求错误
		throw new Error(t("sys.api.apiRequestFailed"));
	},
	(error: AxiosError<any>) => {
		const { response, message } = error || {};

		const errMsg = response?.data?.message || message || t("sys.api.errorMessage");
		toast.error(errMsg, {
			position: "top-center",
		});

		const status = response?.status;
		if (status === 401) {
			userStore.getState().actions.clearUserInfoAndToken();
		}
		return Promise.reject(error);
	},
);

class APIClient {
	get<T = any>(config: AxiosRequestConfig): Promise<T> {
		return this.request({ ...config, method: "GET" });
	}

	post<T = any>(config: AxiosRequestConfig): Promise<T> {
		return this.request({ ...config, method: "POST" });
	}

	put<T = any>(config: AxiosRequestConfig): Promise<T> {
		return this.request({ ...config, method: "PUT" });
	}

	delete<T = any>(config: AxiosRequestConfig): Promise<T> {
		return this.request({ ...config, method: "DELETE" });
	}

	request<T = any>(config: AxiosRequestConfig): Promise<T> {
		return new Promise((resolve, reject) => {
			axiosInstance
				.request<any, AxiosResponse<any>>(config)
				.then((res: AxiosResponse<any>) => {
					resolve(res as unknown as Promise<T>);
				})
				.catch((e: Error | AxiosError) => {
					reject(e);
				});
		});
	}
}
export default new APIClient();
