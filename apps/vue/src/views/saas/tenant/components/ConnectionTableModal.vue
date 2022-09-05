<template>
  <BasicModal
    :title="L('ConnectionStrings')"
    :width="800"
    :height="500"
    :showOkBtn="false"
    :showCancelBtn="false"
    @register="registerModal"
  >
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button
          v-if="hasPermission('AbpSaas.Tenants.ManageConnectionStrings')"
          type="primary"
          @click="handleAddNew"
          >{{ L('ConnectionStrings:AddNew') }}</a-button
        >
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'action'">
          <TableAction
            :actions="[
              {
                auth: 'AbpSaas.Tenants.ManageConnectionStrings',
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
    <ConnectionEditModal @register="registerEditModal" @change="handleReloadTable" />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { ref, unref, watch } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { BasicModal, useModal, useModalInner } from '/@/components/Modal';
  import { getConnectionStringsColumns } from '../datas/TableData';
  import { deleteConnectionString, getConnectionStrings } from '/@/api/saas/tenant';
  import ConnectionEditModal from './ConnectionEditModal.vue';

  defineEmits(['register']);
  const { createMessage, createConfirm } = useMessage();
  const { L } = useLocalization(['AbpSaas']);
  const tenantIdRef = ref('');
  const connectionsRef = ref<any[]>([]);
  const { hasPermission } = usePermission();
  const [registerEditModal, { openModal: openEditModal }] = useModal();
  const [registerModal] = useModalInner((data) => {
    tenantIdRef.value = data.id;
  });
  const [registerTable] = useTable({
    rowKey: 'name',
    columns: getConnectionStringsColumns(),
    dataSource: connectionsRef,
    pagination: false,
    striped: false,
    useSearchForm: false,
    showTableSetting: false,
    bordered: true,
    showIndexColumn: false,
    canResize: false,
    actionColumn: {
      width: 120,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });

  function handleAddNew() {
    openEditModal(true, { id: tenantIdRef.value });
  }

  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('TenantDeletionConfirmationMessage', [record.name]),
      okCancel: true,
      onOk: () => {
        return deleteConnectionString(unref(tenantIdRef), record.name).then(() => {
          createMessage.success(L('SuccessfullyDeleted'));
          handleReloadTable();
        });
      },
    });
  }

  function handleReloadTable() {
    const tenantId = unref(tenantIdRef);
    if (tenantId) {
      getConnectionStrings(tenantId).then((res) => {
        connectionsRef.value = res.items;
      });
    }
  }

  watch(
    () => unref(tenantIdRef),
    () => {
      handleReloadTable();
    },
  );
</script>
