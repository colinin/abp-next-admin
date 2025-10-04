<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface';

import type { DataDto } from '../../types/dataDictionaries';

import { defineAsyncComponent, h, onMounted, ref } from 'vue';

import { useVbenDrawer, useVbenModal } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { listToTree, sortby, useAuthorization } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  EllipsisOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, Dropdown, Menu, message, Modal } from 'ant-design-vue';

import { useDataDictionariesApi } from '../../api/useDataDictionariesApi';
import { DataDictionaryPermissions } from '../../constants/permissions';

defineOptions({
  name: 'DataDictionaryTable',
});

const MenuItem = Menu.Item;
const ItemsIcon = createIconifyIcon('material-symbols:align-items-stretch');

const { isGranted } = useAuthorization();
const { deleteApi, getAllApi } = useDataDictionariesApi();

const expandRowKeys = ref<string[]>([]);
const dataDictionaries = ref<DataDto[]>([]);

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
      sortable: true,
      title: $t('AppPlatform.DisplayName:Name'),
      treeNode: true,
    },
    {
      align: 'left',
      field: 'displayName',
      minWidth: 150,
      sortable: true,
      title: $t('AppPlatform.DisplayName:DisplayName'),
    },
    {
      align: 'left',
      field: 'description',
      minWidth: 150,
      sortable: true,
      title: $t('AppPlatform.DisplayName:Description'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      visible: isGranted(
        [
          DataDictionaryPermissions.Default,
          DataDictionaryPermissions.Update,
          DataDictionaryPermissions.Delete,
        ],
        false,
      ),
      width: 220,
    },
  ],
  exportConfig: {},
  keepSource: true,
  rowConfig: {
    keyField: 'id',
  },
  proxyConfig: {
    ajax: {
      query: async ({ page, sort }) => {
        let items = sortby(dataDictionaries.value, sort.field);
        if (sort.order === 'desc') {
          items = items.reverse();
        }
        const result = {
          totalCount: dataDictionaries.value.length,
          items: items.slice(
            (page.currentPage - 1) * page.pageSize,
            page.currentPage * page.pageSize,
          ),
        };
        return new Promise((resolve) => {
          resolve(result);
        });
      },
    },
    response: {
      total: 'totalCount',
      list: 'items',
    },
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
  sortChange: () => {
    gridApi.query();
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
    dataDictionaries.value = treeItems;
    setTimeout(() => gridApi.reload(), 100);
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
      <Button
        v-if="isGranted([DataDictionaryPermissions.Create])"
        :icon="h(PlusOutlined)"
        type="primary"
        @click="onCreate"
      >
        {{ $t('AppPlatform.Data:AddNew') }}
      </Button>
    </template>
    <template #name="{ row }">
      <Button type="link" @click="onManageItems(row)">{{ row.name }}</Button>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          v-if="
            isGranted(
              [
                DataDictionaryPermissions.Default,
                DataDictionaryPermissions.Update,
              ],
              false,
            )
          "
          :icon="h(EditOutlined)"
          block
          type="link"
          @click="onUpdate(row)"
        >
          {{ $t('AbpUi.Edit') }}
        </Button>
        <Button
          v-if="isGranted([DataDictionaryPermissions.Delete])"
          :icon="h(DeleteOutlined)"
          block
          danger
          type="link"
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
        <Dropdown
          v-if="
            isGranted(
              [
                DataDictionaryPermissions.Create,
                DataDictionaryPermissions.ManageItems,
              ],
              false,
            )
          "
        >
          <template #overlay>
            <Menu @click="(info) => onMenuClick(row, info)">
              <MenuItem
                v-if="isGranted([DataDictionaryPermissions.Create])"
                key="children"
              >
                <div class="flex flex-row items-center gap-[4px]">
                  <PlusOutlined />
                  {{ $t('AppPlatform.Data:AddChildren') }}
                </div>
              </MenuItem>
              <MenuItem
                v-if="isGranted([DataDictionaryPermissions.ManageItems])"
                key="items"
              >
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
