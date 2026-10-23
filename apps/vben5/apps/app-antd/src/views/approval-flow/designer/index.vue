<script lang="ts" setup>
import { useRoute } from 'vue-router';

import { Page } from '@vben/common-ui';

import { ApprovalFlowDesigner } from '@abp/approval-flow';

defineOptions({
  name: 'Vben5ApprovalFlowDesigner',
});

const route = useRoute();
const templateId = (route.query.templateId as string) || undefined;
</script>

<template>
  <Page>
    <ApprovalFlowDesigner :template-id="templateId" />
  </Page>
</template>
