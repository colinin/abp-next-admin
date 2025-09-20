<script setup lang="ts">
import type { FavoriteMenu } from '@abp/platform';

import { useRouter } from 'vue-router';

import { openWindow } from '@vben/utils';

import { Workbench } from '@abp/platform';

const router = useRouter();
function navTo(menu: FavoriteMenu) {
  if (menu.path?.startsWith('http')) {
    openWindow(menu.path);
    return;
  }
  if (menu.path?.startsWith('/')) {
    router.push(menu.path).catch((error) => {
      console.error('Navigation failed:', error);
    });
  } else {
    console.warn(
      `Unknown URL for navigation item: ${menu.displayName} -> ${menu.path}`,
    );
  }
}
</script>

<template>
  <Workbench @nav-to="navTo" />
</template>

<style scoped></style>
