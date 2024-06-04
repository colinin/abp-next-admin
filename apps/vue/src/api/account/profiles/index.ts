import { defHttp } from '/@/utils/http/axios';
import {
  MyProfile,
  UpdateMyProfile,
  ChangePassword,
  ChangePhoneNumber,
  TwoFactorEnabled,
  SendEmailConfirmCode,
  ConfirmEmailInput,
  TwoFactorEnabledInput,
  SendChangePhoneNumberCodeInput,
  AuthenticatorDto,
  VerifyAuthenticatorCodeInput,
  AuthenticatorRecoveryCodeDto,
} from './model';

export const get = () => {
  return defHttp.get<MyProfile>({
    url: '/api/account/my-profile',
  });
};

export const update = (input: UpdateMyProfile) => {
  return defHttp.put<MyProfile>({
    url:'/api/account/my-profile',
    data: input,
  });
};

export const changePassword = (input: ChangePassword) => {
  return defHttp.post<void>({
    url: '/api/account/my-profile/change-password',
    data: input,
  });
};

export const sendEmailConfirmLink = (input: SendEmailConfirmCode) => {
  return defHttp.post<void>({
    url: '/api/account/my-profile/send-email-confirm-link',
    data: input,
  });
};

export const confirmEmail = (input: ConfirmEmailInput) => {
  return defHttp.put<void>({
    url: '/api/account/my-profile/confirm-email',
    data: input,
  });
};

export const sendChangePhoneNumberCode = (input: SendChangePhoneNumberCodeInput) => {
  return defHttp.post<void>({
    url: '/api/account/my-profile/send-phone-number-change-code',
    data: input,
  });
};

export const changePhoneNumber = (input: ChangePhoneNumber) => {
  return defHttp.put<void>({
    url: '/api/account/my-profile/change-phone-number',
    data: input,
  });
};

export const getTwoFactorEnabled = () => {
  return defHttp.get<TwoFactorEnabled>({
    url: '/api/account/my-profile/two-factor',
  });
};

export const changeTwoFactorEnabled = (input: TwoFactorEnabledInput) => {
  return defHttp.put<void>({
    url: '/api/account/my-profile/change-two-factor',
    data: input,
  });
};

export const getAuthenticator = () => {
  return defHttp.get<AuthenticatorDto>({
    url: '/api/account/my-profile/authenticator',
  });
}

export const verifyAuthenticatorCode = (input: VerifyAuthenticatorCodeInput) => {
  return defHttp.post<AuthenticatorRecoveryCodeDto>({
    url: '/api/account/my-profile/verify-authenticator-code',
    data: input,
  });
}

export const resetAuthenticator = () => {
  return defHttp.post<void>({
    url: '/api/account/my-profile/reset-authenticator',
  });
}