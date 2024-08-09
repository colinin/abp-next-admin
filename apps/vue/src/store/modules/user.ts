import type { ErrorMessageMode } from '/#/axios';
import { defineStore } from 'pinia';
import { store } from '/@/store';
import { RoleEnum } from '/@/enums/roleEnum';
import { PageEnum } from '/@/enums/pageEnum';
import { ROLES_KEY, TOKEN_KEY, USER_INFO_KEY } from '/@/enums/cacheEnum';
import { getAuthCache, setAuthCache } from '/@/utils/auth';
import { GetUserInfoModel, LoginParams, LoginByPhoneParams } from '/@/api/sys/model/userModel';
import { useAbpStoreWithOut } from './abp';
import { useAppStoreWithOut } from './app';

import { loginApi, loginPhoneApi, getUserInfo } from '/@/api/sys/user';
import { revokeSession } from '/@/api/account/profiles';
import { useI18n } from '/@/hooks/web/useI18n';
import { useMessage } from '/@/hooks/web/useMessage';
import { router } from '/@/router';
import { usePermissionStore } from '/@/store/modules/permission';
import { RouteRecordRaw } from 'vue-router';
import { PAGE_NOT_FOUND_ROUTE } from '/@/router/routes/basic';
import { h } from 'vue';

import { mgr } from '/@/utils/auth/oidc';
import { formatUrl } from '/@/api/oss-management/files/private';
import { isNullOrWhiteSpace } from '/@/utils/strings';

interface UserState {
  userInfo: Nullable<GetUserInfoModel>;
  token?: string;
  roleList: RoleEnum[];
  sessionTimeout?: boolean;
  lastUpdateTime: number;
  /** sso标记,用于后台退出 */
  sso?: boolean;
}

