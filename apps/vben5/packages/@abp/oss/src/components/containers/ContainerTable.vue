<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { OssContainerDto } from '../../types/containes';

import { defineAsyncComponent, h, ref } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { formatToDateTime } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import { DeleteOutlined, PlusOutlined } from '@ant-design/icons-vue';
import { Button, message, Modal } from 'ant-design-vue';

import { useContainesApi } from '../../api/useContainesApi';

defineOptions({
  name: 'ContainerTable',
});

const { cancel, deleteApi, getListApi } = useContainesApi();

const selectedKeys = ref<string[]>([]);

const formOptions: VbenFormProps = {
  collapsed: true,
  collapsedRows: 2,
  commonConfig: {
    componentProps: {
      class: 'w-full',
    },
  },
  schema: [
    {
      component: 'Input',
      componentProps: {
        allowClear: true,
      },
      fieldName: 'filter',
      formItemClass: 'col-span-3 items-baseline',
      label: $t('AbpUi.Search'),
    },
  ],
  // 控制表单是否显示折叠按钮
  showCollapseButton: true,
  // 按下回车时是否提交表单
  submitOnEnter: true,
};

const gridOptions: VxeGridProps<OssContainerDto> = {
  columns: [
    {
      align: 'left',
      field: 'name',
      minWidth: 150,
      title: $t('AbpOssManagement.DisplayName:Name'),
    },
    {
      align: 'left',
      field: 'creationDate',
      formatter({ cellValue }) {
        return cellValue ? formatToDateTime(cellValue) : '';
      },
      minWidth: 120,
      title: $t('AbpOssManagement.DisplayName:CreationDate'),
    },
    {
      align: 'left',
      field: 'lastModifiedDate',
      formatter({ cellValue }) {
        return cellValue ? formatToDateTime(cellValue) : '';
      },
      minWidth: 120,
      title: $t('AbpOssManagement.DisplayName:LastModifiedDate'),
    },
    {
      align: 'left',
      field: 'size',
      minWidth: 150,
      title: $t('AbpOssManagement.DisplayName:Size'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      width: 150,
    },
  ],
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page }, formValues) => {
        const res = await getListApi({
          maxResultCount: page.pageSize,
          skipCount: (page.currentPage - 1) * page.pageSize,
          ...formValues,
        });
        return {
          totalCount: res.maxKeys,
          items: res.containers,
        };
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
    // import: true,
    refresh: true,
    zoom: true,
  },
};

const gridEvents: VxeGridListeners<OssContainerDto> = {
  checkboxAll: (params) => {
    selectedKeys.value = params.records.map((record) => record.name);
  },
  checkboxChange: (params) => {
    selectedKeys.value = params.records.map((record) => record.name);
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const [ContainerModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./ContainerModal.vue'),
  ),
});

function onCreate() {
  modalApi.setData({});
  modalApi.open();
}

function onDelete(row: OssContainerDto) {
  Modal.confirm({
    centered: true,
    content: $t('AbpUi.ItemWillBeDeletedMessageWithFormat', [row.name]),
    onCancel: () => {
      cancel();
    },
    onOk: async () => {
      await deleteApi(row.name);
      message.success($t('AbpUi.DeletedSuccessfully'));
      await gridApi.query();
    },
    title: $t('AbpUi.AreYouSure'),
  });
}
</script>

<template>
  <Grid :table-title="$t('AbpOssManagement.Containers')">
    <template #toolbar-tools>
      <Button :icon="h(PlusOutlined)" type="primary" @click="onCreate">
        {{ $t('AbpOssManagement.Containers:Create') }}
      </Button>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          :icon="h(DeleteOutlined)"
          danger
          block
          type="link"
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
      </div>
    </template>
  </Grid>
  <ContainerModal @change="() => gridApi.query()" />
</template>

<style lang="scss" scoped></style>
