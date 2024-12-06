<script setup lang="ts">
import type { VbenFormProps, VxeGridListeners, VxeGridProps } from '@abp/ui';
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface';

import type { IdentityRoleDto } from '../../types/roles';

import { defineAsyncComponent, h } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenModal } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { useAbpStore } from '@abp/core';
import { PermissionModal } from '@abp/permission';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  EllipsisOutlined,
} from '@ant-design/icons-vue';
import { Button, Dropdown, Menu, Modal, Tag } from 'ant-design-vue';

import { deleteApi, getPagedListApi } from '../../api/roles';
import { IdentitRolePermissions } from '../../constants/permissions';

defineOptions({
  name: 'RoleTable',
});

const MenuItem = Menu.Item;
const MenuOutlined = createIconifyIcon('heroicons-outline:menu-alt-3');
const ClaimOutlined = createIconifyIcon('la:id-card-solid');
const PermissionsOutlined = createIconifyIcon('icon-park-outline:permissions');
const RoleModal = defineAsyncComponent(() => import('./RoleModal.vue'));

const abpStore = useAbpStore();
const { hasAccessByCodes } = useAccess();
const [RolePermissionModal, permissionModalApi] = useVbenModal({
  connectedComponent: PermissionModal,
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
  roleModalApi.setData({
    values: row,
  });
  roleModalApi.open();
};

const handleDelete = (row: IdentityRoleDto) => {
  Modal.confirm({
    centered: true,
    content: $t('AbpIdentity.RoleDeletionConfirmationMessage', [row.name]),
    onOk: () => {
      return deleteApi(row.id).then(() => query());
    },
    title: $t('AbpUi.AreYouSure'),
  });
};

const handleMenuClick = async (row: IdentityRoleDto, info: MenuInfo) => {
  switch (info.key) {
    case 'permissions': {
      const roles = abpStore.application?.currentUser.roles ?? [];
      permissionModalApi.setData({
        displayName: row.name,
        providerKey: row.name,
        providerName: 'R',
        readonly: roles.includes(row.name),
      });
      permissionModalApi.open();
      break;
    }
  }
};
</script>

<template>
  <Grid :table-title="$t('AbpIdentity.Roles')">
    <template #toolbar-tools>
      <Button
        type="primary"
        v-access:code="[IdentitRolePermissions.Create]"
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
        <div class="basis-1/3">
          <Button
            :icon="h(EditOutlined)"
            block
            type="link"
            v-access:code="[IdentitRolePermissions.Update]"
            @click="handleEdit(row)"
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
            v-access:code="[IdentitRolePermissions.Delete]"
            @click="handleDelete(row)"
          >
            {{ $t('AbpUi.Delete') }}
          </Button>
        </div>
        <div class="basis-1/3">
          <Dropdown>
            <template #overlay>
              <Menu @click="(info) => handleMenuClick(row, info)">
                <MenuItem
                  v-if="
                    hasAccessByCodes([IdentitRolePermissions.ManagePermissions])
                  "
                  key="permissions"
                  :icon="h(PermissionsOutlined)"
                >
                  {{ $t('AbpPermissionManagement.Permissions') }}
                </MenuItem>
                <MenuItem
                  v-if="hasAccessByCodes([IdentitRolePermissions.ManageClaims])"
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
              </Menu>
            </template>
            <Button :icon="h(EllipsisOutlined)" type="link" />
          </Dropdown>
        </div>
      </div>
    </template>
  </Grid>
  <RoleEditModal @change="() => query()" />
  <RolePermissionModal />
</template>

<style lang="scss" scoped></style>
