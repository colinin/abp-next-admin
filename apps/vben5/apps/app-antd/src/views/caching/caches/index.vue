<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { CacheKeyTable } from '@abp/cache-management';

defineOptions({
  name: 'Vben5CacheKeys',
});
</script>

<template>
  <Page>
    <CacheKeyTable />
  </Page>
</template>
