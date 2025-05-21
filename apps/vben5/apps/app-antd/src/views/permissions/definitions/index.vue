<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { PermissionDefinitionTable } from '@abp/permissions';

defineOptions({
  name: 'Vben5PermissionsDefinitions',
});
</script>

<template>
  <Page>
    <PermissionDefinitionTable />
  </Page>
</template>
