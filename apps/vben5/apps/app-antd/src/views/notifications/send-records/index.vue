<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { NotificationSendRecordTable } from '@abp/notifications';

defineOptions({
  name: 'Vben5NotificationsSendRecords',
});
</script>

<template>
  <Page>
    <NotificationSendRecordTable />
  </Page>
</template>
