<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { WebhookGroupDefinitionTable } from '@abp/webhooks';

defineOptions({
  name: 'WebhookGroupDefinitions',
});
</script>

<template>
  <Page>
    <WebhookGroupDefinitionTable />
  </Page>
</template>
