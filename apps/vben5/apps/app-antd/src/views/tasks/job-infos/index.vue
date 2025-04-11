<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { JobInfoTable } from '@abp/tasks';

defineOptions({
  name: 'TaskManagementJobInfos',
});
</script>

<template>
  <Page>
    <JobInfoTable />
  </Page>
</template>
