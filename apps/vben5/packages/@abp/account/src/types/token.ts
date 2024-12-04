/** 授权请求数据模型 */
interface TokenRequest {
  /** 客户端id */
  clientId: string;
  /** 客户端密钥 */
  clientSecret: string;
}
/** 用户密码授权请求数据模型 */
interface PasswordTokenRequest extends TokenRequest {
  /** 用户密码 */
  password: string;
  /** 授权范围 */
  scope?: string;
  /** 用户名 */
  userName: string;
}
/** 用户密码授权请求数据模型 */
interface PasswordTokenRequestModel {
  /** 用户密码 */
  password: string;
  /** 用户名 */
  username: string;
}
/** 令牌返回数据模型 */
interface TokenResult {
  /** 访问令牌 */
  accessToken: string;
  /** 过期时间 */
  expiresIn: number;
  /** 刷新令牌 */
  refreshToken: string;
  /** 令牌类型 */
  tokenType: string;
}
/** oauth标准令牌返回结构 */
interface OAuthTokenResult {
  /** 访问令牌 */
  access_token: string;
  /** 过期时间 */
  expires_in: number;
  /** 刷新令牌 */
  refresh_token: string;
  /** 令牌类型 */
  token_type: string;
}

export type {
  OAuthTokenResult,
  PasswordTokenRequest,
  PasswordTokenRequestModel,
  TokenRequest,
  TokenResult,
};
