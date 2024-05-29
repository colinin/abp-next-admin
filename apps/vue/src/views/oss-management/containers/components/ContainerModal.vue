<template>
  <BasicModal
    @register="registerModal"
    :title="L('Containers')"
    :width="466"
    :min-height="66"
    @ok="handleSubmit"
  >
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { nextTick } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { BasicForm, useForm } from '/@/components/Form';
  import { createContainer } from '/@/api/oss-management/containers';
  import { getModalFormSchemas } from './ModalData';

  const emits = defineEmits(['change', 'register']);

  const { createMessage } = useMessage();
  const { L } = useLocalization(['AbpOssManagement', 'AbpUi']);
  const [registerForm, { validate, resetFields }] = useForm({
    labelWidth: 120,
    schemas: getModalFormSchemas(),
    showActionButtonGroup: false,
    actionColOptions: {
      span: 24,
    },
  });
  const [registerModal, { changeOkLoading, closeModal }] = useModalInner(() => {
    nextTick(() => {
      resetFields();
    });
  });

  function handleSubmit() {
    validate().then((input) => {
      changeOkLoading(true);
      createContainer(input.name)
        .then((res) => {
          createMessage.success(L('Successful'));
          emits('change', res);
          closeModal();
        })
        .finally(() => {
          changeOkLoading(false);
        });
    });
  }
</script>
