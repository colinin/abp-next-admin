<template>
  <WebhookDefinitionTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import WebhookDefinitionTable from './components/WebhookDefinitionTable.vue';
  export default defineComponent({
    name: 'WebhooksDefinitions',
    components: {
      WebhookDefinitionTable,
    },
    setup() {},
  });
</script>
