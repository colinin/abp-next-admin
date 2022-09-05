<template>
  <BasicModal
    @register="registerModal"
    :title="L('Share')"
    @ok="handleSubmit"
  >
    <BasicForm @register="registerForm" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { nextTick } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { getShareModalSchemas } from './data';
  import { share } from '/@/api/oss-management/private';

  const { L } = useLocalization(['AbpOssManagement', 'AbpUi']);
  const { createMessage } = useMessage();
  const [registerForm, { validate, resetFields, setFieldsValue }] = useForm({
    labelAlign: 'left',
    labelWidth: 120,
    showActionButtonGroup: false,
    schemas: getShareModalSchemas(),
  });
  const [registerModal, { changeOkLoading, closeModal }] = useModalInner((data) => {
    nextTick(() => {
      resetFields();
      setFieldsValue(data);
    });
  });

  function handleSubmit() {
    validate().then((input) => {
      changeOkLoading(true);
      share(input).then(() => {
        createMessage.success(L('Successful'));
        closeModal();
      }).finally(() => {
        changeOkLoading(false);
      });
    });
  }
</script>
