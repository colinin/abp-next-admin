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
    <ApiResourceSecretModal @register="registerModal" @change="handleChange" />
  </div>
</template>

<script lang="ts" setup>
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { Button } from 'ant-design-vue';
  import { useModal } from '/@/components/Modal';
  import { BasicTable, TableAction } from '/@/components/Table';
  import { ApiResourceSecret } from '/@/api/identity-server/model/apiResourcesModel';
  import { getSecretColumns } from '../datas/TableData';
  import ApiResourceSecretModal from './ApiResourceSecretModal.vue';

  const emits = defineEmits(['register', 'secrets-new', 'secrets-delete']);
  defineProps({
    secrets: {
      type: [Array] as PropType<ApiResourceSecret[]>,
      required: true,
      default: () => [],
    },
  });

  const { L } = useLocalization('AbpIdentityServer');
  const [registerModal, { openModal }] = useModal();

  function handleAddNew() {
    openModal(true, {});
  }

  function handleDelete(record) {
    emits('secrets-delete', record);
  }

  function handleChange(input) {
    emits('secrets-new', input);
  }
</script>
