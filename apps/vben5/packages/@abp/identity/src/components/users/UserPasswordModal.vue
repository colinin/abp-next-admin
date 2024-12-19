<!-- eslint-disable no-unused-vars -->
<script setup lang="ts">
import { h } from 'vue';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { Button, message } from 'ant-design-vue';

import { changePasswordApi } from '../../api/users';
import { useRandomPassword } from '../../hooks';

defineOptions({
  name: 'UserPasswordModal',
});
const emits = defineEmits<{
  (event: 'change'): void;
}>();

const { generatePassword } = useRandomPassword();

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
      component: 'InputSearch',
      componentProps: (_, actions) => {
        return {
          allowClear: false,
          enterButton: h(
            Button,
            {
              type: 'primary',
            },
            () => $t('AbpIdentity.RandomPassword'),
          ),
          onSearch: () => {
            actions.resetForm();
            actions.setFieldValue('password', generatePassword());
          },
        };
      },
      fieldName: 'password',
      label: $t('AbpIdentity.Password'),
      rules: 'required',
    },
  ],
  showDefaultActions: false,
});

const [Modal, modalApi] = useVbenModal({
  closeOnClickModal: false,
  closeOnPressEscape: false,
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onConfirm: async () => {
    try {
      modalApi.setState({ confirmLoading: true });
      await formApi.validateAndSubmitForm();
    } finally {
      modalApi.setState({ confirmLoading: false });
    }
  },
  title: $t('AbpIdentity.SetPassword'),
});

async function onSubmit(input: Record<string, any>) {
  const { id } = modalApi.getData<Record<string, any>>();
  await changePasswordApi(id, { password: input.password });
  message.success($t('AbpUi.SavedSuccessfully'));
  emits('change');
  modalApi.close();
}
</script>

<template>
  <Modal>
    <Form />
  </Modal>
</template>

<style scoped></style>
