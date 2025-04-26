<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { TokenTable } from '@abp/openiddict';

defineOptions({
  name: 'Vben5OpenIddictTokens',
});
</script>

<template>
  <Page>
    <TokenTable />
  </Page>
</template>
