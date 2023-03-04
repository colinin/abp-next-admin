<template>
  <TemplateDefinitionTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import TemplateDefinitionTable from './components/TemplateDefinitionTable.vue';

  export default defineComponent({
    name: 'TemplateDefinitions',
    components: { TemplateDefinitionTable },
  });
</script>
