import type { Ref } from 'vue';

import { unref } from 'vue';
import { useMessage } from '/@/hooks/web/useMessage';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { TableActionType, useTable } from '/@/components/Table';
import { getDataColumns } from '../datas/TableData';
import { getSearchFormSchemas } from '../datas/ModalData';
import { DeleteAsyncById, GetListAsyncByInput } from '/@/api/saas/tenant';
import { formatPagedRequest } from '/@/utils/http/abp/helper';

interface UseTenantTable {
  tableElRef: Ref<Nullable<TableActionType>>;
}

export function useTenantTable({ tableElRef }: UseTenantTable) {
  const { createMessage, createConfirm } = useMessage();
  const { L } = useLocalization('AbpSaas');
  const [registerTable, {}] = useTable({
    rowKey: 'id',
    title: L('Tenants'),
    columns: getDataColumns(),
    api: GetListAsyncByInput,
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

  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessageWithFormat', [record.name]),
      okCancel: true,
      onOk: () => {
        return DeleteAsyncById(record.id).then(() => {
          createMessage.success(L('SuccessfullyDeleted'));
          handleReload();
        });
      },
    });
  }

  function handleReload() {
    const tableEl = unref(tableElRef);
    tableEl?.reload();
  }

  return {
    registerTable,
    handleDelete,
    handleReload,
  };
}
