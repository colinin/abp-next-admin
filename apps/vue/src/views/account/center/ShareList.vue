<template>
  <BasicTable @register="registerTable">
    <template #action="{ record }">
      <TableAction
        :stop-button-propagation="true"
        :actions="[
          {
            color: 'error',
            label: L('Delete'),
            icon: 'ant-design:delete-outlined',
            ifShow: deleteEnabled,
            onClick: handleDelete.bind(null, record),
          },
        ]"
      />
    </template>
  </BasicTable>
</template>

<script lang="ts" setup>
  import { defineProps, defineEmits, watchEffect } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getShareDataColumns } from './data';
  import { getShareList as getShares } from '/@/api/oss-management/private';

  const props = defineProps({
    selectGroup: {
      type: String,
      required: true,
      default: 'private',
    },
    deleteEnabled: {
      type: Boolean,
      required: true,
      default: false,
    },
  });
  const emit = defineEmits(['delete:file:share']);

  const { L } = useLocalization('AbpOssManagement', 'AbpUi');
  const { createConfirm } = useMessage();
  const [registerTable, { setTableData }] = useTable({
    rowKey: 'url',
    columns: getShareDataColumns(),
    title: L('FileList'),
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
      width: 180,
      title: L('Actions'),
      dataIndex: 'action',
      slots: { customRender: 'action' },
    },
  });

  watchEffect(() => {
    props.selectGroup === 'share' && _fetchShareList();
  });

  function _fetchShareList() {
    getShares().then((res) => {
      setTableData(res.items);
    });
  }

  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessage'),
      onOk: () => {
        emit('delete:file:share', record);
      },
    });
  }
</script>
