<script setup lang="ts">
import type {
  EntityChangeDto,
  EntityChangeGetWithUsernameInput,
} from '../../types/entity-changes';

import { ref } from 'vue';

import { useVbenDrawer } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useEntityChangesApi } from '../../api/useEntityChangesApi';
import EntityChangeTable from '../entity-changes/EntityChangeTable.vue';

const entityChanges = ref<EntityChangeDto[]>([]);

interface State extends EntityChangeGetWithUsernameInput {
  subject?: string;
}

const { getListWithUsernameApi } = useEntityChangesApi();
const [Drawer, drawerApi] = useVbenDrawer({
  class: 'w-auto',
  onCancel() {
    drawerApi.close();
  },
  onConfirm: async () => {},
  onOpenChange: async (isOpen: boolean) => {
    let title = $t('AbpAuditLogging.EntitiesChanged');
    if (isOpen) {
      const state = drawerApi.getData<State>();
      const { items } = await getListWithUsernameApi({
        entityId: state.entityId,
        entityTypeFullName: state.entityTypeFullName,
      });
      entityChanges.value = items.map((item) => {
        return {
          ...item.entityChange,
          userName: item.userName,
        };
      });
      state.subject && (title += ` - ${state.subject}`);
    }
    drawerApi.setState({ title });
  },
  title: $t('AbpAuditLogging.EntitiesChanged'),
});
</script>

<template>
  <Drawer>
    <div style="max-width: 800px">
      <EntityChangeTable :data="entityChanges" show-user-name />
    </div>
  </Drawer>
</template>

<style scoped></style>
