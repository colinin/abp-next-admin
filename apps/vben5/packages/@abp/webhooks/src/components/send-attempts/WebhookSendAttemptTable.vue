<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { WebhookSendRecordDto } from '../../types/sendAttempts';

import { h, ref } from 'vue';

import { $t } from '@vben/locales';

import { formatToDateTime } from '@abp/core';
import { useHttpStatusCodeMap } from '@abp/request';
import { useTenantsApi } from '@abp/saas';
import { useVbenVxeGrid } from '@abp/ui';
import { DeleteOutlined, EditOutlined } from '@ant-design/icons-vue';
import { Button, message, Modal, Tag } from 'ant-design-vue';
import debounce from 'lodash.debounce';

import { useSendAttemptsApi } from '../../api/useSendAttemptsApi';

defineOptions({
  name: 'WebhookSendAttemptTable',
});

const { getPagedListApi: getTenantsApi } = useTenantsApi();
const { cancel, deleteApi, getPagedListApi } = useSendAttemptsApi();
const { getHttpStatusColor, httpStatusCodeMap } = useHttpStatusCodeMap();

const tenantFilter = ref<string>();
const selectedKeys = ref<string[]>([]);

const httpStatusOptions = Object.keys(httpStatusCodeMap).map((key) => {
  return {
    label: httpStatusCodeMap[Number(key)],
    value: key,
  };
});

const formOptions: VbenFormProps = {
  collapsed: true,
  collapsedRows: 3,
  commonConfig: {
    componentProps: {
      class: 'w-full',
    },
  },
  fieldMappingTime: [
    [
      'creationTime',
      ['beginCreationTime', 'endCreationTime'],
      ['YYYY-MM-DD 00:00:00', 'YYYY-MM-DD 23:59:59'],
    ],
  ],
  schema: [
    {
      component: 'ApiSelect',
      componentProps: () => {
        return {
          allowClear: true,
          api: getTenantsApi,
          filterOption: false,
          labelField: 'normalizedName',
          onSearch: debounce((filter?: string) => {
            tenantFilter.value = filter;
          }, 500),
          params: {
            filter: tenantFilter.value || undefined,
          },
          resultField: 'items',
          showSearch: true,
          valueField: 'id',
        };
      },
      fieldName: 'tenantId',
      label: $t('WebhooksManagement.DisplayName:TenantId'),
    },
    {
      component: 'RangePicker',
      fieldName: 'creationTime',
      label: $t('WebhooksManagement.DisplayName:CreationTime'),
    },
    {
      component: 'Select',
      componentProps: {
        allowClear: true,
        options: [
          {
            label: $t('WebhooksManagement.ResponseState:Successed'),
            value: true,
          },
          {
            label: $t('WebhooksManagement.ResponseState:Failed'),
            value: false,
          },
        ],
      },
      fieldName: 'state',
      label: $t('WebhooksManagement.DisplayName:State'),
    },
    {
      component: 'Select',
      componentProps: {
        options: httpStatusOptions,
      },
      fieldName: 'responseStatusCode',
      label: $t('WebhooksManagement.DisplayName:ResponseStatusCode'),
    },
    {
      component: 'Input',
      componentProps: {
        allowClear: true,
      },
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

const gridOptions: VxeGridProps<WebhookSendRecordDto> = {
  columns: [
    {
      align: 'center',
      type: 'checkbox',
      width: 40,
    },
    {
      align: 'left',
      field: 'tenantId',
      minWidth: 150,
      title: $t('WebhooksManagement.DisplayName:TenantId'),
    },
    {
      align: 'left',
      field: 'responseStatusCode',
      minWidth: 150,
      slots: { default: 'responseStatusCode' },
      title: $t('WebhooksManagement.DisplayName:ResponseStatusCode'),
    },
    {
      align: 'left',
      field: 'creationTime',
      formatter({ cellValue }) {
        return cellValue ? formatToDateTime(cellValue) : '';
      },
      minWidth: 120,
      title: $t('WebhooksManagement.DisplayName:CreationTime'),
    },
    {
      align: 'center',
      field: 'response',
      minWidth: 300,
      title: $t('WebhooksManagement.DisplayName:Response'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      width: 200,
    },
  ],
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page }, formValues) => {
        return await getPagedListApi({
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
    // import: true,
    refresh: true,
    zoom: true,
  },
};

const gridEvents: VxeGridListeners<WebhookSendRecordDto> = {
  checkboxAll: (params) => {
    selectedKeys.value = params.records.map((record) => record.id);
  },
  checkboxChange: (params) => {
    selectedKeys.value = params.records.map((record) => record.id);
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

function onUpdate(_row: WebhookSendRecordDto) {}

function onDelete(row: WebhookSendRecordDto) {
  Modal.confirm({
    centered: true,
    content: $t('AbpUi.ItemWillBeDeletedMessageWithFormat', [row.title]),
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
  <Grid :table-title="$t('WebhooksManagement.SendAttempts')">
    <template #responseStatusCode="{ row }">
      <Tag :color="getHttpStatusColor(row.responseStatusCode)">
        {{ httpStatusCodeMap[row.responseStatusCode] }}
      </Tag>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button :icon="h(EditOutlined)" type="link" @click="onUpdate(row)">
          {{ $t('AbpUi.Edit') }}
        </Button>
        <Button
          :icon="h(DeleteOutlined)"
          danger
          type="link"
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
      </div>
    </template>
  </Grid>
</template>

<style lang="scss" scoped></style>
