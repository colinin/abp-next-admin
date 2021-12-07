<template>
  <LanguageTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import LanguageTable from './components/LanguageTable.vue';
  export default defineComponent({
    name: 'Languages',
    components: {
      LanguageTable,
    },
    setup() {},
  });
</script>
