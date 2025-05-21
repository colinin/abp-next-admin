<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface';

import type { VbenFormProps } from '@vben/common-ui';

import type { IdentityRoleDto } from '../../types/roles';

import { defineAsyncComponent, h } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenDrawer, useVbenModal } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { AuditLogPermissions, EntityChangeDrawer } from '@abp/auditing';
import { Events, useAbpStore, useEventBus, useFeatures } from '@abp/core';
import { PermissionModal } from '@abp/permissions';
import { MenuAllotModal } from '@abp/platform';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  EllipsisOutlined,
} from '@ant-design/icons-vue';
import { Button, Dropdown, Menu, message, Modal, Tag } from 'ant-design-vue';

import { useRolesApi } from '../../api/useRolesApi';
import { IdentityRolePermissions } from '../../constants/permissions';

defineOptions({
  name: 'RoleTable',
});

const MenuItem = Menu.Item;
const MenuOutlined = createIconifyIcon('heroicons-outline:menu-alt-3');
const ClaimOutlined = createIconifyIcon('la:id-card-solid');
const PermissionsOutlined = createIconifyIcon('icon-park-outline:permissions');
const AuditLogIcon = createIconifyIcon('fluent-mdl2:compliance-audit');

const RoleModal = defineAsyncComponent(() => import('./RoleModal.vue'));
const ClaimModal = defineAsyncComponent(() => import('./RoleClaimModal.vue'));
const RuleModal = defineAsyncComponent(() => import('./RoleRuleDrawer.vue'));

const abpStore = useAbpStore();
const { publish } = useEventBus();
const { isEnabled } = useFeatures();
const { hasAccessByCodes } = useAccess();
const { cancel, deleteApi, getPagedListApi } = useRolesApi();

const [RolePermissionModal, permissionModalApi] = useVbenModal({
  connectedComponent: PermissionModal,
});
const [RoleClaimModal, claimModalApi] = useVbenModal({
  connectedComponent: ClaimModal,
});
const [RoleMenuModal, menuModalApi] = useVbenModal({
  connectedComponent: MenuAllotModal,
});
const [RoleRuleDrawer, roleRuleDrawerApi] = useVbenDrawer({
  connectedComponent: RuleModal,
});
const [RoleChangeDrawer, roleChangeDrawerApi] = useVbenDrawer({
  connectedComponent: EntityChangeDrawer,
});

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

const gridOptions: VxeGridProps<IdentityRoleDto> = {
  columns: [
    {
      align: 'left',
      field: 'name',
      slots: { default: 'name' },
      title: $t('AbpIdentity.DisplayName:RoleName'),
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

const gridEvents: VxeGridListeners<IdentityRoleDto> = {
  cellClick: () => {},
};
const [RoleEditModal, roleModalApi] = useVbenModal({
  connectedComponent: RoleModal,
});
const [Grid, { query }] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const handleAdd = () => {
  roleModalApi.setData({});
  roleModalApi.open();
};

const handleEdit = (row: IdentityRoleDto) => {
  roleModalApi.setData(row);
  roleModalApi.open();
};

const handleDelete = (row: IdentityRoleDto) => {
  Modal.confirm({
    centered: true,
    content: $t('AbpIdentity.RoleDeletionConfirmationMessage', [row.name]),
    onCancel: () => {
      cancel('User closed cancel delete modal.');
    },
    onOk: async () => {
      await deleteApi(row.id);
      message.success($t('AbpUi.DeletedSuccessfully'));
      query();
    },
    title: $t('AbpUi.AreYouSure'),
  });
};

const handleMenuClick = async (row: IdentityRoleDto, info: MenuInfo) => {
  switch (info.key) {
    case 'claims': {
      claimModalApi.setData(row);
      claimModalApi.open();
      break;
    }
    case 'entity-changes': {
      roleChangeDrawerApi.setData({
        entityId: row.id,
        entityTypeFullName: 'Volo.Abp.Identity.IdentityRole',
        subject: row.name,
      });
      roleChangeDrawerApi.open();
      break;
    }
    case 'entity-rules': {
      roleRuleDrawerApi.setData(row);
      roleRuleDrawerApi.open();
      break;
    }
    case 'menus': {
      menuModalApi.setData({
        identity: row.name,
      });
      menuModalApi.open();
      break;
    }
    case 'permissions': {
      permissionModalApi.setData({
        displayName: row.name,
        providerKey: row.name,
        providerName: 'R',
      });
      permissionModalApi.open();
      break;
    }
  }
};

function onPermissionChange(_name: string, key: string) {
  const roles = abpStore.application?.currentUser.roles ?? [];
  if (roles.includes(key)) {
    publish(Events.PermissionChange);
  }
}
</script>

<template>
  <Grid :table-title="$t('AbpIdentity.Roles')">
    <template #toolbar-tools>
      <Button
        type="primary"
        v-access:code="[IdentityRolePermissions.Create]"
        @click="handleAdd"
      >
        {{ $t('AbpIdentity.NewRole') }}
      </Button>
    </template>
    <template #name="{ row }">
      <Tag v-if="row.isStatic" color="#8baac4" style="margin-right: 5px">
        {{ $t('AbpIdentity.Static') }}
      </Tag>
      <Tag v-if="row.isDefault" color="#108ee9" style="margin-right: 5px">
        {{ $t('AbpIdentity.DisplayName:IsDefault') }}
      </Tag>
      <Tag v-if="row.isPublic" color="#87d068" style="margin-right: 5px">
        {{ $t('AbpIdentity.Public') }}
      </Tag>
      <span>{{ row.name }}</span>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          :icon="h(EditOutlined)"
          block
          type="link"
          v-access:code="[IdentityRolePermissions.Update]"
          @click="handleEdit(row)"
        >
          {{ $t('AbpUi.Edit') }}
        </Button>
        <Button
          v-if="row.isStatic === false"
          :icon="h(DeleteOutlined)"
          block
          danger
          type="link"
          v-access:code="[IdentityRolePermissions.Delete]"
          @click="handleDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
        <Dropdown>
          <template #overlay>
            <Menu @click="(info) => handleMenuClick(row, info)">
              <MenuItem
                v-if="
                  hasAccessByCodes([IdentityRolePermissions.ManagePermissions])
                "
                key="permissions"
                :icon="h(PermissionsOutlined)"
              >
                {{ $t('AbpPermissionManagement.Permissions') }}
              </MenuItem>
              <MenuItem
                v-if="hasAccessByCodes([IdentityRolePermissions.ManageClaims])"
                key="claims"
                :icon="h(ClaimOutlined)"
              >
                {{ $t('AbpIdentity.ManageClaim') }}
              </MenuItem>
              <MenuItem
                v-if="hasAccessByCodes(['Platform.Menu.ManageRoles'])"
                key="menus"
                :icon="h(MenuOutlined)"
              >
                {{ $t('AppPlatform.Menu:Manage') }}
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
              <MenuItem key="entity-rules">
                {{ '数据权限' }}
              </MenuItem>
            </Menu>
          </template>
          <Button :icon="h(EllipsisOutlined)" type="link" />
        </Dropdown>
      </div>
    </template>
  </Grid>
  <RoleEditModal @change="() => query()" />
  <RoleClaimModal @change="query" />
  <RolePermissionModal @change="onPermissionChange" />
  <RoleChangeDrawer />
  <RoleRuleDrawer />
  <RoleMenuModal subject="role" />
</template>

<style lang="scss" scoped></style>
