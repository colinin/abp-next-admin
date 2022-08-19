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
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'name'">
          <Tag v-if="record.isStatic" style="margin-right: 5px" color="#8baac4">{{ L('Static') }}</Tag>
          <Tag v-if="record.isDefault" style="margin-right: 5px" color="#108ee9">{{ L('DisplayName:IsDefault') }}</Tag>
          <Tag v-if="record.isPublic" style="margin-right: 5px" color="#87d068">{{ L('Public') }}</Tag>
          <span>{{ record.name }}</span>
        </template>
        <template v-else-if="column.key === 'action'">
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
                onClick: handleShowClaims.bind(null, record),
              },
              {
                auth: 'Platform.Menu.ManageRoles',
                label: L('Menu:Manage'),
                onClick: handleSetMenu.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <RoleModal @register="registerModal" @change="reloadTable" />
    <PermissionModal @register="registerPermissionModal" />
    <ClaimModal
      @register="registerClaimModal"
      :fetch-api="getRoleClaims"
      :create-api="createClaim"
      :update-api="updateClaim"
      :delete-api="deleteClaim"
    />
    <MenuModal
      @register="registerMenuModal"
      :loading="loadMenuRef"
      :get-menu-api="getListByRole"
      @change="handleChangeMenu"
      @change:startup="handleChangeStartupMenu"
    />
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref } from 'vue';
  import { Tag } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { useModal } from '/@/components/Modal';
  import { PermissionModal } from '/@/components/Permission';
  import { BasicTable, TableAction } from '/@/components/Table';
  import { useRoleTable } from '../hooks/useRoleTable';
  import { usePermission as usePermissionModal } from '../hooks/usePermission';
  import { getListByRole, setRoleMenu, setRoleStartupMenu } from '/@/api/platform/menu';
  import { getClaimList as getRoleClaims, createClaim, updateClaim, deleteClaim } from '/@/api/identity/role';
  import RoleModal from './RoleModal.vue';
  import MenuModal from '../../components/MenuModal.vue';
  import ClaimModal from '../../components/ClaimModal.vue';

  export default defineComponent({
    name: 'RoleTable',
    components: {
      BasicTable,
      ClaimModal,
      MenuModal,
      RoleModal,
      Tag,
      TableAction,
      PermissionModal,
    },
    setup() {
      const { L } = useLocalization(['AbpIdentity', 'AppPlatform']);
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

      function handleChangeStartupMenu(roleName, meunId) {
        setRoleStartupMenu(roleName, meunId);
      }

      function handleAddNew() {
        openModal(true, {}, true);
      }

      function handleEdit(record) {
        openModal(true, record, true);
      }

      function handleShowClaims(record) {
        openClaimModal(true, { id: record.id });
      }

      return {
        L,
        loadMenuRef,
        hasPermission,
        registerTable,
        reloadTable,
        registerModal,
        registerClaimModal,
        handleShowClaims,
        registerPermissionModal,
        showPermissionModal,
        registerMenuModal,
        handleAddNew,
        handleEdit,
        handleSetMenu,
        handleDelete,
        handleChangeMenu,
        handleChangeStartupMenu,
        getListByRole,
        getRoleClaims,
        createClaim,
        updateClaim,
        deleteClaim,
      };
    },
  });
</script>
