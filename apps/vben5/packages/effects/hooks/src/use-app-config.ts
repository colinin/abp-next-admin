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
    VITE_GLOB_AUTHORITY,
    VITE_GLOB_AUDIENCE,
    VITE_GLOB_CLIENT_ID,
    VITE_GLOB_CLIENT_SECRET,
    VITE_GLOB_ONLY_OIDC,
    VITE_GLOB_UI_FRAMEWORK,
  } = config;

  return {
    apiURL: VITE_GLOB_API_URL,
    authority: VITE_GLOB_AUTHORITY,
    audience: VITE_GLOB_AUDIENCE,
    clientId: VITE_GLOB_CLIENT_ID,
    clientSecret: VITE_GLOB_CLIENT_SECRET,
    onlyOidc: VITE_GLOB_ONLY_OIDC === 'true',
    uiFramework: VITE_GLOB_UI_FRAMEWORK,
  };
}
