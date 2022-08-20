<template>
  <OrganizationUnitPage />
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import OrganizationUnitPage from './components/OrganizationUnitPage.vue';

  export default defineComponent({
    name: 'OrganizationUnits',
    components: { OrganizationUnitPage },
  });
</script>
