<script setup lang="ts">
import type { VbenFormProps, VxeGridListeners, VxeGridProps } from '@abp/ui';
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface';

import type { IdentityClaimTypeDto } from '../../types/claim-types';

import { defineAsyncComponent, h } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenDrawer, useVbenModal } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { AuditLogPermissions, EntityChangeDrawer } from '@abp/auditing';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  EllipsisOutlined,
} from '@ant-design/icons-vue';
import { Button, Dropdown, Menu, message, Modal } from 'ant-design-vue';

import { useClaimTypesApi } from '../../api/useClaimTypesApi';
import { IdentityClaimTypePermissions } from '../../constants/permissions';
import { ValueType } from '../../types/claim-types';

defineOptions({
  name: 'ClaimTypeTable',
});

const MenuItem = Menu.Item;
const ClaimTypeModal = defineAsyncComponent(
  () => import('./ClaimTypeModal.vue'),
);
const CheckIcon = createIconifyIcon('ant-design:check-outlined');
const CloseIcon = createIconifyIcon('ant-design:close-outlined');
const AuditLogIcon = createIconifyIcon('fluent-mdl2:compliance-audit');

const { hasAccessByCodes } = useAccess();
const { cancel, deleteApi, getPagedListApi } = useClaimTypesApi();

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

const gridOptions: VxeGridProps<IdentityClaimTypeDto> = {
  columns: [
    {
      align: 'left',
      field: 'name',
      minWidth: 120,
      title: $t('AbpIdentity.IdentityClaim:Name'),
    },
    {
      align: 'left',
      field: 'valueType',
      formatter: ({ row }) => {
        switch (row.valueType) {
          case ValueType.Boolean: {
            return 'Boolean';
          }
          case ValueType.DateTime: {
            return 'DateTime';
          }
          case ValueType.Int: {
            return 'Int';
          }
          case ValueType.String: {
            return 'String';
          }
          default: {
            return '';
          }
        }
      },
      title: $t('AbpIdentity.IdentityClaim:ValueType'),
    },
    {
      align: 'left',
      field: 'regex',
      title: $t('AbpIdentity.IdentityClaim:Regex'),
    },
    {
      align: 'center',
      field: 'required',
      slots: { default: 'required' },
      title: $t('AbpIdentity.IdentityClaim:Required'),
    },
    {
      align: 'center',
      field: 'isStatic',
      slots: { default: 'static' },
      title: $t('AbpIdentity.IdentityClaim:IsStatic'),
    },
    {
      align: 'left',
      field: 'description',
      title: $t('AbpIdentity.IdentityClaim:Description'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      visible: hasAccessByCodes([
        IdentityClaimTypePermissions.Update,
        IdentityClaimTypePermissions.Delete,
      ]),
      width: 180,
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

const gridEvents: VxeGridListeners<IdentityClaimTypeDto> = {
  cellClick: () => {},
};
const [ClaimTypeEditModal, roleModalApi] = useVbenModal({
  connectedComponent: ClaimTypeModal,
});
const [Grid, { query }] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});
const [ClaimTypeChangeDrawer, roleChangeDrawerApi] = useVbenDrawer({
  connectedComponent: EntityChangeDrawer,
});

const onCreate = () => {
  roleModalApi.setData({});
  roleModalApi.open();
};

const onUpdate = (row: IdentityClaimTypeDto) => {
  roleModalApi.setData(row);
  roleModalApi.open();
};

const onDelete = (row: IdentityClaimTypeDto) => {
  Modal.confirm({
    centered: true,
    content: $t('AbpIdentity.WillDeleteClaim', [row.name]),
    onCancel: () => {
      cancel('User closed delete modal.');
    },
    onOk: async () => {
      await deleteApi(row.id);
      message.success($t('AbpUi.SuccessfullyDeleted'));
      query();
    },
    title: $t('AbpUi.AreYouSure'),
  });
};

const onMenuClick = (row: IdentityClaimTypeDto, info: MenuInfo) => {
  switch (info.key) {
    case 'entity-changes': {
      roleChangeDrawerApi.setData({
        entityId: row.id,
        entityTypeFullName: 'Volo.Abp.Identity.IdentityRole',
        subject: row.name,
      });
      roleChangeDrawerApi.open();
      break;
    }
  }
};
</script>

<template>
  <Grid :table-title="$t('AbpIdentity.DisplayName:ClaimType')">
    <template #toolbar-tools>
      <Button
        type="primary"
        v-access:code="[IdentityClaimTypePermissions.Create]"
        @click="onCreate"
      >
        {{ $t('AbpIdentity.IdentityClaim:New') }}
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
          v-access:code="[IdentityClaimTypePermissions.Update]"
          @click="onUpdate(row)"
        >
          {{ $t('AbpUi.Edit') }}
        </Button>
        <Button
          v-if="row.isStatic === false"
          :icon="h(DeleteOutlined)"
          block
          danger
          type="link"
          v-access:code="[IdentityClaimTypePermissions.Delete]"
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
        <Dropdown>
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
  <ClaimTypeEditModal @change="() => query()" />
  <ClaimTypeChangeDrawer />
</template>

<style lang="scss" scoped></style>
