interface UserInfo {
  [property: string]: any;
  /**
   * 邮箱地址
   */
  email: string;
  /**
   * 邮件地址是否已验证
   */
  emailVerified: string;
  /**
   * 名称
   */
  givenName: string;
  /**
   * 用户名
   */
  name: string;
  /**
   * 手机号是否已验证
   */
  phoneNumberVerified: string;
  /**
   * 用户名
   */
  preferredUsername: string;
  /**
   * 角色列表
   */
  role: string[];
  /**
   * 用户标识
   */
  sub: string;
  /**
   * 用户名
   */
  uniqueName: string;
}
/** oauth标准用户信息结构 */
interface OAuthUserInfo {
  [property: string]: any;
  /**
   * 邮箱地址
   */
  email: string;
  /**
   * 邮件地址是否已验证
   */
  email_verified: string;
  /**
   * 名称
   */
  given_name: string;
  /**
   * 用户名
   */
  name: string;
  /**
   * 手机号是否已验证
   */
  phone_number_verified: string;
  /**
   * 用户名
   */
  preferred_username: string;
  /**
   * 角色列表
   */
  role: string[];
  /**
   * 用户标识
   */
  sub: string;
  /**
   * 用户名
   */
  unique_name: string;
}

export type { OAuthUserInfo, UserInfo };
