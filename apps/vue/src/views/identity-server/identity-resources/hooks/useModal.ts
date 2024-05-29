import type { Ref } from 'vue';

import { computed, ref, reactive, unref, watch } from 'vue';
import { cloneDeep } from 'lodash-es';
import { useMessage } from '/@/hooks/web/useMessage';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { useValidation } from '/@/hooks/abp/useValidation';

import { get, create, update } from '/@/api/identity-server/identity-resources';
import { IdentityResource } from '/@/api/identity-server/identity-resources/model';

interface UseModal {
  modelIdRef: Ref<string>;
  formElRef: Ref<any>;
  tabActivedKey: Ref<string>;
}

export function useModal({ modelIdRef, formElRef, tabActivedKey }: UseModal) {
  const { createMessage } = useMessage();
  const { L } = useLocalization('AbpIdentityServer');
  const { ruleCreator } = useValidation();
  const modelRef = ref<IdentityResource>({} as IdentityResource);

  const isEdit = computed(() => {
    if (unref(modelRef)?.id) {
      return true;
    }
    return false;
  });
  const formTitle = computed(() => {
    return isEdit.value
      ? L('Resource:Name', [unref(modelRef)?.displayName ?? unref(modelRef)?.name] as Recordable)
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
    () => unref(modelIdRef),
    (id) => {
      unref(formElRef)?.resetFields();
      if (id) {
        get(id).then((res) => {
          modelRef.value = res;
        });
      } else {
        modelRef.value = Object.assign({
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
      formEl.validate().then(() => {
        const input = cloneDeep(unref(modelRef));
        const api = isEdit.value
          ? update(input.id, Object.assign(input))
          : create(Object.assign(input));
        api.then((res) => {
          createMessage.success(L('Successful'));
          resolve(res);
        }).catch((error) => {
          reject(error);
        });
      }).catch((error) => {
        reject(error);
      });
    });
  }

  return {
    isEdit,
    modelRef,
    formRules,
    formTitle,
    handleChangeTab,
    handleVisibleModal,
    handleSubmit,
  };
}
