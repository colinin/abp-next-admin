<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { ApprovalTemplateTable } from '@abp/approval-flow';

defineOptions({
  name: 'Vben5ApprovalFlowTemplates',
});
</script>

<template>
  <Page>
    <ApprovalTemplateTable />
  </Page>
</template>
