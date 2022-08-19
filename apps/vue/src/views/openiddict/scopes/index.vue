<template>
  <ScopeTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import ScopeTable from './components/ScopeTable.vue';

  export default defineComponent({
    name: 'OpenIddictScopes',
    components: { ScopeTable },
  });
</script>
