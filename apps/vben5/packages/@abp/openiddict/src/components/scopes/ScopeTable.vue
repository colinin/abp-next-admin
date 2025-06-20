<script setup lang="ts">
import type { VxeGridProps } from '@abp/ui';
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface';

import type { VbenFormProps } from '@vben/common-ui';

import type { OpenIddictScopeDto } from '../../types/scopes';

import { defineAsyncComponent, h } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenDrawer, useVbenModal } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { AuditLogPermissions, EntityChangeDrawer } from '@abp/auditing';
import { formatToDateTime, useFeatures } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  EllipsisOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, Dropdown, Menu, message, Modal } from 'ant-design-vue';

import { useScopesApi } from '../../api/useScopesApi';
import { ScopesPermissions } from '../../constants/permissions';

defineOptions({
  name: 'ScopeTable',
});

const MenuItem = Menu.Item;

const AuditLogIcon = createIconifyIcon('fluent-mdl2:compliance-audit');
const CheckIcon = createIconifyIcon('ant-design:check-outlined');
const CloseIcon = createIconifyIcon('ant-design:close-outlined');

const { isEnabled } = useFeatures();
const { hasAccessByCodes } = useAccess();
const { deleteApi, getPagedListApi } = useScopesApi();

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
const [ScopeChangeDrawer, scopeChangeDrawerApi] = useVbenDrawer({
  connectedComponent: EntityChangeDrawer,
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
      message.success($t('AbpUi.DeletedSuccessfully'));
      query();
    },
    title: $t('AbpUi.AreYouSure'),
  });
};
const onMenuClick = (row: OpenIddictScopeDto, info: MenuInfo) => {
  switch (info.key) {
    case 'entity-changes': {
      scopeChangeDrawerApi.setData({
        entityId: row.id,
        entityTypeFullName: 'Volo.Abp.OpenIddict.Scopes.OpenIddictScope',
        subject: row.name,
      });
      scopeChangeDrawerApi.open();
      break;
    }
  }
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
        <Button
          :icon="h(EditOutlined)"
          block
          type="link"
          v-access:code="[ScopesPermissions.Update]"
          @click="onUpdate(row)"
        >
          {{ $t('AbpUi.Edit') }}
        </Button>
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
        <Dropdown v-if="isEnabled('AbpAuditing.Logging.AuditLog')">
          <template #overlay>
            <Menu @click="(info) => onMenuClick(row, info)">
              <MenuItem
                v-if="hasAccessByCodes([AuditLogPermissions.Default])"
                key="entity-changes"
                :icon="h(AuditLogIcon)"
              >
                {{ $t('AbpAuditLogging.EntitiesChanged') }}
              </MenuItem>
            </Menu>
          </template>
          <Button :icon="h(EllipsisOutlined)" type="link" />
        </Dropdown>
      </div>
    </template>
  </Grid>
  <ScopeModal @change="() => query()" />
  <ScopeChangeDrawer />
</template>

<style lang="scss" scoped></style>
