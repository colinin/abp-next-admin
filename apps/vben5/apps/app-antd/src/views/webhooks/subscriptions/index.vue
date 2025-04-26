<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { WebhookSubscriptionTable } from '@abp/webhooks';

defineOptions({
  name: 'Vben5WebhooksSubscriptions',
});
</script>

<template>
  <Page>
    <WebhookSubscriptionTable />
  </Page>
</template>
