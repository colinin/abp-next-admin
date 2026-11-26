<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { AuditLogPage } from '@abp/auditing';

defineOptions({
  name: 'Vben5AuditingAuditLogs',
});
</script>

<template>
  <Page auto-content-height content-class="bg-background">
    <AuditLogPage />
  </Page>
</template>
