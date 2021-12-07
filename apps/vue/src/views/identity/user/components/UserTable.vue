<template>
  <div class="content">
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button
          v-if="hasPermission('AbpIdentity.Users.Create')"
          type="primary"
          @click="handleAddNew"
          >{{ L('NewUser') }}</a-button
        >
      </template>
      <template #phoneNumberConfirmed="{ record }">
        <Tag
          style="margin-right: 10px; margin-bottom: 5px"
          :color="record.phoneNumberConfirmed ? 'green' : 'orange'"
        >
          {{ record.phoneNumberConfirmed }}
        </Tag>
      </template>
      <template #emailConfirmed="{ record }">
        <Tag
          style="margin-right: 10px; margin-bottom: 5px"
          :color="record.emailConfirmed ? 'green' : 'orange'"
        >
          {{ record.emailConfirmed }}
        </Tag>
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              auth: 'AbpIdentity.Users.Update',
              label: L('Edit'),
              icon: 'ant-design:edit-outlined',
              onClick: handleEdit.bind(null, record),
            },
            {
              auth: 'AbpIdentity.Users.Delete',
              color: 'error',
              label: L('Delete'),
              icon: 'ant-design:delete-outlined',
              onClick: handleDelete.bind(null, record),
            },
          ]"
          :dropDownActions="[
            {
              auth: 'AbpIdentity.Users.Update',
              label: L('Lockout'),
              ifShow: lockEnable(record),
              onClick: showLockModal.bind(null, record.id),
            },
            {
              auth: 'AbpIdentity.Users.Update',
              label: L('UnLock'),
              ifShow: record.lockoutEnabled && !lockEnable(record),
              onClick: handleUnlock.bind(null, record),
            },
            {
              auth: 'AbpIdentity.Users.ManagePermissions',
              label: L('Permissions'),
              onClick: showPermissionModal.bind(null, record.id),
            },
            {
              auth: 'AbpIdentity.Users.ManageClaims',
              label: L('Claim'),
              onClick: openClaimModal.bind(null, true, record, true),
            },
            {
              auth: 'AbpIdentity.Users.Update',
              label: L('SetPassword'),
              onClick: showPasswordModal.bind(null, record.id),
            },
            {
              auth: 'Platform.Menu.ManageUsers',
              label: L('Menu:Manage'),
              onClick: handleSetMenu.bind(null, record),
            },
          ]"
        />
      </template>
    </BasicTable>
    <UserModal @register="registerModal" @change="reloadTable" />
    <PermissionModal @register="registerPermissionModal" />
    <PasswordModal @register="registerPasswordModal" />
    <ClaimModal @register="registerClaimModal" />
    <LockModal @register="registerLockModal" @change="reloadTable" />
    <MenuModal
      @register="registerMenuModal"
      :loading="loadMenuRef"
      :get-menu-api="getListByUser"
      @change="handleChangeMenu"
    />
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { Tag } from 'ant-design-vue';
  import { useModal } from '/@/components/Modal';
  import { PermissionModal } from '/@/components/Permission';
  import { BasicTable, TableAction } from '/@/components/Table';
  import UserModal from './UserModal.vue';
  import PasswordModal from './PasswordModal.vue';
  import ClaimModal from './ClaimModal.vue';
  import LockModal from './LockModal.vue';
  import MenuModal from '../../components/MenuModal.vue';
  import { useUserTable } from '../hooks/useUserTable';
  import { usePassword } from '../hooks/usePassword';
  import { useLock } from '../hooks/useLock';
  import { usePermission as usePermissionModal } from '../hooks/usePermission';
  import { getListByUser, setUserMenu } from '/@/api/platform/menu';

  export default defineComponent({
    name: 'UserTable',
    components: {
      BasicTable,
      ClaimModal,
      PermissionModal,
      TableAction,
      Tag,
      UserModal,
      PasswordModal,
      LockModal,
      MenuModal,
    },
    setup(_props, { emit }) {
      const { L } = useLocalization('AbpIdentity', 'AppPlatform');
      const loadMenuRef = ref(false);
      const { hasPermission } = usePermission();
      const [registerModal, { openModal }] = useModal();
      const { lockEnable, registerTable, reloadTable, handleDelete } = useUserTable();
      const { registerLockModal, showLockModal, handleUnLock } = useLock({ emit });
      const { registerPasswordModal, showPasswordModal } = usePassword();
      const [registerClaimModal, { openModal: openClaimModal }] = useModal();
      const [registerMenuModal, { openModal: openMenuModal, closeModal: closeMenuModal }] =
        useModal();
      const { registerModel: registerPermissionModal, showPermissionModal } = usePermissionModal();

      function handleSetMenu(record) {
        openMenuModal(true, { identity: record.id }, true);
      }

      function handleChangeMenu(userId, menuIds) {
        loadMenuRef.value = true;
        setUserMenu({
          userId: userId,
          menuIds: menuIds,
        })
          .then(() => {
            closeMenuModal();
          })
          .finally(() => {
            loadMenuRef.value = false;
          });
      }

      return {
        L,
        loadMenuRef,
        hasPermission,
        lockEnable,
        registerTable,
        reloadTable,
        registerModal,
        openModal,
        registerPermissionModal,
        showPermissionModal,
        registerPasswordModal,
        showPasswordModal,
        registerClaimModal,
        openClaimModal,
        handleDelete,
        registerLockModal,
        showLockModal,
        handleUnLock,
        registerMenuModal,
        handleSetMenu,
        handleChangeMenu,
        getListByUser,
      };
    },
    methods: {
      handleAddNew() {
        this.openModal(true, {}, true);
      },
      handleEdit(record) {
        this.openModal(true, record, true);
      },
      handleUnlock(record) {
        this.handleUnLock(record.id).then(() => {
          this.reloadTable();
        });
      },
    },
  });
</script>
