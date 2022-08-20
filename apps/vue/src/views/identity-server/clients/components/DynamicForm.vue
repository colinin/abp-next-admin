<template>
  <div>
    <BasicTitle class="title">{{ title }}</BasicTitle>
    <BasicForm @register="registerForm" @submit="handleSubmit" />
    <BasicTable @register="registerTable">
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
  </div>
</template>

<script lang="ts" setup>
  import { ref, watch } from 'vue';
  import { BasicTitle } from '/@/components/Basic';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicForm, FormSchema, useForm } from '/@/components/Form';
  import { BasicTable, BasicColumn, TableAction, useTable } from '/@/components/Table';

  const emits = defineEmits(['new', 'delete']);
  const props = defineProps({
    schemas: {
      type: [Array] as PropType<FormSchema[]>,
      required: true,
      default: () => [],
    },
    labelWidth: {
      type: Number,
    },
    columns: {
      type: [Array] as PropType<BasicColumn[]>,
      required: true,
      default: () => [],
    },
    dataSource: {
      type: [Array] as PropType<Recordable[]>,
      required: true,
      default: () => [],
    },
    rowKey: {
      type: String,
      default: () => 'id',
    },
    showHeader: {
      type: Boolean,
      default: () => false,
    },
    title: {
      type: String,
      default: () => '',
    },
    tableTitle: {
      type: String,
      default: () => '',
    },
  });

  const { L } = useLocalization('AbpIdentityServer');
  const modelRef = ref({});
  const [registerForm, { resetFields }] = useForm({
    model: modelRef,
    labelWidth: props.labelWidth,
    schemas: props.schemas,
    showResetButton: false,
    submitButtonOptions: {
      text: L('AddNew'),
      // icon: 'ant-design:plus-outlined',
    },
  });
  const [registerTable, { setTableData }] = useTable({
    rowKey: props.rowKey,
    showHeader: props.showHeader,
    title: props.tableTitle,
    columns: props.columns,
    dataSource: props.dataSource,
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

  watch(
    () => props.dataSource,
    (data) => {
      setTableData(data);
    },
    {
      deep: true,
    },
  );

  function handleSubmit(input) {
    emits('new', input);
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
