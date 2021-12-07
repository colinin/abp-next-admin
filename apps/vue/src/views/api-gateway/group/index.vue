<template>
  <GroupTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import GroupTable from './components/GroupTable.vue';
  export default defineComponent({
    name: 'Groups',
    components: {
      GroupTable,
    },
    setup() {},
  });
</script>
