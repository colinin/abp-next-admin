<template>
  <BasicTable @register="registerTable">
    <template #bodyCell="{ column, record }">
      <template v-if="column.key === 'actions'">
        <TableAction
          :actions="[
            {
              auth: 'AbpAuditing.SecurityLog.Delete',
              color: 'error',
              label: L('Delete'),
              icon: 'ant-design:delete-outlined',
              onClick: handleDelete.bind(null, record),
            },
          ]"
        />
      </template>
    </template>
  </BasicTable>
</template>

<script lang="ts" setup>
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getDataColumns } from './TableData';
  import { getSearchFormSchemas } from './ModalData';
  import { deleteById, getList } from '/@/api/auditing/security-logs';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';

  const { createMessage, createConfirm } = useMessage();
  const { L } = useLocalization('AbpAuditLogging');
  const [registerTable, { reload }] = useTable({
    rowKey: 'id',
    title: L('SecurityLog'),
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
    scroll: { x: 'max-content', y: '100%' },
    actionColumn: {
      width: 120,
      title: L('Actions'),
      dataIndex: 'actions',
    },
  });

  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessage'),
      okCancel: true,
      onOk: () => {
        return deleteById(record.id).then(() => {
          createMessage.success(L('SuccessfullyDeleted'));
          reload();
        });
      },
    });
  }
</script>
