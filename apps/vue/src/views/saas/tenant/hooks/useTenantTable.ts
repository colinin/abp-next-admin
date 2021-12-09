import type { Ref } from 'vue';

import { unref } from 'vue';
import { Modal } from 'ant-design-vue';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { TableActionType, useTable } from '/@/components/Table';
import { getDataColumns } from '../datas/TableData';
import { getSearchFormSchemas } from '../datas//ModalData';
import { deleteById, getList } from '/@/api/saas/tenant';
import { formatPagedRequest } from '/@/utils/http/abp/helper';

interface UseTenantTable {
  tableElRef: Ref<Nullable<TableActionType>>;
}

export function useTenantTable({ tableElRef }: UseTenantTable) {
  const { L } = useLocalization('AbpTenantManagement');
  const [registerTable, {}] = useTable({
    rowKey: 'id',
    title: L('Tenants'),
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

  function handleDelete(record) {
    Modal.warning({
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessageWithFormat', [record.name] as Recordable),
      okCancel: true,
      onOk: () => {
        deleteById(record.id).then(() => {
          const tableEl = unref(tableElRef);
          tableEl?.reload();
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
