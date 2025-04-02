<script setup lang="ts">
import { useVbenForm, useVbenModal, z } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { usePasswordValidator } from '@abp/identity';
import { message } from 'ant-design-vue';

import { useProfileApi } from '../../api';

const { validate } = usePasswordValidator();
const { changePasswordApi } = useProfileApi();

interface FormModel {
  currentPassword: string;
  newPassword: string;
  newPasswordConfirm: string;
}

const [Form, formApi] = useVbenForm({
  handleSubmit: onSubmit,
  schema: [
    {
      component: 'InputPassword',
      fieldName: 'currentPassword',
      label: $t('AbpAccount.DisplayName:CurrentPassword'),
      rules: 'required',
    },
    {
      component: 'InputPassword',
      fieldName: 'newPassword',
      label: $t('AbpAccount.DisplayName:NewPassword'),
      rules: z
        .string()
        .superRefine(async (newPassword, ctx) => {
          try {
            await validate(newPassword);
          } catch (error) {
            ctx.addIssue({
              code: z.ZodIssueCode.custom,
              message: String(error),
            });
          }
        })
        .refine(
          async (newPassword) => {
            const input = (await formApi.getValues()) as FormModel;
            return input.currentPassword !== newPassword;
          },
          {
            message: $t('AbpAccount.NewPasswordSameAsOld'),
          },
        )
        .refine(
          async (newPassword) => {
            const input = (await formApi.getValues()) as FormModel;
            return input.newPasswordConfirm === newPassword;
          },
          {
            message: $t(
              'AbpIdentity.Volo_Abp_Identity:PasswordConfirmationFailed',
            ),
          },
        ),
    },
    {
      component: 'InputPassword',
      fieldName: 'newPasswordConfirm',
      label: $t('AbpAccount.DisplayName:NewPasswordConfirm'),
      rules: z.string().refine(
        async (newPasswordConfirm) => {
          const input = (await formApi.getValues()) as FormModel;
          return input.newPassword === newPasswordConfirm;
        },
        {
          message: $t(
            'AbpIdentity.Volo_Abp_Identity:PasswordConfirmationFailed',
          ),
        },
      ),
    },
  ],
  showDefaultActions: false,
});
const [Modal, modalApi] = useVbenModal({
  async onConfirm() {
    await formApi.validateAndSubmitForm();
  },
});
async function onSubmit(values: Record<string, any>) {
  try {
    modalApi.setState({ submitting: true });
    await changePasswordApi({
      currentPassword: values.currentPassword,
      newPassword: values.newPassword,
    });
    message.success($t('AbpIdentity.PasswordChangedMessage'));
    modalApi.close();
  } finally {
    modalApi.setState({ submitting: false });
  }
}
</script>

<template>
  <Modal :title="$t('AbpAccount.ResetMyPassword')">
    <Form />
  </Modal>
</template>

<style scoped></style>
