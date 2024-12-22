<script setup lang="ts">
import type { OpenIddictApplicationDto } from '../../types';

import { ref } from 'vue';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { message } from 'ant-design-vue';

import { getApi, updateApi } from '../../api/applications';

defineOptions({
  name: 'ApplicationSecretModal',
});
const emits = defineEmits<{
  (event: 'change', data: OpenIddictApplicationDto): void;
}>();

const applicationModel = ref<OpenIddictApplicationDto>();
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
      component: 'InputPassword',
      fieldName: 'clientSecret',
      label: $t('AbpOpenIddict.DisplayName:ClientSecret'),
      rules: 'required',
    },
  ],
  showDefaultActions: false,
});
const [Modal, modalApi] = useVbenModal({
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onConfirm: async () => {
    await formApi.validateAndSubmitForm();
  },
  onOpenChange: async (isOpen) => {
    if (isOpen) {
      try {
        modalApi.setState({ loading: true });
        const { id } = modalApi.getData<OpenIddictApplicationDto>();
        await onGet(id);
      } finally {
        modalApi.setState({ loading: false });
      }
    }
  },
  title: $t('AbpOpenIddict.ManageSecret'),
});
async function onGet(id: string) {
  const dto = await getApi(id);
  applicationModel.value = dto;
}
async function onSubmit(input: Record<string, any>) {
  const dto = await updateApi(applicationModel.value!.id, {
    ...applicationModel.value!,
    clientSecret: input.clientSecret,
  });
  message.success($t('AbpUi.SavedSuccessfully'));
  emits('change', dto);
  modalApi.close();
}
</script>

<template>
  <Modal>
    <Form />
  </Modal>
</template>

<style scoped></style>
