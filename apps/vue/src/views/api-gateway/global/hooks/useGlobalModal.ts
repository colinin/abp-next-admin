import { Ref } from 'vue';

import { computed, ref, unref, reactive, watchEffect } from 'vue';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { useValidation } from '/@/hooks/abp/useValidation';
import { getActivedList } from '/@/api/api-gateway/group';
import { getLoadBalancerProviders } from '/@/api/api-gateway/basic';
import { getByAppId, create, update } from '/@/api/api-gateway/global';
import {
  CreateGlobalConfiguration,
  UpdateGlobalConfiguration,
} from '/@/api/api-gateway/model/globalModel';
import { ReturnInnerMethods } from '/@/components/Modal';

interface UseGlobalModal {
  emit: EmitType;
  appIdRef: Ref<string>;
  formElRef: Ref<any>;
  modalMethods: ReturnInnerMethods;
}

export function useGlobalModal({ emit, appIdRef, formElRef, modalMethods }: UseGlobalModal) {
  const { L } = useLocalization('ApiGateway');
  const { ruleCreator } = useValidation();
  const modelRef = ref<Recordable>({});
  const appIdOptions = ref<any>([]);
  const balancerOptions = ref<any>([]);

  const radioOptions = reactive([
    { label: L('DisplayName:Enable'), value: true },
    { label: L('DisplayName:Disable'), value: false },
  ]);

  const discoveryOptions = reactive([
    { label: L('None'), value: '' },
    { label: 'Consul', value: 'Consul' },
    { label: 'Eureka', value: 'Eureka' },
    { label: 'Zookeeper', value: 'Zookeeper' },
  ]);

  const formRules = reactive({
    appId: ruleCreator.fieldRequired({ name: 'AppId', prefix: 'DisplayName' }),
    baseUrl: ruleCreator.fieldRequired({ name: 'BaseUrl', prefix: 'DisplayName' }),
  });

  const formTitle = computed(() => {
    const model = unref(modelRef);
    if (model && model.itemId) {
      return L('Group:EditBy', [model.appId]);
    }
    return L('Group:AddNew');
  });

  watchEffect(() => {
    initModel();
    !unref(appIdRef) && fetchOptions();
    unref(appIdRef) && fetchModel();
  });

  function initModel() {
    // 嵌套对象初始化一下
    modelRef.value = {
      downstreamScheme: 'HTTP',
      qoSOptions: {
        timeoutValue: 10000,
        durationOfBreak: 60000,
        exceptionsAllowedBeforeBreaking: 50,
      },
      rateLimitOptions: {
        httpStatusCode: 429,
        disableRateLimitHeaders: false,
        quotaExceededMessage:
          '{\n  "error": {\n    "code": "429",\n    "message": "您的操作过快,请稍后再试!",\n    "details": "您的操作过快,请稍后再试!",\n    "data": {},\n    "validationErrors": []\n  }\n}',
      },
      httpHandlerOptions: {
        useProxy: false,
        useTracing: false,
        allowAutoRedirect: false,
        useCookieContainer: false,
      },
      loadBalancerOptions: {},
      serviceDiscoveryProvider: {},
    };
  }

  function fetchOptions() {
    getActivedList().then((res) => {
      appIdOptions.value = res.items.map((item) => {
        return {
          label: item.appName,
          value: item.appId,
        };
      });
    });
    getLoadBalancerProviders().then((res) => {
      balancerOptions.value = res.items.map((item) => {
        return {
          label: item.displayName,
          value: item.type,
        };
      });
    });
  }

  function fetchModel() {
    getByAppId(unref(appIdRef)).then((res) => {
      modelRef.value = res;
    });
  }

  function handleCancel() {
    const formEl = unref(formElRef);
    formEl.clearValidate();
  }

  function handleSubmit() {
    const formEl = unref(formElRef);
    formEl.validate().then(() => {
      const model = unref(modelRef);
      const { changeOkLoading, closeModal } = modalMethods;
      changeOkLoading(true);
      const api = model.itemId
        ? update(model as UpdateGlobalConfiguration)
        : create(model as CreateGlobalConfiguration);
      api
        .then((res) => {
          initModel();
          closeModal();
          emit('change', res);
        })
        .finally(() => {
          changeOkLoading(false);
        });
    });
  }

  return {
    modelRef,
    formTitle,
    formRules,
    radioOptions,
    appIdOptions,
    balancerOptions,
    discoveryOptions,
    handleCancel,
    handleSubmit,
  };
}
