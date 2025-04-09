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

export type { GetTwoFactorProvidersInput, SendEmailSigninCodeDto, SendPhoneSigninCodeDto, TwoFactorProvider };
