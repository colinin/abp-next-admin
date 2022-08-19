<template>
  <div class="content">
    <BasicTable @register="registerTable">
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'responseStatusCode'">
          <Tag :color="getHttpStatusColor(record.responseStatusCode)">{{
            httpStatusCodeMap[record.responseStatusCode]
          }}</Tag>
        </template>
        <template v-else-if="column.key === 'action'">
          <TableAction
            :stop-button-propagation="true"
            :actions="[
              {
                auth: 'AbpWebhooks.SendAttempts',
                label: L('Edit'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEdit.bind(null, record),
              },
              {
                auth: 'AbpWebhooks.SendAttempts.Delete',
                color: 'error',
                label: L('Delete'),
                icon: 'ant-design:delete-outlined',
                onClick: handleDelete.bind(null, record),
              },
            ]"
            :dropDownActions="[
              {
                auth: 'AbpWebhooks.SendAttempts.Resend',
                label: L('Resend'),
                onClick: handleResend.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <SendAttemptModal @register="registerModal" />
  </div>
</template>

<script lang="ts" setup>
  import { Tag } from 'ant-design-vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useModal } from '/@/components/Modal';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import { getDataColumns } from '../datas/TableData';
  import { getSearchFormSchemas } from '../datas/ModalData';
  import { httpStatusCodeMap, getHttpStatusColor } from '../../typing';
  import { getList, deleteById, resend } from '/@/api/webhooks/send-attempts';
  import SendAttemptModal from './SendAttemptModal.vue';

  const { createConfirm, createMessage } = useMessage();
  const { L } = useLocalization(['WebhooksManagement', 'AbpUi']);
  const [registerModal, { openModal }] = useModal();
  const [registerTable, { reload, setLoading }] = useTable({
    rowKey: 'id',
    title: L('SendAttempts'),
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
    clickToRowSelect: false,
    formConfig: getSearchFormSchemas(),
    actionColumn: {
      width: 220,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });

  function handleEdit(record) {
    openModal(true, record);
  }

  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessage'),
      okCancel: true,
      onOk: () => {
        deleteById(record.id).then(() => {
          reload();
          createMessage.success(L('Successful'));
        });
      },
    });
  }

  function handleResend(record) {
    setLoading(true);
    resend(record.id)
      .then(() => {
        createMessage.success(L('Successful'));
      })
      .finally(() => {
        setLoading(false);
      });
  }
</script>
