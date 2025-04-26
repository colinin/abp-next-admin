<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { WebhookDefinitionTable } from '@abp/webhooks';

defineOptions({
  name: 'Vben5WebhooksDefinitions',
});
</script>

<template>
  <Page>
    <WebhookDefinitionTable />
  </Page>
</template>
