<template>
  <LoggingTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import LoggingTable from './components/LoggingTable.vue';
  export default defineComponent({
    name: 'Loggings',
    components: {
      LoggingTable,
    },
    setup() {},
  });
</script>
