<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <Button type="primary" @click="handleAddNew">{{ L('AddNew') }}</Button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'required'">
          <Switch :checked="record.required" readonly />
        </template>
        <template v-else-if="column.key === 'enabled'">
          <Switch :checked="record.enabled" readonly />
        </template>
        <template v-else-if="column.key === 'emphasize'">
          <Switch :checked="record.emphasize" readonly />
        </template>
        <template v-else-if="column.key === 'showInDiscoveryDocument'">
          <Switch :checked="record.showInDiscoveryDocument" readonly />
        </template>
        <template v-else-if="column.key === 'action'">
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
