import { defAbpHttp } from '/@/utils/http/abp';
import {
  MyProfile,
  UpdateMyProfile,
  ChangePassword,
  ChangePhoneNumber,
  TwoFactorEnabled,
  SendEmailConfirmCode,
  ConfirmEmailInput,
} from './model/profilesModel';

enum Api {
  Get = '/api/account/my-profile',
  Update = '/api/account/my-profile',
  ChangePassword = '/api/account/my-profile/change-password',
  SendChangePhoneNumberCode = '/api/account/my-profile/send-phone-number-change-code',
  ChangePhoneNumber = '/api/account/my-profile/change-phone-number',
  GetTwoFactorEnabled = '/api/account/my-profile/two-factor',
  ChangeTwoFactorEnabled = '/api/account/my-profile/change-two-factor',
  SendEmailConfirmLink = '/api/account/my-profile/send-email-confirm-link',
  ConfirmEmail = '/api/account/my-profile/confirm-email',
}

export const get = () => {
  return defAbpHttp.get<MyProfile>({
    url: Api.Get,
  });
};

export const update = (input: UpdateMyProfile) => {
  return defAbpHttp.put<MyProfile>({
    url: Api.Update,
    data: input,
  });
};

export const changePassword = (input: ChangePassword) => {
  return defAbpHttp.post<void>({
    url: Api.ChangePassword,
    data: input,
  });
};

export const sendEmailConfirmLink = (input: SendEmailConfirmCode) => {
  return defAbpHttp.post<void>({
    url: Api.SendEmailConfirmLink,
    data: input,
  });
};

export const confirmEmail = (input: ConfirmEmailInput) => {
  return defAbpHttp.put<void>({
    url: Api.ConfirmEmail,
    data: input,
  });
};

export const sendChangePhoneNumberCode = (phoneNumber: string) => {
  return defAbpHttp.post<void>({
    url: Api.SendChangePhoneNumberCode,
    data: {
      newPhoneNumber: phoneNumber,
    },
  });
};

export const changePhoneNumber = (input: ChangePhoneNumber) => {
  return defAbpHttp.put<void>({
    url: Api.ChangePhoneNumber,
    data: input,
  });
};

export const getTwoFactorEnabled = () => {
  return defAbpHttp.get<TwoFactorEnabled>({
    url: Api.GetTwoFactorEnabled,
  });
};

export const changeTwoFactorEnabled = (enabled: boolean) => {
  return defAbpHttp.put<void>({
    url: Api.ChangeTwoFactorEnabled,
    data: {
      enabled: enabled,
    },
  });
};
