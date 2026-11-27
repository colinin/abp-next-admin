<script lang="ts" setup>
import { Page } from '@vben/common-ui';

import { JobInfoTable } from '@abp/task-management';

defineOptions({
  name: 'Vben5TasksJobInfos',
});
</script>

<template>
  <Page>
    <JobInfoTable />
  </Page>
</template>
