<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { NotificationGroupDefinitionTable } from '@abp/notifications';

defineOptions({
  name: 'Vben5NotificationsGroupDefinitions',
});
</script>

<template>
  <Page>
    <NotificationGroupDefinitionTable />
  </Page>
</template>
