<script setup lang="ts">
import type {
  AbpEditorFormSchema,
  AbpEditorModalFormOptions,
  AbpEditorTab,
} from '@abp/components/crud/modal';
import type { VxeGridProps } from '@abp/ui';
import type { TransferItem } from 'ant-design-vue/es/transfer';
import type { DataNode, EventDataNode } from 'ant-design-vue/es/tree';

import type { ActionItem, VbenFormProps } from '@vben/common-ui';

import type {
  IdentityUserCreateDto,
  IdentityUserDto,
  IdentityUserUpdateDto,
  IdentityUserVto,
} from '../../types/users';

import { computed, h, reactive, ref } from 'vue';

import { useAccess } from '@vben/access';
import {
  confirm,
  useVbenDrawer,
  useVbenModal,
  VbenDescriptions,
  VbenDescriptionsItem,
} from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { AuditLogPermissions, EntityChangeDrawer } from '@abp/auditing';
import { useAbpEditorModal } from '@abp/components/crud/modal';
import { useAbpDataTable } from '@abp/components/crud/table';
import {
  formatToDateTime,
  useAbpStore,
  useFeatures,
  useNotifications,
  useSettings,
} from '@abp/core';
import { PermissionModal } from '@abp/permissions';
import { MenuAllotModal } from '@abp/platform';
import {
  DeleteOutlined,
  EditOutlined,
  LockOutlined,
  LoginOutlined,
  PlusOutlined,
  UnlockOutlined,
} from '@ant-design/icons-vue';
import { Button, Tag } from 'ant-design-vue';

import { useOrganizationUnitsApi } from '../../api/useOrganizationUnitsApi';
import { useUsersApi } from '../../api/useUsersApi';
import {
  IdentitySessionPermissions,
  IdentityUserPermissions,
} from '../../constants/permissions';
import UserClaimModal from './UserClaimModal.vue';
import UserLockModal from './UserLockModal.vue';
import UserPasswordModal from './UserPasswordModal.vue';
import UserSessionDrawer from './UserSessionDrawer.vue';

defineOptions({
  name: 'UserPage',
});

const emit = defineEmits<{
  (event: 'impersonation', user: IdentityUserDto): void;
}>();

type UserFormValues = IdentityUserVto & {
  details?: IdentityUserDto;
  organizationUnits?: string[];
};

const abpStore = useAbpStore();
const { isEnabled } = useFeatures();
const { hasAccessByCodes } = useAccess();
const { isTrue } = useSettings();
const { toastSuccess } = useNotifications();
const {
  createApi,
  deleteApi,
  getApi,
  getAssignableRolesApi,
  getOrganizationUnitsApi,
  getRolesApi,
  getPagedListApi,
  unLockApi,
  updateApi,
} = useUsersApi();
const { getChildrenApi, getRootListApi } = useOrganizationUnitsApi();

const canManageRoles = hasAccessByCodes([
  'AbpIdentity.Users.Update.ManageRoles',
]);
const canManageOu = hasAccessByCodes([
  'AbpIdentity.Users.ManageOrganizationUnits',
]);

const PasswordIcon = createIconifyIcon('carbon:password');
const MenuOutlined = createIconifyIcon('heroicons-outline:menu-alt-3');
const ClaimOutlined = createIconifyIcon('la:id-card-solid');
const PermissionsOutlined = createIconifyIcon('icon-park-outline:permissions');
const SessionIcon = createIconifyIcon('carbon:prompt-session');
const AuditLogIcon = createIconifyIcon('fluent-mdl2:compliance-audit');

const isEditMode = ref(false);
const assignedRoles = ref<TransferItem[]>([]);
const organizationUnits = ref<DataNode[]>([]);
const checkedOuKeys = ref<string[]>([]);
const loadedOuKeys = ref<string[]>([]);
const details = ref<IdentityUserDto>();

