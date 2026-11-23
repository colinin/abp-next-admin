<script setup lang="ts">
import type { EditionDto } from '../../types';

import { ref } from 'vue';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { getModalData } from '@abp/core';
import { message } from 'ant-design-vue';

import { useEditionsApi } from '../../api';

const emits = defineEmits<{
  (event: 'change', val: EditionDto): void;
}>();

const edition = ref<EditionDto>();

const { cancel, createApi, getApi, updateApi } = useEditionsApi();

const [Form, formApi] = useVbenForm({
  async handleSubmit(values) {
    await onSubmit(values);
  },
  schema: [
    {
      component: 'Input',
      componentProps: {
        allowClear: true,
        autocomplete: 'off',
      },
      fieldName: 'displayName',
      label: $t('AbpSaas.DisplayName:EditionName'),
      rules: 'required',
    },
  ],
  showDefaultActions: false,
});

const [Modal, modalApi] = useVbenModal<EditionDto>({
  onClosed: cancel,
  async onConfirm() {
    await formApi.validateAndSubmitForm();
  },
  async onOpenChange(isOpen) {
    if (isOpen) {
      await onGet();
    }
  },
  title: $t('AbpSaas.Editions'),
});

async function onGet() {
  const { id } = getModalData(modalApi);
  if (!id) {
    formApi.setValues({});
    edition.value = undefined;
    modalApi.setState({ title: $t('AbpSaas.NewEdition') });
    return;
  }
  try {
    modalApi.setState({ loading: true });
    const editionDto = await getApi(id);
    modalApi.setState({
      title: `${$t('AbpSaas.Editions')} - ${editionDto.displayName}`,
    });
    formApi.setValues(editionDto);
    edition.value = editionDto;
  } finally {
    modalApi.setState({ loading: false });
  }
}

async function onSubmit(values: Record<string, any>) {
  const editionId = edition.value?.id;
  const api = editionId
    ? updateApi(editionId, {
        concurrencyStamp: values.concurrencyStamp,
        displayName: values.displayName,
      })
    : createApi({
        displayName: values.displayName,
      });
  try {
    modalApi.setState({ submitting: true });
    const dto = await api;
    message.success($t('AbpUi.SavedSuccessfully'));
    emits('change', dto);
    modalApi.close();
  } finally {
    modalApi.setState({ submitting: false });
  }
}
</script>

<template>
  <Modal>
    <Form />
  </Modal>
</template>

<style scoped></style>
