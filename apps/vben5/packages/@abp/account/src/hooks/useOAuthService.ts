import type {
  PasswordTokenRequestModel,
  PhoneNumberTokenRequest,
  QrCodeTokenRequest,
} from '../types/token';

import { userManager } from '../utils/auth';

export function useOAuthService() {
  async function login() {
    return userManager.signinRedirect();
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

  async function logout() {
    return userManager.signoutRedirect();
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
    loginByPassword,
    loginBySmsCode,
    loginByQrCode,
    logout,
    refreshToken,
    revokeTokens,
    getAccessToken,
    isAuthenticated,
    handleCallback,
    getUser,
  };
}
