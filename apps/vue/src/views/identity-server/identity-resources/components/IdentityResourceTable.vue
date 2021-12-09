<template>
  <div>
    <BasicTable @register="registerTable">
      <template #enabled="{ record }">
        <Switch :checked="record.enabled" disabled />
      </template>
      <template #required="{ record }">
        <Switch :checked="record.required" disabled />
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
              auth: 'AbpIdentityServer.IdentityResources.Update',
              icon: 'ant-design:edit-outlined',
              label: L('Resource:Edit'),
              onClick: handleEdit.bind(null, record),
            },
            {
              auth: 'AbpIdentityServer.IdentityResources.Delete',
              color: 'error',
              icon: 'ant-design:delete-outlined',
              label: L('Resource:Delete'),
              onClick: handleDelete.bind(null, record),
            },
          ]"
        />
      </template>
    </BasicTable>
    <IdentityResourceModal @register="registerModal" @change="handleChange" />
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
  import { deleteById, getList } from '/@/api/identity-server/identityResources';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import IdentityResourceModal from './IdentityResourceModal.vue';

  export default defineComponent({
    name: 'IdentityResourceTable',
    components: { IdentityResourceModal, BasicTable, Button, Switch, TableAction },
    setup() {
      const { L } = useLocalization('AbpIdentityServer');
      const [registerModal, { openModal, closeModal }] = useModal();
      const [registerTable, { reload }] = useTable({
        rowKey: 'id',
        title: L('DisplayName:IdentityResources'),
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
