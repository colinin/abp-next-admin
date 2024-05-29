import { useLocalization } from '/@/hooks/abp/useLocalization';
import { useMessage } from '/@/hooks/web/useMessage';
import { useTable } from '/@/components/Table';
import { deleteById, getList } from '/@/api/identity/roles';
import { formatPagedRequest } from '/@/utils/http/abp/helper';
import { getDataColumns } from '../datas/TableData';
import { getSearchFormSchemas } from '../datas/ModalData';

export function useRoleTable() {
  const { L } = useLocalization('AbpIdentity');
  const { createMessage, createConfirm } = useMessage();
  const [registerTable, { reload: reloadTable }] = useTable({
    rowKey: 'id',
    title: L('Roles'),
    columns: getDataColumns(),
    api: getList,
    beforeFetch: formatPagedRequest,
    pagination: true,
    striped: false,
    useSearchForm: true,
    showTableSetting: true,
    bordered: true,
    showIndexColumn: false,
    canResize: false,
    immediate: true,
    rowSelection: { type: 'checkbox' },
    formConfig: getSearchFormSchemas(),
    actionColumn: {
      width: 220,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });

  function handleDelete(role) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessageWithFormat', [role.name] as Recordable),
      okCancel: true,
      onOk: () => {
        return deleteById(role.id).then(() => {
          createMessage.success(L('SuccessfullyDeleted'));
          reloadTable();
        });
      },
    });
  }

  return {
    registerTable,
    reloadTable,
    handleDelete,
  };
}
