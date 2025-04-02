<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface';

import type { VbenFormProps } from '@vben/common-ui';

import type { TenantDto } from '../../types/tenants';

import { defineAsyncComponent, h } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenDrawer, useVbenModal } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { AuditLogPermissions, EntityChangeDrawer } from '@abp/auditing';
import { useFeatures } from '@abp/core';
import { FeatureModal } from '@abp/features';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  EllipsisOutlined,
} from '@ant-design/icons-vue';
import { Button, Dropdown, Menu, message, Modal } from 'ant-design-vue';

import { useTenantsApi } from '../../api/useTenantsApi';
import { TenantsPermissions } from '../../constants/permissions';

defineOptions({
  name: 'EditionTable',
});

const MenuItem = Menu.Item;
const CheckIcon = createIconifyIcon('ant-design:check-outlined');
const CloseIcon = createIconifyIcon('ant-design:close-outlined');
const AuditLogIcon = createIconifyIcon('fluent-mdl2:compliance-audit');
const ConnectionIcon = createIconifyIcon('mdi:connection');
const FeatureIcon = createIconifyIcon('pajamas:feature-flag');

const { isEnabled } = useFeatures();
const { hasAccessByCodes } = useAccess();
const { cancel, deleteApi, getPagedListApi } = useTenantsApi();

const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: false,
  schema: [
    {
      component: 'Input',
      componentProps: {
        allowClear: true,
        autocomplete: 'off',
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

const gridOptions: VxeGridProps<TenantDto> = {
  columns: [
    {
      align: 'center',
      field: 'isActive',
      slots: { default: 'isActive' },
      title: $t('AbpSaas.DisplayName:IsActive'),
      width: 120,
    },
    {
      align: 'left',
      field: 'name',
      title: $t('AbpSaas.DisplayName:Name'),
    },
    {
      align: 'left',
      field: 'editionName',
      title: $t('AbpSaas.DisplayName:EditionName'),
      width: 160,
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      visible: hasAccessByCodes([
        TenantsPermissions.Update,
        TenantsPermissions.Delete,
      ]),
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

const gridEvents: VxeGridListeners<TenantDto> = {
  cellClick: () => {},
};
const [TenantModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(() => import('./TenantModal.vue')),
});
const [TenantConnectionStringsModal, connectionStringsModalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./ConnectionStringsModal.vue'),
  ),
});
const [TenantChangeDrawer, entityChangeDrawerApi] = useVbenDrawer({
  connectedComponent: EntityChangeDrawer,
});
const [TenantFeatureModal, tenantFeatureModalApi] = useVbenModal({
  connectedComponent: FeatureModal,
});
const [Grid, { query }] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const onCreate = () => {
  modalApi.setData({});
  modalApi.open();
};

const onUpdate = (row: TenantDto) => {
  modalApi.setData(row);
  modalApi.open();
};

const onDelete = (row: TenantDto) => {
  Modal.confirm({
    centered: true,
    content: $t('AbpSaas.TenantDeletionConfirmationMessage', [row.name]),
    onCancel: () => {
      cancel();
    },
    onOk: async () => {
      await deleteApi(row.id);
      message.success($t('AbpUi.DeletedSuccessfully'));
      query();
    },
    title: $t('AbpUi.AreYouSure'),
  });
};

const onMenuClick = (row: TenantDto, info: MenuInfo) => {
  switch (info.key) {
    case 'connection-strings': {
      connectionStringsModalApi.setData(row);
      connectionStringsModalApi.open();
      break;
    }
    case 'entity-changes': {
      entityChangeDrawerApi.setData({
        entityId: row.id,
        entityTypeFullName: 'LINGYUN.Abp.Saas.Tenant',
        subject: row.name,
      });
      entityChangeDrawerApi.open();
      break;
    }
    case 'features': {
      tenantFeatureModalApi.setData({
        displayName: row.name,
        providerKey: row.id,
        providerName: 'T',
      });
      tenantFeatureModalApi.open();
    }
  }
};
</script>

<template>
  <Grid :table-title="$t('AbpSaas.Tenants')">
    <template #toolbar-tools>
      <Button
        type="primary"
        v-access:code="[TenantsPermissions.Create]"
        @click="onCreate"
      >
        {{ $t('AbpSaas.NewTenant') }}
      </Button>
    </template>
    <template #isActive="{ row }">
      <div class="flex flex-row justify-center">
        <CheckIcon v-if="row.isActive" class="text-green-500" />
        <CloseIcon v-else class="text-red-500" />
      </div>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          :icon="h(EditOutlined)"
          block
          type="link"
          v-access:code="[TenantsPermissions.Update]"
          @click="onUpdate(row)"
        >
          {{ $t('AbpUi.Edit') }}
        </Button>
        <Button
          :icon="h(DeleteOutlined)"
          block
          danger
          type="link"
          v-access:code="[TenantsPermissions.Delete]"
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
        <Dropdown>
          <template #overlay>
            <Menu @click="(info) => onMenuClick(row, info)">
              <MenuItem
                v-if="
                  hasAccessByCodes([TenantsPermissions.ManageConnectionStrings])
                "
                key="connection-strings"
                :icon="h(ConnectionIcon)"
              >
                {{ $t('AbpSaas.ConnectionStrings') }}
              </MenuItem>
              <MenuItem
                v-if="hasAccessByCodes([TenantsPermissions.ManageFeatures])"
                key="features"
                :icon="h(FeatureIcon)"
              >
                {{ $t('AbpSaas.ManageFeatures') }}
              </MenuItem>
              <MenuItem
                v-if="
                  isEnabled('AbpAuditing.Logging.AuditLog') &&
                  hasAccessByCodes([AuditLogPermissions.Default])
                "
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
  <TenantModal @change="() => query()" />
  <TenantConnectionStringsModal />
  <TenantFeatureModal />
  <TenantChangeDrawer />
</template>

<style lang="scss" scoped></style>
