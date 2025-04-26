<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { SettingDefinitionTable } from '@abp/settings';

defineOptions({
  name: 'Vben5SettingsDefinitions',
});
</script>

<template>
  <Page>
    <SettingDefinitionTable />
  </Page>
</template>
