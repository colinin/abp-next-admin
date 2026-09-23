<script setup lang="ts">
import type { WorkspaceDefinitionRecordDto } from '../../types/workspaces';

import { ref } from 'vue';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useMessage } from '@abp/ui';

import { useWorkspaceDefinitionsApi } from '../../api/useWorkspaceDefinitionsApi';

const emit = defineEmits<{
  (event: 'change', data: WorkspaceDefinitionRecordDto): void;
}>();

const message = useMessage();
const { getApi, updateApi } = useWorkspaceDefinitionsApi();

const formModel = ref<WorkspaceDefinitionRecordDto>();

const [Form, formApi] = useVbenForm({
  schema: [
    {
      fieldName: 'apiKey',
      component: 'InputPassword',
      label: $t('AIManagement.DisplayName:ApiKey'),
      rules: 'required',
    },
  ],
  showDefaultActions: false,
  handleSubmit: onSubmit,
});

const [Modal, modalApi] = useVbenModal({
  title: $t('AIManagement.ResetApiKey'),
  async onOpenChange(isOpen) {
    if (isOpen) {
      const { id } = modalApi.getData<WorkspaceDefinitionRecordDto>();
      const dto = await getApi(id);
      formModel.value = dto;
    }
  },
  async onConfirm() {
    await formApi.validateAndSubmitForm();
  },
});

async function onSubmit(values: Record<string, string>) {
  try {
    modalApi.lock();
    const dto = await updateApi(formModel.value!.id, {
      ...formModel.value!,
      apiKey: values.apiKey,
    });
    message.success($t('AbpUi.SavedSuccessfully'));
    emit('change', dto);
    modalApi.close();
  } finally {
    modalApi.unlock();
  }
}
</script>

<template>
  <Modal>
    <Form />
  </Modal>
</template>

<style scoped></style>
