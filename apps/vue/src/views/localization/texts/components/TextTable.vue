<template>
  <div class="content">
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button
          v-if="hasPermission('LocalizationManagement.Text.Create')"
          type="primary"
          @click="handleAddNew"
          >{{ L('Text:AddNew') }}</a-button
        >
      </template>
      <template #enable="{ record }">
        <Switch :checked="record.enable" disabled />
      </template>
      <template #action="{ record }">
        <TableAction
          :stop-button-propagation="true"
          :actions="[
            {
              auth: 'LocalizationManagement.Text.Update',
              label: L('Edit'),
              icon: 'ant-design:edit-outlined',
              onClick: handleEdit.bind(null, record),
            },
            {
              auth: 'LocalizationManagement.Text.Delete',
              color: 'error',
              label: L('Delete'),
              icon: 'ant-design:delete-outlined',
              onClick: handleDelete.bind(null, record),
            },
          ]"
        />
      </template>
    </BasicTable>
    <TextModal @change="handleChange" @register="registerModal" />
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import { Switch, Modal } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { useModal } from '/@/components/Modal';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import { getList, deleteById } from '/@/api/localization/texts';
  import { getDataColumns } from './TableData';
  import { getSearchFormSchemas } from './ModalData';
  import TextModal from './TextModal.vue';

  export default defineComponent({
    name: 'TextTable',
    components: {
      BasicTable,
      Switch,
      TableAction,
      TextModal,
    },
    setup() {
      const { L } = useLocalization('LocalizationManagement', 'AbpUi');
      const { hasPermission } = usePermission();
      const [registerModal, { openModal }] = useModal();
      const [registerTable, { reload }] = useTable({
        rowKey: 'id',
        title: L('Texts'),
        columns: getDataColumns(),
        api: getList,
        beforeFetch: (request) => {
          formatPagedRequest(request);
          // 处理类型为boolean时的控制台警告
          request.onlyNull = request.onlyNull === 1;
        },
        pagination: true,
        striped: false,
        useSearchForm: true,
        showTableSetting: true,
        bordered: true,
        showIndexColumn: false,
        canResize: false,
        immediate: false,
        rowSelection: { type: 'checkbox' },
        formConfig: getSearchFormSchemas(),
        actionColumn: {
          width: 180,
          title: L('Actions'),
          dataIndex: 'action',
          slots: { customRender: 'action' },
        },
      });

      function handleChange() {
        reload();
      }

      function handleAddNew() {
        openModal(true, { id: null });
      }

      function handleEdit(record) {
        openModal(true, record);
      }

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
        hasPermission,
        registerTable,
        registerModal,
        openModal,
        handleChange,
        handleAddNew,
        handleEdit,
        handleDelete,
      };
    },
  });
</script>
