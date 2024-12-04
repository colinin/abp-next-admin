import type { Recordable, UserInfo } from '@vben/types';

import { ref } from 'vue';
import { useRouter } from 'vue-router';

import { DEFAULT_HOME_PATH, LOGIN_PATH } from '@vben/constants';
import { resetAllStores, useAccessStore, useUserStore } from '@vben/stores';

import { getUserInfoApi, loginApi } from '@abp/account';
import { useAbpStore } from '@abp/core';
import { notification } from 'ant-design-vue';
import { defineStore } from 'pinia';

import { getConfigApi } from '#/api/core/abp';
import { $t } from '#/locales';

export const useAuthStore = defineStore('auth', () => {
  const accessStore = useAccessStore();
  const userStore = useUserStore();
  const abpStore = useAbpStore();
  const router = useRouter();

  const loginLoading = ref(false);

  /**
   * 异步处理登录操作
   * Asynchronously handle the login process
   * @param params 登录表单数据
   */
  async function authLogin(
    params: Recordable<any>,
    onSuccess?: () => Promise<void> | void,
  ) {
    // 异步处理用户登录操作并获取 accessToken
    let userInfo: null | UserInfo = null;
    try {
      loginLoading.value = true;
      const loginResult = await loginApi(params as any);
      const { accessToken, tokenType, refreshToken } = loginResult;
      // 如果成功获取到 accessToken
      if (accessToken) {
        accessStore.setAccessToken(`${tokenType} ${accessToken}`);
        accessStore.setRefreshToken(refreshToken);

        userInfo = await fetchUserInfo();

        userStore.setUserInfo(userInfo);

        if (accessStore.loginExpired) {
          accessStore.setLoginExpired(false);
        } else {
          onSuccess
            ? await onSuccess?.()
            : await router.push(userInfo.homePath || DEFAULT_HOME_PATH);
        }

        if (userInfo?.realName) {
          notification.success({
            description: `${$t('authentication.loginSuccessDesc')}:${userInfo?.realName}`,
            duration: 3,
            message: $t('authentication.loginSuccess'),
          });
        }
      }
    } finally {
      loginLoading.value = false;
    }

    return {
      userInfo,
    };
  }

  async function logout(redirect: boolean = true) {
    try {
      // await logoutApi();
    } catch {
      // 不做任何处理
    }
    resetAllStores();
    accessStore.setLoginExpired(false);

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
    let userInfo: ({ [key: string]: any } & UserInfo) | null = null;
    const userInfoRes = await getUserInfoApi();
    const abpConfig = await getConfigApi();
    userInfo = {
      userId: userInfoRes.sub,
      username: userInfoRes.uniqueName,
      realName: userInfoRes.name,
      avatar: userInfoRes.avatarUrl ?? userInfoRes.picture,
      desc: userInfoRes.uniqueName ?? userInfoRes.name,
      email: userInfoRes.email ?? userInfoRes.email,
      token: '',
      roles: abpConfig.currentUser.roles,
      homePath: '/',
    };
    userStore.setUserInfo(userInfo);
    abpStore.setApplication(abpConfig);
    accessStore.setAccessCodes(Object.keys(abpConfig.auth.grantedPolicies));
    return userInfo;
  }

  function $reset() {
    loginLoading.value = false;
  }

  return {
    $reset,
    authLogin,
    fetchUserInfo,
    loginLoading,
    logout,
  };
});
