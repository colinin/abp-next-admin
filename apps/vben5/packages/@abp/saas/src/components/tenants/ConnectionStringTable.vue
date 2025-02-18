<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { TenantConnectionStringDto } from '../../types/tenants';

import { defineAsyncComponent, h } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { DeleteOutlined, EditOutlined } from '@ant-design/icons-vue';
import { Button, Popconfirm } from 'ant-design-vue';
import { VxeGrid } from 'vxe-table';

const props = defineProps<{
  connectionStrings: TenantConnectionStringDto[];
  delete?: (data: TenantConnectionStringDto) => Promise<void>;
  submit?: (data: TenantConnectionStringDto) => Promise<void>;
}>();
const [ConnectionStringModal, connectionModalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./ConnectionStringModal.vue'),
  ),
});
const gridOptions: VxeGridProps<TenantConnectionStringDto> = {
  columns: [
    {
      align: 'left',
      field: 'name',
      title: $t('AbpSaas.DisplayName:Name'),
      width: 150,
    },
    {
      align: 'left',
      field: 'value',
      title: $t('AbpSaas.DisplayName:Value'),
    },
    {
      align: 'center',
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      width: 180,
    },
  ],
  exportConfig: {},
  keepSource: true,
  toolbarConfig: {
    buttons: [
      {
        code: 'add',
        icon: 'vxe-icon-add',
        name: $t('AbpSaas.ConnectionStrings:AddNew'),
        status: 'primary',
      },
    ],
  },
};
const gridEvents: VxeGridListeners<TenantConnectionStringDto> = {
  toolbarButtonClick(params) {
    if (params.code === 'add') {
      handleCreate();
    }
  },
};
function handleCreate() {
  connectionModalApi.setData({});
  connectionModalApi.open();
}
function handleUpdate(row: TenantConnectionStringDto) {
  connectionModalApi.setData(row);
  connectionModalApi.open();
}
async function onDelete(row: TenantConnectionStringDto) {
  props.delete && (await props.delete(row));
}
</script>

<template>
  <div>
    <VxeGrid v-bind="gridOptions" v-on="gridEvents" :data="connectionStrings">
      <template #action="{ row }">
        <div class="flex flex-row">
          <Button
            :icon="h(EditOutlined)"
            block
            type="link"
            @click="handleUpdate(row)"
          >
            {{ $t('AbpUi.Edit') }}
          </Button>
          <Popconfirm
            :title="$t('AbpUi.AreYouSure')"
            trigger="click"
            @confirm="onDelete(row)"
          >
            <template #description>
              <span>{{
                $t('AbpUi.ItemWillBeDeletedMessageWithFormat', [row.name])
              }}</span>
            </template>
            <Button :icon="h(DeleteOutlined)" block danger type="link">
              {{ $t('AbpUi.Delete') }}
            </Button>
          </Popconfirm>
        </div>
      </template>
    </VxeGrid>
    <ConnectionStringModal :submit="submit" />
  </div>
</template>

<style scoped></style>
