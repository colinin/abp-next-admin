<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { NotificationDefinitionTable } from '@abp/notifications';

defineOptions({
  name: 'NotificationDefinitions',
});
</script>

<template>
  <Page>
    <NotificationDefinitionTable />
  </Page>
</template>
