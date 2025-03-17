<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { MyProfile } from '@abp/account';

defineOptions({
  name: 'MyProfiles',
});
</script>

<template>
  <Page>
    <MyProfile />
  </Page>
</template>
