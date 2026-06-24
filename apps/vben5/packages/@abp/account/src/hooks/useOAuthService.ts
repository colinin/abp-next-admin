import type { SigninRedirectArgs, SignoutRedirectArgs } from 'oidc-client-ts';

import type {
  ImpersonationTokenRequest,
  LinkUserTokenRequest,
  PasswordTokenRequestModel,
  PhoneNumberTokenRequest,
  QrCodeTokenRequest,
} from '../types/token';

import { userManager } from '../utils/auth';

export function useOAuthService() {
  async function login(args?: SigninRedirectArgs) {
    return userManager.signinRedirect(args);
  }

  async function loginByLinkUser(input: LinkUserTokenRequest) {
    return userManager.signinLinkUser(input);
  }

  async function loginByImpersonation(input: ImpersonationTokenRequest) {
    return userManager.signinImpersonation(input);
  }

  async function loginByPassword(input: PasswordTokenRequestModel) {
    return userManager.signinResourceOwnerCredentials(input);
  }

  async function loginBySmsCode(input: PhoneNumberTokenRequest) {
    return userManager.signinSmsCode(input);
  }

  async function loginByQrCode(input: QrCodeTokenRequest) {
    return userManager.signinQrCode(input);
  }

  async function logout(args?: SignoutRedirectArgs) {
    return userManager.signoutRedirect(args);
  }

  async function revokeTokens() {
    return userManager.revokeTokens(['access_token', 'refresh_token']);
  }

  async function refreshToken() {
    return userManager.signinSilent();
  }

  async function getAccessToken() {
    const user = await userManager.getUser();
    return user?.access_token;
  }

  async function isAuthenticated() {
    const user = await userManager.getUser();
    return !!user && !user.expired;
  }

  async function handleCallback() {
    return userManager.signinRedirectCallback();
  }

  async function getUser() {
    return userManager.getUser();
  }

  return {
    login,
    loginByLinkUser,
    loginByPassword,
    loginBySmsCode,
    loginByQrCode,
    logout,
    loginByImpersonation,
    refreshToken,
    revokeTokens,
    getAccessToken,
    isAuthenticated,
    handleCallback,
    getUser,
  };
}
