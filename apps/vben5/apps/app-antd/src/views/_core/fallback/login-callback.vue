<script lang="ts" setup>
import { onMounted } from 'vue';

import { useOAuthError } from '@abp/account';
import { Modal } from 'ant-design-vue';

import { useAuthStore } from '#/store/auth';

const { formatError } = useOAuthError();
const authStore = useAuthStore();

onMounted(async () => {
  await authStore.oidcCallback((error) => {
    Modal.warn({
      centered: true,
      maskClosable: false,
      closable: false,
      title: formatError(error),
      onOk: () => {
        authStore.logout();
      },
    });
  });
});
</script>

<template>
  <div>{{ $t('page.auth.processingLogin') }}</div>
</template>
