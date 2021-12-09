<template>
  <BasicTable @register="registerTable">
    <template #toolbar>
      <a-button
        type="primary"
        pre-icon="ant-design:plus-outlined"
        :disabled="!addRoleEnabled"
        @click="handleAddNew"
      >
        {{ L('OrganizationUnit:AddRole') }}
      </a-button>
    </template>
    <template #action="{ record }">
      <TableAction
        :stop-button-propagation="true"
        :actions="[
          {
            auth: 'AbpIdentity.OrganizationUnits.ManageRoles',
            color: 'error',
            label: L('Delete'),
            icon: 'ant-design:delete-outlined',
            onClick: handleDelete.bind(null, record),
          },
        ]"
      />
    </template>
  </BasicTable>
  <RoleModal @register="registerModal" :ou-id="ouId" @change="handleChange" />
</template>

<script lang="ts">
  import { computed, defineComponent, unref } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { BasicTable, TableAction } from '/@/components/Table';
  import { useRoleTable } from '../hooks/useRoleTable';
  import { MemberProps } from '../types/props';
  import { useModal } from '/@/components/Modal';
  import RoleModal from './RoleModal.vue';

  export default defineComponent({
    name: 'RoleTable',
    components: { BasicTable, RoleModal, TableAction },
    props: {
      ouId: { type: String },
    },
    setup(props) {
      const { L } = useLocalization('AbpIdentity');
      const { hasPermission } = usePermission();
      const getProps = computed(() => {
        return { ...props } as MemberProps;
      });
      const addRoleEnabled = computed(() => {
        if (!unref(getProps).ouId) {
          return false;
        }
        return hasPermission('AbpIdentity.OrganizationUnits.ManageRoles');
      });
      const { registerTable, reloadRoles, handleDelete } = useRoleTable({ getProps });
      const [registerModal, { openModal }] = useModal();

      function handleAddNew() {
        openModal();
      }

      function handleChange() {
        reloadRoles();
      }

      return {
        L,
        addRoleEnabled,
        registerTable,
        registerModal,
        handleDelete,
        handleAddNew,
        handleChange,
      };
    },
  });
</script>
