import type {
  ApplicationConfig,
  VbenAdminProAppConfigRaw,
} from '@vben/types/global';

/**
 * 由 vite-inject-app-config 注入的全局配置
 */
export function useAppConfig(
  env: Record<string, any>,
  isProduction: boolean,
): ApplicationConfig {
  // 生产环境下，直接使用 window._VBEN_ADMIN_PRO_APP_CONF_ 全局变量
  const config = isProduction
    ? window._VBEN_ADMIN_PRO_APP_CONF_
    : (env as VbenAdminProAppConfigRaw);

  const {
    VITE_GLOB_API_URL,
    VITE_GLOB_AUTH_CLIENT_ID,
    VITE_GLOB_AUTH_CLIENT_SECRET,
    VITE_GLOB_AUTH_AUTHORITY,
    VITE_GLOB_AUTH_AUDIENCE,
    VITE_GLOB_AUTH_ONLY_OIDC,
    VITE_GLOB_AUTH_ONLY_OIDC_HINT,
    VITE_GLOB_AUTH_DISABLE_PKCE,
    VITE_GLOB_AUTH_PROMPT,
    VITE_GLOB_AUTH_DINGDING_CORP_ID,
    VITE_GLOB_AUTH_DINGDING_CLIENT_ID,
    VITE_GLOB_UI_FRAMEWORK,
  } = config;

  const applicationConfig: ApplicationConfig = {
    apiURL: VITE_GLOB_API_URL,
    auth: {
      clientId: VITE_GLOB_AUTH_CLIENT_ID,
      clientSecret: VITE_GLOB_AUTH_CLIENT_SECRET,
      authority: VITE_GLOB_AUTH_AUTHORITY,
      audience: VITE_GLOB_AUTH_AUDIENCE,
      onlyOidc: VITE_GLOB_AUTH_ONLY_OIDC === 'true',
      onlyOidcHint: VITE_GLOB_AUTH_ONLY_OIDC_HINT === 'true',
      prompt: VITE_GLOB_AUTH_PROMPT,
      disablePKCE: VITE_GLOB_AUTH_DISABLE_PKCE === 'true',
    },
    uiFramework: VITE_GLOB_UI_FRAMEWORK,
  };
  if (VITE_GLOB_AUTH_DINGDING_CORP_ID && VITE_GLOB_AUTH_DINGDING_CLIENT_ID) {
    applicationConfig.auth.dingding = {
      clientId: VITE_GLOB_AUTH_DINGDING_CLIENT_ID,
      corpId: VITE_GLOB_AUTH_DINGDING_CORP_ID,
    };
  }

  return applicationConfig;
}
