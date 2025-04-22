<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { TemplateDefinitionTable } from '@abp/text-templating';

defineOptions({
  name: 'TemplateDefinitions',
});
</script>

<template>
  <Page>
    <TemplateDefinitionTable />
  </Page>
</template>
