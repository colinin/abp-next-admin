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

<script lang="ts">
  import { defineComponent, toRefs } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { FormSchema } from '/@/components/Form';
  import { BasicColumn } from '/@/components/Table';
  import { Client } from '/@/api/identity-server/model/clientsModel';
  import DynamicForm from './DynamicForm.vue';
  import { useUrl } from '../hooks/useUrl';

  export default defineComponent({
    name: 'ClientLogoutRedirectUris',
    components: { DynamicForm },
    props: {
      modelRef: {
        type: Object as PropType<Client>,
        required: true,
      },
    },
    setup(props) {
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

      return {
        L,
        schemas,
        columns,
        handleAddNew,
        handleDelete,
      };
    },
  });
</script>