const defaultModel: UserFormValues = {
  email: '',
  emailConfirmed: false,
  extraProperties: {},
  isActive: true,
  lockoutEnabled: true,
  name: '',
  phoneNumber: '',
  phoneNumberConfirmed: false,
  roleNames: [],
  twoFactorEnabled: false,
  userName: '',
};

const tabs = reactive<AbpEditorTab[]>([]);

function syncTabs(isEdit: boolean) {
  const next: AbpEditorTab[] = [
    { key: 'basic', title: $t('AbpIdentity.UserInformations') },
    ...(canManageRoles
      ? [{ key: 'role', title: $t('AbpIdentity.Roles') }]
      : []),
    ...(isEdit && canManageOu
      ? [
          {
            key: 'organizationUnits',
            title: $t('AbpIdentity.OrganizationUnits'),
          },
        ]
      : []),
    ...(isEdit ? [{ key: 'details', title: $t('AbpIdentity.Details') }] : []),
  ];
  tabs.splice(0, tabs.length, ...next);
}

syncTabs(false);

function buildSchema(edit: boolean): AbpEditorFormSchema<UserFormValues>[] {
  const schema: AbpEditorFormSchema<UserFormValues>[] = [
    {
      component: 'Switch',
      controlClass: 'w-auto',
      fieldName: 'isActive',
      label: $t('AbpIdentity.DisplayName:IsActive'),
      tabKey: 'basic',
    },
    {
      component: 'Input',
      componentProps: () => ({
        disabled: edit && !isTrue('Abp.Identity.User.IsUserNameUpdateEnabled'),
      }),
      fieldName: 'userName',
      label: $t('AbpIdentity.UserName'),
      rules: 'required',
      tabKey: 'basic',
    },
    {
      component: 'InputPassword',
      fieldName: 'password',
      hide: edit,
      label: $t('AbpIdentity.Password'),
      rules: edit ? undefined : 'required',
      tabKey: 'basic',
    },
    {
      component: 'Input',
      fieldName: 'surname',
      label: $t('AbpIdentity.DisplayName:Surname'),
      tabKey: 'basic',
    },
    {
      component: 'Input',
      fieldName: 'name',
      label: $t('AbpIdentity.DisplayName:Name'),
      tabKey: 'basic',
    },
    {
      component: 'Input',
      componentProps: () => ({
        disabled: edit && !isTrue('Abp.Identity.User.IsEmailUpdateEnabled'),
      }),
      fieldName: 'email',
      label: $t('AbpIdentity.DisplayName:Email'),
      rules: 'required',
      tabKey: 'basic',
    },
    {
      component: 'Input',
      fieldName: 'phoneNumber',
      label: $t('AbpIdentity.DisplayName:PhoneNumber'),
      tabKey: 'basic',
    },
    {
      component: 'Switch',
      controlClass: 'w-auto',
      fieldName: 'lockoutEnabled',
      label: $t('AbpIdentity.DisplayName:LockoutEnabled'),
      tabKey: 'basic',
    },
  ];

  if (canManageRoles) {
    schema.push({
      component: 'Transfer',
      componentProps: () => ({
        dataSource: assignedRoles.value,
        listStyle: { height: '338px', width: '47%' },
        render: (item: any) => item.title,
        titles: [$t('AbpOpenIddict.Assigned'), $t('AbpOpenIddict.Available')],
      }),
      fieldName: 'roleNames',
      hideLabel: true,
      label: '',
      tabKey: 'role',
    });
  }
  if (canManageOu) {
    schema.push({
      component: 'Tree',
      hide: !edit,
      componentProps: () => ({
        blockNode: true,
        checkStrictly: true,
        checkable: true,
        checkedKeys: checkedOuKeys.value,
        disabled: true,
        loadData: loadOrganizationUnitChildren,
        loadedKeys: loadedOuKeys.value,
        treeData: organizationUnits.value,
      }),
      fieldName: 'organizationUnits',
      hideLabel: true,
      label: '',
      tabKey: 'organizationUnits',
    });
  }
  schema.push({
    component: 'Input',
    fieldName: 'details',
    hide: !edit,
    hideLabel: true,
    label: '',
    tabKey: 'details',
  });

  return schema;
}

