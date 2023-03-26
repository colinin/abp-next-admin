<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <Button
          v-auth="['AbpOpenIddict.Applications.Create']"
          type="primary"
          @click="handleAddNew"
        >
          {{ L('Applications:AddNew') }}
        </Button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'action'">
          <TableAction
            :stop-button-propagation="true"
            :actions="[
              {
                auth: 'AbpOpenIddict.Applications.Update',
                label: L('Edit'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEdit.bind(null, record),
              },
              {
                auth: 'AbpOpenIddict.Applications.Delete',
                label: L('Delete'),
                color: 'error',
                icon: 'ant-design:delete-outlined',
                onClick: handleDelete.bind(null, record),
              },
            ]"
            :dropDownActions="[
              {
                auth: 'AbpOpenIddict.Applications.ManagePermissions',
                label: L('ManagePermissions'),
                onClick: handlePermission.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <ApplicationModal @register="registerModal" @change="reload" />
    <PermissionModal @register="registerPermissionModal" />
  </div>
</template>

<script lang="ts" setup>
  import { Button } from 'ant-design-vue';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getDataColumns } from '../datas/TableData';
  import { getSearchFormProps } from '../datas/ModalData';
  import { useModal } from '/@/components/Modal';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { GetListAsyncByInput, DeleteAsyncById } from '/@/api/openiddict/open-iddict-application';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import { PermissionModal } from '/@/components/Permission';
  import ApplicationModal from './ApplicationModal.vue';

  const { L } = useLocalization(['AbpOpenIddict']);
  const { createConfirm, createMessage } = useMessage();
  const [registerModal, { openModal }] = useModal();
  const [registerPermissionModal, { openModal: openPermissionModal }] = useModal();
  const [registerTable, { reload }] = useTable({
    rowKey: 'id',
    title: L('Applications'),
    api: GetListAsyncByInput,
    columns: getDataColumns(),
    beforeFetch: formatPagedRequest,
    pagination: true,
    striped: false,
    useSearchForm: true,
    showIndexColumn: false,
    showTableSetting: true,
    formConfig: getSearchFormProps(),
    bordered: true,
    canResize: true,
    immediate: true,
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

  function handlePermission(record) {
    const props = {
      providerName: 'C',
      providerKey: record.clientId,
    };
    openPermissionModal(true, props, true);
  }

  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessage'),
      onOk: () => {
        return DeleteAsyncById(record.key).then(() => {
          createMessage.success(L('SuccessfullyDeleted'));
          reload();
        });
      },
    });
  }
</script>
