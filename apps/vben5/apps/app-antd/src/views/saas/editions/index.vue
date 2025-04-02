<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { EditionTable } from '@abp/saas';

defineOptions({
  name: 'SaasEditions',
});
</script>

<template>
  <Page>
    <EditionTable />
  </Page>
</template>
