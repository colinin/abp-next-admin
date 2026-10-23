<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { DoneApprovalTable } from '@abp/approval-flow';

defineOptions({
  name: 'Vben5ApprovalFlowMyApprovalDone',
});
</script>

<template>
  <Page>
    <DoneApprovalTable />
  </Page>
</template>
