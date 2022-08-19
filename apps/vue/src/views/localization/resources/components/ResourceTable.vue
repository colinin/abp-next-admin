<template>
  <div class="content">
    <BasicTable @register="registerTable" />
  </div>
</template>

<script lang="ts">
  import { defineComponent, onMounted } from 'vue';
  import { Switch } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { BasicTable, useTable } from '/@/components/Table';
  import { getList } from '/@/api/localization/resources';
  import { getDataColumns } from './TableData';

  export default defineComponent({
    name: 'ResourceTable',
    components: {
      BasicTable,
      Switch,
    },
    setup() {
      const { L } = useLocalization(['LocalizationManagement', 'AbpUi']);
      const { hasPermission } = usePermission();
      const [registerTable, { setTableData, getForm }] = useTable({
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
      });
      onMounted(fetchResources);

      function fetchResources() {
        const form = getForm();
        return form.validate().then(() => {
          return getList().then((res) => {
            setTableData(res.items);
          });
        });
      }

      function handleChange() {
        fetchResources();
      }

      return {
        L,
        hasPermission,
        registerTable,
        handleChange,
      };
    },
  });
</script>
