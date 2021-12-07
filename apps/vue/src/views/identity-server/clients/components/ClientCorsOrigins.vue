<template>
  <DynamicForm
    :title="L('Client:AllowedCorsOrigins')"
    :schemas="schemas"
    :columns="columns"
    :data-source="modelRef.allowedCorsOrigins"
    rowKey="origin"
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
    name: 'ClientCorsOrigins',
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
          field: 'origin',
          component: 'Input',
          label: 'Url',
          colProps: { span: 24 },
          required: true,
        },
      ];
      const columns: BasicColumn[] = [
        {
          dataIndex: 'origin',
          align: 'left',
          width: 'auto',
          sorter: true,
        },
      ];
      const { handleCorsOriginsChange } = useUrl({ modelRef: toRefs(props).modelRef });

      function handleAddNew(record) {
        handleCorsOriginsChange('add', record.origin);
      }

      function handleDelete(record) {
        handleCorsOriginsChange('delete', record.origin);
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
