<template>
  <div>
    <BasicTable ref="tableElRef" @register="registerTable">
      <template #toolbar>
        <a-button
          v-if="hasPermission('AbpSaas.Tenants.Create')"
          type="primary"
          @click="handleAddNew"
          >{{ L('NewTenant') }}</a-button
        >
        <a-button
          v-if="hasPermission('FeatureManagement.ManageHostFeatures')"
          type="primary"
          @click="handleManageHostFeature"
          >{{ L('ManageHostFeatures') }}</a-button
        >
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'isActive'">
          <CheckOutlined v-if="record.isActive" class="enable" />
          <CloseOutlined v-else class="disable" />
        </template>
        <template v-else-if="column.key === 'action'">
          <TableAction
            :actions="[
              {
                auth: 'AbpSaas.Tenants.Update',
                label: L('Edit'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEdit.bind(null, record),
              },
              {
                auth: 'AbpSaas.Tenants.Delete',
                color: 'error',
                label: L('Delete'),
                icon: 'ant-design:delete-outlined',
                onClick: handleDelete.bind(null, record),
              },
            ]"
            :dropDownActions="[
              {
                auth: 'AbpSaas.Tenants.ManageFeatures',
                label: L('ManageFeatures'),
                onClick: handleManageTenantFeature.bind(null, record),
              },
              {
                auth: 'AbpSaas.Tenants.ManageConnectionStrings',
                label: L('ConnectionStrings'),
                onClick: openConnectModal.bind(null, true, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <FeatureModal @register="registerFeatureModal" />
    <TenantModal @register="registerModal" @change="handleReload" />
    <ConnectionTableModal @register="registerConnectModal" />
  </div>
</template>

<script lang="ts" setup>
  import { ref } from 'vue';
  import { CheckOutlined, CloseOutlined } from '@ant-design/icons-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { useModal } from '/@/components/Modal';
  import { BasicTable, TableAction, TableActionType } from '/@/components/Table';
  import { useTenantTable } from '../hooks/useTenantTable';
  import { useTenantModal } from '../hooks/useTenantModal';
  import { useFeatureModal } from '../hooks/useFeatureModal';
  import { FeatureModal } from '../../../feature';
  import TenantModal from './TenantModal.vue';
  import ConnectionTableModal from './ConnectionTableModal.vue';

  const { L } = useLocalization(['AbpSaas', 'AbpFeatureManagement']);
  const { hasPermission } = usePermission();
  const tableElRef = ref<Nullable<TableActionType>>(null);
  const [registerConnectModal, { openModal: openConnectModal }] = useModal();
  const { registerModal, handleAddNew, handleEdit } = useTenantModal();
  const { registerTable, handleDelete, handleReload } = useTenantTable({ tableElRef });
  const {
    registerModal: registerFeatureModal,
    handleManageHostFeature,
    handleManageTenantFeature,
  } = useFeatureModal();
</script>
