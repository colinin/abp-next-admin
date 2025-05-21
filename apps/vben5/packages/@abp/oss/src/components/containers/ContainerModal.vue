<script setup lang="ts">
import type { OssContainerDto } from '../../types';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { message } from 'ant-design-vue';

import { useContainesApi } from '../../api';

const emits = defineEmits<{
  (event: 'change', data: OssContainerDto): void;
}>();

const { cancel, createApi } = useContainesApi();

const [Form, formApi] = useVbenForm({
  handleSubmit: onSubmit,
  schema: [
    {
      component: 'Input',
      fieldName: 'name',
      label: $t('AbpOssManagement.DisplayName:Name'),
      rules: 'required',
    },
  ],
  showDefaultActions: false,
});
const [Modal, modalApi] = useVbenModal({
  onCancel: cancel,
  onConfirm: async () => {
    await formApi.validateAndSubmitForm();
  },
});

async function onSubmit(values: Record<string, any>) {
  try {
    modalApi.setState({ submitting: true });
    const dto = await createApi(values.name);
    message.success($t('AbpUi.SavedSuccessfully'));
    emits('change', dto);
    modalApi.close();
  } finally {
    modalApi.setState({ submitting: false });
  }
}
</script>

<template>
  <Modal :title="$t('AbpOssManagement.Containers:Create')">
    <Form />
  </Modal>
</template>

<style scoped></style>
