<script setup lang="ts">
import type { SettingsUpdateInput } from '../../types';

import { ref } from 'vue';

import { $t } from '@vben/locales';

import { isEmail, useAbpStore } from '@abp/core';
import { Button, Form, InputSearch, message, Modal } from 'ant-design-vue';

import {
  getGlobalSettingsApi,
  getTenantSettingsApi,
  sendTestEmailApi,
  setGlobalSettingsApi,
  setTenantSettingsApi,
} from '../../api/settings';
import SettingForm from './SettingForm.vue';

defineOptions({
  name: 'SystemSettingForm',
});

const FormItem = Form.Item;

const abpStore = useAbpStore();

const sending = ref(false);

async function onGet() {
  const api = abpStore.application?.currentTenant.isAvailable
    ? getTenantSettingsApi
    : getGlobalSettingsApi;
  const { items } = await api();
  return items;
}

async function onSubmit(input: SettingsUpdateInput) {
  const api = abpStore.application?.currentTenant.isAvailable
    ? setTenantSettingsApi
    : setGlobalSettingsApi;
  await api(input);
}

async function onSendMail(email: string) {
  if (!isEmail(email)) {
    Modal.warn({
      centered: true,
      content: $t('AbpValidation.The {0} field is not a valid e-mail address', [
        $t('AbpSettingManagement.TargetEmailAddress'),
      ]),
      title: $t('AbpValidation.ThisFieldIsNotValid'),
    });
    return;
  }
  try {
    sending.value = true;
    await sendTestEmailApi(email);
    message.success($t('AbpSettingManagement.SuccessfullySent'));
  } finally {
    sending.value = false;
  }
}
</script>

<template>
  <SettingForm :get-api="onGet" :submit-api="onSubmit">
    <template #send-test-email="{ detail }">
      <FormItem
        :extra="detail.description"
        :label="detail.displayName"
        name="testEmail"
      >
        <InputSearch
          v-model:value="detail.value"
          :loading="sending"
          :placeholder="$t('AbpSettingManagement.TargetEmailAddress')"
          @search="onSendMail"
        >
          <template #enterButton>
            <Button :loading="sending" type="primary">
              {{ $t('AbpSettingManagement.Send') }}
            </Button>
          </template>
        </InputSearch>
      </FormItem>
    </template>
  </SettingForm>
</template>

<style scoped></style>
