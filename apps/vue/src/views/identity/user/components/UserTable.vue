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
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'userName'">
          <span>{{ record.userName }}</span>
          <Tag v-if="!lockEnable(record)" style="margin-left: 5px" color="orange">{{ L('Lockout') }}</Tag>
          <Tag v-if="!record.isActive" style="margin-left: 5px" color="red">{{ L('UnActived') }}</Tag>
        </template>
        <template v-if="column.key === 'phoneNumber'">
          <template v-if="record.phoneNumber">
            <span style="margin-right: 5px">{{ record.phoneNumber }}</span>
            <Tag v-if="record.phoneNumberConfirmed" color="green">{{ L('Confirmed') }}</Tag>
            <Tag v-else color="orange">{{ L('UnConfirmed') }}</Tag>
          </template>
        </template>
        <template v-if="column.key === 'email'">
          <template v-if="record.email">
            <span style="margin-right: 5px">{{ record.email }}</span>
            <Tag v-if="record.emailConfirmed" color="green">{{ L('Confirmed') }}</Tag>
            <Tag v-else color="orange">{{ L('UnConfirmed') }}</Tag>
          </template>
        </template>
        <template v-else-if="column.key === 'action'">
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
                onClick: showPermissionModal.bind(null, record.id, record.userName),
              },
              {
                auth: 'AbpIdentity.Users.ManageClaims',
                label: L('Claim'),
                onClick: handleShowClaims.bind(null, record),
              },
              {
                auth: 'AbpIdentity.Users.Update',
                label: L('SetPassword'),
                ifShow: !record.isExternal, // 外部扩展用户不允许修改密码
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
      </template>
    </BasicTable>
    <UserModal @register="registerModal" @change="reloadTable" />
    <PermissionModal @register="registerPermissionModal" />
    <PasswordModal @register="registerPasswordModal" />
    <ClaimModal
      @register="registerClaimModal"
      :fetch-api="getUserClaims"
      :create-api="createClaim"
      :update-api="updateClaim"
      :delete-api="deleteClaim"
    />
    <LockModal @register="registerLockModal" @change="reloadTable" />
    <MenuModal
      @register="registerMenuModal"
      :loading="loadMenuRef"
      :get-menu-api="getListByUser"
      @change="handleChangeMenu"
      @change:startup="handleChangeStartupMenu"
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
  import { useUserTable } from '../hooks/useUserTable';
  import { usePassword } from '../hooks/usePassword';
  import { useLock } from '../hooks/useLock';
  import { usePermission as usePermissionModal } from '../hooks/usePermission';
  import { getListByUser, setUserMenu, setUserStartupMenu } from '/@/api/platform/menu';
  import { getClaimList as getUserClaims, createClaim, updateClaim, deleteClaim } from '/@/api/identity/user';
  import UserModal from './UserModal.vue';
  import PasswordModal from './PasswordModal.vue';
  import LockModal from './LockModal.vue';
  import MenuModal from '../../components/MenuModal.vue';
  import ClaimModal from '../../components/ClaimModal.vue';

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
      const { L } = useLocalization(['AbpIdentity', 'AppPlatform']);
      const loadMenuRef = ref(false);
      const nullFormElRef = ref(null);
      const { hasPermission } = usePermission();
      const [registerModal, { openModal }] = useModal();
      const { lockEnable, registerTable, reloadTable, handleDelete } = useUserTable();
      const { registerLockModal, showLockModal, handleUnLock } = useLock({ emit });
      const { registerPasswordModal, showPasswordModal } = usePassword(nullFormElRef);
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

      function handleAddNew() {
        openModal(true, {}, true);
      }

      function handleEdit(record) {
        openModal(true, record, true);
      }

      function handleUnlock(record) {
        handleUnLock(record.id).then(() => {
          reloadTable();
        });
      }

      function handleChangeStartupMenu(userId, meunId) {
        setUserStartupMenu(userId, meunId);
      }

      function handleShowClaims(record) {
        openClaimModal(true, { id: record.id });
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
        handleShowClaims,
        handleDelete,
        registerLockModal,
        showLockModal,
        handleUnLock,
        registerMenuModal,
        handleAddNew,
        handleEdit,
        handleUnlock,
        handleSetMenu,
        handleChangeMenu,
        handleChangeStartupMenu,
        getListByUser,
        getUserClaims,
        createClaim,
        updateClaim,
        deleteClaim,
      };
    },
  });
</script>
