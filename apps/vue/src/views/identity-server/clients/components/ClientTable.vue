<template>
  <div>
    <BasicTable @register="registerTable">
      <template #enabled="{ record }">
        <Switch :checked="record.enabled" disabled />
      </template>
      <template #toolbar>
        <Button type="primary" @click="handleAddNew">{{ L('AddNew') }}</Button>
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              auth: 'AbpIdentityServer.Clients.Update',
              icon: 'ant-design:edit-outlined',
              label: L('Edit'),
              onClick: handleEdit.bind(null, record),
            },
            {
              auth: 'AbpIdentityServer.Clients.Delete',
              color: 'error',
              icon: 'ant-design:delete-outlined',
              label: L('Delete'),
              onClick: handleDelete.bind(null, record),
            },
          ]"
          :dropDownActions="[
            {
              auth: 'AbpIdentityServer.Clients.ManagePermissions',
              label: L('Permissions'),
              onClick: handlePermission.bind(null, record),
            },
            {
              auth: 'AbpIdentityServer.Clients.Clone',
              label: L('Client:Clone'),
              onClick: handleClone.bind(null, record),
            },
          ]"
        />
      </template>
    </BasicTable>
    <ClientModal @register="registerModal" @change="handleChange" />
    <PermissionModal @register="registerPermissionModal" />
    <ClientClone @register="registerCloneModal" @change="handleChange" />
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import { Button, Modal, Switch } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { useModal } from '/@/components/Modal';
  import { getDataColumns } from '../datas/TableData';
  import { getSearchFormSchemas } from '../datas/ModalData';
  import { deleteById, getList } from '/@/api/identity-server/clients';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import ClientModal from './ClientModal.vue';
  import ClientClone from './ClientClone.vue';
  import { PermissionModal } from '/@/components/Permission';

  export default defineComponent({
    name: 'ApiScopeTable',
    components: {
      ClientModal,
      ClientClone,
      BasicTable,
      Button,
      Switch,
      TableAction,
      PermissionModal,
    },
    setup() {
      const { L } = useLocalization('AbpIdentityServer');
      const [registerModal, { openModal, closeModal }] = useModal();
      const [registerCloneModal, { openModal: openCloneModal }] = useModal();
      const [registerPermissionModal, { openModal: openPermissionModal }] = useModal();
      const [registerTable, { reload }] = useTable({
        rowKey: 'id',
        title: L('DisplayName:ApiScopes'),
        columns: getDataColumns(),
        api: getList,
        beforeFetch: formatPagedRequest,
        pagination: true,
        striped: false,
        useSearchForm: true,
        showTableSetting: true,
        bordered: true,
        showIndexColumn: false,
        canResize: true,
        immediate: true,
        canColDrag: true,
        formConfig: getSearchFormSchemas(),
        actionColumn: {
          width: 200,
          title: L('Actions'),
          dataIndex: 'action',
          slots: { customRender: 'action' },
        },
      });

      function handleAddNew() {
        openModal(true, {});
      }

      function handleEdit(record) {
        openModal(true, record);
      }

      function handleChange() {
        closeModal();
        reload();
      }

      function handleClone(record) {
        openCloneModal(true, record);
      }

      function handlePermission(record) {
        const props = {
          providerName: 'C',
          providerKey: record.clientId,
        };
        openPermissionModal(true, props, true);
      }

      function handleDelete(record) {
        Modal.warning({
          title: L('AreYouSure'),
          content: L('ItemWillBeDeletedMessage'),
          okCancel: true,
          onOk: () => {
            deleteById(record.id).then(() => {
              reload();
            });
          },
        });
      }

      return {
        L,
        registerModal,
        registerTable,
        handleAddNew,
        handleDelete,
        handleEdit,
        handleChange,
        handleClone,
        registerCloneModal,
        handlePermission,
        registerPermissionModal,
      };
    },
  });
</script>
