import type { Locale } from 'ant-design-vue/es/locale';

import type { App } from 'vue';

import type { LocaleSetupOptions } from '@vben/locales';

import {
  $t,
  setupI18n as coreSetup,
  createDayjsLocale,
  createMessageLoader,
  createUiLocale,
  defineLocaleConfig,
} from '@vben/locales';
import { vxeLocaleAdapter, vxeLocales } from '@vben/plugins/vxe-table/locales';
import {
  preferences,
  preferencesManager,
  updatePreferences,
} from '@vben/preferences';

import { loadApprovalFlowMessages } from '@abp/approval-flow';
import { loadComponentMessages } from '@abp/components/locales';
import { useAbpStore } from '@abp/core';
import { useLocalizationsApi } from '@abp/localization';
import { loadPaltformMessages } from '@abp/platform';
import dayjs from 'dayjs';

const { adapter: ui, locale: antdLocale } = createUiLocale<Locale>();

const localeConfig = defineLocaleConfig({
  adapters: {
    dayjs: createDayjsLocale(dayjs.locale),
    ui,
    vxe: vxeLocaleAdapter,
  },
  languages: {
    'zh-CN': {
      label: '简体中文',
      resources: {
        dayjs: () => import('dayjs/locale/zh-cn'),
        ui: () => import('ant-design-vue/es/locale/zh_CN'),
        vxe: vxeLocales['zh-CN'],
      },
    },
    'en-US': {
      label: 'English',
      resources: {
        dayjs: () => import('dayjs/locale/en'),
        ui: () => import('ant-design-vue/es/locale/en_US'),
        vxe: vxeLocales['en-US'],
      },
    },
  },
  loadMessages: async (lang) => {
    const defaultLoader = createMessageLoader(
      import.meta.glob('./langs/**/*.json'),
    );
    const [
      appLocaleMessages,
      approvalFlowLocales,
      compLocales,
      platformLocales,
      abpLocales,
    ] = await Promise.all([
      defaultLoader(lang),
      loadApprovalFlowMessages(lang),
      loadComponentMessages(lang),
      loadPaltformMessages(lang),
      loadAbpLocale(lang),
    ]);
    return {
      ...appLocaleMessages,
      ...approvalFlowLocales?.default,
      ...compLocales?.default,
      ...platformLocales?.default,
      ...abpLocales,
    };
  },
});

/**
 * 加载abp的语言包
 * @param lang
 */
async function loadAbpLocale(lang: string) {
  const abpStore = useAbpStore();
  const { getLocalizationApi } = useLocalizationsApi();
  let localization = abpStore.localization;

  if (lang !== localization?.currentCulture.cultureName) {
    localization = await getLocalizationApi({
      cultureName: lang,
      onlyDynamics: false,
    });
  }
  abpStore.setLocalization(localization);
  const locales = abpStore.getI18nLocales();
  return locales;
}

export type AppLocale = keyof typeof localeConfig.languages;

async function setupI18n(app: App, options: LocaleSetupOptions = {}) {
  await coreSetup(app, {
    ...localeConfig,
    defaultLocale: preferencesManager.getInitialPreferences().app.locale,
    initialLocale: preferences.app.locale,
    missingWarn: !import.meta.env.PROD,
    ...options,
    onLocaleChanged(locale) {
      if (preferences.app.locale !== locale) {
        updatePreferences({ app: { locale } });
      }
      return options.onLocaleChanged?.(locale);
    },
  });
}

export { $t, antdLocale, setupI18n };
