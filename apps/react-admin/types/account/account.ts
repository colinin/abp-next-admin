import type { NameValue } from "#/abp-core";

interface GetTwoFactorProvidersInput {
	userId: string;
}

interface SendEmailSigninCodeDto {
	emailAddress: string;
}

interface SendPhoneSigninCodeDto {
	phoneNumber: string;
}

type TwoFactorProvider = NameValue<string>;

interface ExternalSignUpApiDto {
	userName: string;
	emailAddress: string;
}

interface PhoneResetPasswordDto {
	code: string;
	newPassword: string;
	phoneNumber: string;
}

export type {
	GetTwoFactorProvidersInput,
	SendEmailSigninCodeDto,
	SendPhoneSigninCodeDto,
	PhoneResetPasswordDto,
	TwoFactorProvider,
	ExternalSignUpApiDto,
};
