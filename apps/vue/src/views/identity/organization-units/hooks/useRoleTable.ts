import { ComputedRef } from 'vue';
import { ref, unref, watch } from 'vue';
import { useTable } from '/@/components/Table';
import { useMessage } from '/@/hooks/web/useMessage';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { getDataColumns } from '../../role/datas/TableData';
import { Role } from '/@/api/identity/roles/model';
import { getRoleList } from '/@/api/identity/organization-units';
import { MemberProps } from '../types/props';
import { removeOrganizationUnit } from '/@/api/identity/roles';

interface UseRoleTable {
  getProps: ComputedRef<MemberProps>;
}

export function useRoleTable({ getProps }: UseRoleTable) {
  const { L } = useLocalization('AbpIdentity');
  const { createMessage, createConfirm } = useMessage();
  const dataSource = ref([] as Role[]);
  const [registerTable] = useTable({
    rowKey: 'id',
    columns: getDataColumns(),
    dataSource: dataSource,
    pagination: false,
    striped: false,
    useSearchForm: false,
    showTableSetting: true,
    tableSetting: {
      redo: false,
    },
    bordered: true,
    showIndexColumn: false,
    canResize: false,
    immediate: false,
    rowSelection: { type: 'checkbox' },
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
      content: L('OrganizationUnit:AreYouSureRemoveRole', [role.name] as Recordable),
      okCancel: true,
      onOk: () => {
        return removeOrganizationUnit(role.id, unref(getProps).ouId)
          .then(() => {
            createMessage.success(L('SuccessfullyDeleted'));
            reloadRoles();
          });
      },
    });
  }

  function reloadRoles() {
    getRoleList(unref(getProps).ouId, {
      filter: '',
      skipCount: 0,
      sorting: '',
      maxResultCount: 1000,
    }).then((res) => {
      dataSource.value = res.items;
    });
  }

  watch(
    () => unref(getProps).ouId,
    (id) => {
      if (id) {
        reloadRoles();
      }
    },
  );

  return {
    registerTable,
    reloadRoles,
    handleDelete,
  };
}
