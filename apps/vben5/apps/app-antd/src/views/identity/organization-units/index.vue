<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { OrganizationUnitPage } from '@abp/identity';

defineOptions({
  name: 'IdentityOrganizationUnits',
});
</script>

<template>
  <Page>
    <OrganizationUnitPage />
  </Page>
</template>
