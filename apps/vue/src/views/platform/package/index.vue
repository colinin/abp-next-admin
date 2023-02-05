<template>
  <PackageTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import PackageTable from './components/PackageTable.vue';

  export default defineComponent({
    name: 'PlatformPackages',
    components: { PackageTable },
  });
</script>
