<!-- eslint-disable no-unused-vars -->
<script setup lang="ts">
import type { IdentityUserDto } from '../../types/users';

import { h } from 'vue';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { getModalData, useNotifications } from '@abp/core';
import { Button } from 'ant-design-vue';

import { useUsersApi } from '../../api/useUsersApi';
import { useRandomPassword } from '../../hooks';
defineOptions({
  name: 'UserPasswordModal',
});

const emits = defineEmits<{
  (event: 'change'): void;
}>();

const { toastSuccess } = useNotifications();

const { generatePassword } = useRandomPassword();
const { cancel, changePasswordApi } = useUsersApi();

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
      componentProps: () => {
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
            formApi.resetForm();
            formApi.setFieldValue('password', generatePassword());
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

const [Modal, modalApi] = useVbenModal<IdentityUserDto>({
  closeOnClickModal: false,
  closeOnPressEscape: false,
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onClosed() {
    cancel('User password modal has closed!');
  },
  onConfirm: async () => {
    try {
      modalApi.setState({ submitting: true });
      await formApi.validateAndSubmitForm();
    } finally {
      modalApi.setState({ submitting: false });
    }
  },
  onOpenChange(isOpen) {
    let title = $t('AbpIdentity.SetPassword');
    if (isOpen) {
      const { userName } = getModalData(modalApi);
      title += ` - ${userName}`;
    }
    modalApi.setState({ title });
  },
  title: $t('AbpIdentity.SetPassword'),
});

async function onSubmit(input: Record<string, any>) {
  const { id } = getModalData(modalApi);
  await changePasswordApi(id, { password: input.password });
  toastSuccess($t('AbpUi.SavedSuccessfully'));
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
