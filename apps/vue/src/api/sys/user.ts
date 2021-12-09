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

enum Api {
  Login = '/connect/token',
  Logout = '/logout',
  GetUserInfo = '/connect/userinfo',
  GetPermCode = '/getPermCode',
}

/**
 * @description: user login api
 */
export function loginApi(params: LoginParams, mode: ErrorMessageMode = 'modal') {
  const setting = useGlobSetting();
  const tokenParams = {
    client_id: setting.clientId,
    client_secret: setting.clientSecret,
    grant_type: 'password',
    username: params.username,
    password: params.password,
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
    });
}

export function getPermCode() {
  return defHttp.get<string[]>({ url: Api.GetPermCode });
}

export function doLogout() {
  return defHttp.get({ url: Api.Logout });
}
