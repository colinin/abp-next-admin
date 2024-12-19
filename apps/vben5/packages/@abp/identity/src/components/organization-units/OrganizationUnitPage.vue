<script setup lang="ts">
import { ref } from 'vue';

import OrganizationUnitTable from './OrganizationUnitTable.vue';
import OrganizationUnitTree from './OrganizationUnitTree.vue';

defineOptions({
  name: 'OrganizationUnitPage',
});

const selectedKey = ref<string>();

const onSelected = (id?: string) => {
  selectedKey.value = id;
};
</script>

<template>
  <div class="flex flex-row gap-2">
    <div class="basis-1/3">
      <OrganizationUnitTree @selected="onSelected" />
    </div>
    <div class="basis-2/3">
      <OrganizationUnitTable :selected-key="selectedKey" />
    </div>
  </div>
</template>

<style scoped></style>
