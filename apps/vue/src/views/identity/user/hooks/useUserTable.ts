import { computed } from 'vue';
import { useMessage } from '/@/hooks/web/useMessage';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { useTable } from '/@/components/Table';
import { deleteById, getList } from '/@/api/identity/users';
import { formatPagedRequest } from '/@/utils/http/abp/helper';
import { getDataColumns } from '../datas/TableData';
import { getSearchFormSchemas } from '../datas/ModalData';

export function useUserTable() {
  const { createMessage, createConfirm } = useMessage();
  const { L } = useLocalization('AbpIdentity');
  const [registerTable, { reload: reloadTable }] = useTable({
    rowKey: 'id',
    title: L('Users'),
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

  const lockEnd = computed(() => {
    return (record) => {
      if (record.lockoutEnd) {
        const lockTime = new Date(record.lockoutEnd);
        if (lockTime) {
          // 锁定时间高于当前时间不显示
          const nowTime = new Date();
          return lockTime > nowTime;
        }
      }
      return false;
    };
  });

  function handleDelete(user) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessageWithFormat', [user.userName] as Recordable),
      okCancel: true,
      onOk: () => {
        return deleteById(user.id)
          .then(() => {
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
    lockEnd,
  };
}
