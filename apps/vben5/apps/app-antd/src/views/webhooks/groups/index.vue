<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { WebhookGroupDefinitionTable } from '@abp/webhooks';

defineOptions({
  name: 'Vben5WebhooksGroupDefinitions',
});
</script>

<template>
  <Page>
    <WebhookGroupDefinitionTable />
  </Page>
</template>
