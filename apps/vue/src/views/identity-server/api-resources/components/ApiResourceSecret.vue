<template>
  <div>
    <BasicTable
      rowKey="type"
      :columns="getSecretColumns()"
      :dataSource="secrets"
      :pagination="false"
      :showTableSetting="true"
      :maxHeight="230"
      :actionColumn="{
        width: 100,
        title: L('Actions'),
        dataIndex: 'action',
      }"
    >
      <template #toolbar>
        <Button type="primary" @click="handleAddNew">{{ L('Secret:New') }}</Button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'action'">
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
      </template>
    </BasicTable>
    <BasicModal v-bind="$attrs" @register="registerModal" @ok="handleSubmit" :title="title">
      <BasicForm @register="registerForm" />
    </BasicModal>
  </div>
</template>

<script lang="ts" setup>
  import { ref } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { Button } from 'ant-design-vue';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModal } from '/@/components/Modal';
  import { BasicTable, TableAction } from '/@/components/Table';
  import { ApiResourceSecret } from '/@/api/identity-server/model/apiResourcesModel';
  import { getSecretColumns } from '../datas/TableData';
  import { getSecretFormSchemas } from '../datas/ModalData';

  const emits = defineEmits(['register', 'secrets-new', 'secrets-delete']);
  defineProps({
    secrets: {
      type: [Array] as PropType<ApiResourceSecret[]>,
      required: true,
      default: () => [],
    },
  });

  const { createMessage } = useMessage();
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
    emits('secrets-delete', record);
  }

  function handleSubmit() {
    validate().then((input) => {
      createMessage.success(L('Successful'));
      emits('secrets-new', input);
      resetFields();
      closeModal();
    });
  }
</script>
