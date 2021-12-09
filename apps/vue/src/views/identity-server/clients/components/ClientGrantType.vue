<template>
  <DynamicForm
    :title="L('Client:AllowedGrantTypes')"
    :schemas="schemas"
    :columns="columns"
    :data-source="modelRef.allowedGrantTypes"
    rowKey="grantType"
    @new="handleAddNew"
    @delete="handleDelete"
  />
</template>

<script lang="ts">
  import { defineComponent, toRefs } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { FormSchema } from '/@/components/Form';
  import { BasicColumn } from '/@/components/Table';
  import DynamicForm from './DynamicForm.vue';
  import { useGrantType } from '../hooks/useGrantType';
  import { Client } from '/@/api/identity-server/model/clientsModel';

  export default defineComponent({
    name: 'ClientGrantType',
    components: { DynamicForm },
    props: {
      modelRef: {
        type: Object as PropType<Client>,
        required: true,
      },
    },
    setup(props) {
      const { L } = useLocalization('AbpIdentityServer');
      const { grantTypeOptions, handleGrantTypeChanged } = useGrantType({
        modelRef: toRefs(props).modelRef,
      });
      const schemas: FormSchema[] = [
        {
          field: 'grantType',
          component: 'Select',
          label: L('Client:AllowedGrantTypes'),
          colProps: { span: 24 },
          required: true,
          componentProps: {
            options: grantTypeOptions,
          },
        },
      ];
      const columns: BasicColumn[] = [
        {
          dataIndex: 'grantType',
          align: 'left',
          width: 'auto',
          sorter: true,
        },
      ];

      function handleAddNew(record) {
        handleGrantTypeChanged('add', record);
      }

      function handleDelete(record) {
        handleGrantTypeChanged('delete', record);
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
