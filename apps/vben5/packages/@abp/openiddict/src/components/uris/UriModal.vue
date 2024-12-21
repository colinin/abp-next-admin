<script setup lang="ts">
import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

defineOptions({
  name: 'UriModal',
});

const emits = defineEmits<{
  (event: 'change', data: string): void;
}>();

const [Form, formApi] = useVbenForm({
  handleSubmit: onSubmit,
  schema: [
    {
      component: 'Input',
      componentProps: {
        autocomplete: 'off',
      },
      fieldName: 'uri',
      label: 'Uri',
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
  title: $t('AbpOpenIddict.Uri:AddNew'),
});
function onSubmit(input: Record<string, any>) {
  emits('change', input.uri);
  modalApi.close();
}
</script>

<template>
  <Modal>
    <Form />
  </Modal>
</template>

<style scoped></style>
