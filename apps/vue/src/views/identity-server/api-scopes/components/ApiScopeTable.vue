<template>
  <div>
    <BasicTable @register="registerTable">
      <template #required="{ record }">
        <Switch :checked="record.required" disabled />
      </template>
      <template #enabled="{ record }">
        <Switch :checked="record.enabled" disabled />
      </template>
      <template #emphasize="{ record }">
        <Switch :checked="record.emphasize" disabled />
      </template>
      <template #discovery="{ record }">
        <Switch :checked="record.showInDiscoveryDocument" disabled />
      </template>
      <template #toolbar>
        <Button type="primary" @click="handleAddNew">{{ L('AddNew') }}</Button>
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              auth: 'AbpIdentityServer.ApiScopes.Update',
              icon: 'ant-design:edit-outlined',
              label: L('ApiScopes:Edit'),
              onClick: handleEdit.bind(null, record),
            },
            {
              auth: 'AbpIdentityServer.ApiScopes.Delete',
              color: 'error',
              icon: 'ant-design:delete-outlined',
              label: L('ApiScopes:Delete'),
              onClick: handleDelete.bind(null, record),
            },
          ]"
        />
      </template>
    </BasicTable>
    <ApiScopeModal @register="registerModal" @change="handleChange" />
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
  import { deleteById, getList } from '/@/api/identity-server/apiScopes';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import ApiScopeModal from './ApiScopeModal.vue';

  export default defineComponent({
    name: 'ApiScopeTable',
    components: { ApiScopeModal, BasicTable, Button, Switch, TableAction },
    setup() {
      const { L } = useLocalization('AbpIdentityServer');
      const [registerModal, { openModal, closeModal }] = useModal();
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
      };
    },
  });
</script>
