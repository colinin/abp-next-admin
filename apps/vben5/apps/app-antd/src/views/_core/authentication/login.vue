<script lang="ts" setup>
import type { TwoFactorError } from '@abp/account';

import type { ExtendedFormApi, VbenFormSchema } from '@vben/common-ui';
import type { Recordable } from '@vben/types';

import { computed, nextTick, onMounted, useTemplateRef } from 'vue';

import { AuthenticationLogin, useVbenModal, z } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useAbpStore } from '@abp/core';

import { useAbpConfigApi } from '#/api/core/useAbpConfigApi';
import { useAuthStore } from '#/store';

import ThirdPartyLogin from './third-party-login.vue';
import TwoFactorLogin from './two-factor-login.vue';

interface LoginInstance {
  getFormApi(): ExtendedFormApi | undefined;
}

defineOptions({ name: 'Login' });

const abpStore = useAbpStore();
const authStore = useAuthStore();

const { getConfigApi } = useAbpConfigApi();

const login = useTemplateRef<LoginInstance>('login');

const formSchema = computed((): VbenFormSchema[] => {
  let schemas: VbenFormSchema[] = [
    {
      component: 'Input',
      componentProps: {
        placeholder: $t('authentication.usernameTip'),
      },
      fieldName: 'username',
      label: $t('authentication.username'),
      rules: z.string().min(1, { message: $t('authentication.usernameTip') }),
    },
    {
      component: 'InputPassword',
      componentProps: {
        placeholder: $t('authentication.password'),
      },
      fieldName: 'password',
      label: $t('authentication.password'),
      rules: z.string().min(1, { message: $t('authentication.passwordTip') }),
    },
  ];
  if (abpStore.application?.multiTenancy.isEnabled) {
    schemas = [
      {
        component: 'TenantSelect',
        fieldName: 'tenant',
        componentProps: {
          onChange: onInit,
        },
      },
      ...schemas,
    ];
  }
  return schemas;
});
const [TwoFactorModal, twoFactorModalApi] = useVbenModal({
  connectedComponent: TwoFactorLogin,
});
async function onInit() {
  const abpConfig = await getConfigApi();
  abpStore.setApplication(abpConfig);
  nextTick(() => {
    const formApi = login.value?.getFormApi();
    formApi?.setFieldValue('tenant', abpConfig.currentTenant.name);
  });
}
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

onMounted(onInit);
</script>

<template>
  <div>
    <AuthenticationLogin
      ref="login"
      :form-schema="formSchema"
      :loading="authStore.loginLoading"
      @submit="onLogin"
    >
      <!-- 第三方登录 -->
      <template #third-party-login>
        <ThirdPartyLogin />
      </template>
    </AuthenticationLogin>
    <TwoFactorModal />
  </div>
</template>
