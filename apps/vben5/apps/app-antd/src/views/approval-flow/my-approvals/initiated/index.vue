<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { InitiatedApprovalTable } from '@abp/approval-flow';

defineOptions({
  name: 'Vben5ApprovalFlowMyApprovalInitiated',
});
</script>

<template>
  <Page>
    <InitiatedApprovalTable />
  </Page>
</template>
