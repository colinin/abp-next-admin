<template>
  <div class="content">
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button
          v-if="hasPermission('AbpIdentity.Roles.Create')"
          type="primary"
          @click="handleAddNew"
          >{{ L('NewRole') }}</a-button
        >
      </template>
      <template #action="{ record }">
        <TableAction
          :stop-button-propagation="true"
          :actions="[
            {
              auth: 'AbpIdentity.Roles.Update',
              label: L('Edit'),
              icon: 'ant-design:edit-outlined',
              onClick: handleEdit.bind(null, record),
            },
            {
              auth: 'AbpIdentity.Roles.Delete',
              color: 'error',
              label: L('Delete'),
              icon: 'ant-design:delete-outlined',
              ifShow: !record.isStatic,
              onClick: handleDelete.bind(null, record),
            },
          ]"
          :dropDownActions="[
            {
              auth: 'AbpIdentity.Roles.ManagePermissions',
              label: L('Permissions'),
              onClick: showPermissionModal.bind(null, record.name),
            },
            {
              auth: 'AbpIdentity.Users.ManageClaims',
              label: L('Claim'),
              onClick: openClaimModal.bind(null, true, record, true),
            },
            {
              auth: 'Platform.Menu.ManageRoles',
              label: PL('Menu:Manage'),
              onClick: handleSetMenu.bind(null, record),
            },
          ]"
        />
      </template>
    </BasicTable>
    <RoleModal @register="registerModal" @change="reloadTable" />
    <PermissionModal @register="registerPermissionModal" />
    <ClaimModal @register="registerClaimModal" />
    <MenuModal
      @register="registerMenuModal"
      :loading="loadMenuRef"
      :get-menu-api="getListByRole"
      @change="handleChangeMenu"
    />
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { useModal } from '/@/components/Modal';
  import RoleModal from './RoleModal.vue';
  import ClaimModal from './ClaimModal.vue';
  import MenuModal from '../../components/MenuModal.vue';
  import { PermissionModal } from '/@/components/Permission';
  import { BasicTable, TableAction } from '/@/components/Table';
  import { useRoleTable } from '../hooks/useRoleTable';
  import { usePermission as usePermissionModal } from '../hooks/usePermission';
  import { getListByRole, setRoleMenu } from '/@/api/platform/menu';

  export default defineComponent({
    name: 'RoleTable',
    components: {
      BasicTable,
      ClaimModal,
      MenuModal,
      RoleModal,
      TableAction,
      PermissionModal,
    },
    setup() {
      const { L } = useLocalization('AbpIdentity');
      const { L: PL } = useLocalization('AppPlatform');
      const loadMenuRef = ref(false);
      const { hasPermission } = usePermission();
      const [registerModal, { openModal }] = useModal();
      const [registerClaimModal, { openModal: openClaimModal }] = useModal();
      const [registerMenuModal, { openModal: openMenuModal, closeModal: closeMenuModal }] =
        useModal();
      const { registerTable, reloadTable, handleDelete } = useRoleTable();
      const { registerModel: registerPermissionModal, showPermissionModal } = usePermissionModal();

      function handleSetMenu(record) {
        openMenuModal(true, { identity: record.name }, true);
      }

      function handleChangeMenu(roleName, menuIds) {
        loadMenuRef.value = true;
        setRoleMenu({
          roleName: roleName,
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
        PL,
        loadMenuRef,
        hasPermission,
        registerTable,
        reloadTable,
        registerModal,
        openModal,
        registerClaimModal,
        openClaimModal,
        registerPermissionModal,
        showPermissionModal,
        registerMenuModal,
        handleSetMenu,
        handleDelete,
        handleChangeMenu,
        getListByRole,
      };
    },
    methods: {
      handleAddNew() {
        this.openModal(true, {}, true);
      },
      handleEdit(record) {
        this.openModal(true, record, true);
      },
    },
  });
</script>