export const useUserStore = defineStore({
  id: 'app-user',
  state: (): UserState => ({
    // user info
    userInfo: null,
    // token
    token: undefined,
    // roleList
    roleList: [],
    // Whether the login expired
    sessionTimeout: false,
    // Last fetch time
    lastUpdateTime: 0,
  }),
  getters: {
    getSso(state): boolean {
      return state.sso === true;
    },
    getUserInfo(state): GetUserInfoModel {
      return state.userInfo || getAuthCache<GetUserInfoModel>(USER_INFO_KEY) || {};
    },
    getToken(state): string {
      return state.token || getAuthCache<string>(TOKEN_KEY);
    },
    getRoleList(state): RoleEnum[] {
      return state.roleList.length > 0 ? state.roleList : getAuthCache<RoleEnum[]>(ROLES_KEY);
    },
    getSessionTimeout(state): boolean {
      return !!state.sessionTimeout;
    },
    getLastUpdateTime(state): number {
      return state.lastUpdateTime;
    },
  },
  actions: {
    setSso(sso: boolean) {
      this.sso = sso;
    },
    setToken(info: string | undefined) {
      this.token = info ? info : ''; // for null or undefined value
      setAuthCache(TOKEN_KEY, info);
    },
    setRoleList(roleList: RoleEnum[]) {
      this.roleList = roleList;
      setAuthCache(ROLES_KEY, roleList);
    },
    setUserInfo(info: GetUserInfoModel) {
      this.userInfo = info;
      this.lastUpdateTime = new Date().getTime();
      setAuthCache(USER_INFO_KEY, info);
    },
    setSessionTimeout(flag: boolean) {
      this.sessionTimeout = flag;
    },
    resetState() {
      this.setUserInfo({});
      this.setToken('');
      this.setRoleList([]);
      this.setSessionTimeout(false);
      const abpStore = useAbpStoreWithOut();
      abpStore.resetSession();
    },
    /**
     * @description: login
     */
    async login(
      params: LoginParams & {
        goHome?: boolean;
        isPortalLogin?: boolean;
        mode?: ErrorMessageMode;
        loginCallback?: () => Promise<void>;
      },
    ): Promise<GetUserInfoModel | null> {
      try {
        const { goHome = true, mode, isPortalLogin, loginCallback, ...loginParams } = params;
        const data = await loginApi(loginParams, mode, isPortalLogin);
        const { access_token } = data;
        this.setSso(false);
        this.setToken(access_token);
        return this.afterLoginAction(goHome, loginCallback);
      } catch (error) {
        return Promise.reject(error);
      }
    },

    async loginByPhone(
      params: LoginByPhoneParams & {
        goHome?: boolean;
        mode?: ErrorMessageMode;
        loginCallback?: () => Promise<void>;
      },
    ): Promise<GetUserInfoModel | null> {
      try {
        const { goHome = true, mode, loginCallback, ...loginParams } = params;
        const data = await loginPhoneApi(loginParams, mode);
        const { access_token } = data;
        this.setSso(false);
        this.setToken(access_token);
        return this.afterLoginAction(goHome, loginCallback);
      } catch (error) {
        return Promise.reject(error);
      }
    },

    async oidcLogin(user: { access_token: string }) {
      this.setSso(true);
      this.setToken(user.access_token);
      return this.afterLoginAction(true);
    },

    async afterLoginAction(goHome?: boolean, loginCallback?: () => Promise<void>): Promise<GetUserInfoModel | null> {
      if (!this.getToken) return null;
      // get user info
      await this.getUserInfoAction(loginCallback);

      try {
        const appStore = useAppStoreWithOut();
        await appStore.initlizeTheme();
      } catch(error) {
        console.warn('Failed to synchronize the user theme.');
      }

      const sessionTimeout = this.sessionTimeout;
      if (sessionTimeout) {
        this.setSessionTimeout(false);
      } else {
        const permissionStore = usePermissionStore();
        if (!permissionStore.isDynamicAddedRoute) {
          const routes = await permissionStore.buildRoutesAction();
          routes.forEach((route) => {
            router.addRoute(route as unknown as RouteRecordRaw);
          });
          router.addRoute(PAGE_NOT_FOUND_ROUTE as unknown as RouteRecordRaw);
          permissionStore.setDynamicAddedRoute(true);
        }
        goHome && (await router.replace(this.userInfo?.homePath || PageEnum.BASE_HOME));
      }
      return this.userInfo;
    },
    async getUserInfoAction(loginCallback?: () => Promise<void>): Promise<GetUserInfoModel> {
      const userInfo = await getUserInfo();

      const abpStore = useAbpStoreWithOut();

      let currentUser = abpStore.getApplication.currentUser;
      //  避免多次请求接口
      if (userInfo?.sub !== currentUser.id) {
        await abpStore.initlizeAbpApplication();
        currentUser = abpStore.getApplication.currentUser;
      }

      if (loginCallback) {
        await loginCallback();
      }

      const outgoingUserInfo: { [key: string]: any } = {
        // 从 currentuser 接口获取
        userId: currentUser.id,
        username: currentUser.userName,
        roles: currentUser.roles,
        // 从 userinfo 端点获取
        realName: userInfo?.nickname,
        phoneNumber: userInfo?.phone_number,
        phoneNumberConfirmed: userInfo?.phone_number_verified === 'True',
        email: userInfo?.email,
        emailConfirmed: userInfo?.email_verified === 'True',
      };
      if (userInfo?.avatarUrl) {
        outgoingUserInfo.avatar = formatUrl(userInfo.avatarUrl);
      }
      if (userInfo?.picture) {
        outgoingUserInfo.avatar = formatUrl(userInfo.picture);
      }
      this.setUserInfo(outgoingUserInfo);

      return outgoingUserInfo;
    },
    /**
     * @description: logout
     */
    async logout(goLogin = false) {
      this.setToken(undefined);
      this.setSessionTimeout(false);
      this.setUserInfo({});
      if (this.getSso === true) {
        this.setSso(false);
        mgr.signoutRedirect();
      } else {
        goLogin && router.push(PageEnum.BASE_LOGIN);
      }
    },

    /**
     * @description: Confirm before logging out
     */
    confirmLoginOut() {
      const { createConfirm } = useMessage();
      const { t } = useI18n();
      createConfirm({
        iconType: 'warning',
        title: () => h('span', t('sys.app.logoutTip')),
        content: () => h('span', t('sys.app.logoutMessage')),
        onOk: async () => {
          const abpStore = useAbpStoreWithOut();
          const sessionId = abpStore.getApplication?.currentUser?.sessionId
          if (!isNullOrWhiteSpace(sessionId)) {
            abpStore.resetSession();
            await revokeSession(sessionId).finally(() => this.logout(true));
          } else {
            await this.logout(true);
          }
        },
      });
    },
  },
});

// Need to be used outside the setup
export function useUserStoreWithOut() {
  return useUserStore(store);
}
