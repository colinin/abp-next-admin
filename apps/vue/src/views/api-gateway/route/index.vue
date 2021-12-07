<template>
  <RouteTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import RouteTable from './components/RouteTable.vue';
  export default defineComponent({
    name: 'Routes',
    components: {
      RouteTable,
    },
    setup() {},
  });
</script>
