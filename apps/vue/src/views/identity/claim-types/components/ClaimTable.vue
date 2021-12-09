<template>
  <div class="content">
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button
          v-if="hasPermission('AbpIdentity.IdentityClaimTypes.Create')"
          type="primary"
          @click="handleAddNew"
          >{{ L('IdentityClaim:New') }}</a-button
        >
      </template>
      <template #types="{ record }">
        <span>{{ valueTypeMap[record.valueType] }}</span>
      </template>
      <template #required="{ record }">
        <Switch :checked="record.required" disabled />
      </template>
      <template #static="{ record }">
        <Switch :checked="record.isStatic" disabled />
      </template>
      <template #action="{ record }">
        <TableAction
          :stop-button-propagation="true"
          :actions="[
            {
              auth: 'AbpIdentity.IdentityClaimTypes.Update',
              label: L('Edit'),
              icon: 'ant-design:edit-outlined',
              ifShow: !record.isStatic,
              onClick: handleEdit.bind(null, record),
            },
            {
              auth: 'AbpIdentity.IdentityClaimTypes.Delete',
              color: 'error',
              label: L('Delete'),
              icon: 'ant-design:delete-outlined',
              ifShow: !record.isStatic,
              onClick: handleDelete.bind(null, record),
            },
          ]"
        />
      </template>
    </BasicTable>
    <ClaimModal @register="registerModal" @change="reloadTable" />
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import { Switch } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { useModal } from '/@/components/Modal';
  import { BasicTable, TableAction } from '/@/components/Table';
  import { useClaimTable } from '../hooks/useClaimTable';
  import ClaimModal from './ClaimModal.vue';

  export default defineComponent({
    name: 'ClaimTable',
    components: {
      BasicTable,
      ClaimModal,
      Switch,
      TableAction,
    },
    setup() {
      const { L } = useLocalization('AbpIdentity');
      const { hasPermission } = usePermission();
      const [registerModal, { openModal }] = useModal();
      const { valueTypeMap, registerTable, reloadTable, handleDelete } = useClaimTable();

      return {
        L,
        hasPermission,
        valueTypeMap,
        registerTable,
        reloadTable,
        registerModal,
        openModal,
        handleDelete,
      };
    },
    methods: {
      handleAddNew() {
        this.openModal(true, {}, true);
      },
      handleEdit(record) {
        this.openModal(true, record, true);
      },
    },
  });
</script>
