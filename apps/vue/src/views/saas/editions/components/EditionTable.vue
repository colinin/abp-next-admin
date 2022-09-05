<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button
          v-if="hasPermission('AbpSaas.Editions.Create')"
          type="primary"
          @click="handleAddNew"
          >{{ L('NewEdition') }}</a-button
        >
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'action'">
          <TableAction
            :actions="[
              {
                auth: 'AbpSaas.Editions.Update',
                label: L('Edit'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEdit.bind(null, record),
              },
              {
                auth: 'AbpSaas.Editions.Delete',
                color: 'error',
                label: L('Delete'),
                icon: 'ant-design:delete-outlined',
                onClick: handleDelete.bind(null, record),
              },
            ]"
            :dropDownActions="[
              {
                auth: 'AbpSaas.Editions.ManageFeatures',
                label: L('ManageFeatures'),
                onClick: handleManageFeature.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <EditionModal @register="registerModal" @change="reload" />
    <FeatureModal @register="registerFeatureModal" />
  </div>
</template>

<script lang="ts" setup>
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useModal } from '/@/components/Modal';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { useFeatureModal } from '../hooks/useFeatureModal';
  import { FeatureModal } from '../../../feature';
  import { deleteById, getList } from '../../../../api/saas/editions';
  import { getDataColumns } from '../datas/TableData';
  import { getSearchFormSchemas } from '../datas//ModalData';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import EditionModal from './EditionModal.vue';

  const { L } = useLocalization(['AbpSaas', 'AbpFeatureManagement']);
  const { createConfirm, createMessage } = useMessage();
  const { hasPermission } = usePermission();
  const [registerModal, { openModal }] = useModal();
  const { registerModal: registerFeatureModal, handleManageFeature } = useFeatureModal();
  const [registerTable, { reload }] = useTable({
    rowKey: 'id',
    title: L('Editions'),
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
      width: 200,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });

  function handleAddNew() {
    openModal(true, {});
  }

  function handleEdit(record) {
    openModal(true, record);
  }

  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessageWithFormat', record.displayName),
      okCancel: true,
      onOk: () => {
        return deleteById(record.id).then(() => {
          createMessage.success(L('SuccessfullyDeleted'));
          reload();
        });
      },
    });
  }
</script>
