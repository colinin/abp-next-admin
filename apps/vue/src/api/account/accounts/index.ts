import { defHttp } from '/@/utils/http/axios';
import {
    Register,
    PhoneRegister,
    PhoneResetPassword,
    SendPhoneSigninCodeInput,
    SendEmailSigninCodeInput,
    SendPhoneRegisterCodeInput,
    SendPhoneResetPasswordCodeInput,
    GetTwoFactorProvidersInput,
} from './model';
import { User } from '/@/api/identity/model/userModel';

export const passwordRegister = (input: Register) => {
  return defHttp.post<User>({
    url: '/api/account/register',
    data: input,
  });
};

export const phoneRegister = (input: PhoneRegister) => {
  return defHttp.post<void>({
    url: '/api/account/phone/register',
    data: input,
  });
};

export const resetPassword = (input: PhoneResetPassword) => {
  return defHttp.put<void>({
    url: '/api/account/phone/reset-password',
    data: input,
  });
};

export const sendPhoneSigninCode = (input: SendPhoneSigninCodeInput) => {
  return defHttp.post<void>({
    url: '/api/account/phone/send-signin-code',
    data: input,
  });
};

export const sendEmailSigninCode = (input: SendEmailSigninCodeInput) => {
  return defHttp.post<void>({
    url: '/api/account/email/send-signin-code',
    data: input,
  });
};

export const sendPhoneRegisterCode = (input: SendPhoneRegisterCodeInput) => {
  return defHttp.post<void>({
    url: '/api/account/phone/send-register-code',
    data: input,
  });
};

export const sendPhoneResetPasswordCode = (input: SendPhoneResetPasswordCodeInput) => {
  return defHttp.post<void>({
    url: '/api/account/phone/send-password-reset-code',
    data: input,
  });
};

export const getTwoFactorProviders = (input: GetTwoFactorProvidersInput) => {
  return defHttp.get<ListResultDto<NameValue<string>>>({
    url: `/api/account/two-factor-providers?userId=${input.userId}`,
  });
}
