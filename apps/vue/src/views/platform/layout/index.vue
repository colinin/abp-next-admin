<template>
  <LayoutTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import LayoutTable from './components/LayoutTable.vue';
  export default defineComponent({
    name: 'Layouts',
    components: {
      LayoutTable,
    },
    setup() {},
  });
</script>
