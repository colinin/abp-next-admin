<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { EmailMessageTable } from '@abp/platform';

defineOptions({
  name: 'PlatformEmailMessages',
});
</script>

<template>
  <Page>
    <EmailMessageTable />
  </Page>
</template>
