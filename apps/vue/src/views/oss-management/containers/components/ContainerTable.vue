<template>
  <div class="content">
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button
          v-if="hasPermission('AbpOssManagement.Container.Create')"
          type="primary"
          @click="handleAddNew"
          >{{ L('Containers:Create') }}</a-button
        >
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'action'">
          <TableAction
            :stop-button-propagation="true"
            :actions="[
              {
                auth: 'AbpOssManagement.Container.Delete',
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
    <BasicModal
      @register="registerModal"
      :title="L('Containers')"
      :width="466"
      :min-height="66"
      @ok="handleSubmit"
    >
      <BasicForm @register="registerForm" />
    </BasicModal>
  </div>
</template>

<script lang="ts" setup>
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { BasicModal, useModal } from '/@/components/Modal';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { createContainer, deleteContainer, getContainers } from '/@/api/oss-management/oss';
  import { getDataColumns } from './TableData';
  import { getSearchFormSchemas, getModalFormSchemas } from './ModalData';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';

  const { createMessage, createConfirm } = useMessage();
  const { L } = useLocalization(['AbpOssManagement', 'AbpUi']);
  const { hasPermission } = usePermission();
  const [registerModal, { openModal, closeModal }] = useModal();
  const [registerForm, { validate, resetFields }] = useForm({
    labelWidth: 120,
    schemas: getModalFormSchemas(),
    showActionButtonGroup: false,
    actionColOptions: {
      span: 24,
    },
  });
  const [registerTable, { reload }] = useTable({
    rowKey: 'name',
    title: L('Containers'),
    columns: getDataColumns(),
    api: getContainers,
    fetchSetting: {
      pageField: 'skipCount',
      sizeField: 'maxResultCount',
      listField: 'containers',
      totalField: 'maxKeys',
    },
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
      width: 120,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });

  function handleAddNew() {
    resetFields();
    openModal(true, {});
  }

  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessage'),
      okCancel: true,
      onOk: () => {
        deleteContainer(record.name).then(() => {
          createMessage.success(L('SuccessfullyDeleted'));
          reload();
        });
      },
    });
  }

  function handleSubmit() {
    validate().then((input) => {
      createContainer(input.name).then(() => {
        createMessage.success(L('Successful'));
        closeModal();
        reload();
      });
    });
  }
</script>
