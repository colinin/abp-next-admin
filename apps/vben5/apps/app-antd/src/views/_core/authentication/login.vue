<script lang="ts" setup>
import type { TwoFactorError } from '@abp/account';

import type { VbenFormSchema } from '@vben/common-ui';
import type { Recordable } from '@vben/types';

import { computed } from 'vue';

import { AuthenticationLogin, useVbenModal, z } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useAuthStore } from '#/store';

import TwoFactorLogin from './two-factor-login.vue';

defineOptions({ name: 'Login' });

const authStore = useAuthStore();

const formSchema = computed((): VbenFormSchema[] => {
  return [
    {
      component: 'VbenInput',
      componentProps: {
        placeholder: $t('authentication.usernameTip'),
      },
      fieldName: 'username',
      label: $t('authentication.username'),
      rules: z.string().min(1, { message: $t('authentication.usernameTip') }),
    },
    {
      component: 'VbenInputPassword',
      componentProps: {
        placeholder: $t('authentication.password'),
      },
      fieldName: 'password',
      label: $t('authentication.password'),
      rules: z.string().min(1, { message: $t('authentication.passwordTip') }),
    },
  ];
});
const [TwoFactorModal, twoFactorModalApi] = useVbenModal({
  connectedComponent: TwoFactorLogin,
});
async function onLogin(params: Recordable<any>) {
  try {
    await authStore.authLogin(params);
  } catch (error) {
    onTwoFactorError(params, error);
  }
}
function onTwoFactorError(params: Recordable<any>, error: any) {
  const tfaError = error as TwoFactorError;
  if (tfaError.twoFactorToken) {
    twoFactorModalApi.setData({
      ...tfaError,
      ...params,
    });
    twoFactorModalApi.open();
  }
}
</script>

<template>
  <div>
    <AuthenticationLogin
      :form-schema="formSchema"
      :loading="authStore.loginLoading"
      @submit="onLogin"
    />
    <TwoFactorModal />
  </div>
</template>
