<template>
  <div class="content">
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button
          v-if="hasPermission('LocalizationManagement.Language.Create')"
          type="primary"
          @click="handleAddNew"
          >{{ L('Language:AddNew') }}</a-button
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
              auth: 'LocalizationManagement.Language.Update',
              label: L('Edit'),
              icon: 'ant-design:edit-outlined',
              onClick: handleEdit.bind(null, record),
            },
            {
              auth: 'LocalizationManagement.Language.Delete',
              color: 'error',
              label: L('Delete'),
              icon: 'ant-design:delete-outlined',
              onClick: handleDelete.bind(null, record),
            },
          ]"
        />
      </template>
    </BasicTable>
    <LanguageModal @change="handleChange" @register="registerModal" />
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
  import { getList, deleteById } from '/@/api/localization/languages';
  import { getDataColumns } from './TableData';
  import { getSearchFormSchemas } from './ModalData';
  import LanguageModal from './LanguageModal.vue';

  export default defineComponent({
    name: 'LanguageTable',
    components: {
      BasicTable,
      Switch,
      TableAction,
      LanguageModal,
    },
    setup() {
      const { L } = useLocalization('LocalizationManagement');
      const { hasPermission } = usePermission();
      const [registerModal, { openModal }] = useModal();
      const [registerTable, { reload }] = useTable({
        rowKey: 'id',
        title: L('Languages'),
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
