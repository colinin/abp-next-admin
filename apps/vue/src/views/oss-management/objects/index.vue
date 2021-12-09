<template>
  <OssTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import OssTable from './components/OssTable.vue';
  export default defineComponent({
    name: 'Oss',
    components: {
      OssTable,
    },
    setup() {},
  });
</script>
