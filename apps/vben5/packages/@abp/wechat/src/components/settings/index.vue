<script setup lang="ts">
import type { SettingsUpdateInput } from '@abp/settings';

import { useAbpStore } from '@abp/core';
import { SettingForm, useSettingsApi } from '@abp/settings';

import { useWechatSettingsApi } from '../../api/useWechatSettingsApi';

defineOptions({
  name: 'MaterialInspectSettings',
});

const abpStore = useAbpStore();
const { getGlobalSettingsApi, getTenantSettingsApi } = useWechatSettingsApi();
const { setGlobalSettingsApi, setTenantSettingsApi } = useSettingsApi();

async function onGet() {
  const getSettingsApi = abpStore.application?.currentTenant.isAvailable
    ? getTenantSettingsApi
    : getGlobalSettingsApi;
  const { items } = await getSettingsApi();
  return items;
}

async function onSubmit(input: SettingsUpdateInput) {
  const setSettingsApi = abpStore.application?.currentTenant.isAvailable
    ? setTenantSettingsApi
    : setGlobalSettingsApi;
  await setSettingsApi(input);
}
</script>

<template>
  <SettingForm :get-api="onGet" :submit-api="onSubmit" />
</template>

<style scoped></style>
