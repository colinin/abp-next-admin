<template>
  <SubscriptionTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';

  import SubscriptionTable from './components/SubscriptionTable.vue';
  export default defineComponent({
    name: 'Subscriptions',
    components: {
      SubscriptionTable,
    },
    setup() {},
  });
</script>
