import { ComputedRef } from 'vue';
import { Modal } from 'ant-design-vue';
import { ref, unref, watch } from 'vue';
import { useTable } from '/@/components/Table';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { getDataColumns } from '../../role/datas/TableData';
import { Role } from '/@/api/identity/model/roleModel';
import { getRoleList } from '/@/api/identity/organization-units';
import { MemberProps } from '../types/props';
import { removeOrganizationUnit } from '/@/api/identity/role';

interface UseRoleTable {
  getProps: ComputedRef<MemberProps>;
}

export function useRoleTable({ getProps }: UseRoleTable) {
  const { L } = useLocalization('AbpIdentity');
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
      slots: { customRender: 'action' },
    },
  });

  function handleDelete(role) {
    Modal.warning({
      title: L('AreYouSure'),
      content: L('OrganizationUnit:AreYouSureRemoveRole', [role.name] as Recordable),
      okCancel: true,
      onOk: () => {
        removeOrganizationUnit(role.id, unref(getProps).ouId).then(() => reloadRoles());
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
