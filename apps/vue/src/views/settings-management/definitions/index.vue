<template>
  <SettingDefinitionTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import SettingDefinitionTable from './components/SettingDefinitionTable.vue';
  export default defineComponent({
    name: 'SettingDefinitions',
    components: {
      SettingDefinitionTable,
    },
    setup() {},
  });
</script>
