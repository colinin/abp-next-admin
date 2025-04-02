<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { TenantTable } from '@abp/saas';

defineOptions({
  name: 'SaasTenants',
});
</script>

<template>
  <Page>
    <TenantTable />
  </Page>
</template>
