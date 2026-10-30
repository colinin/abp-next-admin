import type { SupportedLanguagesType } from '@vben/locales';

import { loadLocalesMapFromDir } from '@vben/locales';

const modules = import.meta.glob('./langs/**/*.json');

const localesMap = loadLocalesMapFromDir(
  /\.\/langs\/([^/]+)\/(.*)\.json$/,
  modules,
);

/**
 * Load approval-flow designer localization resources.
 * @param lang Current language.
 * @returns Localization resources.
 */
export function loadApprovalFlowMessages(lang: SupportedLanguagesType) {
  const locales = localesMap[lang]?.();
  return locales;
}
