<script setup lang="ts">
// oxlint-disable typescript/no-non-null-assertion
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

const [Modal, modalApi] = useVbenModal<WorkspaceDefinitionRecordDto>({
  title: $t('AIManagement.ResetApiKey'),
  async onOpened() {
    formModel.value = undefined;
    const data = modalApi.getData();
    if (data?.id) {
      const dto = await getApi(data.id);
      formModel.value = dto;
    }
  },
  async onConfirm() {
    await formApi.validateAndSubmit();
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
