<script lang="ts" setup>
import type { ShouldChangePasswordError, TwoFactorError } from '@abp/account';

import type { ExtendedFormApi, VbenFormSchema } from '@vben/common-ui';
import type { Recordable } from '@vben/types';

import { computed, nextTick, onMounted, useTemplateRef } from 'vue';

import { AuthenticationLogin, useVbenModal, z } from '@vben/common-ui';
import { useAppConfig } from '@vben/hooks';
import { $t } from '@vben/locales';

import { useAbpStore, useSettings } from '@abp/core';
import { Modal } from 'ant-design-vue';

import { useAbpConfigApi } from '#/api/core/useAbpConfigApi';
import { useAuthStore } from '#/store';

import ShouldChangePassword from './should-change-password.vue';
import ThirdPartyLogin from './third-party-login.vue';
import TwoFactorLogin from './two-factor-login.vue';

interface LoginInstance {
  getFormApi(): ExtendedFormApi | undefined;
}

defineOptions({ name: 'Login' });

const { onlyOidc } = useAppConfig(import.meta.env, import.meta.env.PROD);

const abpStore = useAbpStore();
const authStore = useAuthStore();

const { isTrue } = useSettings();

const { getConfigApi } = useAbpConfigApi();

const login = useTemplateRef<LoginInstance>('login');

const formSchema = computed((): VbenFormSchema[] => {
  if (onlyOidc) {
    return [];
  }
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
const [ShouldChangePasswordModal, changePasswordModalApi] = useVbenModal({
  connectedComponent: ShouldChangePassword,
});
async function onInit() {
  if (onlyOidc === true) {
    setTimeout(() => {
      Modal.confirm({
        centered: true,
        title: $t('page.auth.oidcLogin'),
        content: $t('page.auth.oidcLoginMessage'),
        maskClosable: false,
        closable: false,
        cancelButtonProps: {
          disabled: true,
        },
        async onOk() {
          await authStore.oidcLogin();
        },
      });
    }, 300);
    return;
  }
  const abpConfig = await getConfigApi();
  abpStore.setApplication(abpConfig);
  nextTick(() => {
    const formApi = login.value?.getFormApi();
    formApi?.setFieldValue('tenant', abpConfig.currentTenant.name);
  });
}
async function onLogin(params: Recordable<any>) {
  if (onlyOidc === true) {
    await authStore.oidcLogin();
    return;
  }
  try {
    await authStore.authLogin(params);
  } catch (error) {
    onTwoFactorError(params, error);
    onShouldChangePasswordError(params, error);
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
function onShouldChangePasswordError(params: Recordable<any>, error: any) {
  const scpError = error as ShouldChangePasswordError;
  if (scpError.changePasswordToken) {
    changePasswordModalApi.setData({
      ...scpError,
      ...params,
    });
    changePasswordModalApi.open();
  }
}

onMounted(onInit);
</script>

<template>
  <div v-if="!onlyOidc">
    <AuthenticationLogin
      ref="login"
      :form-schema="formSchema"
      :loading="authStore.loginLoading"
      :show-register="isTrue('Abp.Account.IsSelfRegistrationEnabled')"
      @submit="onLogin"
    >
      <!-- 第三方登录 -->
      <template #third-party-login>
        <ThirdPartyLogin />
      </template>
    </AuthenticationLogin>
    <TwoFactorModal />
    <ShouldChangePasswordModal />
  </div>
</template>
