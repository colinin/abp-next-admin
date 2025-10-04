<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { MenuDto } from '../../types/menus';
import type { MenuDrawerState } from './types';

import { defineAsyncComponent, h, onMounted, ref } from 'vue';

import { useVbenDrawer } from '@vben/common-ui';
import { IconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { listToTree, sortby, useAuthorization } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, message, Modal } from 'ant-design-vue';

import { useLayoutsApi } from '../../api';
import { useMenusApi } from '../../api/useMenusApi';
import { MenuPermissions } from '../../constants/permissions';

defineOptions({
  name: 'MenuTable',
});

const { isGranted } = useAuthorization();
const { deleteApi, getAllApi } = useMenusApi();
const { getPagedListApi: getLayoutsApi } = useLayoutsApi();

const expandRowKeys = ref<string[]>([]);
const menus = ref<MenuDto[]>([]);

const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: true,
  collapsedRows: 2,
  // 所有表单项共用，可单独在表单内覆盖
  commonConfig: {
    // 在label后显示一个冒号
    colon: true,
    // 所有表单项
    componentProps: {
      class: 'w-full',
    },
  },
  handleSubmit: onGet,
  schema: [
    {
      component: 'ApiSelect',
      componentProps: {
        allowClear: true,
        api: getLayoutsApi,
        labelField: 'displayName',
        resultField: 'items',
        valueField: 'id',
      },
      fieldName: 'layoutId',
      label: $t('AppPlatform.DisplayName:Layout'),
    },
    {
      component: 'Input',
      fieldName: 'filter',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('AbpUi.Search'),
    },
  ],
  // 控制表单是否显示折叠按钮
  showCollapseButton: true,
  // 过滤条件变更时是否提交表单
  submitOnChange: true,
  // 按下回车时是否提交表单
  submitOnEnter: true,
};

const gridOptions: VxeGridProps<MenuDto> = {
  columns: [
    {
      align: 'center',
      type: 'seq',
      width: 50,
    },
    {
      align: 'left',
      field: 'name',
      minWidth: 250,
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
          MenuPermissions.Default,
          MenuPermissions.Create,
          MenuPermissions.Update,
          MenuPermissions.Delete,
        ],
        false,
      ),
      width: 300,
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
        let items = sortby(menus.value, sort.field);
        if (sort.order === 'desc') {
          items = items.reverse();
        }
        const result = {
          totalCount: menus.value.length,
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

const gridEvents: VxeGridListeners<MenuDto> = {
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
  formOptions,
  gridEvents,
  gridOptions,
});

const [MenuDrawer, drawerApi] = useVbenDrawer({
  connectedComponent: defineAsyncComponent(() => import('./MenuDrawer.vue')),
});

async function onGet() {
  try {
    gridApi.setLoading(true);
    const input = await gridApi.formApi.getValues();
    const { items } = await getAllApi(input);
    const treeItems = listToTree(items, {
      id: 'id',
      pid: 'parentId',
    });
    menus.value = treeItems;
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

function onCreate(row?: MenuDto) {
  drawerApi.setData<MenuDrawerState>({
    editMenu: {
      layoutId: row?.layoutId,
      parentId: row?.id,
    },
    rootMenus: menus.value,
  });
  drawerApi.open();
}

function onUpdate(row: MenuDto) {
  drawerApi.setData<MenuDrawerState>({
    editMenu: row,
    rootMenus: menus.value,
  });
  drawerApi.open();
}

function onDelete(row: MenuDto) {
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

onMounted(onGet);
</script>

<template>
  <Grid :table-title="$t('AppPlatform.DisplayName:Menus')">
    <template #toolbar-tools>
      <Button
        v-if="isGranted([MenuPermissions.Create])"
        :icon="h(PlusOutlined)"
        type="primary"
        @click="onCreate()"
      >
        {{ $t('AppPlatform.Menu:AddNew') }}
      </Button>
    </template>
    <template #name="{ row }">
      <div class="flex flex-row items-center gap-2">
        <IconifyIcon v-if="row.meta.icon" :icon="row.meta.icon" />
        <span>{{ row.name }}</span>
      </div>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          v-if="isGranted([MenuPermissions.Create])"
          :icon="h(PlusOutlined)"
          block
          type="link"
          @click="onCreate(row)"
        >
          {{ $t('AppPlatform.Menu:AddChildren') }}
        </Button>
        <Button
          v-if="
            isGranted([MenuPermissions.Default, MenuPermissions.Update], false)
          "
          :icon="h(EditOutlined)"
          block
          type="link"
          @click="onUpdate(row)"
        >
          {{ $t('AbpUi.Edit') }}
        </Button>
        <Button
          v-if="isGranted([MenuPermissions.Delete])"
          :icon="h(DeleteOutlined)"
          block
          danger
          type="link"
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
      </div>
    </template>
  </Grid>
  <MenuDrawer @change="onGet" />
</template>

<style scoped></style>
