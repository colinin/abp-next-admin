<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { TenantTable } from '@abp/saas';

defineOptions({
  name: 'Vben5SaasTenants',
});
</script>

<template>
  <Page>
    <TenantTable />
  </Page>
</template>
