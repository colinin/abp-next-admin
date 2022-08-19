<template>
  <TemplateTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import TemplateTable from './components/TemplateTable.vue';

  export default defineComponent({
    name: 'Templates',
    components: { TemplateTable },
  });
</script>
