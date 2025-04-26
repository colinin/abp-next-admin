<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { NotificationDefinitionTable } from '@abp/notifications';

defineOptions({
  name: 'Vben5NotificationsDefinitions',
});
</script>

<template>
  <Page>
    <NotificationDefinitionTable />
  </Page>
</template>
