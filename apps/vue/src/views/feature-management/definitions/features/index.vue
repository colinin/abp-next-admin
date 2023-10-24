<template>
  <FeatureDefinitionTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import FeatureDefinitionTable from './components/FeatureDefinitionTable.vue';
  export default defineComponent({
    name: 'FeaturesDefinitions',
    components: {
      FeatureDefinitionTable,
    },
    setup() {},
  });
</script>
