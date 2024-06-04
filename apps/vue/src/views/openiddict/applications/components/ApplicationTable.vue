<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <Button v-auth="['AbpOpenIddict.Applications.Create']" type="primary" @click="handleAddNew">
          {{ L('Applications:AddNew') }}
        </Button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'action'">
          <TableAction
            :stop-button-propagation="true"
            :actions="[
              {
                auth: 'AbpOpenIddict.Applications.Update',
                label: L('Edit'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEdit.bind(null, record),
              },
              {
                auth: 'AbpOpenIddict.Applications.Delete',
                label: L('Delete'),
                color: 'error',
                icon: 'ant-design:delete-outlined',
                onClick: handleDelete.bind(null, record),
              },
            ]"
            :dropDownActions="[
              {
                auth: 'AbpOpenIddict.Applications.ManagePermissions',
                label: L('ManagePermissions'),
                onClick: handleManagePermissions.bind(null, record),
              },
              {
                auth: 'AbpOpenIddict.Applications.ManageFeatures',
                label: L('ManageFeatures'),
                onClick: handleManageFeatures.bind(null, record),
              },
              {
                auth: 'AbpOpenIddict.Applications.ManageSecret',
                label: L('GenerateSecret'),
                ifShow: getShowSecret(record),
                onClick: handleGenerateSecret.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <ApplicationModal @register="registerModal" @change="reload" />
    <ApplicationSecretModal @register="registerSecretModal" @change="reload" />
    <PermissionModal @register="registerPermissionModal" />
    <FeatureModal @register="registerFeatureModal" />
  </div>
</template>

<script lang="ts" setup>
  import { computed } from 'vue';
  import { Button } from 'ant-design-vue';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getDataColumns } from '../datas/TableData';
  import { useModal } from '/@/components/Modal';
  import { FeatureModal } from '/@/components/Abp';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { getList, deleteById } from '/@/api/openiddict/open-iddict-application';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import { PermissionModal } from '/@/components/Permission';
  import ApplicationModal from './ApplicationModal.vue';
  import ApplicationSecretModal from './ApplicationSecretModal.vue';

  const { L } = useLocalization(['AbpOpenIddict', 'AbpUi']);
  const { createConfirm, createMessage } = useMessage();
  const [registerModal, { openModal }] = useModal();
  const [registerSecretModal, { openModal: openSecretModal }] = useModal();
  const [registerFeatureModal, { openModal: openFeatureModal }] = useModal();
  const [registerPermissionModal, { openModal: openPermissionModal }] = useModal();
  const [registerTable, { reload }] = useTable({
    rowKey: 'id',
    title: L('Applications'),
    api: getList,
    columns: getDataColumns(),
    beforeFetch: formatPagedRequest,
    pagination: true,
    striped: false,
    useSearchForm: true,
    showIndexColumn: false,
    showTableSetting: true,
    formConfig: {
      labelWidth: 100,
      schemas: [
        {
          field: 'filter',
          component: 'Input',
          label: L('Search'),
          colProps: { span: 24 },
        },
      ],
    },
    bordered: true,
    canResize: true,
    immediate: true,
    actionColumn: {
      width: 200,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });
  const getShowSecret = computed(() => {
    return (record) => {
      return record.clientType === 'confidential';
    };
  });

  function handleAddNew() {
    openModal(true, {});
  }

  function handleEdit(record) {
    openModal(true, record);
  }

  function handleGenerateSecret(record) {
    openSecretModal(true, record);
  }

  function handleManagePermissions(record) {
    const props = {
      providerName: 'C',
      providerKey: record.clientId,
    };
    openPermissionModal(true, props, true);
  }

  function handleManageFeatures(record) {
    const props = {
      providerName: 'C',
      providerKey: record.clientId,
    };
    openFeatureModal(true, props, true);
  }

  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessage'),
      onOk: () => {
        return deleteById(record.id).then(() => {
          createMessage.success(L('SuccessfullyDeleted'));
          reload();
        });
      },
    });
  }
</script>
