<template>
  <AggregateRouteTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import AggregateRouteTable from './components/AggregateRouteTable.vue';
  export default defineComponent({
    name: 'Routes',
    components: {
      AggregateRouteTable,
    },
    setup() {},
  });
</script>
