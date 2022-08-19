<template>
  <NotificationTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import NotificationTable from './components/NotificationTable.vue';

  export default defineComponent({
    name: 'Notifications',
    components: { NotificationTable },
  });
</script>
