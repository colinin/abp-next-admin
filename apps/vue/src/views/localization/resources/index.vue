<template>
  <ResourceTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import ResourceTable from './components/ResourceTable.vue';
  export default defineComponent({
    name: 'Resources',
    components: {
      ResourceTable,
    },
    setup() {},
  });
</script>
