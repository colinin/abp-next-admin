import type { ListResultDto } from "#/abp-core";
import type {
	GetTwoFactorProvidersInput,
	TwoFactorProvider,
	SendEmailSigninCodeDto,
	SendPhoneSigninCodeDto,
	ExternalSignUpApiDto,
	PhoneResetPasswordDto,
} from "#/account/account";
import requestClient from "@/api/request";

/**
 * Get available two-factor authentication providers
 */
export const getTwoFactorProvidersApi = (input: GetTwoFactorProvidersInput) =>
	requestClient.get<ListResultDto<TwoFactorProvider>>("/api/account/two-factor-providers", {
		params: input,
	});

/**
 * Send sign-in verification email
 */
export const sendEmailSigninCodeApi = (input: SendEmailSigninCodeDto) =>
	requestClient.post("/api/account/email/send-signin-code", input);

/**
 * Send sign-in verification SMS
 */
export const sendPhoneSigninCodeApi = (input: SendPhoneSigninCodeDto) =>
	requestClient.post("/api/account/phone/send-signin-code", input);

export const externalSignUpApi = (input: ExternalSignUpApiDto) =>
	requestClient.post("/api/account/external/register", input, { withCredentials: true });

export const resetPasswordApi = (input: PhoneResetPasswordDto) =>
	requestClient.put("/api/account/phone/reset-password", input);
