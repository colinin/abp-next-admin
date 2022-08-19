<template>
  <OssManagePage />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import OssManagePage from './components/OssManagePage.vue';
  export default defineComponent({
    name: 'Oss',
    components: {
      OssManagePage,
    },
    setup() {},
  });
</script>
