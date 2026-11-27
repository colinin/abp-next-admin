<script setup lang="ts">
import type { BlobContainerDto } from '../../types/containers';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useNotifications } from '@abp/core';

import { useBlobContainersApi } from '../../api/useBlobContainersApi';
defineOptions({
  name: 'BlobContainerModal',
});

const emits = defineEmits<{
  (event: 'change', data: BlobContainerDto): void;
}>();

const { toastSuccess } = useNotifications();

const { cancel, createApi } = useBlobContainersApi();

const [Form, formApi] = useVbenForm({
  handleSubmit: onSubmit,
  schema: [
    {
      component: 'Input',
      fieldName: 'name',
      label: $t('BlobManagement.DisplayName:Name'),
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
    const dto = await createApi({ name: values.name });
    toastSuccess($t('AbpUi.SavedSuccessfully'));
    emits('change', dto);
    modalApi.close();
  } finally {
    modalApi.setState({ submitting: false });
  }
}
</script>

<template>
  <Modal :title="$t('BlobManagement.BlobContainers:Create')">
    <Form />
  </Modal>
</template>

<style scoped></style>
