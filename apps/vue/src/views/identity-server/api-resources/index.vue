<template>
  <ApiResourceTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import ApiResourceTable from './components/ApiResourceTable.vue';
  export default defineComponent({
    name: 'ApiResources',
    components: {
      ApiResourceTable,
    },
    setup() {},
  });
</script>
