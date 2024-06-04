<template>
  <div class="content">
    <BasicTable @register="registerTable">
      <template #toolbar>
        <Button
          v-auth="['LocalizationManagement.Language.Create']"
          type="primary"
          @click="handleAddNew"
        >
          {{ L('Language:AddNew') }}
        </Button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'action'">
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
    <LanguageModal @register="registerModal" />
  </div>
</template>

<script lang="ts" setup>
  import { onMounted } from 'vue';
  import { Button } from 'ant-design-vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { useModal } from '/@/components/Modal';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import { getList, getByName, deleteByName } from '/@/api/localization/languages';
  import { Language } from '/@/api/localization/languages/model';
  import { getDataColumns } from './TableData';
  import LanguageModal from './LanguageModal.vue';

  const { createConfirm, createMessage } = useMessage();
  const { L } = useLocalization(['LocalizationManagement', 'AbpLocalization', 'AbpUi']);
  const [registerModal, { openModal }] = useModal();
  const [registerTable, { setLoading, setTableData, getForm }] = useTable({
    rowKey: 'cultureName',
    title: L('Languages'),
    columns: getDataColumns(),
    beforeFetch: formatPagedRequest,
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
      submitFunc: fetchLanguages,
    },
    actionColumn: {
      width: 150,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });
  onMounted(fetchLanguages);

  function fetchLanguages() {
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

  function handleEdit(record: Language) {
    getByName(record.cultureName).then((dto) => {
      openModal(true, dto);
    });
  }

  function handleDelete(record: Language) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessage'),
      onOk: () => {
        return deleteByName(record.cultureName).then(() => {
          createMessage.success(L('SuccessfullyDeleted'));
          fetchLanguages();
        });
      },
    });
  }
</script>
