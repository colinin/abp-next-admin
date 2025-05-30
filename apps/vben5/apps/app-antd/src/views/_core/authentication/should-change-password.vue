<script setup lang="ts">
import { useVbenForm, useVbenModal, z } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { usePasswordValidator } from '@abp/identity';

import { useAuthStore } from '#/store/auth';

interface FormModel {
  currentPassword: string;
  newPassword: string;
  newPasswordConfirm: string;
}

interface ModalState {
  changePasswordToken: string;
  password: string;
  username: string;
  userId: string;
}

const authStore = useAuthStore();
const { validate } = usePasswordValidator();

const [Form, formApi] = useVbenForm({
  schema: [
    {
      component: 'InputPassword',
      fieldName: 'password',
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
  handleSubmit: onSubmit,
});
const [Modal, modalApi] = useVbenModal({
  draggable: true,
  closeOnClickModal: false,
  closeOnPressEscape: false,
  async onConfirm() {
    await formApi.validateAndSubmitForm();
  },
});
async function onSubmit(values: Record<string, any>) {
  modalApi.setState({ submitting: true });
  try {
    const state = modalApi.getData<ModalState>();
    await authStore.authLogin({
      username: state.username,
      password: state.password,
      NewPassword: values.newPassword,
      ChangePasswordToken: state.changePasswordToken,
    });
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