const formOptions: AbpEditorModalFormOptions<UserFormValues> = {
  commonConfig: {
    colon: true,
    controlClass: 'w-full',
  },
  schema: buildSchema(false),
  tabs,
};

function toCreatePayload(values: UserFormValues): IdentityUserCreateDto {
  return {
    email: values.email,
    isActive: values.isActive,
    lockoutEnabled: values.lockoutEnabled,
    name: values.name,
    password: values.password,
    phoneNumber: values.phoneNumber,
    roleNames: values.roleNames ?? [],
    surname: values.surname,
    userName: values.userName,
  } as unknown as IdentityUserCreateDto;
}

function toUpdatePayload(values: UserFormValues): IdentityUserUpdateDto {
  return {
    email: values.email,
    isActive: values.isActive,
    lockoutEnabled: values.lockoutEnabled,
    name: values.name,
    phoneNumber: values.phoneNumber,
    roleNames: values.roleNames ?? [],
    surname: values.surname,
    userName: values.userName,
  } as unknown as IdentityUserUpdateDto;
}

async function loadAssignableRoles() {
  const { items } = await getAssignableRolesApi();
  assignedRoles.value = items.map((item) => ({
    key: item.name,
    title: item.name,
  }));
}

async function loadRoles(userId: string, dto: IdentityUserDto) {
  const result = await getRolesApi(userId);
  dto.roleNames = result.items.map((item) => item.name);
  if (assignedRoles.value.length === 0) {
    await loadAssignableRoles();
  }
}

async function loadOrganizationUnits(userId: string) {
  const [rootResult, userOuResult] = await Promise.all([
    getRootListApi(),
    getOrganizationUnitsApi(userId),
  ]);
  organizationUnits.value = rootResult.items.map((item) => ({
    children: [],
    isLeaf: false,
    key: item.id,
    title: item.displayName,
  }));
  checkedOuKeys.value = userOuResult.items.map((item) => item.id);
}

async function loadOrganizationUnitChildren(node: EventDataNode) {
  const nodeKey = String(node.key);
  const { items } = await getChildrenApi({ id: nodeKey });
  const dataRef = node.dataRef;
  if (dataRef) {
    dataRef.isLeaf = items.length === 0;
    dataRef.children = items.map((item): DataNode => ({
      children: [],
      isLeaf: false,
      key: item.id,
      title: item.displayName,
    }));
  }
  organizationUnits.value = [...organizationUnits.value];
  loadedOuKeys.value.push(nodeKey);
}

async function onOpen({
  dto,
  formApi,
  mode,
}: {
  dto?: IdentityUserDto;
  formApi: any;
  mode: string;
}) {
  isEditMode.value = mode === 'edit';
  syncTabs(isEditMode.value);
  await formApi.updateSchema(buildSchema(isEditMode.value));

  assignedRoles.value = [];
  organizationUnits.value = [];
  checkedOuKeys.value = [];
  loadedOuKeys.value = [];
  details.value = undefined;

  if (mode === 'create') {
    if (canManageRoles) {
      await loadAssignableRoles();
    }
    return;
  }
  if (!dto) {
    return;
  }

  details.value = dto;
  await Promise.all([
    canManageRoles ? loadRoles(dto.id, dto) : Promise.resolve(),
    canManageOu ? loadOrganizationUnits(dto.id) : Promise.resolve(),
  ]);
}

const [UserEditModal, userModalApi] = useAbpEditorModal<
  IdentityUserDto,
  UserFormValues
>({
  create: async (values) => createApi(toCreatePayload(values)),
  defaultValues: defaultModel,
  formOptions,
  get: getApi,
  idKey: (dto) => dto.id,
  modalOptions: {
    class: 'w-[800px]',
  },
  onOpen,
  refresh: async () => {
    await tableApi.refresh();
  },
  titleKey: (dto) => dto.userName,
  update: async (id, values) => updateApi(id, toUpdatePayload(values)),
});

