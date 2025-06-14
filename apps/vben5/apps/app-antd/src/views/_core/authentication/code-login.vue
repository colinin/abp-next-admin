<script lang="ts" setup>
import type { ExtendedFormApi, VbenFormSchema } from '@vben/common-ui';
import type { Recordable } from '@vben/types';

import { computed, ref, useTemplateRef } from 'vue';

import { AuthenticationCodeLogin, z } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useAccountApi } from '@abp/account';
import { isPhone } from '@abp/core';

import { useAuthStore } from '#/store/auth';

interface CodeLoginExpose {
  getFormApi(): ExtendedFormApi;
}

defineOptions({ name: 'CodeLogin' });

const authStore = useAuthStore();
const { sendPhoneSigninCodeApi } = useAccountApi();

const loading = ref(false);
const codeLogin = useTemplateRef<CodeLoginExpose>('codeLogin');

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
  ];
});
async function onSendCode() {
  const formApi = codeLogin.value?.getFormApi();
  const input = await formApi?.getValues();
  await sendPhoneSigninCodeApi({
    phoneNumber: input!.phoneNumber,
  });
}
/**
 * 异步处理登录操作
 * Asynchronously handle the login process
 * @param values 登录表单数据
 */
async function handleLogin(values: Recordable<any>) {
  try {
    loading.value = true;
    await authStore.phoneLogin(values.phoneNumber, values.code);
  } finally {
    loading.value = false;
  }
}
</script>

<template>
  <AuthenticationCodeLogin
    ref="codeLogin"
    :form-schema="formSchema"
    :loading="loading"
    @submit="handleLogin"
  />
</template>
