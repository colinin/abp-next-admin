<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { LayoutDto } from '../../types/layouts';

import { defineAsyncComponent, h } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useAuthorization } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, message, Modal } from 'ant-design-vue';

import { useLayoutsApi } from '../../api/useLayoutsApi';
import { LayoutPermissions } from '../../constants/permissions';

defineOptions({
  name: 'LayoutTable',
});

const { isGranted } = useAuthorization();
const { deleteApi, getPagedListApi } = useLayoutsApi();

const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: true,
  // 所有表单项共用，可单独在表单内覆盖
  commonConfig: {
    // 在label后显示一个冒号
    colon: true,
    // 所有表单项
    componentProps: {
      class: 'w-full',
    },
  },
  schema: [
    {
      component: 'Input',
      fieldName: 'filter',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('AbpUi.Search'),
    },
  ],
  // 控制表单是否显示折叠按钮
  showCollapseButton: true,
  // 按下回车时是否提交表单
  submitOnEnter: true,
};

const gridOptions: VxeGridProps<LayoutDto> = {
  columns: [
    {
      align: 'center',
      fixed: 'left',
      type: 'seq',
      width: 80,
    },
    {
      align: 'left',
      field: 'name',
      fixed: 'left',
      minWidth: 180,
      sortable: true,
      title: $t('AppPlatform.DisplayName:Name'),
    },
    {
      align: 'left',
      field: 'displayName',
      minWidth: 150,
      sortable: true,
      title: $t('AppPlatform.DisplayName:DisplayName'),
    },
    {
      align: 'center',
      field: 'path',
      minWidth: 200,
      sortable: true,
      title: $t('AppPlatform.DisplayName:Path'),
    },
    {
      align: 'left',
      field: 'framework',
      minWidth: 180,
      sortable: true,
      title: $t('AppPlatform.DisplayName:UIFramework'),
    },
    {
      align: 'left',
      field: 'description',
      minWidth: 220,
      sortable: true,
      title: $t('AppPlatform.DisplayName:Description'),
    },
    {
      align: 'left',
      field: 'redirect',
      minWidth: 160,
      sortable: true,
      title: $t('AppPlatform.DisplayName:Redirect'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      visible: isGranted(
        [
          LayoutPermissions.Default,
          LayoutPermissions.Update,
          LayoutPermissions.Delete,
        ],
        false,
      ),
      width: 220,
    },
  ],
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page, sort }, formValues) => {
        const sorting = sort.order ? `${sort.field} ${sort.order}` : undefined;
        return await getPagedListApi({
          sorting,
          maxResultCount: page.pageSize,
          skipCount: (page.currentPage - 1) * page.pageSize,
          ...formValues,
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
      code: 'query',
    },
    zoom: true,
  },
};
const gridEvents: VxeGridListeners<LayoutDto> = {
  sortChange: () => {
    gridApi.query();
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridOptions,
  gridEvents,
});

const [LayoutModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(() => import('./LayoutModal.vue')),
});

function onCreate() {
  modalApi.setData({});
  modalApi.open();
}

function onUpdate(row: LayoutDto) {
  modalApi.setData(row);
  modalApi.open();
}

function onDelete(row: LayoutDto) {
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
        gridApi.query();
      } finally {
        gridApi.setLoading(false);
      }
    },
    title: $t('AbpUi.AreYouSure'),
  });
}
</script>

<template>
  <Grid :table-title="$t('AppPlatform.DisplayName:Layout')">
    <template #toolbar-tools>
      <Button
        v-if="isGranted([LayoutPermissions.Create])"
        :icon="h(PlusOutlined)"
        type="primary"
        @click="onCreate"
      >
        {{ $t('AppPlatform.Layout:AddNew') }}
      </Button>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          v-if="
            isGranted(
              [LayoutPermissions.Default, LayoutPermissions.Update],
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
          v-if="isGranted([LayoutPermissions.Delete])"
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
  <LayoutModal @change="() => gridApi.query()" />
</template>

<style lang="scss" scoped></style>
