<template>
  <BackgroundJobInfoTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import BackgroundJobInfoTable from './components/BackgroundJobInfoTable.vue';
  export default defineComponent({
    components: {
      BackgroundJobInfoTable,
    },
    setup() {},
  });
</script>