const searchFormOptions: VbenFormProps = {
  collapsed: false,
  schema: [
    {
      component: 'Input',
      fieldName: 'filter',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('AbpUi.Search'),
    },
  ],
  showCollapseButton: true,
  submitOnEnter: true,
};

const gridOptions: VxeGridProps<IdentityUserDto> = {
  columns: [
    {
      field: 'isActive',
      slots: { default: 'active' },
      sortable: true,
      title: $t('AbpIdentity.DisplayName:IsActive'),
    },
    {
      field: 'userName',
      minWidth: 100,
      sortable: true,
      title: $t('AbpIdentity.DisplayName:UserName'),
    },
    {
      align: 'left',
      field: 'email',
      minWidth: 120,
      slots: { default: 'email' },
      sortable: true,
      title: $t('AbpIdentity.DisplayName:Email'),
    },
    {
      field: 'surname',
      sortable: true,
      title: $t('AbpIdentity.DisplayName:Surname'),
    },
    {
      field: 'name',
      sortable: true,
      title: $t('AbpIdentity.DisplayName:Name'),
    },
    {
      align: 'left',
      field: 'phoneNumber',
      slots: { default: 'phoneNumber' },
      sortable: true,
      title: $t('AbpIdentity.DisplayName:PhoneNumber'),
    },
    {
      field: 'lockoutEnd',
      formatter: ({ cellValue }) =>
        cellValue ? formatToDateTime(cellValue) : '',
      sortable: true,
      title: $t('AbpIdentity.LockoutEnd'),
    },
  ],
  exportConfig: {},
  keepSource: true,
  toolbarConfig: {
    custom: true,
    export: true,
    refresh: true,
    refreshOptions: { code: 'query' },
    zoom: true,
  },
};

