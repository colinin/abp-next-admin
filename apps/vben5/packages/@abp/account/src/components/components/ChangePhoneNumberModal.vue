<script setup lang="ts">
import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { message } from 'ant-design-vue';

import { useProfileApi } from '../../api';

const emits = defineEmits<{
  (event: 'change', data: string): void;
}>();

const { changePhoneNumberApi, sendChangePhoneNumberCodeApi } = useProfileApi();

const [Form, formApi] = useVbenForm({
  handleSubmit: onSubmit,
  schema: [
    {
      component: 'Input',
      fieldName: 'newPhoneNumber',
      label: $t('AbpIdentity.DisplayName:NewPhoneNumber'),
      rules: 'required',
    },
    {
      component: 'VbenPinInput',
      componentProps: {
        createText: (countdown: number) => {
          const text =
            countdown > 0
              ? $t('authentication.sendText', [countdown])
              : $t('authentication.sendCode');
          return text;
        },
        handleSendCode: onSendCode,
      },
      fieldName: 'code',
      label: $t('AbpIdentity.DisplayName:SmsVerifyCode'),
      rules: 'required',
    },
  ],
  showDefaultActions: false,
});
const [Modal, modalApi] = useVbenModal({
  async onConfirm() {
    await formApi.validateAndSubmitForm();
  },
});

async function onSendCode() {
  const result = await formApi.validateField('newPhoneNumber');
  if (!result.valid) {
    throw new Error(result.errors.join('\n'));
  }
  const input = await formApi.getValues();
  await sendChangePhoneNumberCodeApi({
    newPhoneNumber: input.newPhoneNumber,
  });
}

async function onSubmit(values: Record<string, any>) {
  try {
    modalApi.setState({ submitting: true });
    await changePhoneNumberApi({
      code: values.code,
      newPhoneNumber: values.newPhoneNumber,
    });
    message.success($t('AbpAccount.PhoneNumberChangedMessage'));
    emits('change', values.newPhoneNumber);
    modalApi.close();
  } finally {
    modalApi.setState({ submitting: false });
  }
}
</script>

<template>
  <Modal :title="$t('AbpIdentity.PhoneNumber')">
    <Form />
  </Modal>
</template>

<style scoped></style>
