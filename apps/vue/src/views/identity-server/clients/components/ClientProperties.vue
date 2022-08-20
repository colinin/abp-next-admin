<template>
  <DynamicForm
    :label-width="120"
    :schemas="schemas"
    :columns="columns"
    :data-source="modelRef.properties"
    :show-header="true"
    rowKey="value"
    @new="handleAddNew"
    @delete="handleDelete"
  />
</template>

<script lang="ts" setup>
  import { toRefs } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { FormSchema } from '/@/components/Form';
  import { BasicColumn } from '/@/components/Table';
  import { useProperty } from '../hooks/useProperty';
  import { Client } from '/@/api/identity-server/model/clientsModel';
  import DynamicForm from './DynamicForm.vue';

  const props = defineProps({
    modelRef: {
      type: Object as PropType<Client>,
      required: true,
    },
  });

  const { L } = useLocalization('AbpIdentityServer');
  const schemas: FormSchema[] = [
    {
      field: 'type',
      component: 'Input',
      label: L('Propertites:Key'),
      colProps: { span: 24 },
      required: true,
    },
    {
      field: 'value',
      component: 'Input',
      label: L('Propertites:Value'),
      colProps: { span: 24 },
      required: true,
    },
  ];
  const columns: BasicColumn[] = [
    {
      dataIndex: 'type',
      title: L('Propertites:Key'),
      align: 'left',
      width: '150',
      sorter: true,
    },
    {
      dataIndex: 'value',
      title: L('Propertites:Value'),
      align: 'left',
      width: 'auto',
      sorter: true,
    },
  ];
  const { handleNewProperty, handleDeleteProperty } = useProperty({
    modelRef: toRefs(props).modelRef,
  });

  function handleAddNew(record) {
    handleNewProperty(record);
  }

  function handleDelete(record) {
    handleDeleteProperty(record);
  }
</script>
