<script setup lang="ts">
import type {
  EntityChangeDto,
  EntityChangeGetWithUsernameInput,
} from '../../types/entity-changes';

import { ref } from 'vue';

import { useVbenDrawer } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { getListWithUsernameApi } from '../../api/entity-changes';
import EntityChangeTable from '../entity-changes/EntityChangeTable.vue';

const entityChanges = ref<EntityChangeDto[]>([]);

const [Drawer, drawerApi] = useVbenDrawer({
  class: 'w-auto',
  onCancel() {
    drawerApi.close();
  },
  onConfirm: async () => {},
  onOpenChange: async (isOpen: boolean) => {
    if (isOpen) {
      const input = drawerApi.getData<EntityChangeGetWithUsernameInput>();
      const { items } = await getListWithUsernameApi(input);
      entityChanges.value = items.map((item) => {
        return {
          ...item.entityChange,
          userName: item.userName,
        };
      });
    }
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
