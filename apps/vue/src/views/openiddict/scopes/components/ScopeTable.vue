<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <Button v-auth="['AbpOpenIddict.Scopes.Create']" type="primary" @click="handleAddNew">
          {{ L('Scopes:AddNew') }}
        </Button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'action'">
          <TableAction
            :stop-button-propagation="true"
            :actions="[
              {
                auth: 'AbpOpenIddict.Scopes.Update',
                label: L('Edit'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEdit.bind(null, record),
              },
              {
                auth: 'AbpOpenIddict.Scopes.Delete',
                label: L('Delete'),
                color: 'error',
                icon: 'ant-design:delete-outlined',
                onClick: handleDelete.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <ScopeModal @register="registerModal" @change="reload" />
  </div>
</template>

<script lang="ts" setup>
  import { Button } from 'ant-design-vue';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getDataColumns } from '../datas/TableData';
  import { useModal } from '/@/components/Modal';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { getList, deleteById } from '/@/api/openiddict/open-iddict-scope';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import ScopeModal from './ScopeModal.vue';

  const { L } = useLocalization(['AbpOpenIddict', 'AbpUi']);
  const { createConfirm, createMessage } = useMessage();
  const [registerModal, { openModal }] = useModal();
  const [registerTable, { reload }] = useTable({
    rowKey: 'id',
    title: L('Scopes'),
    api: getList,
    columns: getDataColumns(),
    beforeFetch: formatPagedRequest,
    pagination: true,
    striped: false,
    useSearchForm: true,
    showIndexColumn: false,
    showTableSetting: true,
    formConfig: {
      labelWidth: 100,
      schemas: [
        {
          field: 'filter',
          component: 'Input',
          label: L('Search'),
          colProps: { span: 24 },
        },
      ],
    },
    bordered: true,
    canResize: true,
    immediate: true,
    actionColumn: {
      width: 150,
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

  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessage'),
      onOk: () => {
        return deleteById(record.id).then(() => {
          createMessage.success(L('SuccessfullyDeleted'));
          reload();
        });
      },
    });
  }
</script>
