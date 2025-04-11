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
  commonConfig: {
    formItemClass: 'w-full',
  },
  handleSubmit: onSubmit,
  schema: [
    {
      component: 'Input',
      componentProps: {
        allowClear: true,
        autocomplete: 'off',
      },
      fieldName: 'key',
      label: $t('component.extra_property_dictionary.key'),
      rules: 'required',
    },
    {
      component: 'Textarea',
      componentProps: {
        allowClear: true,
        autocomplete: 'off',
        autoSize: {
          minRows: 2,
        },
        class: 'w-full',
        showCount: true,
      },
      fieldName: 'value',
      label: $t('component.extra_property_dictionary.value'),
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
  onOpenChange(isOpen) {
    if (isOpen) {
      const input = modalApi.getData();
      if (input) {
        formApi.setValues(input);
      }
    }
  },
  title: $t('component.extra_property_dictionary.title'),
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
