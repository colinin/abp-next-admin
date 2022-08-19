import { defAbpHttp } from '/@/utils/http/abp';
import { Register, PhoneRegister, PhoneResetPassword } from './model/accountsModel';
import { User } from '/@/api/identity/model/userModel';
import { ListResultDto } from '../model/baseModel';
import { format } from '/@/utils/strings';

enum Api {
  Register = '/api/account/register',
  RegisterByPhone = '/api/account/phone/register',
  ResetPassword = '/api/account/phone/reset-password',
  SendEmailSignCode = '/api/account/email/send-signin-code',
  SendPhoneSignCode = '/api/account/phone/send-signin-code',
  SendPhoneRegisterCode = '/api/account/phone/send-register-code',
  SendPhoneResetPasswordCode = '/api/account/phone/send-password-reset-code',
  GetTwoFactorProviders = '/api/account/two-factor-providers?userId={userId}',
}

export const register = (input: Register) => {
  return defAbpHttp.post<User>({
    url: Api.Register,
    data: input,
  });
};

export const registerByPhone = (input: PhoneRegister) => {
  return defAbpHttp.post<void>({
    url: Api.RegisterByPhone,
    data: input,
  });
};

export const resetPassword = (input: PhoneResetPassword) => {
  return defAbpHttp.put<void>({
    url: Api.ResetPassword,
    data: input,
  });
};

export const sendPhoneSignCode = (phoneNumber: string) => {
  return defAbpHttp.post<void>({
    url: Api.SendPhoneSignCode,
    data: {
      phoneNumber: phoneNumber,
    },
  });
};

export const sendEmailSignCode = (emailAddress: string) => {
  return defAbpHttp.post<void>({
    url: Api.SendEmailSignCode,
    data: {
      emailAddress: emailAddress,
    },
  });
};

export const sendPhoneRegisterCode = (phoneNumber: string) => {
  return defAbpHttp.post<void>({
    url: Api.SendPhoneRegisterCode,
    data: {
      phoneNumber: phoneNumber,
    },
  });
};

export const sendPhoneResetPasswordCode = (phoneNumber: string) => {
  return defAbpHttp.post<void>({
    url: Api.SendPhoneResetPasswordCode,
    data: {
      phoneNumber: phoneNumber,
    },
  });
};

export const getTwoFactorProviders = (userId: string) => {
  return defAbpHttp.get<ListResultDto<NameValue<String>>>({
    url: format(Api.GetTwoFactorProviders, { userId: userId }),
  });
}
