<template>
  <FormItem name="enableLocalLogin" :label="L('Client:EnableLocalLogin')">
    <Checkbox :checked="modelRef.enableLocalLogin" @change="handleCheckedChange">{{
      L('Client:EnableLocalLogin')
    }}</Checkbox>
  </FormItem>
  <DynamicForm
    :label-width="150"
    :schemas="schemas"
    :columns="columns"
    :data-source="modelRef.identityProviderRestrictions"
    :show-header="true"
    rowKey="provider"
    @new="handleAddNew"
    @delete="handleDelete"
  />
</template>

<script lang="ts">
  import { defineComponent, toRefs } from 'vue';
  import { Checkbox, Form } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { FormSchema } from '/@/components/Form';
  import { BasicColumn } from '/@/components/Table';
  import DynamicForm from './DynamicForm.vue';
  import { useIdentityProvider } from '../hooks/useIdentityProvider';
  import { Client } from '/@/api/identity-server/model/clientsModel';

  export default defineComponent({
    name: 'ClientIdentityProvider',
    components: { Checkbox, DynamicForm, FormItem: Form.Item },
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
          field: 'provider',
          component: 'Input',
          label: L('Client:IdentityProviderRestrictions'),
          colProps: { span: 24 },
          required: true,
        },
      ];
      const columns: BasicColumn[] = [
        {
          dataIndex: 'provider',
          title: L('Client:IdentityProviderRestrictions'),
          align: 'left',
          width: 'auto',
          sorter: true,
        },
      ];
      const { handleIdpChange, handleCheckedChange } = useIdentityProvider({
        modelRef: toRefs(props).modelRef,
      });

      function handleAddNew(record) {
        console.log(record);
        handleIdpChange('add', record);
      }

      function handleDelete(record) {
        console.log(record);
        handleIdpChange('delete', record);
      }

      return {
        L,
        schemas,
        columns,
        handleAddNew,
        handleDelete,
        handleCheckedChange,
      };
    },
  });
</script>
