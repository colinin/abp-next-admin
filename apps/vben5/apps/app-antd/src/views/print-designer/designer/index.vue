<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { PrintDesigner } from '@abp/print-designer';

defineOptions({
  name: 'Vben5PrintDesignerPrintDesigner',
});
</script>

<template>
  <Page auto-content-height>
    <PrintDesigner />
  </Page>
</template>
