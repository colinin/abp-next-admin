<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface';

import type { DataDto } from '../../types/dataDictionaries';

import { defineAsyncComponent, h, onMounted, reactive, ref } from 'vue';

import { useVbenDrawer, useVbenModal } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { listToTree } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  EllipsisOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, Dropdown, Menu, message, Modal } from 'ant-design-vue';

import { useDataDictionariesApi } from '../../api/useDataDictionariesApi';

defineOptions({
  name: 'DataDictionaryTable',
});

const MenuItem = Menu.Item;
const ItemsIcon = createIconifyIcon('material-symbols:align-items-stretch');

const { deleteApi, getAllApi } = useDataDictionariesApi();

const expandRowKeys = ref<string[]>([]);
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
  rowConfig: {
    keyField: 'id',
  },
  toolbarConfig: {
    custom: true,
    export: true,
    refresh: {
      queryMethod: onGet,
    },
    zoom: true,
  },
  treeConfig: {
    accordion: true,
    parentField: 'parentId',
    rowField: 'id',
    transform: false,
  },
};
const gridEvents: VxeGridListeners<DataDto> = {
  pageChange(params) {
    pageState.current = params.currentPage;
    pageState.size = params.pageSize;
    onPageChange();
  },
  toggleTreeExpand(params) {
    if (params.expanded) {
      expandRowKeys.value.push(params.row.id);
    } else {
      expandRowKeys.value = expandRowKeys.value.filter(
        (key) => key !== params.row.id,
      );
    }
    onExpandChange();
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  gridEvents,
  gridOptions,
});

const [DataDictionaryModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./DataDictionaryModal.vue'),
  ),
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
    const treeItems = listToTree(items, {
      id: 'id',
      pid: 'parentId',
    });
    pageState.total = treeItems.length;
    dataDictionaries.value = treeItems;
    onPageChange();
  } finally {
    gridApi.setLoading(false);
  }
}

function onExpandChange() {
  gridApi.setGridOptions({
    treeConfig: {
      expandRowKeys: expandRowKeys.value,
    },
  });
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

function onCreate() {
  modalApi.setData({});
  modalApi.open();
}

function onUpdate(row: DataDto) {
  modalApi.setData(row);
  modalApi.open();
}

function onDelete(row: DataDto) {
  Modal.confirm({
    afterClose: () => {
      gridApi.setLoading(false);
    },
    centered: true,
    content: `${$t('AbpUi.ItemWillBeDeletedMessage')}`,
    onOk: async () => {
      try {
        gridApi.setLoading(true);
        await deleteApi(row.id);
        message.success($t('AbpUi.DeletedSuccessfully'));
        onGet();
      } finally {
        gridApi.setLoading(false);
      }
    },
    title: $t('AbpUi.AreYouSure'),
  });
}

function onMenuClick(row: DataDto, info: MenuInfo) {
  switch (info.key) {
    case 'children': {
      modalApi.setData({
        displayName: row.displayName,
        parentId: row.id,
      });
      modalApi.open();
      break;
    }
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
  <DataDictionaryModal @change="onGet" />
  <DataDictionaryItemDrawer />
</template>

<style scoped></style>
