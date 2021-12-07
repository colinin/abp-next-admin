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
          v-if="hasPermission('AbpTenantManagement.Tenants.Create')"
          type="primary"
          @click="handleAddNew"
          >{{ L('ConnectionStrings:AddNew') }}</a-button
        >
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              auth: 'AbpTenantManagement.Tenants.ManageConnectionStrings',
              color: 'error',
              label: L('Delete'),
              icon: 'ant-design:delete-outlined',
              onClick: handleDelete.bind(null, record),
            },
          ]"
        />
      </template>
    </BasicTable>
    <BasicModal @register="registerEditModal" :title="L('ConnectionStrings')" @ok="handleSubmit">
      <BasicForm @register="registerEditForm" />
    </BasicModal>
  </BasicModal>
</template>

<script lang="ts">
  import { defineComponent, ref, unref, watch } from 'vue';
  import { Modal } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { BasicModal, useModal, useModalInner } from '/@/components/Modal';
  import { getConnectionStringsColumns } from '../datas/TableData';
  import { getConnectionFormSchemas } from '../datas//ModalData';
  import {
    deleteConnectionString,
    getConnectionStrings,
    setConnectionString,
  } from '/@/api/saas/tenant';

  export default defineComponent({
    components: { BasicForm, BasicModal, BasicTable, TableAction },
    setup() {
      const { L } = useLocalization('AbpTenantManagement');
      const tenantIdRef = ref('');
      const connectionsRef = ref<any[]>([]);
      const { hasPermission } = usePermission();
      const [registerEditForm, { validate }] = useForm({
        schemas: getConnectionFormSchemas(),
        showActionButtonGroup: false,
      });
      const [registerEditModal, { openModal: openEditModal, closeModal: closeEditModal }] =
        useModal();
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
          slots: { customRender: 'action' },
        },
      });

      function handleAddNew() {
        openEditModal();
      }

      function handleDelete(record) {
        Modal.warning({
          title: L('AreYouSure'),
          content: L('TenantDeletionConfirmationMessage', [record.name]),
          okCancel: true,
          onOk: () => {
            deleteConnectionString(unref(tenantIdRef), record.name).then(() => {
              handleReloadTable();
            });
          },
        });
      }

      function handleSubmit() {
        validate().then((input) => {
          setConnectionString(unref(tenantIdRef), input).then(() => {
            closeEditModal();
            handleReloadTable();
          });
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

      return {
        L,
        hasPermission,
        registerModal,
        registerEditForm,
        registerEditModal,
        registerTable,
        handleAddNew,
        handleDelete,
        handleSubmit,
      };
    },
  });
</script>
