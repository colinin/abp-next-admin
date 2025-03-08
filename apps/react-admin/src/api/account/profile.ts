import type {
	ProfileDto,
	UpdateProfileDto,
	ChangePasswordInput,
	TwoFactorEnabledDto,
	AuthenticatorDto,
	VerifyAuthenticatorCodeInput,
	AuthenticatorRecoveryCodeDto,
	SendEmailConfirmCodeDto,
	ConfirmEmailInput,
} from "#/account/profile";
import requestClient from "@/api/request";

/**
 * Get profile information
 */
export const getApi = () => requestClient.get<ProfileDto>("/api/account/my-profile");

/**
 * Update profile information
 */
export const updateApi = (input: UpdateProfileDto) => requestClient.put<ProfileDto>("/api/account/my-profile", input);

/**
 * Change password
 */
export const changePasswordApi = (input: ChangePasswordInput) =>
	requestClient.post("/api/account/my-profile/change-password", input);

/**
 * Get two-factor authentication status
 */
export const getTwoFactorEnabledApi = () =>
	requestClient.get<TwoFactorEnabledDto>("/api/account/my-profile/two-factor");

/**
 * Set two-factor authentication status
 */
export const changeTwoFactorEnabledApi = (input: TwoFactorEnabledDto) =>
	requestClient.put("/api/account/my-profile/change-two-factor", input);

/**
 * Get authenticator configuration
 */
export const getAuthenticatorApi = () => requestClient.get<AuthenticatorDto>("/api/account/my-profile/authenticator");

/**
 * Verify authenticator code
 */
export const verifyAuthenticatorCodeApi = (input: VerifyAuthenticatorCodeInput) =>
	requestClient.post<AuthenticatorRecoveryCodeDto>("/api/account/my-profile/verify-authenticator-code", input);

/**
 * Reset authenticator
 */
export const resetAuthenticatorApi = () => requestClient.post("/api/account/my-profile/reset-authenticator");

/**
 * Send email confirmation link
 */
export const sendEmailConfirmLinkApi = (input: SendEmailConfirmCodeDto) =>
	requestClient.post("/api/account/my-profile/send-email-confirm-link", input);

/**
 * Confirm email
 */
export const confirmEmailApi = (input: ConfirmEmailInput) =>
	requestClient.put("/api/account/my-profile/confirm-email", input);
