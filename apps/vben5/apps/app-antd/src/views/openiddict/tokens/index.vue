<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { TokenTable } from '@abp/openiddict';

defineOptions({
  name: 'OpenIddictTokens',
});
</script>

<template>
  <Page>
    <TokenTable />
  </Page>
</template>
