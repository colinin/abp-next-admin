<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { RoleTable } from '@abp/identity';

defineOptions({
  name: 'Vben5IdentityRoles',
});
</script>

<template>
  <Page>
    <RoleTable />
  </Page>
</template>
