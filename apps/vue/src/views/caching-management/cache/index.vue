<template>
  <CacheTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import CacheTable from './components/CacheTable.vue';

  export default defineComponent({
    name: 'Caches',
    components: { CacheTable },
  });
</script>
