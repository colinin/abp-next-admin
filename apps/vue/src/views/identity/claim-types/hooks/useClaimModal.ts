import type { Ref } from 'vue';

import { computed, unref, watch } from 'vue';
import { IdentityClaimType } from '/@/api/identity/model/claimModel';
import { FormActionType } from '/@/components/Form';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { create, update } from '/@/api/identity/claim';
import { getModalFormSchemas } from '../datas/ModalData';

interface UseClaimModal {
  claimRef: Ref<Nullable<IdentityClaimType>>;
  formElRef: Ref<Nullable<FormActionType>>;
}

export function useClaimModal({ claimRef, formElRef }: UseClaimModal) {
  const { L } = useLocalization('AbpIdentity');
  const formTitle = computed(() => {
    return unref(claimRef)?.id ? L('Edit') : L('IdentityClaim:New');
  });
  const formSchemas = computed(() => {
    return [...getModalFormSchemas()];
  });

  function handleSubmit(input) {
    return input.id ? update(input.id, input) : create(input);
  }

  watch(
    () => unref(claimRef),
    (data) => {
      const formEl = unref(formElRef);
      formEl?.resetFields();
      formEl?.setFieldsValue(data);
    },
    {
      immediate: true,
    },
  );

  return {
    formTitle,
    formSchemas,
    handleSubmit,
  };
}
