<template>
  <FormItem name="alwaysSendClientClaims" :label="L('Client:AlwaysSendClientClaims')">
    <Checkbox :checked="modelRef.alwaysSendClientClaims" @change="handleCheckedChange">{{
      L('Client:AlwaysSendClientClaims')
    }}</Checkbox>
  </FormItem>
  <FormItem
    name="alwaysIncludeUserClaimsInIdToken"
    :label="L('Client:AlwaysIncludeUserClaimsInIdToken')"
    :label-col="{ span: 8 }"
    :wrapper-col="{ span: 16 }"
  >
    <Checkbox :checked="modelRef.alwaysIncludeUserClaimsInIdToken" @change="handleCheckedChange">{{
      L('Client:AlwaysIncludeUserClaimsInIdToken')
    }}</Checkbox>
  </FormItem>
  <DynamicForm
    :label-width="120"
    :schemas="schemas"
    :columns="columns"
    :data-source="modelRef.claims"
    :show-header="true"
    rowKey="value"
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
  import { useClaim } from '../hooks/useClaim';
  import { Client } from '/@/api/identity-server/model/clientsModel';
  import { getActivedList } from '/@/api/identity/claim';

  export default defineComponent({
    name: 'ClientClaim',
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
          field: 'type',
          component: 'ApiSelect',
          label: L('Claims:Type'),
          colProps: { span: 24 },
          required: true,
          componentProps: {
            api: () => getActivedList(),
            resultField: 'items',
            labelField: 'name',
            valueField: 'name',
          },
        },
        {
          field: 'value',
          component: 'Input',
          label: L('Claims:Value'),
          colProps: { span: 24 },
          required: true,
          // TODO: 选择的type ValueType变化时需要调整输入控件类型
        },
      ];
      const columns: BasicColumn[] = [
        {
          dataIndex: 'type',
          title: L('Claims:Type'),
          align: 'left',
          width: '150',
          sorter: true,
        },
        {
          dataIndex: 'value',
          title: L('Claims:Value'),
          align: 'left',
          width: 'auto',
          sorter: true,
        },
      ];
      const { handleClaimChange, handleCheckedChange } = useClaim({
        modelRef: toRefs(props).modelRef,
      });

      function handleAddNew(record) {
        handleClaimChange('add', record);
      }

      function handleDelete(record) {
        handleClaimChange('delete', record);
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
