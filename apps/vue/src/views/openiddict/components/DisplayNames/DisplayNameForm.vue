<template>
  <Card :title="L('DisplayName:DisplayNames')">
    <BasicForm @register="registerForm" @submit="handleSubmit" />
    <BasicTable @register="registerTable" :data-source="dataSource">
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'action'">
          <TableAction
            :actions="[
              {
                color: 'error',
                icon: 'ant-design:delete-outlined',
                label: L('Delete'),
                onClick: handleDelete.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
  </Card>
</template>

<script lang="ts" setup>
  import { computed, ref } from 'vue';
  import { Card } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getList as getLanguages } from '/@/api/localization/languages';

  const emits = defineEmits(['create', 'delete']);
  const props = defineProps({
    displayNames: {
      type: Object as PropType<Recordable>,
      default: () => {},
    },
  });

  const { L } = useLocalization(['AbpOpenIddict', 'AbpLocalization', 'AbpUi']);
  const modelRef = ref({});
  const [registerForm, { resetFields }] = useForm({
    model: modelRef,
    schemas: [
      {
        field: 'culture',
        component: 'ApiSelect',
        label: L('DisplayName:CultureName'),
        colProps: { span: 24 },
        required: true,
        componentProps: {
          api: getLanguages,
          params: {
            skipCount: 0,
            maxResultCount: 100,
          },
          resultField: 'items',
          labelField: 'displayName',
          valueField: 'cultureName',
        },
      },
      {
        field: 'displayName',
        component: 'Input',
        label: L('DisplayName:DisplayName'),
        colProps: { span: 24 },
        required: true,
      },
    ],
    showResetButton: false,
    submitButtonOptions: {
      text: L('DisplayName:AddNew'),
      // icon: 'ant-design:plus-outlined',
    },
  });
  const [registerTable] = useTable({
    rowKey: 'culture',
    showHeader: false,
    columns: [
      {
        dataIndex: 'culture',
        align: 'left',
        width: 100,
        sorter: true,
      },
      {
        dataIndex: 'displayName',
        align: 'left',
        width: 'auto',
        sorter: true,
      },
    ],
    pagination: false,
    striped: false,
    useSearchForm: false,
    showTableSetting: false,
    showIndexColumn: false,
    bordered: false,
    actionColumn: {
      width: 200,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });
  const dataSource = computed(() => {
    if (!props.displayNames) {
      return [];
    }
    return Object.keys(props.displayNames).map((key) => {
      return {
        culture: key,
        displayName: props.displayNames[key],
      };
    });
  });

  function handleSubmit(input) {
    emits('create', input);
    resetFields();
  }

  function handleDelete(record) {
    emits('delete', record);
  }
</script>

<style lang="less" scoped>
  .title {
    margin-bottom: 20px;
  }
</style>
