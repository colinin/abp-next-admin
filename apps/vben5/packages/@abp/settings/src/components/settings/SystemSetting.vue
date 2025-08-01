<script setup lang="ts">
import type { SettingsUpdateInput } from '../../types';

import { ref } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { isEmail, useAbpStore } from '@abp/core';
import { FeatureModal } from '@abp/features';
import { Button, Form, InputSearch, message, Modal } from 'ant-design-vue';

import { useSettingsApi } from '../../api/useSettingsApi';
import SettingForm from './SettingForm.vue';

defineOptions({
  name: 'SystemSettingForm',
});

const FormItem = Form.Item;

const abpStore = useAbpStore();
const {
  getGlobalSettingsApi,
  getTenantSettingsApi,
  sendTestEmailApi,
  setGlobalSettingsApi,
  setTenantSettingsApi,
} = useSettingsApi();
const [HostFeatureModal, featureModalApi] = useVbenModal({
  connectedComponent: FeatureModal,
});

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

function onFeatureManage() {
  featureModalApi.setData({
    providerName: 'T',
  });
  featureModalApi.open();
}
</script>

<template>
  <SettingForm :get-api="onGet" :submit-api="onSubmit">
    <template #toolbar>
      <Button
        v-access:code="['FeatureManagement.ManageHostFeatures']"
        ghost
        post-icon="ant-design:setting-outlined"
        type="primary"
        @click="onFeatureManage"
      >
        {{ $t('AbpFeatureManagement.ManageHostFeatures') }}
      </Button>
    </template>
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
  <HostFeatureModal />
</template>

<style scoped></style>
