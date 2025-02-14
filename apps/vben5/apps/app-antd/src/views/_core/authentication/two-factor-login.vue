<script setup lang="ts">
import type { TwoFactorProvider } from '@abp/account';
import type { FormExpose } from 'ant-design-vue/es/form/Form';

import { computed, ref, toValue, useTemplateRef } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useAccountApi } from '@abp/account';
import { Button, Form, Input, Select } from 'ant-design-vue';

import { useAuthStore } from '#/store/auth';

const FormItem = Form.Item;

interface TwoFactorState {
  username: string;
  password: string;
  twoFactorToken: string;
  userId: string;
  twoFactorProvider: string;
  code: string;
  email: string;
  phoneNumber: string;
}

const formModel = ref({} as TwoFactorState);
const form = useTemplateRef<FormExpose>('formRef');
const twoFactorProviders = ref<TwoFactorProvider[]>([]);
const sendValidCodeLoading = ref(false);
const sendValidCodeInternal = ref(0);
const getSendValidCodeLoading = computed(() => {
  return sendValidCodeInternal.value > 0;
});
const getSendValidCodeTitle = computed(() => {
  if (sendValidCodeInternal.value > 0) {
    return `${sendValidCodeInternal.value}`;
  }
  return $t('abp.oauth.twoFactor.getCode');
});

const authStore = useAuthStore();
const {
  getTwoFactorProvidersApi,
  sendEmailSigninCodeApi,
  sendPhoneSigninCodeApi,
} = useAccountApi();

const [Modal, modalApi] = useVbenModal({
  title: $t('abp.oauth.twoFactor.title'),
  fullscreenButton: false,
  closeOnClickModal: false,
  closeOnPressEscape: false,
  onConfirm: onLogin,
  async onOpenChange(isOpen) {
    if (isOpen) {
      const state = modalApi.getData<TwoFactorState>();
      formModel.value = state;
      const { items } = await getTwoFactorProvidersApi({
        userId: state.userId,
      });
      twoFactorProviders.value = items;
    }
  },
});
/** 二次认证登陆 */
async function onLogin() {
  modalApi.setState({
    confirmLoading: true,
    closable: false,
  });
  try {
    await form.value?.validate();
    const model = toValue(formModel);
    await authStore.authLogin({
      username: model.username,
      password: model.password,
      TwoFactorProvider: model.twoFactorProvider,
      TwoFactorCode: model.code,
    });
  } finally {
    modalApi.setState({
      confirmLoading: false,
      closable: true,
    });
  }
}
/** 发送邮件验证代码 */
async function onSendEmail() {
  await form.value?.validateFields('email');
  await onSendCode(() =>
    sendEmailSigninCodeApi({
      emailAddress: formModel.value.email,
    }),
  );
}
/** 发送短信验证代码 */
async function onSendSms() {
  await form.value?.validateFields('phoneNumber');
  await onSendCode(() =>
    sendPhoneSigninCodeApi({
      phoneNumber: formModel.value.phoneNumber,
    }),
  );
}
/** 发送验证代码 */
async function onSendCode(api: () => Promise<void>) {
  try {
    sendValidCodeLoading.value = true;
    sendValidCodeInternal.value = 60;
    await api();
    setInterval(() => {
      if (sendValidCodeInternal.value <= 0) {
        return;
      }
      sendValidCodeInternal.value -= 1;
    }, 1000);
  } catch {
    sendValidCodeInternal.value = 0;
  } finally {
    sendValidCodeLoading.value = false;
  }
}
</script>

<template>
  <Modal>
    <Form
      ref="formRef"
      :label-col="{ span: 6 }"
      :model="formModel"
      :wrapper-col="{ span: 18 }"
    >
      <!-- 选择验证器 -->
      <FormItem
        :label="$t('abp.oauth.twoFactor.authenticator')"
        name="twoFactorProvider"
        required
      >
        <Select
          v-model:value="formModel.twoFactorProvider"
          :field-names="{ label: 'name', value: 'value' }"
          :options="twoFactorProviders"
          allow-clear
        />
      </FormItem>
      <!-- 邮件验证 -->
      <template v-if="formModel.twoFactorProvider === 'Email'">
        <FormItem
          :label="$t('abp.oauth.twoFactor.emailAddress')"
          name="email"
          required
        >
          <Input
            v-model:value="formModel.email"
            autocomplete="off"
            type="email"
          />
        </FormItem>
        <FormItem :label="$t('abp.oauth.twoFactor.code')" name="code" required>
          <div class="flex flex-row gap-4">
            <Input v-model:value="formModel.code" autocomplete="off" />
            <Button
              :disabled="getSendValidCodeLoading"
              :loading="sendValidCodeLoading"
              @click="onSendEmail"
            >
              {{ getSendValidCodeTitle }}
            </Button>
          </div>
        </FormItem>
      </template>
      <!-- 手机号码验证 -->
      <template v-if="formModel.twoFactorProvider === 'Phone'">
        <FormItem
          :label="$t('abp.oauth.twoFactor.phoneNumber')"
          name="phoneNumber"
          required
        >
          <Input v-model:value="formModel.phoneNumber" autocomplete="off" />
        </FormItem>
        <FormItem :label="$t('abp.oauth.twoFactor.code')" name="code" required>
          <div class="flex flex-row gap-4">
            <Input v-model:value="formModel.code" autocomplete="off" />
            <Button
              :disabled="getSendValidCodeLoading"
              :loading="sendValidCodeLoading"
              @click="onSendSms"
            >
              {{ getSendValidCodeTitle }}
            </Button>
          </div>
        </FormItem>
      </template>
      <!-- 身份验证程序验证 -->
      <template v-else-if="formModel.twoFactorProvider === 'Authenticator'">
        <FormItem :label="$t('abp.oauth.twoFactor.code')" name="code" required>
          <Input v-model:value="formModel.code" autocomplete="off" />
        </FormItem>
      </template>
    </Form>
  </Modal>
</template>

<style scoped></style>
