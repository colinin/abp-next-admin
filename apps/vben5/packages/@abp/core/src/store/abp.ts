import type {
  ApplicationConfigurationDto,
  ApplicationLocalizationDto,
} from '../types/dto';

import { ref } from 'vue';

import { acceptHMRUpdate, defineStore } from 'pinia';

export const useAbpStore = defineStore(
  'abp',
  () => {
    const application = ref<ApplicationConfigurationDto>();
    const localization = ref<ApplicationLocalizationDto>();
    /** 获取 i18n 格式本地化文本 */
    function getI18nLocales() {
      const abpLocales: Record<string, any> = {};
      if (!localization.value) {
        return abpLocales;
      }
      const resources = localization.value.resources;
      // AbpValidation.The field {0} is invalid.
      Object.keys(resources).forEach((resource) => {
        // resource --> AbpValidation
        const resourceLocales: Record<string, any> = {};
        const resourcesByName = resources[resource];
        if (resourcesByName) {
          Object.keys(resourcesByName.texts).forEach((key) => {
            // The field {0} is invalid. --> The field {0} is invalid_
            let localeKey = key.replaceAll('.', '_');
            // The field {0} is invalid. --> The field {0} is invalid
            localeKey.endsWith('_') &&
              (localeKey = localeKey.slice(
                0,
                Math.max(0, localeKey.length - 1),
              ));
            // _The field {0} is invalid --> The field {0} is invalid
            localeKey.startsWith('_') &&
              (localeKey = localeKey.slice(0, Math.max(1, localeKey.length)));
            resourceLocales[localeKey] = resourcesByName.texts[key];
          });
          abpLocales[resource] = resourceLocales;
        }
      });
      return abpLocales;
    }
    function setApplication(val: ApplicationConfigurationDto) {
      application.value = val;
    }

    function setLocalization(val: ApplicationLocalizationDto) {
      localization.value = val;
    }

    function $reset() {
      application.value = undefined;
    }

    return {
      $reset,
      application,
      getI18nLocales,
      localization,
      setApplication,
      setLocalization,
    };
  },
  {
    persist: true,
  },
);

// 解决热更新问题
const hot = import.meta.hot;
if (hot) {
  hot.accept(acceptHMRUpdate(useAbpStore, hot));
}
