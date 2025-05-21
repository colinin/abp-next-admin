<script setup lang="ts">
import type { OssObjectDto } from '../../types/objects';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { message } from 'ant-design-vue';

import { useObjectsApi } from '../../api';

interface ModalState {
  bucket: string;
  path?: string;
}

const emits = defineEmits<{
  (event: 'change', data: OssObjectDto): void;
}>();

const { createApi } = useObjectsApi();

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
  onConfirm: async () => {
    await formApi.validateAndSubmitForm();
  },
});

async function onSubmit(values: Record<string, any>) {
  try {
    const state = modalApi.getData<ModalState>();
    modalApi.setState({ submitting: true });
    const dto = await createApi({
      bucket: state.bucket,
      fileName: values.name,
      overwrite: false,
      path: state.path,
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
  <Modal :title="$t('AbpOssManagement.Objects:CreateFolder')">
    <Form />
  </Modal>
</template>

<style scoped></style>
