<template>
  <GroupDefinitionTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import GroupDefinitionTable from './components/GroupDefinitionTable.vue';
  export default defineComponent({
    name: 'WebhooksGroupDefinitions',
    components: {
      GroupDefinitionTable,
    },
    setup() {},
  });
</script>
