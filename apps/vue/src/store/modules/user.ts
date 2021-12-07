import type { ErrorMessageMode } from '/#/axios';
import { defineStore } from 'pinia';
import { store } from '/@/store';
import { RoleEnum } from '/@/enums/roleEnum';
import { PageEnum } from '/@/enums/pageEnum';
import { ROLES_KEY, TOKEN_KEY, USER_INFO_KEY } from '/@/enums/cacheEnum';
import { getAuthCache, setAuthCache } from '/@/utils/auth';
import { GetUserInfoModel, LoginParams, LoginByPhoneParams } from '/@/api/sys/model/userModel';
import { useAbpStoreWithOut } from './abp';

import { loginApi, loginPhoneApi, getUserInfo } from '/@/api/sys/user';
import { useI18n } from '/@/hooks/web/useI18n';
import { useMessage } from '/@/hooks/web/useMessage';
import { router } from '/@/router';
import { usePermissionStore } from '/@/store/modules/permission';
import { RouteRecordRaw } from 'vue-router';
import { PAGE_NOT_FOUND_ROUTE } from '/@/router/routes/basic';
import { h } from 'vue';

import { mgr } from '/@/utils/auth/oidc';
import { formatUrl } from '/@/api/oss-management/private';

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
    getSso(): boolean {
      return this.sso === true;
    },
    getUserInfo(): GetUserInfoModel {
      return this.userInfo || getAuthCache<GetUserInfoModel>(USER_INFO_KEY) || {};
    },
    getToken(): string {
      return this.token || getAuthCache<string>(TOKEN_KEY);
    },
    getRoleList(): RoleEnum[] {
      return this.roleList.length > 0 ? this.roleList : getAuthCache<RoleEnum[]>(ROLES_KEY);
    },
    getSessionTimeout(): boolean {
      return !!this.sessionTimeout;
    },
    getLastUpdateTime(): number {
      return this.lastUpdateTime;
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
        mode?: ErrorMessageMode;
      },
    ): Promise<GetUserInfoModel | null> {
      try {
        const { goHome = true, mode, ...loginParams } = params;
        const data = await loginApi(loginParams, mode);
        const { access_token } = data;
        this.setSso(false);
        this.setToken(access_token);
        return this.afterLoginAction(goHome);
      } catch (error) {
        return Promise.reject(error);
      }
    },

    async loginByPhone(
      params: LoginByPhoneParams & {
        goHome?: boolean;
        mode?: ErrorMessageMode;
      },
    ): Promise<GetUserInfoModel | null> {
      try {
        const { goHome = true, mode, ...loginParams } = params;
        const data = await loginPhoneApi(loginParams, mode);
        const { access_token } = data;
        this.setSso(false);
        this.setToken(access_token);
        return this.afterLoginAction(goHome);
      } catch (error) {
        return Promise.reject(error);
      }
    },

    async oidcLogin(user: { access_token: string }) {
      this.setSso(true);
      this.setToken(user.access_token);
      return this.afterLoginAction(true);
    },

    async afterLoginAction(goHome?: boolean): Promise<GetUserInfoModel | null> {
      if (!this.getToken) return null;
      // get user info
      const userInfo = await this.getUserInfoAction();

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
        goHome && (await router.replace(userInfo?.homePath || PageEnum.BASE_HOME));
      }
      return userInfo;
    },
    async getUserInfoAction(): Promise<GetUserInfoModel> {
      const abpStore = useAbpStoreWithOut();
      await abpStore.initlizeAbpApplication();
      const userInfo = await getUserInfo();
      const currentUser = abpStore.getApplication.currentUser;

      const outgoingUserInfo: { [key: string]: any } = {
        // 从 currentuser 接口获取
        userId: currentUser.id,
        username: currentUser.userName,
        roles: currentUser.roles,
        // 从 userinfo 端点获取
        realName: userInfo.nickname,
        phoneNumber: userInfo.phone_number,
        phoneNumberConfirmed: userInfo.phone_number_verified === 'True',
        email: userInfo.email,
        emailConfirmed: userInfo.email_verified === 'True',
      };
      if (userInfo.avatarUrl) {
        outgoingUserInfo.avatar = formatUrl(userInfo.avatarUrl);
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
          await this.logout(true);
        },
      });
    },
  },
});

// Need to be used outside the setup
export function useUserStoreWithOut() {
  return useUserStore(store);
}
