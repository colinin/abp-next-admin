<script setup lang="ts">
import { Card, Empty } from 'ant-design-vue';
</script>

<template>
  <Card :bordered="false" :title="$t('abp.account.settings.noticeSettings')">
    <Empty />
  </Card>
</template>

<style scoped></style>
