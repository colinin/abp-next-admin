<template>
  <BasicTable @register="registerTable">
    <template #toolbar>
      <a-button
        type="primary"
        pre-icon="ant-design:plus-outlined"
        :disabled="!addMemberEnabled"
        @click="handleAddNew"
      >
        {{ L('OrganizationUnit:AddMember') }}
      </a-button>
    </template>
    <template #action="{ record }">
      <TableAction
        :stop-button-propagation="true"
        :actions="[
          {
            auth: 'AbpIdentity.OrganizationUnits.ManageUsers',
            color: 'error',
            label: L('Delete'),
            icon: 'ant-design:delete-outlined',
            onClick: handleDelete.bind(null, record),
          },
        ]"
      />
    </template>
  </BasicTable>
  <MemberModal @register="registerModal" :ou-id="ouId" @change="handleChange" />
</template>

<script lang="ts">
  import { computed, defineComponent, unref } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { BasicTable, TableAction } from '/@/components/Table';
  import { useMemberTable } from '../hooks/useMemberTable';
  import { MemberProps } from '../types/props';
  import { useModal } from '/@/components/Modal';
  import MemberModal from './MemberModal.vue';

  export default defineComponent({
    name: 'MemberTable',
    components: { BasicTable, MemberModal, TableAction },
    props: {
      ouId: { type: String },
    },
    setup(props) {
      const { L } = useLocalization('AbpIdentity');
      const { hasPermission } = usePermission();
      const getProps = computed(() => {
        return { ...props } as MemberProps;
      });
      const addMemberEnabled = computed(() => {
        if (!unref(getProps).ouId) {
          return false;
        }
        return hasPermission('AbpIdentity.OrganizationUnits.ManageUsers');
      });
      const { registerTable, reloadMembers, handleDelete } = useMemberTable({ getProps });
      const [registerModal, { openModal }] = useModal();

      function handleAddNew() {
        openModal();
      }

      function handleChange() {
        reloadMembers();
      }

      return {
        L,
        addMemberEnabled,
        registerTable,
        handleChange,
        handleDelete,
        registerModal,
        handleAddNew,
      };
    },
  });
</script>