const [UserLockModalWrapper, lockModalApi] = useVbenModal({
  connectedComponent: UserLockModal,
});
const [UserPasswordModalWrapper, pwdModalApi] = useVbenModal({
  connectedComponent: UserPasswordModal,
});
const [UserClaimModalWrapper, claimModalApi] = useVbenModal({
  connectedComponent: UserClaimModal,
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
const [UserSessionDrawerWrapper, userSessionDrawerApi] = useVbenDrawer({
  connectedComponent: UserSessionDrawer,
});

const isCurrentUser = (user: IdentityUserDto) =>
  abpStore.application?.currentUser.id === user.id;

function getLockEnd(row: IdentityUserDto) {
  if (row.lockoutEnd) {
    return new Date(row.lockoutEnd) < new Date();
  }
  return true;
}

const isDeleteEnabled = (user: IdentityUserDto) =>
  !isCurrentUser(user) && hasAccessByCodes([IdentityUserPermissions.Delete]);

const isLockEnabled = (user: IdentityUserDto) =>
  !isCurrentUser(user) &&
  user.isActive &&
  getLockEnd(user) &&
  hasAccessByCodes([IdentityUserPermissions.Update]);

const isUnLockEnabled = (user: IdentityUserDto) =>
  !isCurrentUser(user) &&
  user.isActive &&
  !getLockEnd(user) &&
  hasAccessByCodes([IdentityUserPermissions.Update]);

const isImpersonationEnabled = (user: IdentityUserDto) =>
  !isCurrentUser(user) &&
  hasAccessByCodes([IdentityUserPermissions.Impersonation]);

const isAuditLogEnabled = computed(
  () =>
    isEnabled('AbpAuditing.Logging.AuditLog') &&
    hasAccessByCodes([AuditLogPermissions.Default]),
);

function handleAdd() {
  void userModalApi.create();
}

function handleEdit(row: IdentityUserDto) {
  void userModalApi.update(row);
}

async function handleDelete(row: IdentityUserDto) {
  await confirm({
    centered: true,
    content: $t('AbpIdentity.UserDeletionConfirmationMessage', [row.userName]),
    icon: 'warning',
    title: $t('AbpUi.AreYouSure'),
    async beforeClose(scope) {
      if (scope.isConfirm) {
        await deleteApi(row.id);
        toastSuccess($t('AbpUi.DeletedSuccessfully'));
        await tableApi.refresh();
      }
      return true;
    },
  });
}

function handleImpersonation(row: IdentityUserDto) {
  void confirm({
    centered: true,
    content: $t('AbpAccount.ImpersonatorWarning', [row.userName]),
    icon: 'warning',
    title: $t('AbpUi.AreYouSure'),
  })
    .then(() => emit('impersonation', row))
    .catch(() => undefined);
}

async function handleUnlock(row: IdentityUserDto) {
  await unLockApi(row.id);
  toastSuccess($t('AbpUi.SavedSuccessfully'));
  await tableApi.refresh();
}

function getEditActions(row: IdentityUserDto): ActionItem[] {
  return [
    {
      auth: [IdentityUserPermissions.Update],
      icon: EditOutlined,
      key: 'edit',
      onClick: () => handleEdit(row),
      text: $t('AbpUi.Edit'),
    },
    {
      auth: [IdentityUserPermissions.Delete],
      danger: true,
      disabled: !isDeleteEnabled(row),
      icon: DeleteOutlined,
      key: 'delete',
      onClick: () => void handleDelete(row),
      text: $t('AbpUi.Delete'),
    },
  ];
}

function getDropdownActions(row: IdentityUserDto): ActionItem[] {
  const actions: ActionItem[] = [];

  if (isLockEnabled(row)) {
    actions.push({
      icon: LockOutlined,
      key: 'lock',
      onClick: () => {
        lockModalApi.setData(row);
        lockModalApi.open();
      },
      text: $t('AbpIdentity.Lock'),
    });
  }
  if (isUnLockEnabled(row)) {
    actions.push({
      icon: UnlockOutlined,
      key: 'unlock',
      onClick: () => void handleUnlock(row),
      text: $t('AbpIdentity.UnLock'),
    });
  }
  if (hasAccessByCodes([IdentityUserPermissions.ManagePermissions])) {
    actions.push({
      icon: PermissionsOutlined,
      key: 'permissions',
      onClick: () => {
        permissionModalApi.setData({
          displayName: row.userName,
          providerKey: row.id,
          providerName: 'U',
          readonly: isCurrentUser(row),
        });
        permissionModalApi.open();
      },
      text: $t('AbpPermissionManagement.Permissions'),
    });
  }
  if (hasAccessByCodes([IdentitySessionPermissions.Default])) {
    actions.push({
      icon: SessionIcon,
      key: 'session',
      onClick: () => {
        userSessionDrawerApi.setData(row);
        userSessionDrawerApi.open();
      },
      text: $t('AbpIdentity.IdentitySessions'),
    });
  }
  if (hasAccessByCodes([IdentityUserPermissions.ManageClaims])) {
    actions.push({
      icon: ClaimOutlined,
      key: 'claims',
      onClick: () => {
        claimModalApi.setData(row);
        claimModalApi.open();
      },
      text: $t('AbpIdentity.ManageClaim'),
    });
  }
  if (isImpersonationEnabled(row)) {
    actions.push({
      icon: LoginOutlined,
      key: 'impersonation',
      onClick: () => handleImpersonation(row),
      text: $t('AbpIdentity.ImpersonationUser'),
    });
  }
  if (hasAccessByCodes([IdentityUserPermissions.Update])) {
    actions.push({
      icon: PasswordIcon,
      key: 'password',
      onClick: () => {
        pwdModalApi.setData(row);
        pwdModalApi.open();
      },
      text: $t('AbpIdentity.SetPassword'),
    });
  }
  if (hasAccessByCodes(['Platform.Menu.ManageUsers'])) {
    actions.push({
      icon: MenuOutlined,
      key: 'menus',
      onClick: () => {
        menuModalApi.setData({ identity: row.id });
        menuModalApi.open();
      },
      text: $t('AppPlatform.Menu:Manage'),
    });
  }
  if (isAuditLogEnabled.value) {
    actions.push({
      icon: AuditLogIcon,
      key: 'entity-changes',
      onClick: () => {
        userChangeDrawerApi.setData({
          entityId: row.id,
          entityTypeFullName: 'Volo.Abp.Identity.IdentityUser',
          subject: row.userName,
        });
        userChangeDrawerApi.open();
      },
      text: $t('AbpAuditLogging.EntitiesChanged'),
    });
  }

  return actions;
}

const [DataTable, tableApi] = useAbpDataTable<IdentityUserDto>({
  actionColumn: { title: $t('AbpUi.Actions'), width: 220 },
  actions: getEditActions,
  dropdownActions: getDropdownActions,
  formOptions: searchFormOptions,
  gridOptions,
  rowKey: 'id',
  load: ({ pager, searchValues, sorting }) =>
    getPagedListApi({
      maxResultCount: pager.maxResultCount,
      skipCount: pager.skipCount,
      sorting,
      ...searchValues,
    }),
});

defineExpose({
  refresh: () => tableApi.refresh(),
});
</script>

<template>
  <DataTable :table-title="$t('AbpIdentity.Users')">
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
        <span :class="row.isActive ? 'text-green-600' : 'text-red-600'">
          {{ row.isActive ? '✓' : '✕' }}
        </span>
      </div>
    </template>

    <template #email="{ row }">
      <div class="flex flex-row gap-1">
        <Tag :color="row.emailConfirmed ? 'success' : 'warning'">
          {{
            row.emailConfirmed
              ? $t('abp.account.settings.security.verified')
              : $t('abp.account.settings.security.unVerified')
          }}
        </Tag>
        <span>{{ row.email }}</span>
      </div>
    </template>

    <template #phoneNumber="{ row }">
      <div class="flex flex-row gap-1">
        <Tag
          v-if="row.phoneNumber"
          :color="row.phoneNumberConfirmed ? 'success' : 'warning'"
        >
          {{
            row.phoneNumberConfirmed
              ? $t('abp.account.settings.security.verified')
              : $t('abp.account.settings.security.unVerified')
          }}
        </Tag>
        <span>{{ row.phoneNumber }}</span>
      </div>
    </template>
  </DataTable>

  <UserEditModal>
    <template #form-details>
      <VbenDescriptions v-if="details" :column="1" bordered size="small">
        <VbenDescriptionsItem
          :content="formatToDateTime(details.creationTime)"
          :label="$t('AbpIdentity.CreationTime')"
        />
        <VbenDescriptionsItem
          :content="formatToDateTime(details.lastModificationTime)"
          :label="$t('AbpIdentity.ModificationTime')"
        />
        <VbenDescriptionsItem
          :content="formatToDateTime(details.lastPasswordChangeTime)"
          :label="$t('AbpIdentity.PasswordUpdateTime')"
        />
        <VbenDescriptionsItem
          :content="details.accessFailedCount"
          :label="$t('AbpIdentity.FailedAccessCount')"
        />
        <VbenDescriptionsItem
          :content="formatToDateTime(details.lockoutEnd)"
          :label="$t('AbpIdentity.LockoutEndTime')"
        />
      </VbenDescriptions>
    </template>
  </UserEditModal>

  <UserLockModalWrapper @change="() => tableApi.refresh()" />
  <UserClaimModalWrapper @change="() => tableApi.refresh()" />
  <UserPasswordModalWrapper @change="() => tableApi.refresh()" />
  <UserPermissionModal />
  <UserSessionDrawerWrapper />
  <UserChangeDrawer />
  <UserMenuModal subject="user" />
</template>

<style lang="scss" scoped></style>
