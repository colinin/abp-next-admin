<template>
  <MenuTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import MenuTable from './components/MenuTable.vue';
  export default defineComponent({
    name: 'Menus',
    components: {
      MenuTable,
    },
    setup() {},
  });
</script>
