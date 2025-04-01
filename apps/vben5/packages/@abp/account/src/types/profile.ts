import type { ExtensibleObject, IHasConcurrencyStamp } from '@abp/core';

interface ProfileDto extends ExtensibleObject, IHasConcurrencyStamp {
  /** 电子邮件 */
  email: string;
  hasPassword: boolean;
  /** 是否外部用户 */
  isExternal: boolean;
  /** 名称 */
  name?: string;
  /** 手机号码 */
  phoneNumber?: string;
  /** 姓氏 */
  surname?: string;
  /** 用户名 */
  userName: string;
}

interface UpdateProfileDto extends ExtensibleObject, IHasConcurrencyStamp {
  /** 电子邮件 */
  email: string;
  /** 名称 */
  name?: string;
  /** 手机号码 */
  phoneNumber?: string;
  /** 姓氏 */
  surname?: string;
  /** 用户名 */
  userName: string;
}

interface ChangePasswordInput {
  /** 当前密码 */
  currentPassword: string;
  /** 新密码 */
  newPassword: string;
}

interface ChangePhoneNumberInput {
  code: string;
  newPhoneNumber: string;
}

interface SendChangePhoneNumberCodeInput {
  newPhoneNumber: string;
}

interface ChangePictureInput {
  file: File;
}

interface TwoFactorEnabledDto {
  /** 是否启用二次认证 */
  enabled: boolean;
}

interface AuthenticatorDto {
  authenticatorUri: string;
  isAuthenticated: boolean;
  sharedKey: string;
}

interface VerifyAuthenticatorCodeInput {
  authenticatorCode: string;
}

interface AuthenticatorRecoveryCodeDto {
  recoveryCodes: string[];
}

interface SendEmailConfirmCodeDto {
  appName: string;
  email: string;
  returnUrl?: string;
}

interface ConfirmEmailInput {
  confirmToken: string;
}

export type {
  AuthenticatorDto,
  AuthenticatorRecoveryCodeDto,
  ChangePasswordInput,
  ChangePhoneNumberInput,
  ChangePictureInput,
  ConfirmEmailInput,
  ProfileDto,
  SendChangePhoneNumberCodeInput,
  SendEmailConfirmCodeDto,
  TwoFactorEnabledDto,
  UpdateProfileDto,
  VerifyAuthenticatorCodeInput,
};
