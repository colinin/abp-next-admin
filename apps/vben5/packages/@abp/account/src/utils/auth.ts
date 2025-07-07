import { useAppConfig } from '@vben/hooks';

import { UserManager, WebStorageStateStore } from 'oidc-client-ts';
import SecureLS from 'secure-ls';

const { authority, audience, clientId, clientSecret, disablePKCE } =
  useAppConfig(import.meta.env, import.meta.env.PROD);

const env = import.meta.env.PROD ? 'prod' : 'dev';
const appVersion = import.meta.env.VITE_APP_VERSION;
const namespace = `${import.meta.env.VITE_APP_NAMESPACE}-${appVersion}-${env}`;

const ls = new SecureLS({
  encodingType: 'aes',
  encryptionSecret: import.meta.env.VITE_APP_STORE_SECURE_KEY,
  isCompression: true,
  // @ts-ignore secure-ls does not have a type definition for this
  metaKey: `${namespace}-secure-oidc`,
});
export const userManager = new UserManager({
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
  userStore: new WebStorageStateStore({
    store: import.meta.env.DEV
      ? localStorage
      : {
          length: ls.storage.length,
          clear: ls.clear,
          setItem(key, value) {
            ls.set(key, value);
          },
          getItem(key) {
            return ls.get(key);
          },
          key(index) {
            const keys = ls.getAllKeys();
            return keys[index] ?? null;
          },
          removeItem(key) {
            ls.remove(key);
          },
        },
  }),
  disablePKCE,
});
