<template>
  <TokenTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import TokenTable from './components/TokenTable.vue';

  export default defineComponent({
    name: 'OpenIddictTokens',
    components: { TokenTable },
  });
</script>
