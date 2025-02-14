<script setup lang="ts">
import type { SettingsUpdateInput } from '../../types';

import { getUserSettingsApi, setUserSettingsApi } from '../../api/settings';
import SettingForm from './SettingForm.vue';

defineOptions({
  name: 'UserSettingForm',
});

async function onGet() {
  const { items } = await getUserSettingsApi();
  return items;
}

async function onSubmit(input: SettingsUpdateInput) {
  await setUserSettingsApi(input);
}
</script>

<template>
  <SettingForm :get-api="onGet" :submit-api="onSubmit" />
</template>

<style scoped></style>
