<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { SystemSetting } from '@abp/settings';

defineOptions({
  name: 'Vben5SettingsSystem',
});
</script>

<template>
  <Page auto-content-height>
    <SystemSetting />
  </Page>
</template>
