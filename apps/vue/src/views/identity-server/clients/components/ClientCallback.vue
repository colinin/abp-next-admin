<template>
  <DynamicForm
    :title="L('Client:CallbackUrl')"
    :schemas="schemas"
    :columns="columns"
    :data-source="modelRef.redirectUris"
    rowKey="redirectUri"
    @new="handleAddNew"
    @delete="handleDelete"
  />
</template>

<script lang="ts" setup>
  import { toRefs } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { FormSchema } from '/@/components/Form';
  import { BasicColumn } from '/@/components/Table';
  import { Client } from '/@/api/identity-server/model/clientsModel';
  import { useUrl } from '../hooks/useUrl';
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
      field: 'redirectUri',
      component: 'Input',
      label: 'Url',
      colProps: { span: 24 },
      required: true,
    },
  ];
  const columns: BasicColumn[] = [
    {
      dataIndex: 'redirectUri',
      align: 'left',
      width: 'auto',
      sorter: true,
    },
  ];
  const { handleRedirectUriChange } = useUrl({ modelRef: toRefs(props).modelRef });

  function handleAddNew(record) {
    handleRedirectUriChange('add', record.redirectUri);
  }

  function handleDelete(record) {
    handleRedirectUriChange('delete', record.redirectUri);
  }
</script>
