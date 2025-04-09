<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { FeatureDefinitionTable } from '@abp/features';

defineOptions({
  name: 'FeatureDefinitions',
});
</script>

<template>
  <Page>
    <FeatureDefinitionTable />
  </Page>
</template>
