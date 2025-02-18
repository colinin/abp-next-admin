<script setup lang="ts">
import { useUserStore } from '@vben/stores';

import { GdprCard } from '@abp/gdpr';

const userStore = useUserStore();

const onAccountDeleted = () => {
  userStore.$reset();
  window.location.reload();
};
</script>

<template>
  <GdprCard @account-delete="onAccountDeleted" />
</template>

<style scoped></style>
