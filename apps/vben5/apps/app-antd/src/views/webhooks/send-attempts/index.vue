<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { WebhookSendAttemptTable } from '@abp/webhooks';

defineOptions({
  name: 'Vben5WebhooksSendAttempts',
});
</script>

<template>
  <Page>
    <WebhookSendAttemptTable />
  </Page>
</template>
