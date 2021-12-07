import type { Ref } from 'vue';

import { computed, ref, reactive, unref, watch } from 'vue';
import { message } from 'ant-design-vue';
import { cloneDeep } from 'lodash-es';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { useValidation } from '/@/hooks/abp/useValidation';

import { get, create, update } from '/@/api/identity-server/apiResources';
import { ApiResource } from '/@/api/identity-server/model/apiResourcesModel';

interface UseModal {
  resourceIdRef: Ref<string>;
  formElRef: Ref<any>;
  tabActivedKey: Ref<string>;
}

export function useModal({ resourceIdRef, formElRef, tabActivedKey }: UseModal) {
  const { L } = useLocalization('AbpIdentityServer');
  const { ruleCreator } = useValidation();
  const resourceRef = ref<ApiResource>({} as ApiResource);

  const isEdit = computed(() => {
    if (unref(resourceRef)?.id) {
      return true;
    }
    return false;
  });
  const formTitle = computed(() => {
    return isEdit.value
      ? L('Resource:Name', [
          unref(resourceRef)?.displayName ?? unref(resourceRef)?.name,
        ] as Recordable)
      : L('Resource:New');
  });
  const formRules = reactive({
    name: ruleCreator.fieldRequired({
      name: 'Name',
      resourceName: 'AbpIdentityServer',
      trigger: 'blur',
    }),
  });

  watch(
    () => unref(resourceIdRef),
    (id) => {
      unref(formElRef)?.resetFields();
      if (id) {
        get(id).then((res) => {
          resourceRef.value = res;
        });
      } else {
        resourceRef.value = Object.assign({
          secrets: [],
          scopes: [],
          userClaims: [],
          properties: [],
        });
      }
    },
  );

  function handleChangeTab(activeKey) {
    tabActivedKey.value = activeKey;
  }

  function handleVisibleModal(visible: boolean) {
    if (!visible) {
      tabActivedKey.value = 'basic';
    }
  }

  function handleSubmit() {
    return new Promise<any>((resolve, reject) => {
      const formEl = unref(formElRef);
      formEl
        .validate()
        .then(() => {
          const input = cloneDeep(unref(resourceRef));
          const api = isEdit.value
            ? update(input.id, Object.assign(input))
            : create(Object.assign(input));
          api
            .then((res) => {
              message.success(L('Successful'));
              resolve(res);
            })
            .catch((error) => {
              reject(error);
            });
        })
        .catch((error) => {
          reject(error);
        });
    });
  }

  return {
    isEdit,
    resourceRef,
    formRules,
    formTitle,
    handleChangeTab,
    handleVisibleModal,
    handleSubmit,
  };
}
