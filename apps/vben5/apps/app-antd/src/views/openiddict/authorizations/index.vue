<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { AuthorizationTable } from '@abp/openiddict';

defineOptions({
  name: 'Vben5OpenIddictAuthorizations',
});
</script>

<template>
  <Page>
    <AuthorizationTable />
  </Page>
</template>
