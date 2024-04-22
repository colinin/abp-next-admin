interface Profile extends ExtensibleObject, IHasConcurrencyStamp {
  userName: string;
  email: string;
  name?: string;
  surname?: string;
  phoneNumber?: string;
}

export interface MyProfile extends Profile {
  isExternal: boolean;
  hasPassword: boolean;
}

export type UpdateMyProfile = Profile;

export interface ChangePassword {
  currentPassword: string;
  newPassword: string;
}

export interface ChangePhoneNumber {
  newPhoneNumber: string;
  code: string;
}

export interface TwoFactorEnabled {
  enabled: boolean;
}

export interface SendEmailConfirmCode {
  email: string;
  appName: string;
  returnUrl?: string;
  returnUrlHash?: string;
}

export interface ConfirmEmailInput {
  userId?: string;
  confirmToken: string;
}
