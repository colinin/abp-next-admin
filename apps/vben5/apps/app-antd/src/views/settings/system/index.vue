<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { SystemSetting } from '@abp/settings';

defineOptions({
  name: 'SystemSettings',
});
</script>

<template>
  <Page>
    <SystemSetting />
  </Page>
</template>
