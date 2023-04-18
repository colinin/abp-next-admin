import { defineStore } from 'pinia';
import { store } from '/@/store';

import { ABP_APP_KEY, ABP_API_KEY } from '/@/enums/cacheEnum';
import { createLocalStorage } from '/@/utils/cache';

import { i18n } from '/@/locales/setupI18n';

import { GetAsyncByModel as getApiDefinition } from '/@/api/abp/api-definition';
import { ApplicationApiDescriptionModel } from '/@/api/abp/api-definition/model';
import { GetAsyncByOptions as getApplicationConfiguration } from '/@/api/abp/application-configuration';

const ls = createLocalStorage();
const defaultApp = {};
const defaultApi = new ApplicationApiDescriptionModel();

const lsApplication = (ls.get(ABP_APP_KEY) || defaultApp) as ApplicationConfigurationDto;
const lsApiDefinition = (ls.get(ABP_API_KEY) || defaultApi) as ApplicationApiDescriptionModel;

interface AbpState {
  application: ApplicationConfigurationDto;
  apidefinition: ApplicationApiDescriptionModel;
}

export const useAbpStore = defineStore({
  id: 'abp',
  state: (): AbpState => ({
    application: lsApplication,
    apidefinition: lsApiDefinition,
  }),
  getters: {
    getApplication(state) {
      return state.application ?? defaultApp;
    },
    getApiDefinition(state) {
      return state.apidefinition ?? defaultApi;
    },
  },
  actions: {
    resetSession() {
      // 清除与用户相关的信息
      this.application.auth = {} as ApplicationAuthConfigurationDto;
      this.application.currentUser = {} as CurrentUser;
    },
    setApplication(application: ApplicationConfigurationDto) {
      this.application = application;
      ls.set(ABP_APP_KEY, application);
    },
    setApiDefinition(apidefinition: ApplicationApiDescriptionModel) {
      this.apidefinition = apidefinition;
      ls.set(ABP_API_KEY, apidefinition);
    },
    mergeLocaleMessage(localization: ApplicationLocalizationConfigurationDto) {
      if (localization.languagesMap['vben-admin-ui']) {
        const transferCulture = localization.languagesMap['vben-admin-ui'].filter(
          (x) => x.value === localization.currentCulture.cultureName,
        );
        function transformAbpLocaleMessageDicToI18n(abpLocaleMessageDic) {
          const i18nLocaleMessageDic = {};
          Object.keys(abpLocaleMessageDic).forEach((vKey) => {
            i18nLocaleMessageDic[vKey] = {};
            Object.keys(abpLocaleMessageDic[vKey]).forEach((mKey) => {
              let msgKey = mKey;
              // 处理最后一个字符以适配 i18n
              if (msgKey.endsWith('.')) {
                msgKey = msgKey.substring(0, msgKey.length - 1);
              }
              i18nLocaleMessageDic[vKey][msgKey] = abpLocaleMessageDic[vKey][mKey];
            });
          });
          return i18nLocaleMessageDic;
        }
        if (transferCulture && transferCulture.length > 0) {
          i18n.global.mergeLocaleMessage(
            transferCulture[0].name,
            transformAbpLocaleMessageDicToI18n(localization.values),
          );
        } else {
          i18n.global.mergeLocaleMessage(
            localization.currentCulture.cultureName,
            transformAbpLocaleMessageDicToI18n(localization.values),
          );
        }
      }
    },
    async initlizeAbpApplication() {
      const application = await getApplicationConfiguration();
      this.setApplication(application);

      const { localization } = application;
      this.mergeLocaleMessage(localization);
    },
    async initlizaAbpApiDefinition() {
      const apidefinition = await getApiDefinition();
      this.setApiDefinition(apidefinition);
    },
  },
});

export function useAbpStoreWithOut() {
  return useAbpStore(store);
}
