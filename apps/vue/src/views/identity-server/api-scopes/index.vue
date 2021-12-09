<template>
  <ApiScopeTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import ApiScopeTable from './components/ApiScopeTable.vue';
  export default defineComponent({
    name: 'ApiScopes',
    components: {
      ApiScopeTable,
    },
    setup() {},
  });
</script>
