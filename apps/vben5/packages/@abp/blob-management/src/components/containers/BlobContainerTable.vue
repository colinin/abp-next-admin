<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { BlobContainerDto } from '../../types/containers';

import { defineAsyncComponent, h, ref } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { formatToDateTime } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import { DeleteOutlined, PlusOutlined } from '@ant-design/icons-vue';
import { Button, message, Modal } from 'ant-design-vue';

import { useBlobContainersApi } from '../../api/useBlobContainersApi';

defineOptions({
  name: 'BlobContainerTable',
});

const { cancel, deleteApi, getPagedListApi } = useBlobContainersApi();

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

const gridOptions: VxeGridProps<BlobContainerDto> = {
  columns: [
    {
      align: 'left',
      field: 'name',
      minWidth: 150,
      sortable: true,
      title: $t('BlobManagement.DisplayName:Name'),
    },
    {
      align: 'left',
      field: 'creationTime',
      formatter({ cellValue }) {
        return cellValue ? formatToDateTime(cellValue) : '';
      },
      minWidth: 120,
      sortable: true,
      title: $t('BlobManagement.DisplayName:CreationTime'),
    },
    {
      align: 'left',
      field: 'lastModificationTime',
      formatter({ cellValue }) {
        return cellValue ? formatToDateTime(cellValue) : '';
      },
      minWidth: 120,
      sortable: true,
      title: $t('BlobManagement.DisplayName:LastModificationTime'),
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

const gridEvents: VxeGridListeners<BlobContainerDto> = {
  checkboxAll: (params) => {
    selectedKeys.value = params.records.map((record) => record.name);
  },
  checkboxChange: (params) => {
    selectedKeys.value = params.records.map((record) => record.name);
  },
  sortChange: () => {
    gridApi.query();
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const [BlobContainerModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./BlobContainerModal.vue'),
  ),
});

function onCreate() {
  modalApi.setData({});
  modalApi.open();
}

function onDelete(row: BlobContainerDto) {
  Modal.confirm({
    centered: true,
    content: $t('AbpUi.ItemWillBeDeletedMessageWithFormat', [row.name]),
    onCancel: () => {
      cancel();
    },
    onOk: async () => {
      await deleteApi(row.id);
      message.success($t('AbpUi.DeletedSuccessfully'));
      await gridApi.query();
    },
    title: $t('AbpUi.AreYouSure'),
  });
}
</script>

<template>
  <Grid :table-title="$t('BlobManagement.BlobContainers')">
    <template #toolbar-tools>
      <Button :icon="h(PlusOutlined)" type="primary" @click="onCreate">
        {{ $t('BlobManagement.BlobContainers:Create') }}
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
  <BlobContainerModal @change="() => gridApi.query()" />
</template>

<style lang="scss" scoped></style>
