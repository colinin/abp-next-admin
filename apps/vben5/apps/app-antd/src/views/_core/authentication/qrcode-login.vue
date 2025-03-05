<script lang="ts" setup>
import { LOGIN_PATH } from '@vben/constants';
import { preferences } from '@vben/preferences';

import { QrCodeLogin } from '@abp/account';

import { useAuthStore } from '#/store';

defineOptions({ name: 'QrCodeLogin' });

const authStore = useAuthStore();

async function onConfirm(key: string) {
  await authStore.qrcodeLogin(key);
}
</script>

<template>
  <QrCodeLogin
    :login-path="LOGIN_PATH"
    @confirm="onConfirm"
    :default-avatar="preferences.app.defaultAvatar"
  />
</template>
