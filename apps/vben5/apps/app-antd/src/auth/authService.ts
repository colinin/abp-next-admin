import { useAppConfig } from '@vben/hooks';

import { UserManager, WebStorageStateStore } from 'oidc-client-ts';

const { authority, audience, clientId, clientSecret, disablePKCE } =
  useAppConfig(import.meta.env, import.meta.env.PROD);

const userManager = new UserManager({
  authority,
  client_id: clientId,
  client_secret: clientSecret,
  redirect_uri: `${window.location.origin}/signin-callback`,
  response_type: 'code',
  scope: audience,
  post_logout_redirect_uri: `${window.location.origin}/`,
  silent_redirect_uri: `${window.location.origin}/silent-renew.html`,
  automaticSilentRenew: true,
  loadUserInfo: true,
  userStore: new WebStorageStateStore({ store: window.localStorage }),
  disablePKCE,
});

export default {
  async login() {
    return userManager.signinRedirect();
  },

  async logout() {
    return userManager.signoutRedirect();
  },

  async refreshToken() {
    return userManager.signinSilent();
  },

  async getAccessToken() {
    const user = await userManager.getUser();
    return user?.access_token;
  },

  async isAuthenticated() {
    const user = await userManager.getUser();
    return !!user && !user.expired;
  },

  async handleCallback() {
    return userManager.signinRedirectCallback();
  },

  async getUser() {
    return userManager.getUser();
  },
};
