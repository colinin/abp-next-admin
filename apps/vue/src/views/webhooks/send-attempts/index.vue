<template>
  <SendAttemptTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import SendAttemptTable from './components/SendAttemptTable.vue';
  export default defineComponent({
    name: 'SendAttempts',
    components: {
      SendAttemptTable,
    },
    setup() {},
  });
</script>
