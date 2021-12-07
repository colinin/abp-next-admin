<template>
  <div>
    <BasicTable
      rowKey="type"
      :columns="columns"
      :dataSource="secrets"
      :pagination="false"
      :showTableSetting="true"
      :maxHeight="230"
      :actionColumn="{
        width: 100,
        title: L('Actions'),
        dataIndex: 'action',
        slots: { customRender: 'action' },
      }"
    >
      <template #toolbar>
        <Button type="primary" @click="handleAddNew">{{ L('Secret:New') }}</Button>
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              auth: 'AbpIdentityServer.ApiResources.Delete',
              color: 'error',
              icon: 'ant-design:delete-outlined',
              label: L('Resource:Delete'),
              onClick: handleDelete.bind(null, record),
            },
          ]"
        />
      </template>
    </BasicTable>
    <BasicModal v-bind="$attrs" @register="registerModal" @ok="handleSubmit" :title="title">
      <BasicForm @register="registerForm" />
    </BasicModal>
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { Button } from 'ant-design-vue';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModal } from '/@/components/Modal';
  import { BasicTable, TableAction } from '/@/components/Table';
  import { ApiResourceSecret } from '/@/api/identity-server/model/apiResourcesModel';
  import { getSecretColumns } from '../datas/TableData';
  import { getSecretFormSchemas } from '../datas/ModalData';

  export default defineComponent({
    name: 'ApiResourceSecret',
    components: {
      BasicForm,
      BasicModal,
      BasicTable,
      Button,
      TableAction,
    },
    props: {
      secrets: {
        type: [Array] as PropType<ApiResourceSecret[]>,
        required: true,
        default: () => [],
      },
    },
    emits: ['register', 'secrets-new', 'secrets-delete'],
    setup(_, { emit }) {
      const { L } = useLocalization('AbpIdentityServer');
      const title = ref('');
      const [registerForm, { validate, resetFields }] = useForm({
        labelWidth: 120,
        showActionButtonGroup: false,
        schemas: getSecretFormSchemas(),
      });
      const [registerModal, { openModal, closeModal }] = useModal();

      function handleAddNew() {
        title.value = L('Secret:New');
        openModal(true);
      }

      function handleDelete(record) {
        emit('secrets-delete', record);
      }

      function handleSubmit() {
        validate().then((input) => {
          emit('secrets-new', input);
          resetFields();
          closeModal();
        });
      }

      return {
        L,
        title,
        handleAddNew,
        handleDelete,
        handleSubmit,
        columns: getSecretColumns(),
        registerForm,
        registerModal,
      };
    },
  });
</script>
