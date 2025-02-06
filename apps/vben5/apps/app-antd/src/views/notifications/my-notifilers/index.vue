<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { MyNotificationTable } from '@abp/notifications';

defineOptions({
  name: 'MyNotifications',
});
</script>

<template>
  <Page>
    <MyNotificationTable />
  </Page>
</template>
