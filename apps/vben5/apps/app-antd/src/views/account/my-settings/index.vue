<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { MySetting } from '@abp/account';

defineOptions({
  name: 'MySettings',
});
</script>

<template>
  <Page>
    <MySetting />
  </Page>
</template>
