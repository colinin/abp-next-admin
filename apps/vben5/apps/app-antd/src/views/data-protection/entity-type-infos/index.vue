<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { EntityTypeInfoTable } from '@abp/data-protection';

defineOptions({
  name: 'EntityTypeInfos',
});
</script>

<template>
  <Page>
    <EntityTypeInfoTable />
  </Page>
</template>
