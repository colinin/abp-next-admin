import type { ComputedRef } from 'vue';

import { unref } from 'vue';
import { useLocalization } from '/@/hooks/abp/useLocalization';
import { useModal } from '/@/components/Modal';
import { getUnaddedMemberList } from '/@/api/identity/organization-units';
import { BasicColumn, useTable } from '/@/components/Table';
import { formatPagedRequest } from '/@/utils/http/abp/helper';
import { MemberProps } from '../types/props';

interface UseMemberModal {
  getProps: ComputedRef<MemberProps>;
}

export function useMemberModal({ getProps }: UseMemberModal) {
  const { L } = useLocalization('AbpIdentity');
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

  const [registerTable, { reload: reloadTable, clearSelectedRowKeys }] = useTable({
    rowKey: 'id',
    columns: dataColumns,
    api: getUnaddedMemberList,
    beforeFetch: (request) => {
      request.id = unref(getProps).ouId;
      formatPagedRequest(request);
    },
    pagination: true,
    striped: false,
    useSearchForm: false,
    showTableSetting: true,
    bordered: true,
    showIndexColumn: false,
    canResize: false,
    immediate: true,
    rowSelection: { type: 'checkbox' },
  });

  const [registerModal, { openModal }] = useModal();

  function handleAddNew() {
    openModal(true, {}, true);
  }

  return {
    registerModal,
    handleAddNew,
    registerTable,
    reloadTable,
    clearSelectedRowKeys,
  };
}
