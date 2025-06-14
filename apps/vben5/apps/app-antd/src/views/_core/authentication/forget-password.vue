<script lang="ts" setup>
import type { ExtendedFormApi, VbenFormSchema } from '@vben/common-ui';
import type { Recordable } from '@vben/types';

import { computed, ref, useTemplateRef } from 'vue';
import { useRouter } from 'vue-router';

import { AuthenticationForgetPassword, z } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useAccountApi } from '@abp/account';
import { isPhone } from '@abp/core';
import { usePasswordValidator } from '@abp/identity';
import { message } from 'ant-design-vue';

interface FormModel {
  currentPassword: string;
  newPassword: string;
  newPasswordConfirm: string;
}

interface ForgetPasswordExpose {
  getFormApi(): ExtendedFormApi;
}

defineOptions({ name: 'ForgetPassword' });

const router = useRouter();
const { validate } = usePasswordValidator();
const { resetPasswordApi, sendPhoneResetPasswordCodeApi } = useAccountApi();

const loading = ref(false);
const forgetPassword = useTemplateRef<ForgetPasswordExpose>('forgetPassword');

const formSchema = computed((): VbenFormSchema[] => {
  return [
    {
      component: 'Input',
      componentProps: {
        placeholder: $t('AbpAccount.DisplayName:PhoneNumber'),
      },
      fieldName: 'phoneNumber',
      label: $t('AbpAccount.DisplayName:PhoneNumber'),
      rules: z
        .string()
        .min(1, { message: $t('authentication.mobileTip') })
        .refine((v) => isPhone(v), {
          message: $t('authentication.mobileErrortip'),
        }),
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
        placeholder: $t('authentication.code'),
      },
      fieldName: 'code',
      label: $t('authentication.code'),
      rules: z.string().min(1, { message: $t('authentication.codeTip') }),
    },
    {
      component: 'InputPassword',
      componentProps: {
        placeholder: $t('AbpAccount.DisplayName:NewPassword'),
      },
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
            const formApi = forgetPassword.value?.getFormApi();
            const input = (await formApi?.getValues()) as FormModel;
            return input.currentPassword !== newPassword;
          },
          {
            message: $t('AbpAccount.NewPasswordSameAsOld'),
          },
        )
        .refine(
          async (newPassword) => {
            const formApi = forgetPassword.value?.getFormApi();
            const input = (await formApi?.getValues()) as FormModel;
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
      componentProps: {
        placeholder: $t('AbpAccount.DisplayName:NewPasswordConfirm'),
      },
      fieldName: 'newPasswordConfirm',
      label: $t('AbpAccount.DisplayName:NewPasswordConfirm'),
      rules: z.string().refine(
        async (newPasswordConfirm) => {
          const formApi = forgetPassword.value?.getFormApi();
          const input = (await formApi?.getValues()) as FormModel;
          return input.newPassword === newPasswordConfirm;
        },
        {
          message: $t(
            'AbpIdentity.Volo_Abp_Identity:PasswordConfirmationFailed',
          ),
        },
      ),
    },
  ];
});

async function onSendCode() {
  const formApi = forgetPassword.value?.getFormApi();
  const input = await formApi?.getValues();
  await sendPhoneResetPasswordCodeApi({
    phoneNumber: input!.phoneNumber,
  });
}

async function handleSubmit(values: Recordable<any>) {
  loading.value = true;
  try {
    await resetPasswordApi({
      code: values.code,
      phoneNumber: values.phoneNumber,
      newPassword: values.newPassword,
    });
    message.success($t('AbpAccount.YourPasswordIsSuccessfullyReset'));
    router.push('/auth/login');
  } finally {
    loading.value = false;
  }
}
</script>

<template>
  <AuthenticationForgetPassword
    ref="forgetPassword"
    :form-schema="formSchema"
    :submit-button-text="$t('AbpAccount.ResetPassword')"
    :loading="loading"
    @submit="handleSubmit"
  />
</template>
