<template>
  <PermissionDefinitionTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import PermissionDefinitionTable from './components/PermissionDefinitionTable.vue';
  export default defineComponent({
    name: 'PermissionsDefinitions',
    components: {
      PermissionDefinitionTable,
    },
    setup() {},
  });
</script>
