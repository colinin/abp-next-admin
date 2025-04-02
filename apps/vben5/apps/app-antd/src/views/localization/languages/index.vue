<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { LocalizationLanguageTable } from '@abp/localization';

defineOptions({
  name: 'LocalizationLanguages',
});
</script>

<template>
  <Page>
    <LocalizationLanguageTable />
  </Page>
</template>
