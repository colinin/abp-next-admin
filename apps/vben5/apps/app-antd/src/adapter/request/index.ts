import { preferences } from '@vben/preferences';
import {
  authenticateResponseInterceptor,
  errorMessageResponseInterceptor,
} from '@vben/request';
import { useAccessStore } from '@vben/stores';

import { requestClient } from '@abp/request';
import { message } from 'ant-design-vue';

import { useAuthStore } from '#/store';

export function initRequestClient() {
  /**
   * 重新认证逻辑
   */
  async function doReAuthenticate() {
    console.warn('Access token or refresh token is invalid or expired. ');
    const accessStore = useAccessStore();
    const authStore = useAuthStore();
    accessStore.setAccessToken(null);
    if (
      preferences.app.loginExpiredMode === 'modal' &&
      accessStore.isAccessChecked
    ) {
      accessStore.setLoginExpired(true);
    } else {
      await authStore.logout();
    }
  }

  /**
   * 刷新token逻辑
   */
  async function doRefreshToken() {
    return '';
  }

  function formatToken(token: null | string) {
    return token ? `Bearer ${token}` : null;
  }
  // 请求头处理
  requestClient.addRequestInterceptor({
    fulfilled: async (config) => {
      const accessStore = useAccessStore();
      if (accessStore.accessToken) {
        config.headers.Authorization = `${accessStore.accessToken}`;
      }
      config.headers['Accept-Language'] = preferences.app.locale;
      config.headers['X-Request-From'] = 'vben';
      return config;
    },
  });

  // response数据解构
  requestClient.addResponseInterceptor<any>({
    fulfilled: (response) => {
      const { data, status, headers } = response;

      if (headers._abpwrapresult === 'true') {
        const { code, result, message, details } = data;
        const hasSuccess = data && Reflect.has(data, 'code') && code === '0';
        if (hasSuccess) {
          return result;
        }
        const content = details || message;

        throw Object.assign({}, response, { response, message: content });
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
      enableRefreshToken: preferences.app.enableRefreshToken,
      formatToken,
    }),
  );

  // 通用的错误处理,如果没有进入上面的错误处理逻辑，就会进入这里
  requestClient.addResponseInterceptor(
    errorMessageResponseInterceptor((msg: string, error) => {
      // 这里可以根据业务进行定制,你可以拿到 error 内的信息进行定制化处理，根据不同的 code 做不同的提示，而不是直接使用 message.error 提示 msg
      // 当前mock接口返回的错误字段是 error 或者 message
      const responseData = error?.response?.data ?? {};
      const errorMessage = responseData?.error ?? responseData?.message ?? '';
      // 如果没有错误信息，则会根据状态码进行提示
      message.error(errorMessage || msg);
    }),
  );
}
