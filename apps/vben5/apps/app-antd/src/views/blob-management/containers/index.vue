<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { BlobContainerTable } from '@abp/blob-management';

defineOptions({
  name: 'Vben5BlobContainers',
});
</script>

<template>
  <Page>
    <BlobContainerTable />
  </Page>
</template>
