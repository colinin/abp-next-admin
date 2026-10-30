<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { PendingApprovalTable } from '@abp/approval-flow';

defineOptions({
  name: 'Vben5ApprovalFlowMyApprovalPending',
});
</script>

<template>
  <Page>
    <PendingApprovalTable />
  </Page>
</template>
