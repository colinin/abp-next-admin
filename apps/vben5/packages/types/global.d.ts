import type { RouteMeta as IRouteMeta } from '@vben-core/typings';

import 'vue-router';

declare module 'vue-router' {
  // eslint-disable-next-line @typescript-eslint/no-empty-object-type
  interface RouteMeta extends IRouteMeta {}
}

export interface VbenAdminProAppConfigRaw {
  VITE_GLOB_API_URL: string;
  VITE_GLOB_CLIENT_ID: string;
  VITE_GLOB_CLIENT_SECRET: string;
  VITE_GLOB_AUTHORITY: string;
  VITE_GLOB_AUDIENCE?: string;
  VITE_GLOB_ONLY_OIDC?: string;
  VITE_GLOB_DISABLE_PKCE?: string;
  VITE_GLOB_UI_FRAMEWORK: string;
}

export interface ApplicationConfig {
  apiURL: string;
  authority: string;
  audience?: string;
  clientId: string;
  clientSecret: string;
  onlyOidc?: boolean;
  disablePKCE?: boolean;
  uiFramework: string;
}

declare global {
  interface Window {
    _VBEN_ADMIN_PRO_APP_CONF_: VbenAdminProAppConfigRaw;
  }
}
