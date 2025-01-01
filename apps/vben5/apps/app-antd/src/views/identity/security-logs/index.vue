<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { SecurityLogTable } from '@abp/identity';

defineOptions({
  name: 'IdentitySecurityLogs',
});
</script>

<template>
  <Page>
    <SecurityLogTable />
  </Page>
</template>
