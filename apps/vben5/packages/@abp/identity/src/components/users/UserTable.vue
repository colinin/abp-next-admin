<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface';

import type { VbenFormProps } from '@vben/common-ui';

import type { IdentityUserDto } from '../../types/users';

import { computed, defineAsyncComponent, h } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenDrawer, useVbenModal } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { AuditLogPermissions, EntityChangeDrawer } from '@abp/auditing';
import { formatToDateTime, useAbpStore, useFeatures } from '@abp/core';
import { PermissionModal } from '@abp/permissions';
import { MenuAllotModal } from '@abp/platform';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  EllipsisOutlined,
  LockOutlined,
  PlusOutlined,
  UnlockOutlined,
} from '@ant-design/icons-vue';
import { Button, Dropdown, Menu, message, Modal, Tag } from 'ant-design-vue';

import { useUsersApi } from '../../api/useUsersApi';
import {
  IdentitySessionPermissions,
  IdentityUserPermissions,
} from '../../constants/permissions';

defineOptions({
  name: 'UserTable',
});

const UserModal = defineAsyncComponent(() => import('./UserModal.vue'));
const LockModal = defineAsyncComponent(() => import('./UserLockModal.vue'));
const ClaimModal = defineAsyncComponent(() => import('./UserClaimModal.vue'));
const PasswordModal = defineAsyncComponent(
  () => import('./UserPasswordModal.vue'),
);

const MenuItem = Menu.Item;
const CheckIcon = createIconifyIcon('ant-design:check-outlined');
const CloseIcon = createIconifyIcon('ant-design:close-outlined');
const PasswordIcon = createIconifyIcon('carbon:password');
const MenuOutlined = createIconifyIcon('heroicons-outline:menu-alt-3');
const ClaimOutlined = createIconifyIcon('la:id-card-solid');
const PermissionsOutlined = createIconifyIcon('icon-park-outline:permissions');
const SessionIcon = createIconifyIcon('carbon:prompt-session');
const AuditLogIcon = createIconifyIcon('fluent-mdl2:compliance-audit');

const getLockEnd = computed(() => {
  return (row: IdentityUserDto) => {
    if (row.lockoutEnd) {
      const lockTime = new Date(row.lockoutEnd);
      if (lockTime) {
        // 锁定时间高于当前时间不显示
        const nowTime = new Date();
        return lockTime < nowTime;
      }
    }
    return true;
  };
});

const abpStore = useAbpStore();
const { isEnabled } = useFeatures();
const { hasAccessByCodes } = useAccess();
const { cancel, deleteApi, getPagedListApi, unLockApi } = useUsersApi();

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

