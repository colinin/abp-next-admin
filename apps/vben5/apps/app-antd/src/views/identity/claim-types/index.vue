<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { ClaimTypeTable } from '@abp/identity';

defineOptions({
  name: 'IdentityClaimTypes',
});
</script>

<template>
  <Page>
    <ClaimTypeTable />
  </Page>
</template>
