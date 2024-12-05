<script setup lang="ts">
import { ref } from 'vue';

import { Card, Tabs } from 'ant-design-vue';

import OrganizationUnitRoleTable from './OrganizationUnitRoleTable.vue';
import OrganizationUnitUserTable from './OrganizationUnitUserTable.vue';

defineOptions({
  name: 'OrganizationUnitTable',
});

const props = defineProps<{
  selectedKey?: string;
}>();

const TabPane = Tabs.TabPane;

const activeTab = ref<'roles' | 'users'>('users');
</script>

<template>
  <Card>
    <Tabs v-model:active-key="activeTab">
      <TabPane key="users" :tab="$t('AbpIdentity.Users')" />
      <TabPane key="roles" :tab="$t('AbpIdentity.Roles')" />
    </Tabs>
    <OrganizationUnitUserTable
      v-if="activeTab === 'users'"
      :selected-key="props.selectedKey"
    />
    <OrganizationUnitRoleTable v-else :selected-key="props.selectedKey" />
  </Card>
</template>

<style scoped></style>
