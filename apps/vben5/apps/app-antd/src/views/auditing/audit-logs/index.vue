<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { AuditLogTable } from '@abp/auditing';

defineOptions({
  name: 'AuditingAuditLogs',
});
</script>

<template>
  <Page>
    <AuditLogTable />
  </Page>
</template>
