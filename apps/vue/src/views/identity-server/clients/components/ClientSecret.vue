<template>
  <FormItem name="requireClientSecret" :label="L('Client:RequiredClientSecret')">
    <Checkbox :checked="modelRef.requireClientSecret" @change="handleRequiredChange">{{
      L('Client:RequiredClientSecret')
    }}</Checkbox>
  </FormItem>
  <DynamicForm
    :label-width="120"
    :schemas="schemas"
    :columns="columns"
    :data-source="modelRef.clientSecrets"
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
  import { formatToDateTime } from '/@/utils/dateUtil';
  import { Client } from '/@/api/identity-server/model/clientsModel';
  import DynamicForm from './DynamicForm.vue';
  import { useSecret } from '../hooks/useSecret';

  export default defineComponent({
    name: 'ClientSecret',
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
          component: 'Select',
          label: L('Secret:Type'),
          colProps: { span: 24 },
          required: true,
          componentProps: {
            options: [
              { label: 'JsonWebKey', value: 'JWK' },
              { label: 'SharedSecret', value: 'SharedSecret' },
              { label: 'X509CertificateName', value: 'X509Name' },
              { label: 'X509CertificateBase64', value: 'X509CertificateBase64' },
              { label: 'X509CertificateThumbprint', value: 'X509Thumbprint' },
            ],
          },
        },
        {
          field: 'value',
          component: 'Input',
          label: L('Secret:Value'),
          colProps: { span: 24 },
          required: true,
        },
        {
          field: 'description',
          component: 'InputTextArea',
          label: L('Description'),
          colProps: { span: 24 },
        },
        {
          field: 'expiration',
          component: 'DatePicker',
          label: L('Expiration'),
          colProps: { span: 24 },
          componentProps: {
            style: {
              width: '100%',
            },
          },
        },
      ];
      const columns: BasicColumn[] = [
        {
          dataIndex: 'type',
          title: L('Secret:Type'),
          align: 'left',
          width: '120',
          sorter: true,
        },
        {
          dataIndex: 'value',
          title: L('Secret:Value'),
          align: 'left',
          width: 'auto',
          sorter: true,
        },
        {
          dataIndex: 'description',
          title: L('Description'),
          align: 'left',
          width: '100',
          sorter: true,
        },
        {
          dataIndex: 'expiration',
          title: L('Expiration'),
          align: 'left',
          width: '100',
          sorter: true,
          format: (text) => {
            if (text) {
              return formatToDateTime(text);
            }
            return '';
          },
        },
      ];
      const { handleSecretChange, handleRequiredChange } = useSecret({
        modelRef: toRefs(props).modelRef,
      });

      function handleAddNew(record) {
        handleSecretChange('add', record);
      }

      function handleDelete(record) {
        handleSecretChange('delete', record);
      }

      return {
        L,
        schemas,
        columns,
        handleAddNew,
        handleDelete,
        handleRequiredChange,
      };
    },
  });
</script>
