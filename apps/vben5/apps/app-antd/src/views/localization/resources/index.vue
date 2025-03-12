<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { LocalizationResourceTable } from '@abp/localization';

defineOptions({
  name: 'LocalizationResources',
});
</script>

<template>
  <Page>
    <LocalizationResourceTable />
  </Page>
</template>
