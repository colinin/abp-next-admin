<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { UserTable } from '@abp/identity';

defineOptions({
  name: 'Vben5IdentityUsers',
});
</script>

<template>
  <Page>
    <UserTable />
  </Page>
</template>
