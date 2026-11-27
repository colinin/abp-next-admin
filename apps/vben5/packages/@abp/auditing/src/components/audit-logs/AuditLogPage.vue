<script setup lang="ts">
import { defineAsyncComponent, ref } from 'vue';

import { TabPane, Tabs } from 'ant-design-vue';

const AuditLogTable = defineAsyncComponent(() => import('./AuditLogTable.vue'));
const AuditEntityChangeTable = defineAsyncComponent(
  () => import('./AuditEntityChangeTable.vue'),
);

const activeTabKey = ref<'audit-log' | 'entity-change'>('audit-log');
</script>

<template>
  <Tabs v-model:active-key="activeTabKey" class="full-height-tabs">
    <TabPane key="audit-log" :tab="$t('AbpAuditLogging.AuditLog')">
      <AuditLogTable />
    </TabPane>
    <TabPane key="entity-change" :tab="$t('AbpAuditLogging.EntitiesChanged')">
      <AuditEntityChangeTable />
    </TabPane>
  </Tabs>
</template>

<style scoped>
.full-height-tabs {
  @apply flex h-full min-h-0 flex-col;
}

.full-height-tabs :deep(.ant-tabs-content-holder),
.full-height-tabs :deep(.ant-tabs-content) {
  @apply flex h-full min-h-0 flex-col;
}

.full-height-tabs :deep(.ant-tabs-tabpane) {
  @apply h-full overflow-hidden;
}
</style>
