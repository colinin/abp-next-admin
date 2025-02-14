<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { AuthorizationTable } from '@abp/openiddict';

defineOptions({
  name: 'OpenIddictAuthorizations',
});
</script>

<template>
  <Page>
    <AuthorizationTable />
  </Page>
</template>
