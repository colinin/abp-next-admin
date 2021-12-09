<template>
  <ClaimTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import ClaimTable from './components/ClaimTable.vue';
  export default defineComponent({
    name: 'Claims',
    components: {
      ClaimTable,
    },
    setup() {},
  });
</script>
