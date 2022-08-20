<template>
  <DynamicForm
    :title="L('Client:PostLogoutRedirectUris')"
    :schemas="schemas"
    :columns="columns"
    :data-source="modelRef.postLogoutRedirectUris"
    rowKey="postLogoutRedirectUri"
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
      field: 'postLogoutRedirectUri',
      component: 'Input',
      label: 'Url',
      colProps: { span: 24 },
      required: true,
    },
  ];
  const columns: BasicColumn[] = [
    {
      dataIndex: 'postLogoutRedirectUri',
      align: 'left',
      width: 'auto',
      sorter: true,
    },
  ];
  const { handleLogoutRedirectUris } = useUrl({ modelRef: toRefs(props).modelRef });

  function handleAddNew(record) {
    handleLogoutRedirectUris('add', record.postLogoutRedirectUri);
  }

  function handleDelete(record) {
    handleLogoutRedirectUris('delete', record.postLogoutRedirectUri);
  }
</script>
