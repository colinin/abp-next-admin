<template>
  <div>
    <BasicTable
      rowKey="key"
      :columns="columns"
      :dataSource="properties"
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
        <Button type="primary" @click="handleAddNew">{{ L('Propertites:New') }}</Button>
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              auth: 'AbpIdentityServer.ApiResources.ManageProperties',
              color: 'error',
              icon: 'ant-design:delete-outlined',
              label: L('Propertites:Delete'),
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
  import { Button } from 'ant-design-vue';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModal } from '/@/components/Modal';
  import { BasicTable, TableAction } from '/@/components/Table';
  import { Property } from '/@/api/identity-server/model/basicModel';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { getDataColumns } from './TableData';
  import { getFormSchemas } from './ModalData';

  export default defineComponent({
    name: 'Properties',
    components: {
      BasicForm,
      BasicModal,
      BasicTable,
      Button,
      TableAction,
    },
    props: {
      properties: {
        type: [Array] as PropType<Property[]>,
        required: true,
        default: () => [],
      },
    },
    emits: ['new', 'props-new', 'props-delete'],
    setup(_, { emit }) {
      const { L } = useLocalization('AbpIdentityServer');
      const title = ref('');
      const [registerForm, { validate, resetFields }] = useForm({
        labelWidth: 120,
        showActionButtonGroup: false,
        schemas: getFormSchemas(),
      });
      const [registerModal, { openModal, closeModal }] = useModal();

      function handleAddNew() {
        title.value = L('Propertites:New');
        openModal(true);
      }

      function handleDelete(record) {
        emit('props-delete', record);
      }

      function handleSubmit() {
        validate().then((input) => {
          emit('props-new', input);
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
        columns: getDataColumns(),
        registerForm,
        registerModal,
      };
    },
  });
</script>
