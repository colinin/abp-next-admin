<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    @ok="handleSaveChanges"
    :title="formTitle"
    :loading="submiting"
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
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicForm, FormActionType } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useRoleModal } from '../hooks/useRoleModal';
  import { Role } from '/@/api/identity/roles/model';

  const emits = defineEmits(['change', 'register']);

  const submiting = ref(false);
  const roleRef = ref<Nullable<Role>>(null);
  const formElRef = ref<Nullable<FormActionType>>(null);
  const { createMessage } = useMessage();
  const { L } = useLocalization('AbpIdentity');
  const [registerModal, { closeModal }] = useModalInner((val) => {
    roleRef.value = val;
  });
  const { formTitle, formSchemas, handleSubmit } = useRoleModal({ roleRef, formElRef });

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
