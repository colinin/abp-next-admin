<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { SessionTable } from '@abp/identity';

defineOptions({
  name: 'Vben5IdentitySessions',
});
</script>

<template>
  <Page>
    <SessionTable />
  </Page>
</template>
