<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface';

import type { DataDto } from '../../types/dataDictionaries';

import { defineAsyncComponent, h, onMounted, reactive, ref } from 'vue';

import { useVbenDrawer } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  EllipsisOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, Dropdown, Menu } from 'ant-design-vue';

import { useDataDictionariesApi } from '../../api/useDataDictionariesApi';

defineOptions({
  name: 'DataDictionaryTable',
});

const MenuItem = Menu.Item;
const ItemsIcon = createIconifyIcon('material-symbols:align-items-stretch');

const { getAllApi } = useDataDictionariesApi();

const dataDictionaries = ref<DataDto[]>([]);
const pageState = reactive({
  current: 1,
  size: 10,
  total: 0,
});

const gridOptions: VxeGridProps<DataDto> = {
  columns: [
    {
      align: 'center',
      type: 'seq',
      width: 50,
    },
    {
      align: 'left',
      field: 'name',
      minWidth: 150,
      slots: { default: 'name' },
      title: $t('AppPlatform.DisplayName:Name'),
      treeNode: true,
    },
    {
      align: 'left',
      field: 'displayName',
      minWidth: 150,
      title: $t('AppPlatform.DisplayName:DisplayName'),
    },
    {
      align: 'left',
      field: 'description',
      minWidth: 150,
      title: $t('AppPlatform.DisplayName:Description'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      width: 220,
    },
  ],
  exportConfig: {},
  keepSource: true,
  toolbarConfig: {
    custom: true,
    export: true,
    refresh: false,
    zoom: true,
  },
  treeConfig: {
    accordion: true,
    parentField: 'parentId',
    rowField: 'id',
    transform: true,
  },
};
const gridEvents: VxeGridListeners<DataDto> = {
  pageChange(params) {
    pageState.current = params.currentPage;
    pageState.size = params.pageSize;
    onPageChange();
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  gridEvents,
  gridOptions,
});

const [DataDictionaryItemDrawer, itemDrawerApi] = useVbenDrawer({
  connectedComponent: defineAsyncComponent(
    () => import('./DataDictionaryItemDrawer.vue'),
  ),
});

async function onGet() {
  try {
    gridApi.setLoading(true);
    const { items } = await getAllApi();
    pageState.total = items.length;
    dataDictionaries.value = items;
    onPageChange();
  } finally {
    gridApi.setLoading(false);
  }
}

function onPageChange() {
  const items = dataDictionaries.value.slice(
    (pageState.current - 1) * pageState.size,
    pageState.current * pageState.size,
  );
  gridApi.setGridOptions({
    data: items,
    pagerConfig: {
      currentPage: pageState.current,
      pageSize: pageState.size,
      total: pageState.total,
    },
  });
}

function onCreate() {}

function onUpdate(_row: DataDto) {}

function onDelete(_row: DataDto) {}

function onMenuClick(row: DataDto, info: MenuInfo) {
  switch (info.key) {
    case 'items': {
      onManageItems(row);
      break;
    }
  }
}

function onManageItems(row: DataDto) {
  itemDrawerApi.setData(row);
  itemDrawerApi.open();
}

onMounted(onGet);
</script>

<template>
  <Grid :table-title="$t('AppPlatform.DisplayName:DataDictionary')">
    <template #toolbar-tools>
      <Button :icon="h(PlusOutlined)" type="primary" @click="onCreate">
        {{ $t('AppPlatform.Data:AddNew') }}
      </Button>
    </template>
    <template #name="{ row }">
      <Button type="link" @click="onManageItems(row)">{{ row.name }}</Button>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          :icon="h(EditOutlined)"
          block
          type="link"
          @click="onUpdate(row)"
        >
          {{ $t('AbpUi.Edit') }}
        </Button>
        <Button
          :icon="h(DeleteOutlined)"
          block
          danger
          type="link"
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
        <Dropdown>
          <template #overlay>
            <Menu @click="(info) => onMenuClick(row, info)">
              <MenuItem key="children">
                <div class="flex flex-row items-center gap-[4px]">
                  <PlusOutlined />
                  {{ $t('AppPlatform.Data:AddChildren') }}
                </div>
              </MenuItem>
              <MenuItem key="items">
                <div class="flex flex-row items-center gap-[4px]">
                  <ItemsIcon />
                  {{ $t('AppPlatform.Data:Items') }}
                </div>
              </MenuItem>
            </Menu>
          </template>
          <Button :icon="h(EllipsisOutlined)" type="link" />
        </Dropdown>
      </div>
    </template>
  </Grid>
  <DataDictionaryItemDrawer />
</template>

<style scoped></style>
