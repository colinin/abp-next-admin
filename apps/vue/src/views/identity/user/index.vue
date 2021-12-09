<template>
  <UserTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import UserTable from './components/UserTable.vue';
  export default defineComponent({
    name: 'Users',
    components: {
      UserTable,
    },
    setup() {},
  });
</script>
