<template>
  <TenantTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import TenantTable from './components/TenantTable.vue';
  export default defineComponent({
    name: 'Tenants',
    components: { TenantTable },
    setup() {},
  });
</script>
