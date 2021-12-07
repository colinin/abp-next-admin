<template>
  <BasicTable @register="registerTable">
    <template #action="{ record }">
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
  </BasicTable>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import { Modal } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getDataColumns } from './TableData';
  import { getSearchFormSchemas } from './ModalData';
  import { deleteById, getList } from '/@/api/identity/securityLog';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';

  export default defineComponent({
    name: 'SecurityLogTable',
    components: { BasicTable, TableAction },
    setup() {
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
        actionColumn: {
          width: 120,
          title: L('Actions'),
          dataIndex: 'action',
          slots: { customRender: 'action' },
        },
      });

      function handleDelete(record) {
        Modal.warning({
          title: L('AreYouSure'),
          content: L('ItemWillBeDeletedMessage'),
          okCancel: true,
          onOk: () => {
            deleteById(record.id).then(() => {
              reload();
            });
          },
        });
      }

      return {
        L,
        registerTable,
        handleDelete,
      };
    },
  });
</script>
