<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { PermissionGroupDefinitionTable } from '@abp/permissions';

defineOptions({
  name: 'PermissionGroupDefinitions',
});
</script>

<template>
  <Page>
    <PermissionGroupDefinitionTable />
  </Page>
</template>
