<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { BookTable } from '@abp/demo';

defineOptions({
  name: 'DemoBooks',
});
</script>

<template>
  <Page>
    <BookTable />
  </Page>
</template>
