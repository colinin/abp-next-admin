<template>
  <ContainerTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import ContainerTable from './components/ContainerTable.vue';
  export default defineComponent({
    name: 'Containers',
    components: {
      ContainerTable,
    },
    setup() {},
  });
</script>
