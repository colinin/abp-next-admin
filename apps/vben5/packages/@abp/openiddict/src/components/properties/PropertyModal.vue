<script setup lang="ts">
import type { PropertyInfo } from './types';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

defineOptions({
  name: 'PropertyModal',
});

const emits = defineEmits<{
  (event: 'change', data: PropertyInfo): void;
}>();

const [Form, formApi] = useVbenForm({
  handleSubmit: onSubmit,
  schema: [
    {
      component: 'Input',
      componentProps: {
        autocomplete: 'off',
      },
      fieldName: 'key',
      label: $t('AbpOpenIddict.Propertites:Key'),
      rules: 'required',
    },
    {
      component: 'Input',
      componentProps: {
        autocomplete: 'off',
      },
      fieldName: 'value',
      label: $t('AbpOpenIddict.Propertites:Value'),
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
  title: $t('AbpOpenIddict.Propertites'),
});
function onSubmit(input: Record<string, any>) {
  emits('change', {
    key: input.key,
    value: input.value,
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
