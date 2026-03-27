<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { AIToolDefinitionTable } from '@abp/ai-management';

defineOptions({
  name: 'Vben5AIToolDefinitions',
});
</script>

<template>
  <Page>
    <AIToolDefinitionTable />
  </Page>
</template>
