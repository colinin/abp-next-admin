<template>
  <NotificationDefinitionTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import NotificationDefinitionTable from './components/NotificationDefinitionTable.vue';
  export default defineComponent({
    name: 'NotificationsDefinitions',
    components: {
      NotificationDefinitionTable,
    },
    setup() {},
  });
</script>
