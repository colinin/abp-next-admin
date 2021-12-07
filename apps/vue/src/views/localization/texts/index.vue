<template>
  <TextTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import TextTable from './components/TextTable.vue';
  export default defineComponent({
    name: 'Texts',
    components: {
      TextTable,
    },
    setup() {},
  });
</script>
