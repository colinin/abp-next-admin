<script lang="ts" setup>
import { onMounted } from 'vue';

import { useAuthStore } from '#/store/auth';

const authStore = useAuthStore();

onMounted(async () => {
  await authStore.oidcCallback();
});
</script>

<template>
  <div>{{ $t('page.auth.processingLogin') }}</div>
</template>
