import type { NameValue } from '@abp/core';

interface GetTwoFactorProvidersInput {
  userId: string;
}

interface SendEmailSigninCodeDto {
  emailAddress: string;
}

interface SendPhoneSigninCodeDto {
  phoneNumber: string;
}

interface SendPhoneResetPasswordCodeDto {
  phoneNumber: string;
}

interface PhoneResetPasswordDto {
  code: string;
  newPassword: string;
  phoneNumber: string;
}

type TwoFactorProvider = NameValue<string>;

export type {
  GetTwoFactorProvidersInput,
  PhoneResetPasswordDto,
  SendEmailSigninCodeDto,
  SendPhoneResetPasswordCodeDto,
  SendPhoneSigninCodeDto,
  TwoFactorProvider,
};
