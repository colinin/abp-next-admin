import type { Ref } from 'vue';

import { computed, ref, reactive, unref } from 'vue';
import { cloneDeep } from 'lodash-es';
import { useMessage } from '/@/hooks/web/useMessage';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { useValidation } from '/@/hooks/abp/useValidation';

import { get, create, update } from '/@/api/identity-server/api-resources';
import { ApiResource } from '/@/api/identity-server/api-resources/model';

interface UseModal {
  formElRef: Ref<any>;
  tabActivedKey: Ref<string>;
}

export function useModal({ formElRef, tabActivedKey }: UseModal) {
  const { createMessage } = useMessage();
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

  function fetchResource(resourceId?: string) {
    resourceRef.value = Object.assign({
      secrets: [],
      scopes: [],
      userClaims: [],
      properties: [],
    });
    if (resourceId) {
      get(resourceId).then((res) => {
        resourceRef.value = res;
      });
    }
  }

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
          api.then((res) => {
            createMessage.success(L('Successful'));
            resolve(res);
          }).catch((error) => {
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
    fetchResource,
  };
}
