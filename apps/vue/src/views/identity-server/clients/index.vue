<template>
  <ClientTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import ClientTable from './components/ClientTable.vue';
  export default defineComponent({
    name: 'Clients',
    components: {
      ClientTable,
    },
    setup() {},
  });
</script>
