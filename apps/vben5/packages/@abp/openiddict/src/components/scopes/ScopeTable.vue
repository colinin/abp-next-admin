<script setup lang="ts">
import type { VbenFormProps, VxeGridProps } from '@abp/ui';

import type { OpenIddictScopeDto } from '../../types/scopes';

import { defineAsyncComponent, h } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { formatToDateTime } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, message, Modal } from 'ant-design-vue';

import { deleteApi, getPagedListApi } from '../../api/scopes';
import { ScopesPermissions } from '../../constants/permissions';

defineOptions({
  name: 'ScopeTable',
});

const CheckIcon = createIconifyIcon('ant-design:check-outlined');
const CloseIcon = createIconifyIcon('ant-design:close-outlined');

const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: false,
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

const gridOptions: VxeGridProps<OpenIddictScopeDto> = {
  columns: [
    {
      align: 'left',
      field: 'name',
      minWidth: 150,
      title: $t('AbpOpenIddict.DisplayName:Name'),
    },
    {
      align: 'left',
      field: 'displayName',
      minWidth: 150,
      title: $t('AbpOpenIddict.DisplayName:DisplayName'),
    },
    {
      align: 'center',
      field: 'description',
      minWidth: 200,
      title: $t('AbpOpenIddict.DisplayName:Description'),
    },
    {
      align: 'center',
      field: 'creationTime',
      formatter: ({ cellValue }) => {
        return cellValue ? formatToDateTime(cellValue) : cellValue;
      },
      minWidth: 120,
      title: $t('AbpOpenIddict.DisplayName:CreationDate'),
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
const [ScopeModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(() => import('./ScopeModal.vue')),
});

const [Grid, { query }] = useVbenVxeGrid({
  formOptions,
  gridOptions,
});

const onCreate = () => {
  modalApi.setData({});
  modalApi.open();
};

const onUpdate = (row: OpenIddictScopeDto) => {
  modalApi.setData(row);
  modalApi.open();
};

const onDelete = (row: OpenIddictScopeDto) => {
  Modal.confirm({
    centered: true,
    content: `${$t('AbpUi.ItemWillBeDeletedMessageWithFormat', [row.name])}`,
    onOk: async () => {
      await deleteApi(row.id);
      message.success($t('AbpUi.SuccessfullyDeleted'));
      query();
    },
    title: $t('AbpUi.AreYouSure'),
  });
};
</script>

<template>
  <Grid :table-title="$t('AbpOpenIddict.Scopes')">
    <template #toolbar-tools>
      <Button
        :icon="h(PlusOutlined)"
        type="primary"
        v-access:code="[ScopesPermissions.Create]"
        @click="onCreate"
      >
        {{ $t('AbpOpenIddict.Scopes:AddNew') }}
      </Button>
    </template>
    <template #required="{ row }">
      <div class="flex flex-row justify-center">
        <CheckIcon v-if="row.required" class="text-green-500" />
        <CloseIcon v-else class="text-red-500" />
      </div>
    </template>
    <template #static="{ row }">
      <div class="flex flex-row justify-center">
        <CheckIcon v-if="row.isStatic" class="text-green-500" />
        <CloseIcon v-else class="text-red-500" />
      </div>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <div class="basis-1/3">
          <Button
            :icon="h(EditOutlined)"
            block
            type="link"
            v-access:code="[ScopesPermissions.Update]"
            @click="onUpdate(row)"
          >
            {{ $t('AbpUi.Edit') }}
          </Button>
        </div>
        <div class="basis-1/3">
          <Button
            :icon="h(DeleteOutlined)"
            block
            danger
            type="link"
            v-access:code="[ScopesPermissions.Delete]"
            @click="onDelete(row)"
          >
            {{ $t('AbpUi.Delete') }}
          </Button>
        </div>
      </div>
    </template>
  </Grid>
  <ScopeModal @change="() => query()" />
</template>

<style lang="scss" scoped></style>
