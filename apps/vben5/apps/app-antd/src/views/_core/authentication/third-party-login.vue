<script setup lang="ts">
import { Button } from 'ant-design-vue';

import { useAuthStore } from '#/store/auth';

defineOptions({
  name: 'ThirdPartyLogin',
});

const authStore = useAuthStore();

async function login() {
  await authStore.oidcLogin();
}
</script>

<template>
  <div class="w-full sm:mx-auto md:max-w-md">
    <div class="mt-4 flex items-center justify-between">
      <span class="w-[35%] border-b border-input dark:border-gray-600"></span>
      <span class="text-center text-xs uppercase text-muted-foreground">
        {{ $t('authentication.thirdPartyLogin') }}
      </span>
      <span class="w-[35%] border-b border-input dark:border-gray-600"></span>
    </div>

    <div class="mt-4 flex flex-wrap justify-center">
      <Button block type="primary" ghost @click="login">
        {{ $t('page.auth.oidcLogin') }}
      </Button>
    </div>
  </div>
</template>
