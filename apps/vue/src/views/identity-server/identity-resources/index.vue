<template>
  <IdentityResourceTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import IdentityResourceTable from './components/IdentityResourceTable.vue';
  export default defineComponent({
    name: 'IdentityResources',
    components: {
      IdentityResourceTable,
    },
    setup() {},
  });
</script>
