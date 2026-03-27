<script setup lang="ts">
import type { VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { AIToolDefinitionRecordDto } from '../../types/tools';

import { defineAsyncComponent, h } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import {
  useAuthorization,
  useLocalization,
  useLocalizationSerializer,
} from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  CheckOutlined,
  CloseOutlined,
  DeleteOutlined,
  EditOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, message, Modal } from 'ant-design-vue';

import { useAIToolsApi } from '../../api/useAIToolsApi';
import { AIToolDefinitionPermissions } from '../../constants/permissions';

defineOptions({
  name: 'WorkspaceDefinitionTable',
});
const { deleteApi, getPagedListApi } = useAIToolsApi();
const { isGranted } = useAuthorization();
const { Lr } = useLocalization();
const { deserialize: deserializeLocalizableString } =
  useLocalizationSerializer();

const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: true,
  collapsedRows: 2,
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
  wrapperClass: 'grid-cols-4',
};

const gridOptions: VxeGridProps<AIToolDefinitionRecordDto> = {
  columns: [
    {
      align: 'left',
      field: 'name',
      sortable: true,
      title: $t('AIManagement.DisplayName:Name'),
    },
    {
      align: 'left',
      field: 'provider',
      sortable: true,
      title: $t('AIManagement.DisplayName:ToolProvider'),
      width: 150,
    },
    {
      align: 'left',
      field: 'description',
      formatter: ({ row }) => {
        if (!row.description) {
          return '';
        }
        const localizableString = deserializeLocalizableString(row.description);
        return Lr(localizableString.resourceName, localizableString.name);
      },
      sortable: true,
      title: $t('AIManagement.DisplayName:Description'),
      width: 180,
    },
    {
      align: 'left',
      field: 'isEnabled',
      slots: { default: 'isEnabled' },
      sortable: true,
      title: $t('AIManagement.DisplayName:IsEnabled'),
      width: 150,
    },
    {
      align: 'left',
      field: 'isGlobal',
      slots: { default: 'isGlobal' },
      sortable: true,
      title: $t('AIManagement.DisplayName:IsGlobal'),
      width: 150,
    },
    {
      field: 'actions',
      fixed: 'right',
      slots: { default: 'actions' },
      title: $t('AbpUi.Actions'),
      width: 220,
      visible: isGranted([
        AIToolDefinitionPermissions.Default,
        AIToolDefinitionPermissions.Delete,
      ]),
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
  sortConfig: {
    remote: true,
    allowBtn: true,
  },
  toolbarConfig: {
    custom: true,
    export: true,
    // import: true,
    refresh: {
      code: 'query',
    },
    zoom: true,
  },
};

const [Grid, gridApi] = useVbenVxeGrid<AIToolDefinitionRecordDto>({
  formOptions,
  gridOptions,
});

const [AIToolDefinitionModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./AIToolDefinitionModal.vue'),
  ),
});

function onCreate() {
  modalApi.setData({});
  modalApi.open();
}

function onUpdate(row: AIToolDefinitionRecordDto) {
  modalApi.setData(row);
  modalApi.open();
}

async function onDelete(row: AIToolDefinitionRecordDto) {
  Modal.confirm({
    centered: true,
    content: $t('AbpUi.ItemWillBeDeletedMessage'),
    onOk: async () => {
      await deleteApi(row.id);
      message.success($t('AbpUi.DeletedSuccessfully'));
      gridApi.query();
    },
    title: $t('AbpUi.AreYouSure'),
  });
}
</script>

<template>
  <Grid :table-title="$t('AIManagement.Tools')">
    <template #toolbar-tools>
      <Button
        v-access:code="[AIToolDefinitionPermissions.Create]"
        :icon="h(PlusOutlined)"
        type="primary"
        @click="onCreate"
      >
        {{ $t('AIManagement.Tools:New') }}
      </Button>
    </template>
    <template #isEnabled="{ row }">
      <div class="flex flex-row justify-center">
        <CheckOutlined v-if="row.isEnabled" class="text-green-500" />
        <CloseOutlined v-else class="text-red-500" />
      </div>
    </template>
    <template #isGlobal="{ row }">
      <div class="flex flex-row justify-center">
        <CheckOutlined v-if="row.isGlobal" class="text-green-500" />
        <CloseOutlined v-else class="text-red-500" />
      </div>
    </template>
    <template #actions="{ row }">
      <div class="flex flex-row">
        <Button
          v-access:code="[AIToolDefinitionPermissions.Default]"
          :icon="h(EditOutlined)"
          block
          type="link"
          @click="onUpdate(row)"
        >
          {{ $t('AbpUi.Edit') }}
        </Button>
        <Button
          v-access:code="[AIToolDefinitionPermissions.Delete]"
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
  <AIToolDefinitionModal @change="() => gridApi.query()" />
</template>

<style lang="scss" scoped></style>
