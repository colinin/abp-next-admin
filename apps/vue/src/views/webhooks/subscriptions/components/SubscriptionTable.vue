<template>
  <div class="content">
    <BasicTable @register="registerTable" @selection-change="handleSelectionChange">
      <template #toolbar>
        <Button v-auth="['AbpWebhooks.Subscriptions.Create']" type="primary" @click="handleAddNew">
          {{ L('Subscriptions:AddNew') }}
        </Button>
        <Button
          v-if="deleteManyEnabled"
          v-auth="['AbpWebhooks.Subscriptions.Delete']"
          danger
          @click="handleDeleteMany"
        >
          {{ L('Delete') }}
        </Button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'isActive'">
          <CheckOutlined v-if="record.isActive" class="enable" />
          <CloseOutlined v-else class="disable" />
        </template>
        <template v-else-if="column.key === 'webhooks'">
          <Tag v-for="hook in record.webhooks" color="blue">{{ hook }}</Tag>
        </template>
        <template v-else-if="column.key === 'action'">
          <TableAction
            :stop-button-propagation="true"
            :actions="[
              {
                auth: 'AbpWebhooks.Subscriptions.Update',
                label: L('Edit'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEdit.bind(null, record),
              },
              {
                auth: 'AbpWebhooks.Subscriptions.Delete',
                color: 'error',
                label: L('Delete'),
                icon: 'ant-design:delete-outlined',
                onClick: handleDelete.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <SubscriptionModal @register="registerModal" @change="reload" />
  </div>
</template>

<script lang="ts" setup>
  import { computed, ref } from 'vue';
  import { Button, Tag } from 'ant-design-vue';
  import { CheckOutlined, CloseOutlined } from '@ant-design/icons-vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useModal } from '/@/components/Modal';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import { getDataColumns } from '../datas/TableData';
  import { getSearchFormSchemas } from '../datas/ModalData';
  import {
    DeleteAsyncById,
    DeleteManyAsyncByInput,
    GetListAsyncByInput,
  } from '/@/api/webhooks/subscriptions';
  import SubscriptionModal from './SubscriptionModal.vue';

  const { createConfirm, createMessage } = useMessage();
  const { L } = useLocalization(['WebhooksManagement', 'AbpUi']);
  const [registerModal, { openModal }] = useModal();
  const [registerTable, { reload, setLoading, clearSelectedRowKeys }] = useTable({
    rowKey: 'id',
    title: L('Subscriptions'),
    columns: getDataColumns(),
    api: GetListAsyncByInput,
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
    rowSelection: {
      type: 'checkbox',
    },
  });
  const selectionKeys = ref<string[]>([]);
    const deleteManyEnabled = computed(() => {
    return selectionKeys.value.length;
  });

  function handleSelectionChange(e: { keys: string[] }) {
    selectionKeys.value = e.keys;
  }

  function handleAddNew() {
    openModal(true, { id: null });
  }

  function handleEdit(record) {
    openModal(true, record);
  }

  function handleDeleteMany() {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessageWithFormat', { 0: L('SelectedItems') }),
      okCancel: true,
      onOk: () => {
        setLoading(true);
        return DeleteManyAsyncByInput({
          recordIds: selectionKeys.value,
        })
          .then(() => {
            createMessage.success(L('SuccessfullyDeleted'));
            clearSelectedRowKeys();
            reload();
          })
          .finally(() => {
            setLoading(false);
          });
      },
    });
  }

  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessage'),
      okCancel: true,
      onOk: () => {
        setLoading(true);
        return DeleteAsyncById(record.id)
          .then(() => {
            createMessage.success(L('SuccessfullyDeleted'));
            clearSelectedRowKeys();
            reload();
            reload();
          })
          .finally(() => {
            setLoading(false);
          });
      },
    });
  }
</script>
