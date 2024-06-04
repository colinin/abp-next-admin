<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    @ok="handleSaveChanges"
    :title="formTitle"
    :loading="submiting"
    :width="660"
    :min-height="400"
  >
    <BasicForm
      ref="formElRef"
      :colon="true"
      :schemas="formSchemas"
      :label-width="120"
      :show-action-button-group="false"
      :action-col-options="{
        span: 24,
      }"
    />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { ref, unref } from 'vue';
  import { BasicForm, FormActionType } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useClaimModal } from '../hooks/useClaimModal';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { IdentityClaimType } from '/@/api/identity/claims/model';

  const emits = defineEmits(['change', 'register']);

  const { createMessage } = useMessage();
  const { L } = useLocalization('AbpIdentity');
  const submiting = ref(false);
  const claimRef = ref<Nullable<IdentityClaimType>>(null);
  const formElRef = ref<Nullable<FormActionType>>(null);
  const [registerModal, { closeModal }] = useModalInner((val) => {
    claimRef.value = val;
  });
  const { formTitle, formSchemas, handleSubmit } = useClaimModal({ claimRef, formElRef });

  function handleSaveChanges() {
    const formEl = unref(formElRef);
    formEl?.validate().then(() => {
      submiting.value = true;
      handleSubmit(formEl.getFieldsValue())
        .then(() => {
          createMessage.success(L('Successful'));
          emits('change');
          closeModal();
        })
        .finally(() => {
          submiting.value = false;
        });
    });
  }
</script>
