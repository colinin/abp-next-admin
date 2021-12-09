export interface Register {
  userName: string;
  emailAddress: string;
  password: string;
  appName: string;
}

export interface PhoneRegister {
  phoneNumber: string;
  password: string;
  code: string;
  name?: string;
  userName?: string;
  emailAddress?: string;
}

export interface PhoneResetPassword {
  phoneNumber: string;
  newPassword: string;
  code: string;
}

export interface SendPhoneSecurityCode {
  phoneNumber: string;
}
