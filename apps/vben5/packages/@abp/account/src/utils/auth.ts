import type { Logger, UserManagerSettings } from 'oidc-client-ts';

import type {
  PasswordTokenRequestModel,
  PhoneNumberTokenRequest,
  QrCodeTokenRequest,
} from '../types/token';

import { useAppConfig } from '@vben/hooks';

import { useRequest } from '@abp/request';
import {
  SigninResponse,
  UserManager,
  WebStorageStateStore,
} from 'oidc-client-ts';
import SecureLS from 'secure-ls';

class AbpUserManager extends UserManager {
  async _fetchUser(logger: Logger, body: URLSearchParams) {
    const { request } = useRequest();
    const url = await this.metadataService.getTokenEndpoint(false);
    if (!this.settings.omitScopeWhenRequesting) {
      body.set('scope', this.settings.scope);
    }
    const resp = await request(url, {
      data: body,
      method: 'POST',
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
      },
    });
    logger.debug('got signin response');
    const response = new SigninResponse(new URLSearchParams());
    Object.assign(response, resp);
    const user = await this._buildUser(response);
    if (user.profile && user.profile.sub) {
      logger.info('success, signed in subject', user.profile.sub);
    } else {
      logger.info('no subject');
    }
    return user;
  }

  _writeChangePasswordToken(
    params: URLSearchParams,
    model: Record<string, any>,
  ) {
    if (model.ChangePasswordToken) {
      params.set('ChangePasswordToken', model.ChangePasswordToken);
    }
    if (model.NewPassword) {
      params.set('NewPassword', model.NewPassword);
    }
  }
  _writeTenantId(params: URLSearchParams, model: Record<string, any>) {
    if (model.tenantId) {
      params.set('tenantId', model.tenantId);
    }
  }
  _writeTwoFactorToken(params: URLSearchParams, model: Record<string, any>) {
    if (model.TwoFactorProvider) {
      params.set('TwoFactorProvider', model.TwoFactorProvider);
    }
    if (model.TwoFactorCode) {
      params.set('TwoFactorCode', model.TwoFactorCode);
    }
  }
  _writeUserId(params: URLSearchParams, model: Record<string, any>) {
    if (model.userId) {
      params.set('userId', model.userId);
    }
  }
  async signinQrCode(params: QrCodeTokenRequest) {
    const logger = this._logger.create('signinQrCode');
    const client_secret = this.settings.client_secret;
    if (!client_secret) {
      logger.error('A client_id is required');
      throw new Error('A client_id is required');
    }
    const body = new URLSearchParams({
      key: params.key,
      grant_type: 'qr_code',
      client_id: this.settings.client_id,
      client_secret,
    });
    this._writeUserId(body, params);
    this._writeTenantId(body, params);
    this._writeTwoFactorToken(body, params);
    return await this._fetchUser(logger, body);
  }

  override async signinResourceOwnerCredentials(
    params: PasswordTokenRequestModel,
  ) {
    const logger = this._logger.create('signinResourceOwnerCredentials');
    const client_secret = this.settings.client_secret;
    if (!client_secret) {
      logger.error('A client_id is required');
      throw new Error('A client_id is required');
    }
    const body = new URLSearchParams({
      username: params.username,
      password: params.password,
      grant_type: 'password',
      client_id: this.settings.client_id,
      client_secret,
    });
    this._writeUserId(body, params);
    this._writeTwoFactorToken(body, params);
    this._writeChangePasswordToken(body, params);
    return await this._fetchUser(logger, body);
  }

  async signinSmsCode(params: PhoneNumberTokenRequest) {
    const logger = this._logger.create('signinSmsCode');
    const client_secret = this.settings.client_secret;
    if (!client_secret) {
      logger.error('A client_id is required');
      throw new Error('A client_id is required');
    }
    const body = new URLSearchParams({
      phone_number: params.phoneNumber,
      phone_verify_code: params.code,
      grant_type: 'phone_verify',
      client_id: this.settings.client_id,
      client_secret,
    });
    this._writeUserId(body, params);
    this._writeTwoFactorToken(body, params);
    return await this._fetchUser(logger, body);
  }
}

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
const oidcSettings: UserManagerSettings = {
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
  prompt: 'select_account',
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
};
const userManager = new AbpUserManager(oidcSettings);

export { oidcSettings, userManager };
