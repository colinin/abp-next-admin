import type { TokenResult } from '@abp/account';

import type { Recordable, UserInfo } from '@vben/types';

import { ref } from 'vue';
import { useRouter } from 'vue-router';

import { LOGIN_PATH } from '@vben/constants';
import { preferences } from '@vben/preferences';
import { resetAllStores, useAccessStore, useUserStore } from '@vben/stores';

import { useOAuthService, useProfileApi } from '@abp/account';
import { Events, useAbpStore, useEventBus } from '@abp/core';
import { notification } from 'ant-design-vue';
import { defineStore } from 'pinia';

import { useAbpConfigApi } from '#/api/core/useAbpConfigApi';
import { $t } from '#/locales';

export const useAuthStore = defineStore('auth', () => {
  const { publish } = useEventBus();
  const { getConfigApi } = useAbpConfigApi();
  const { getPictureApi } = useProfileApi();
  const accessStore = useAccessStore();
  const userStore = useUserStore();
  const abpStore = useAbpStore();
  const router = useRouter();
  const oAuthService = useOAuthService();

  const loginLoading = ref(false);

  async function refreshSession() {
    if (await oAuthService.getAccessToken()) {
      const user = await oAuthService.refreshToken();
      const newToken = `${user?.token_type} ${user?.access_token}`;
      accessStore.setAccessToken(newToken);
      if (user?.refresh_token) {
        accessStore.setRefreshToken(user.refresh_token);
      }
      return newToken;
    }
  }

  async function oidcLogin() {
    await oAuthService.login();
  }

  async function oidcCallback() {
    try {
      const user = await oAuthService.handleCallback();
      return await _loginSuccess({
        accessToken: user.access_token,
        tokenType: user.token_type,
        refreshToken: user.refresh_token ?? '',
        // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
        expiresIn: user.expires_in!,
      });
    } finally {
      loginLoading.value = false;
    }
  }

  async function qrcodeLogin(
    key: string,
    tenantId?: string,
    onSuccess?: () => Promise<void> | void,
  ) {
    try {
      loginLoading.value = true;
      const user = await oAuthService.loginByQrCode({ key, tenantId });
      return await _loginSuccess(
        {
          accessToken: user.access_token,
          tokenType: user.token_type,
          refreshToken: user.refresh_token ?? '',
          // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
          expiresIn: user.expires_in!,
        },
        onSuccess,
      );
    } finally {
      loginLoading.value = false;
    }
  }

  async function phoneLogin(
    phoneNumber: string,
    code: string,
    onSuccess?: () => Promise<void> | void,
  ) {
    try {
      loginLoading.value = true;
      const user = await oAuthService.loginBySmsCode({ phoneNumber, code });
      return await _loginSuccess(
        {
          accessToken: user.access_token,
          tokenType: user.token_type,
          refreshToken: user.refresh_token ?? '',
          // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
          expiresIn: user.expires_in!,
        },
        onSuccess,
      );
    } finally {
      loginLoading.value = false;
    }
  }

  /**
   * 异步处理登录操作
   * Asynchronously handle the login process
   * @param params 登录表单数据
   */
  async function authLogin(
    params: Recordable<any>,
    onSuccess?: () => Promise<void> | void,
  ) {
    try {
      loginLoading.value = true;
      const user = await oAuthService.loginByPassword(params as any);
      return await _loginSuccess(
        {
          accessToken: user.access_token,
          tokenType: user.token_type,
          refreshToken: user.refresh_token ?? '',
          // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
          expiresIn: user.expires_in!,
        },
        onSuccess,
      );
    } finally {
      loginLoading.value = false;
    }
  }

  async function logout(redirect: boolean = true) {
    try {
      if (await oAuthService.getAccessToken()) {
        accessStore.setAccessToken(null);
        await oAuthService.logout();
      } else {
        await oAuthService.revokeTokens();
      }
    } catch {
      // 不做任何处理
    }
    resetAllStores();
    accessStore.setLoginExpired(false);

    publish(Events.UserLogout);

    // 回登录页带上当前路由地址
    await router.replace({
      path: LOGIN_PATH,
      query: redirect
        ? {
            redirect: encodeURIComponent(router.currentRoute.value.fullPath),
          }
        : {},
    });
  }

  async function fetchUserInfo() {
    let userInfo: null | (UserInfo & { [key: string]: any }) = null;
    let userInfoRes: { [key: string]: any } = {};
    const user = await oAuthService.getUser();
    if (user) {
      userInfoRes = user.profile;
    }
    const abpConfig = await getConfigApi();
    const picture = await getPictureApi();
    userInfo = {
      userId: userInfoRes.sub ?? abpConfig.currentUser.id,
      username: userInfoRes.uniqueName ?? abpConfig.currentUser.userName,
      realName:
        userInfoRes.name ??
        abpConfig.currentUser.name ??
        abpConfig.currentUser.userName,
      avatar: URL.createObjectURL(picture) ?? '',
      desc: userInfoRes.uniqueName ?? userInfoRes.name,
      email: userInfoRes.email ?? userInfoRes.email,
      emailVerified:
        userInfoRes.emailVerified ?? abpConfig.currentUser.emailVerified,
      phoneNumber: userInfoRes.phoneNumber ?? abpConfig.currentUser.phoneNumber,
      phoneNumberVerified:
        userInfoRes.phoneNumberVerified ??
        abpConfig.currentUser.phoneNumberVerified,
      token: '',
      roles: abpConfig.currentUser.roles,
      homePath: '/',
    };
    userStore.setUserInfo(userInfo);
    abpStore.setApplication(abpConfig);
    accessStore.setAccessCodes(Object.keys(abpConfig.auth.grantedPolicies));
    return userInfo;
  }

  async function _loginSuccess(
    loginResult: TokenResult,
    onSuccess?: () => Promise<void> | void,
  ) {
    // 异步处理用户登录操作并获取 accessToken
    let userInfo: null | UserInfo = null;
    const { accessToken, tokenType, refreshToken } = loginResult;
    // 如果成功获取到 accessToken
    if (accessToken) {
      accessStore.setAccessToken(`${tokenType} ${accessToken}`);
      accessStore.setRefreshToken(refreshToken);

      userInfo = await fetchUserInfo();

      userStore.setUserInfo(userInfo);

      publish(Events.UserLogin, userInfo);

      if (accessStore.loginExpired) {
        accessStore.setLoginExpired(false);
      } else {
        onSuccess
          ? await onSuccess?.()
          : await router.push(
              userInfo.homePath || preferences.app.defaultHomePath,
            );
      }

      if (userInfo?.realName) {
        notification.success({
          description: `${$t('authentication.loginSuccessDesc')}:${userInfo?.realName}`,
          duration: 3,
          message: $t('authentication.loginSuccess'),
        });
      }
    }

    return {
      userInfo,
    };
  }

  function $reset() {
    const userInfo = userStore.userInfo;
    if (userInfo?.avatar) {
      URL.revokeObjectURL(userInfo?.avatar);
    }
    loginLoading.value = false;
  }

  return {
    $reset,
    authLogin,
    phoneLogin,
    qrcodeLogin,
    oidcLogin,
    oidcCallback,
    fetchUserInfo,
    loginLoading,
    logout,
    refreshSession,
  };
});
