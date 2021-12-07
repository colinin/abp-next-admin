<template>
  <PersistedGrantTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import PersistedGrantTable from './components/PersistedGrantTable.vue';
  export default defineComponent({
    name: 'PersistedGrants',
    components: {
      PersistedGrantTable,
    },
    setup() {},
  });
</script>
