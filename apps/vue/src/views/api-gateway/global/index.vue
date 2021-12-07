<template>
  <GlobalTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import GlobalTable from './components/GlobalTable.vue';
  export default defineComponent({
    name: 'Groups',
    components: {
      GlobalTable,
    },
    setup() {},
  });
</script>
