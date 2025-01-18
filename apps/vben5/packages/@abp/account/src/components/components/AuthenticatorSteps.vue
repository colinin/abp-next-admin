<script setup lang="ts">
import type { FormExpose } from 'ant-design-vue/es/form/Form';

import type {
  AuthenticatorDto,
  VerifyAuthenticatorCodeInput,
} from '../../types';

import { computed, reactive, ref, toValue, useTemplateRef } from 'vue';

import { $t } from '@vben/locales';

import { useClipboard } from '@vueuse/core';
import { useQRCode } from '@vueuse/integrations/useQRCode';
import { Button, Card, Form, Input, message, Steps } from 'ant-design-vue';

import { useProfileApi } from '../../api/useProfileApi';

const props = defineProps<{
  authenticator: AuthenticatorDto;
}>();
const emits = defineEmits<{
  (event: 'done'): void;
}>();
const FormItem = Form.Item;

const { verifyAuthenticatorCodeApi } = useProfileApi();
const { copy } = useClipboard({ legacy: true });

const form = useTemplateRef<FormExpose>('formRef');
const validCodeInput = ref({} as VerifyAuthenticatorCodeInput);
const authenSteps = reactive([
  {
    title: $t('AbpAccount.Authenticator'),
  },
  {
    title: $t('AbpAccount.ValidAuthenticator'),
  },
  {
    title: $t('AbpAccount.RecoveryCode'),
  },
]);
const recoveryCodes = ref<string[]>([]);
const codeValidated = ref(false);
const currentStep = ref(0);
const loading = ref(false);
const getQrcodeUrl = computed(() => {
  return props.authenticator.authenticatorUri;
});
const qrcode = useQRCode(getQrcodeUrl);

function onPreStep() {
  currentStep.value -= 1;
}
function onNextStep() {
  currentStep.value += 1;
}
async function onCopy(text?: string) {
  if (!text) {
    return;
  }
  await copy(text);
  message.success($t('AbpUi.CopiedToTheClipboard'));
}
async function onValidCode() {
  await form.value?.validate();
  try {
    loading.value = true;
    const dto = await verifyAuthenticatorCodeApi(toValue(validCodeInput));
    recoveryCodes.value = dto.recoveryCodes;
    codeValidated.value = true;
    onNextStep();
  } finally {
    loading.value = false;
  }
}
</script>

<template>
  <Card :bordered="false">
    <!-- 步骤表单 -->
    <Steps :current="currentStep" :items="authenSteps" />
    <!-- 步骤一: 扫描二维码 -->
    <Card v-if="currentStep === 0" class="mt-4">
      <template #title>
        <div class="flex h-16 flex-col justify-center">
          <span class="text-lg font-normal">{{
            $t('AbpAccount.Authenticator')
          }}</span>
          <span class="text-sm font-light">{{
            $t('AbpAccount.AuthenticatorDesc')
          }}</span>
        </div>
      </template>
      <div class="mt-2 flex flex-row">
        <div class="basis-1/2">
          <Card
            :title="$t('AbpAccount.Authenticator:UseQrCode')"
            class="mh-350"
            type="inner"
          >
            <div class="flex justify-center">
              <img :src="qrcode" />
            </div>
          </Card>
        </div>
        <div class="basis-1/2">
          <Card
            :title="$t('AbpAccount.Authenticator:InputCode')"
            class="mh-350"
            type="inner"
          >
            <template #extra>
              <Button type="primary" @click="onCopy(authenticator?.sharedKey)">
                {{ $t('AbpAccount.Authenticator:CopyToClipboard') }}
              </Button>
            </template>
            <div
              class="flex items-center justify-center rounded-lg bg-[#dac6c6]"
            >
              <div class="m-4 text-xl font-bold text-blue-600">
                {{ authenticator?.sharedKey }}
              </div>
            </div>
          </Card>
        </div>
      </div>
    </Card>
    <!-- 步骤二: 验证代码 -->
    <Card v-if="currentStep === 1" class="mt-4">
      <template #title>
        <div class="flex h-16 flex-col justify-center">
          <span class="text-lg font-normal">{{
            $t('AbpAccount.ValidAuthenticator')
          }}</span>
          <span class="text-sm font-light">{{
            $t('AbpAccount.ValidAuthenticatorDesc')
          }}</span>
        </div>
      </template>
      <div class="flex flex-row">
        <div class="basis-2/3">
          <Form ref="formRef" :model="validCodeInput">
            <FormItem
              :label="$t('AbpAccount.DisplayName:AuthenticatorCode')"
              name="authenticatorCode"
              required
            >
              <Input v-model:value="validCodeInput.authenticatorCode" />
            </FormItem>
          </Form>
        </div>
        <div class="ml-4 basis-2/3">
          <Button :loading="loading" type="primary" @click="onValidCode">
            {{ $t('AbpAccount.Validation') }}
          </Button>
        </div>
      </div>
    </Card>
    <!-- 步骤三: 恢复代码 -->
    <Card v-if="currentStep === 2" class="mt-4">
      <template #title>
        <div class="flex h-16 flex-col justify-center">
          <span class="text-lg font-normal">{{
            $t('AbpAccount.RecoveryCode')
          }}</span>
          <span class="text-sm font-light">{{
            $t('AbpAccount.RecoveryCodeDesc')
          }}</span>
        </div>
      </template>
      <template #extra>
        <Button type="primary" @click="onCopy(recoveryCodes.join('\r'))">
          {{ $t('AbpAccount.Authenticator:CopyToClipboard') }}
        </Button>
      </template>
      <div
        class="flex flex-col items-center justify-center rounded-lg bg-[#dac6c6]"
      >
        <div class="m-2 text-xl font-bold text-blue-600">
          {{ recoveryCodes.slice(0, 5).join('\r\n') }}
        </div>
        <div class="m-2 text-xl font-bold text-blue-600">
          {{ recoveryCodes.slice(5).join('\r\n') }}
        </div>
      </div>
    </Card>
    <!-- 底部控制按钮 -->
    <template #actions>
      <div class="flex flex-row justify-end gap-2 pr-2">
        <Button v-if="currentStep > 0 && !codeValidated" @click="onPreStep">
          {{ $t('AbpAccount.Steps:PreStep') }}
        </Button>
        <Button
          v-if="currentStep < 2"
          :disabled="currentStep === 1 && !codeValidated"
          type="primary"
          @click="onNextStep"
        >
          {{ $t('AbpAccount.Steps:NextStep') }}
        </Button>
        <Button v-if="currentStep === 2" type="primary" @click="emits('done')">
          {{ $t('AbpAccount.Steps:Done') }}
        </Button>
      </div>
    </template>
  </Card>
</template>

<style scoped>
.mh-350 {
  min-height: 350px;
}
</style>
