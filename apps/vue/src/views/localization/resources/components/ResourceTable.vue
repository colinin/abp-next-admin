<template>
  <div class="content">
    <BasicTable @register="registerTable">
      <template #toolbar>
        <Button
          v-auth="['LocalizationManagement.Resource.Create']"
          type="primary"
          @click="handleAddNew"
        >
          {{ L('Resource:AddNew') }}
        </Button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'action'">
          <TableAction
            :stop-button-propagation="true"
            :actions="[
              {
                auth: 'LocalizationManagement.Resource.Update',
                label: L('Edit'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEdit.bind(null, record),
              },
              {
                auth: 'LocalizationManagement.Resource.Delete',
                label: L('Delete'),
                color: 'error',
                icon: 'ant-design:delete-outlined',
                onClick: handleDelete.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <ResourceModal @register="registerModal" />
  </div>
</template>

<script lang="ts" setup>
  import { onMounted } from 'vue';
  import { Button } from 'ant-design-vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { useModal } from '/@/components/Modal';
  import { getList, getByName, deleteByName } from '/@/api/localization/resources';
  import { Resource } from '/@/api/localization/resources/model';
  import { getDataColumns } from './TableData';
  import ResourceModal from './ResourceModal.vue';

  const { createConfirm, createMessage } = useMessage();
  const { L } = useLocalization(['LocalizationManagement', 'AbpLocalization', 'AbpUi']);
  const [registerModal, { openModal }] = useModal();
  const [registerTable, { setLoading, setTableData, getForm }] = useTable({
    rowKey: 'name',
    title: L('Resources'),
    columns: getDataColumns(),
    pagination: true,
    striped: false,
    useSearchForm: true,
    showTableSetting: true,
    bordered: true,
    showIndexColumn: false,
    canResize: false,
    immediate: false,
    formConfig: {
      labelWidth: 100,
      schemas: [
        {
          field: 'filter',
          component: 'Input',
          label: L('Search'),
          colProps: { span: 24 },
          defaultValue: '',
        },
      ],
      submitFunc: fetchResources,
    },
    actionColumn: {
      width: 150,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });
  onMounted(fetchResources);

  function fetchResources() {
    const form = getForm();
    return form.validate().then((input) => {
      setLoading(true);
      setTableData([]);
      return getList(input)
        .then((res) => {
          setTableData(res.items);
        })
        .finally(() => {
          setLoading(false);
        });
    });
  }

  function handleAddNew() {
    openModal(true, {});
  }

  function handleEdit(record: Resource) {
    getByName(record.name).then((dto) => {
      openModal(true, dto);
    });
  }

  function handleDelete(record: Resource) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessage'),
      onOk: () => {
        return deleteByName(record.name).then(() => {
          createMessage.success(L('SuccessfullyDeleted'));
          fetchResources();
        });
      },
    });
  }
</script>
