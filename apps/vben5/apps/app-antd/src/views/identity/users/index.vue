<script lang="ts" setup>
import type { IdentityUserDto } from '@abp/identity';

import { useRouter } from 'vue-router';

import { Page } from '@vben/common-ui';
import { useTabbarStore } from '@vben/stores';

import { UserTable } from '@abp/identity';

import { useAuthStore } from '#/store/auth';

defineOptions({
  name: 'Vben5IdentityUsers',
});

const router = useRouter();
const authStore = useAuthStore();
const tabbarStore = useTabbarStore();

async function handleImpersonation(user: IdentityUserDto) {
  await authStore.impersonationUserLogin(
    {
      userId: user.id,
      tenantId: user.tenantId,
    },
    () => {
      tabbarStore.closeAllTabs(router);
      window.location.replace('/');
    },
  );
}
</script>

<template>
  <Page>
    <UserTable @impersonation="handleImpersonation" />
  </Page>
</template>
