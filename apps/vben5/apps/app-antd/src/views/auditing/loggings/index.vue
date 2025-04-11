<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { LoggingTable } from '@abp/auditing';

defineOptions({
  name: 'AuditingLoggings',
});
</script>

<template>
  <Page>
    <LoggingTable />
  </Page>
</template>
