<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { EntityTypeInfoTable } from '@abp/data-protection';

defineOptions({
  name: 'Vben5DataProtectionEntityTypeInfos',
});
</script>

<template>
  <Page>
    <EntityTypeInfoTable />
  </Page>
</template>
