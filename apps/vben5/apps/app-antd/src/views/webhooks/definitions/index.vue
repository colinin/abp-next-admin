<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { WebhookDefinitionTable } from '@abp/webhooks';

defineOptions({
  name: 'WebhookDefinitions',
});
</script>

<template>
  <Page>
    <WebhookDefinitionTable />
  </Page>
</template>
