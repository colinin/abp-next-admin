<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { TemplateDto } from '../../types/templates';

import { defineAsyncComponent, h } from 'vue';
import { useRouter } from 'vue-router';

import { useVbenModal } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { formatToDateTime, useAuthorization } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, message, Modal } from 'ant-design-vue';

import { useTemplatesAdminApi } from '../../api/useTemplatesAdminApi';
import { TemplatePermissions } from '../../constants/permissions';

const DesignIcon = createIconifyIcon(
  'material-symbols-light:design-services-outline',
);

const router = useRouter();
const { isGranted } = useAuthorization();
const { deleteApi, getPagedListApi } = useTemplatesAdminApi();

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

const gridOptions: VxeGridProps<TemplateDto> = {
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
      title: $t('名称'),
    },
    {
      align: 'left',
      field: 'description',
      minWidth: 220,
      sortable: true,
      title: $t('描述'),
    },
    {
      align: 'left',
      field: 'creationTime',
      formatter: ({ cellValue }) => {
        return formatToDateTime(cellValue);
      },
      minWidth: 220,
      sortable: true,
      title: $t('创建时间'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      visible: isGranted(
        [
          TemplatePermissions.Default,
          TemplatePermissions.Update,
          TemplatePermissions.Delete,
        ],
        false,
      ),
      width: 240,
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
    refresh: true,
    refreshOptions: {
      code: 'query',
    },
    zoom: true,
  },
};

const gridEvents: VxeGridListeners<TemplateDto> = {
  sortChange: () => {
    gridApi.query();
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridOptions,
  gridEvents,
});

const [TemplateModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(() => import('./TemplateModal.vue')),
});

function onCreate() {
  modalApi.setData({});
  modalApi.open();
}

function onDesign(row: TemplateDto) {
  console.warn(row);
  router.push({
    path: '/print-designer/designer',
    query: {
      id: row.id,
    },
  });
}

function onUpdate(row: TemplateDto) {
  modalApi.setData(row);
  modalApi.open();
}

function onDelete(row: TemplateDto) {
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
  <Grid :table-title="$t('模板管理')">
    <template #toolbar-tools>
      <Button
        v-if="isGranted([TemplatePermissions.Create])"
        :icon="h(PlusOutlined)"
        type="primary"
        @click="onCreate"
      >
        {{ $t('添加新模板') }}
      </Button>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button :icon="h(DesignIcon)" block type="link" @click="onDesign(row)">
          {{ $t('设计模板') }}
        </Button>
        <Button
          v-if="
            isGranted(
              [TemplatePermissions.Default, TemplatePermissions.Update],
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
          v-if="isGranted([TemplatePermissions.Delete])"
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
  <TemplateModal @change="() => gridApi.query()" />
</template>

<style lang="scss" scoped></style>
