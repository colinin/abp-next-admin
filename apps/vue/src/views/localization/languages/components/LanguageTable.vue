<template>
  <div class="content">
    <BasicTable @register="registerTable" />
  </div>
</template>

<script lang="ts" setup>
  import { onMounted } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicTable, useTable } from '/@/components/Table';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import { getList } from '/@/api/localization/languages';
  import { getDataColumns } from './TableData';

  const { L } = useLocalization(['AbpLocalization', 'AbpUi']);
  const [registerTable, { setTableData, getForm }] = useTable({
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
  });
  onMounted(fetchLanguages);

  function fetchLanguages() {
    const form = getForm();
    return form.validate().then(() => {
      return getList().then((res) => {
        setTableData(res.items);
      });
    });
  }
</script>
