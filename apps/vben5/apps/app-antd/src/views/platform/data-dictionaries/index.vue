<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { DataDictionaryTable } from '@abp/platform';

defineOptions({
  name: 'PlatformDataDictionaries',
});
</script>

<template>
  <Page>
    <DataDictionaryTable />
  </Page>
</template>
