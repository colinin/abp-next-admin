import type { RouteMeta as IRouteMeta } from '@vben-core/typings';

import 'vue-router';

declare module 'vue-router' {
  // eslint-disable-next-line @typescript-eslint/no-empty-object-type
  interface RouteMeta extends IRouteMeta {}
}

export interface VbenAdminProAppConfigRaw {
  VITE_GLOB_API_URL: string;
  VITE_GLOB_AUTH_CLIENT_ID: string;
  VITE_GLOB_AUTH_CLIENT_SECRET: string;
  VITE_GLOB_AUTH_AUTHORITY: string;
  VITE_GLOB_AUTH_AUDIENCE?: string;
  VITE_GLOB_AUTH_ONLY_OIDC?: string;
  VITE_GLOB_AUTH_ONLY_OIDC_HINT?: string;
  VITE_GLOB_AUTH_DISABLE_PKCE?: string;
  VITE_GLOB_AUTH_DINGDING_CLIENT_ID: string;
  VITE_GLOB_AUTH_DINGDING_CORP_ID: string;
  VITE_GLOB_UI_FRAMEWORK: string;
}

interface AuthConfig {
  dingding?: {
    clientId: string;
    corpId: string;
  };
  authority: string;
  audience?: string;
  clientId: string;
  clientSecret: string;
  onlyOidc?: boolean;
  onlyOidcHint?: boolean;
  disablePKCE?: boolean;
}

export interface ApplicationConfig {
  apiURL: string;
  auth: AuthConfig;
  uiFramework: string;
}

declare global {
  interface Window {
    _VBEN_ADMIN_PRO_APP_CONF_: VbenAdminProAppConfigRaw;
  }
}
