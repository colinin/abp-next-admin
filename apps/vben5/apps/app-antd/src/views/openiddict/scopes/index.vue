<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { ScopeTable } from '@abp/openiddict';

defineOptions({
  name: 'Vben5OpenIddictScopes',
});
</script>

<template>
  <Page>
    <ScopeTable />
  </Page>
</template>
