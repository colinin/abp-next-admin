<template>
  <EditionTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import EditionTable from './components/EditionTable.vue';
  export default defineComponent({
    name: 'Editions',
    components: { EditionTable },
    setup() {},
  });
</script>
