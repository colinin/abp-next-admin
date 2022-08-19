<template>
  <AuthorizationTable />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import AuthorizationTable from './components/AuthorizationTable.vue';

  export default defineComponent({
    name: 'OpenIddictAuthorizations',
    components: { AuthorizationTable },
  });
</script>
