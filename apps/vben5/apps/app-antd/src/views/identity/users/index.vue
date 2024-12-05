<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { UserTable } from '@abp/identity';

defineOptions({
  name: 'IdentityUsers',
});
</script>

<template>
  <Page>
    <UserTable />
  </Page>
</template>
