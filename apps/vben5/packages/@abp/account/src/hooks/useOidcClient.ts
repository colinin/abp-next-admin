import { userManager } from '../utils/auth';

export function useOidcClient() {
  async function login() {
    return userManager.signinRedirect();
  }

  async function logout() {
    return userManager.signoutRedirect();
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
    logout,
    refreshToken,
    getAccessToken,
    isAuthenticated,
    handleCallback,
    getUser,
  };
}
