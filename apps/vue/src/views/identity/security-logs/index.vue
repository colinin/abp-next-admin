<template>
  <SecurityLogTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import SecurityLogTable from './components/SecurityLogTable.vue';
  export default defineComponent({
    name: 'SecurityLogs',
    components: {
      SecurityLogTable,
    },
    setup() {},
  });
</script>
