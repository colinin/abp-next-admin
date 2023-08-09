/**
 * @description: Login interface parameters
 */
export interface LoginParams {
  username: string;
  password: string;
  twoFactorProvider?: string;
  twoFactorCode?: string;
  enterpriseId?: string;
}

export interface LoginByPhoneParams {
  phoneNumber: string;
  code: string;
}

export interface RoleInfo {
  roleName: string;
  value: string;
}

/**
 * @description: Login interface return value
 */
export interface LoginResultModel {
  /** 访问令牌 */
  access_token: string;
  /** 过期时间 */
  expires_in: number;
  /** 令牌类型 */
  token_type: string;
  /** 刷新令牌 */
  refresh_token: string;
}

export interface PortalLoginModel {
  id: string;
  name: string;
  logo?: string;
}

/**
 * @description: Get user information return value
 */
export interface GetUserInfoModel {
  [key: string]: string;
}
