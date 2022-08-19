<template>
  <JobTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import JobTable from './components/JobTable.vue';
  export default defineComponent({
    name: 'BackgroundJobs',
    components: {
      JobTable,
    },
    setup() {},
  });
</script>
