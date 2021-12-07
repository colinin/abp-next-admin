import { computed } from 'vue';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { useTable } from '/@/components/Table';
import { deleteById, getList } from '/@/api/identity/user';
import { formatPagedRequest } from '/@/utils/http/abp/helper';
import { getDataColumns } from '../datas/TableData';
import { getSearchFormSchemas } from '../datas/ModalData';
import { Modal } from 'ant-design-vue';

export function useUserTable() {
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
      slots: { customRender: 'action' },
    },
  });

  const lockEnable = computed(() => {
    return (record) => {
      // 未启用锁定不显示
      if (!record.lockoutEnabled) {
        return false;
      }
      if (record.lockoutEnd) {
        // 锁定时间高于当前时间不显示
        const lockTime = new Date(record.lockoutEnd);
        const nowTime = new Date();
        if (lockTime > nowTime) {
          return false;
        }
      }
      return true;
    };
  });

  function handleDelete(user) {
    Modal.warning({
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessageWithFormat', [user.userName] as Recordable),
      okCancel: true,
      onOk: () => {
        deleteById(user.id).then(() => {
          reloadTable();
        });
      },
    });
  }

  return {
    registerTable,
    reloadTable,
    handleDelete,
    lockEnable,
  };
}
