<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button
          v-if="hasPermission('ApiGateway.Global.Create')"
          type="primary"
          @click="handleAddNew"
          >{{ L('AddNew') }}</a-button
        >
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              auth: 'ApiGateway.Global.Update',
              label: L('Edit'),
              icon: 'ant-design:edit-outlined',
              onClick: handleEdit.bind(null, record),
            },
            {
              auth: 'ApiGateway.Global.Delete',
              color: 'error',
              label: L('Delete'),
              icon: 'ant-design:delete-outlined',
              onClick: handleDelete.bind(null, record),
            },
          ]"
        />
      </template>
    </BasicTable>
    <GlobalModal @register="registerModal" @change="reloadTable" />
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import { cloneDeep } from 'lodash-es';
  import { Modal } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import { useModal } from '/@/components/Modal';
  import GlobalModal from './GlobalModal.vue';
  import { create, deleteByAppId, getList, update } from '/@/api/api-gateway/global';
  import {
    CreateGlobalConfiguration,
    GlobalConfiguration,
    UpdateGlobalConfiguration,
  } from '/@/api/api-gateway/model/globalModel';
  import { getDataColumns } from './TableData';
  import { getSearchFormSchemas } from './ModalData';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  export default defineComponent({
    name: 'GlobalTable',
    components: { BasicTable, GlobalModal, TableAction },
    setup() {
      const { L } = useLocalization('ApiGateway');
      const { hasPermission } = usePermission();
      const [registerModal, { openModal }] = useModal();
      const [registerTable, { reload: reloadTable }] = useTable({
        rowKey: 'itemId',
        title: L('Globals'),
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
        rowSelection: { type: 'checkbox' },
        formConfig: getSearchFormSchemas(),
        actionColumn: {
          width: 160,
          title: L('Actions'),
          dataIndex: 'action',
          slots: { customRender: 'action' },
        },
      });

      return {
        L,
        registerModal,
        openModal,
        registerTable,
        reloadTable,
        hasPermission,
      };
    },
    methods: {
      handleAddNew() {
        this.openModal(true, {}, true);
      },
      handleEdit(record) {
        this.openModal(true, record, true);
      },
      handleDelete(record: Recordable) {
        Modal.warning({
          title: this.L('AreYouSure'),
          content: this.L('ItemWillBeDeletedMessageWithFormat', [record.appId] as Recordable),
          okCancel: true,
          onOk: () => {
            deleteByAppId(record.appId).then(() => {
              this.reloadTable();
            });
          },
        });
      },
      handleSaveChanges(val) {
        const api: Promise<GlobalConfiguration> = val.id
          ? update(cloneDeep(val) as UpdateGlobalConfiguration)
          : create(cloneDeep(val) as CreateGlobalConfiguration);
        return api.then(() => {
          this.reloadTable();
        });
      },
    },
  });
</script>
