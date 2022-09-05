<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    @ok="handleSubmit"
    :title="L('Secret:New')"
  >
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { nextTick } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { getSecretFormSchemas } from '../datas/ModalData';

  const emits = defineEmits(['register', 'change']);

  const { createMessage } = useMessage();
  const { L } = useLocalization('AbpIdentityServer');
  const [registerForm, { validate, resetFields }] = useForm({
    labelWidth: 120,
    showActionButtonGroup: false,
    schemas: getSecretFormSchemas(),
  });
  const [registerModal, { closeModal }] = useModalInner(() => {
    nextTick(() => {
      resetFields();
    });
  });

  function handleSubmit() {
    validate().then((input) => {
      createMessage.success(L('Successful'));
      emits('change', input);
      closeModal();
    });
  }
</script>
