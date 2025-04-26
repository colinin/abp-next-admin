<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { SmsMessageTable } from '@abp/platform';

defineOptions({
  name: 'Vben5PlatformSmsMessages',
});
</script>

<template>
  <Page>
    <SmsMessageTable />
  </Page>
</template>
