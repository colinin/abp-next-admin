<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { ApplicationTable } from '@abp/openiddict';

defineOptions({
  name: 'Vben5OpenIddictApplications',
});
</script>

<template>
  <Page>
    <ApplicationTable />
  </Page>
</template>
