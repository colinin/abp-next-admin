<script setup lang="ts">
import type { DisplayNameInfo } from './types';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useAbpStore } from '@abp/core';

defineOptions({
  name: 'DisplayNameModal',
});

const emits = defineEmits<{
  (event: 'change', data: DisplayNameInfo): void;
}>();

const { application } = useAbpStore();

const [Form, formApi] = useVbenForm({
  commonConfig: {
    // 所有表单项
    componentProps: {
      class: 'w-full',
    },
  },
  handleSubmit: onSubmit,
  schema: [
    {
      component: 'Select',
      componentProps: {
        autocomplete: 'off',
        fieldNames: {
          label: 'displayName',
          value: 'cultureName',
        },
        options: application?.localization.languages,
      },
      fieldName: 'culture',
      label: $t('AbpOpenIddict.DisplayName:CultureName'),
      rules: 'required',
    },
    {
      component: 'Input',
      componentProps: {
        autocomplete: 'off',
      },
      fieldName: 'displayName',
      label: $t('AbpOpenIddict.DisplayName:DisplayName'),
      rules: 'required',
    },
  ],
  showDefaultActions: false,
});
const [Modal, modalApi] = useVbenModal({
  class: 'w-1/2',
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onConfirm: async () => {
    await formApi.validateAndSubmitForm();
  },
  title: $t('AbpOpenIddict.DisplayName.DisplayNames'),
});
function onSubmit(input: Record<string, any>) {
  emits('change', {
    culture: input.culture,
    displayName: input.displayName,
  });
  modalApi.close();
}
</script>

<template>
  <Modal>
    <Form />
  </Modal>
</template>

<style scoped></style>
