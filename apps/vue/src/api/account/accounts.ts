import { defAbpHttp } from '/@/utils/http/abp';
import { Register, PhoneRegister, PhoneResetPassword } from './model/accountsModel';
import { User } from '/@/api/identity/model/userModel';

enum Api {
  Register = '/api/account/register',
  RegisterByPhone = '/api/account/phone/register',
  ResetPassword = '/api/account/phone/reset-password',
  SendPhoneSignCode = '/api/account/phone/send-signin-code',
  SendPhoneRegisterCode = '/api/account/phone/send-register-code',
  SendPhoneResetPasswordCode = '/api/account/phone/send-password-reset-code',
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
