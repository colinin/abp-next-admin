<template>
  <ApplicationTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import ApplicationTable from './components/ApplicationTable.vue';

  export default defineComponent({
    name: 'OpenIddictApplications',
    components: { ApplicationTable },
  });
</script>
