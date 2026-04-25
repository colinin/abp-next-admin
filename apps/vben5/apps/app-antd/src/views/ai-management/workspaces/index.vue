<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { WorkspaceDefinitionTable } from '@abp/ai-management';

defineOptions({
  name: 'Vben5AIWorkspaceDefinitions',
});
</script>

<template>
  <Page>
    <WorkspaceDefinitionTable />
  </Page>
</template>
