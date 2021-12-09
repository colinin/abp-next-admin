<template>
  <AuditLogTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import AuditLogTable from './components/AuditLogTable.vue';
  export default defineComponent({
    name: 'AuditLogs',
    components: {
      AuditLogTable,
    },
    setup() {},
  });
</script>
