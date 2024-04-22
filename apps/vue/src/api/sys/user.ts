import { defHttp } from '/@/utils/http/axios';
import {
  LoginParams,
  LoginResultModel,
  GetUserInfoModel,
  LoginByPhoneParams,
} from './model/userModel';
import { useGlobSetting } from '/@/hooks/setting';
import { ContentTypeEnum } from '/@/enums/httpEnum';

import { ErrorMessageMode } from '/#/axios';
import { t } from '/@/hooks/web/useI18n';
import { useMessage } from '/@/hooks/web/useMessage';
import { useUserStoreWithOut } from '/@/store/modules/user';

const { createErrorModal } = useMessage();

enum Api {
  Login = '/connect/token',
  Logout = '/logout',
  GetUserInfo = '/connect/userinfo',
  GetPermCode = '/getPermCode',
}

/**
 * @description: user login api
 */
export function loginApi(params: LoginParams, mode: ErrorMessageMode = 'modal', isPortalLogin: boolean = false) {
  const setting = useGlobSetting();
  const tokenParams = {
    client_id: setting.clientId,
    client_secret: setting.clientSecret,
    grant_type: isPortalLogin ? 'portal' : 'password',
    username: params.username,
    password: params.password,
    enterpriseId: params.enterpriseId,
    scope: 'openid email address phone profile offline_access lingyun-abp-application',
    TwoFactorProvider: params.twoFactorProvider,
    TwoFactorCode: params.twoFactorCode,
  };
  return defHttp.post<LoginResultModel>(
    {
      url: Api.Login,
      params: tokenParams,
      headers: {
        'Content-Type': ContentTypeEnum.FORM_URLENCODED,
      },
    },
    {
      errorMessageMode: mode,
      apiUrl: '/connect',
      withToken: false,
    },
  );
}

/**
 * 手机登录
 * @param params
 * @param mode
 * @returns
 */
export function loginPhoneApi(params: LoginByPhoneParams, mode: ErrorMessageMode = 'modal') {
  const setting = useGlobSetting();
  const tokenParams = {
    client_id: setting.clientId,
    client_secret: setting.clientSecret,
    grant_type: 'phone_verify',
    phone_number: params.phoneNumber,
    phone_verify_code: params.code,
  };
  return defHttp.post<LoginResultModel>(
    {
      url: Api.Login,
      params: tokenParams,
      headers: {
        'Content-Type': ContentTypeEnum.FORM_URLENCODED,
      },
    },
    {
      errorMessageMode: mode,
      apiUrl: '/connect',
    },
  );
}

/**
 * @description: getUserInfo
 */
export function getUserInfo() {
  return defHttp.get<GetUserInfoModel>(
    {
      url: Api.GetUserInfo,
    }, 
    {
      errorMessageMode: 'none',
      apiUrl: '/connect',
    }).catch(() => {
      const userStore = useUserStoreWithOut();
      createErrorModal({
        title: t('sys.api.errorTip'),
        content: t('sys.api.getUserInfoErrorMessage'),
        onOk: () => {
          userStore.setToken(undefined);
          userStore.logout(true);
        }
      });
    });
}

export function getPermCode() {
  return defHttp.get<string[]>({ url: Api.GetPermCode });
}

export function doLogout() {
  return defHttp.get({ url: Api.Logout });
}