const gridOptions: VxeGridProps<IdentityUserDto> = {
  columns: [
    {
      field: 'isActive',
      slots: { default: 'active' },
      title: $t('AbpIdentity.DisplayName:IsActive'),
    },
    {
      field: 'userName',
      minWidth: '100px',
      title: $t('AbpIdentity.DisplayName:UserName'),
    },
    {
      align: 'left',
      field: 'email',
      minWidth: '120px',
      slots: { default: 'email' },
      title: $t('AbpIdentity.DisplayName:Email'),
    },
    { field: 'surname', title: $t('AbpIdentity.DisplayName:Surname') },
    { field: 'name', title: $t('AbpIdentity.DisplayName:Name') },
    {
      align: 'left',
      field: 'phoneNumber',
      slots: { default: 'phoneNumber' },
      title: $t('AbpIdentity.DisplayName:PhoneNumber'),
    },
    {
      field: 'lockoutEnd',
      formatter: ({ cellValue }) => {
        return cellValue ? formatToDateTime(cellValue) : '';
      },
      title: $t('AbpIdentity.LockoutEnd'),
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

const gridEvents: VxeGridListeners<IdentityUserDto> = {
  cellClick: () => {},
};
const [UserEditModal, userModalApi] = useVbenModal({
  connectedComponent: UserModal,
});
const [UserLockModal, lockModalApi] = useVbenModal({
  connectedComponent: LockModal,
});
const [UserPasswordModal, pwdModalApi] = useVbenModal({
  connectedComponent: PasswordModal,
});
const [UserClaimModal, claimModalApi] = useVbenModal({
  connectedComponent: ClaimModal,
});
const [UserPermissionModal, permissionModalApi] = useVbenModal({
  connectedComponent: PermissionModal,
});
const [UserMenuModal, menuModalApi] = useVbenModal({
  connectedComponent: MenuAllotModal,
});
const [UserChangeDrawer, userChangeDrawerApi] = useVbenDrawer({
  connectedComponent: EntityChangeDrawer,
});
const [UserSessionDrawer, userSessionDrawerApi] = useVbenDrawer({
  connectedComponent: defineAsyncComponent(
    () => import('./UserSessionDrawer.vue'),
  ),
});
const [Grid, { query }] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const handleAdd = () => {
  userModalApi.setData({});
  userModalApi.open();
};

const handleEdit = (row: IdentityUserDto) => {
  userModalApi.setData(row);
  userModalApi.open();
};

const handleDelete = (row: IdentityUserDto) => {
  Modal.confirm({
    centered: true,
    content: $t('AbpIdentity.UserDeletionConfirmationMessage', [row.userName]),
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

const handleUnlock = async (row: IdentityUserDto) => {
  await unLockApi(row.id);
  await query();
};

const handleMenuClick = async (row: IdentityUserDto, info: MenuInfo) => {
  switch (info.key) {
    case 'claims': {
      claimModalApi.setData(row);
      claimModalApi.open();
      break;
    }
    case 'entity-changes': {
      userChangeDrawerApi.setData({
        entityId: row.id,
        entityTypeFullName: 'Volo.Abp.Identity.IdentityUser',
        subject: row.userName,
      });
      userChangeDrawerApi.open();
      break;
    }
    case 'lock': {
      lockModalApi.setData(row);
      lockModalApi.open();
      break;
    }
    case 'menus': {
      menuModalApi.setData({
        identity: row.id,
      });
      menuModalApi.open();
      break;
    }
    case 'password': {
      pwdModalApi.setData(row);
      pwdModalApi.open();
      break;
    }
    case 'permissions': {
      const userId = abpStore.application?.currentUser.id;
      permissionModalApi.setData({
        displayName: row.userName,
        providerKey: row.id,
        providerName: 'U',
        readonly: userId === row.id,
      });
      permissionModalApi.open();
      break;
    }
    case 'session': {
      userSessionDrawerApi.setData(row);
      userSessionDrawerApi.open();
      break;
    }
    case 'unlock': {
      handleUnlock(row);
      break;
    }
  }
};
</script>

<template>
  <Grid :table-title="$t('AbpIdentity.Users')">
    <template #toolbar-tools>
      <Button
        :icon="h(PlusOutlined)"
        type="primary"
        v-access:code="[IdentityUserPermissions.Create]"
        @click="handleAdd"
      >
        {{ $t('AbpIdentity.NewUser') }}
      </Button>
    </template>
    <template #active="{ row }">
      <div class="flex flex-row justify-center">
        <div :class="row.isActive ? 'text-green-600' : 'text-red-600'">
          <CheckIcon v-if="row.isActive" />
          <CloseIcon v-else />
        </div>
      </div>
    </template>
    <template #email="{ row }">
      <div class="flex flex-row">
        <Tag v-if="row.emailConfirmed" color="success">
          {{ $t('abp.account.settings.security.verified') }}
        </Tag>
        <Tag v-else color="warning">
          {{ $t('abp.account.settings.security.unVerified') }}
        </Tag>
        <span>{{ row.email }}</span>
      </div>
    </template>
    <template #phoneNumber="{ row }">
      <div class="flex flex-row">
        <div v-if="row.phoneNumber">
          <Tag v-if="row.phoneNumberConfirmed" color="success">
            {{ $t('abp.account.settings.security.verified') }}
          </Tag>
          <Tag v-else color="warning">
            {{ $t('abp.account.settings.security.unVerified') }}
          </Tag>
        </div>
        <span>{{ row.phoneNumber }}</span>
      </div>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <div class="basis-1/3">
          <Button
            :icon="h(EditOutlined)"
            block
            type="link"
            v-access:code="[IdentityUserPermissions.Update]"
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
            v-access:code="[IdentityUserPermissions.Delete]"
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
                    hasAccessByCodes([IdentityUserPermissions.Update]) &&
                    row.isActive &&
                    getLockEnd(row)
                  "
                  key="lock"
                  :icon="h(LockOutlined)"
                >
                  {{ $t('AbpIdentity.Lock') }}
                </MenuItem>
                <MenuItem
                  v-if="
                    hasAccessByCodes([IdentityUserPermissions.Update]) &&
                    row.isActive &&
                    !getLockEnd(row)
                  "
                  key="unlock"
                  :icon="h(UnlockOutlined)"
                >
                  {{ $t('AbpIdentity.UnLock') }}
                </MenuItem>
                <MenuItem
                  v-if="
                    hasAccessByCodes([
                      IdentityUserPermissions.ManagePermissions,
                    ])
                  "
                  key="permissions"
                  :icon="h(PermissionsOutlined)"
                >
                  {{ $t('AbpPermissionManagement.Permissions') }}
                </MenuItem>
                <MenuItem
                  v-if="hasAccessByCodes([IdentitySessionPermissions.Default])"
                  key="session"
                  :icon="h(SessionIcon)"
                >
                  {{ $t('AbpIdentity.IdentitySessions') }}
                </MenuItem>
                <MenuItem
                  v-if="
                    hasAccessByCodes([IdentityUserPermissions.ManageClaims])
                  "
                  key="claims"
                  :icon="h(ClaimOutlined)"
                >
                  {{ $t('AbpIdentity.ManageClaim') }}
                </MenuItem>
                <MenuItem
                  v-if="hasAccessByCodes([IdentityUserPermissions.Update])"
                  key="password"
                  :icon="h(PasswordIcon)"
                >
                  {{ $t('AbpIdentity.SetPassword') }}
                </MenuItem>
                <MenuItem
                  v-if="hasAccessByCodes(['Platform.Menu.ManageUsers'])"
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
              </Menu>
            </template>
            <Button :icon="h(EllipsisOutlined)" type="link" />
          </Dropdown>
        </div>
      </div>
    </template>
  </Grid>
  <UserLockModal @change="query" />
  <UserClaimModal @change="query" />
  <UserEditModal @change="() => query()" />
  <UserPasswordModal @change="query" />
  <UserPermissionModal />
  <UserSessionDrawer />
  <UserChangeDrawer />
  <UserMenuModal subject="user" />
</template>

<style lang="scss" scoped></style>
