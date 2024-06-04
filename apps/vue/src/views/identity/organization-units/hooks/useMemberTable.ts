import type { ComputedRef } from 'vue';
import { watch, ref, unref } from 'vue';
import { useMessage } from '/@/hooks/web/useMessage';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { BasicColumn, useTable } from '/@/components/Table';
import { User } from '/@/api/identity/users/model';
import { removeOrganizationUnit } from '/@/api/identity/users';
import { getMemberList } from '/@/api/identity/organization-units';
import { MemberProps } from '../types/props';

interface UseMemberTable {
  getProps: ComputedRef<MemberProps>;
}

export function useMemberTable({ getProps }: UseMemberTable) {
  const { L } = useLocalization('AbpIdentity');
  const { createMessage, createConfirm } = useMessage();
  const dataSource = ref([] as User[]);
  const dataColumns: BasicColumn[] = [
    {
      title: 'id',
      dataIndex: 'id',
      width: 1,
      ifShow: false,
    },
    {
      title: L('DisplayName:UserName'),
      dataIndex: 'userName',
      align: 'left',
      width: 280,
      sorter: true,
    },
    {
      title: L('EmailAddress'),
      dataIndex: 'email',
      align: 'left',
      width: 'auto',
      sorter: true,
    },
  ];

  const [registerTable] = useTable({
    rowKey: 'id',
    columns: dataColumns,
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
    actionColumn: {
      width: 170,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });

  function handleDelete(user) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('OrganizationUnit:AreYouSureRemoveUser', [user.userName] as Recordable),
      okCancel: true,
      onOk: () => {
        return removeOrganizationUnit(user.id, unref(getProps).ouId)
          .then(() => {
            createMessage.success(L('SuccessfullyDeleted'));
            reloadMembers();
          });
      },
    });
  }

  function reloadMembers() {
    getMemberList(unref(getProps).ouId, {
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
        reloadMembers();
      }
    },
  );

  return {
    registerTable,
    handleDelete,
    reloadMembers,
  };
}
