<script setup lang="ts">
import type { BlobDto } from '../../types/blobs';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { message } from 'ant-design-vue';

import { useBlobsApi } from '../../api/useBlobsApi';

interface ModalState {
  containerId: string;
  parentId?: string;
}

const emits = defineEmits<{
  (event: 'change', data: BlobDto): void;
}>();

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

const [Modal, modalApi] = useVbenModal({
  onConfirm: async () => {
    await formApi.validateAndSubmitForm();
  },
});

async function onSubmit(values: Record<string, any>) {
  try {
    const state = modalApi.getData<ModalState>();
    modalApi.setState({ submitting: true });
    const dto = await createFolderApi({
      containerId: state.containerId,
      parentId: state.parentId,
      name: values.name,
    });
    message.success($t('AbpUi.SavedSuccessfully'));
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
