<template>
  <RoleTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import RoleTable from './components/RoleTable.vue';
  export default defineComponent({
    name: 'Roles',
    components: {
      RoleTable,
    },
    setup() {},
  });
</script>
