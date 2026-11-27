<script setup lang="ts">
import type { BlobDto } from '../../types/blobs';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { getModalData, useNotifications } from '@abp/core';

import { useBlobsApi } from '../../api/useBlobsApi';
const emits = defineEmits<{
  (event: 'change', data: BlobDto): void;
}>();

const { toastSuccess } = useNotifications();

interface ModalState {
  containerId: string;
  parentId?: string;
}

const { createFolderApi } = useBlobsApi();

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

const [Modal, modalApi] = useVbenModal<ModalState>({
  onConfirm: async () => {
    await formApi.validateAndSubmit();
  },
});

async function onSubmit(values: Record<string, any>) {
  try {
    const state = getModalData(modalApi);
    modalApi.setState({ submitting: true });
    const dto = await createFolderApi({
      containerId: state.containerId,
      parentId: state.parentId,
      name: values.name,
    });
    toastSuccess($t('AbpUi.SavedSuccessfully'));
    emits('change', dto);
    modalApi.close();
  } finally {
    modalApi.setState({ submitting: false });
  }
}
</script>

<template>
  <Modal :title="$t('BlobManagement.Blobs:CreateFolder')">
    <Form />
  </Modal>
</template>

<style scoped></style>
