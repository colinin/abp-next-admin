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
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'valueType'">
          <span>{{ valueTypeMap[record.valueType] }}</span>
        </template>
        <template v-else-if="column.key === 'required'">
          <CheckOutlined v-if="record.required" class="enable" />
          <CloseOutlined v-else class="disable" />
        </template>
        <template v-else-if="column.key === 'isStatic'">
          <CheckOutlined v-if="record.isStatic" class="enable" />
          <CloseOutlined v-else class="disable" />
        </template>
        <template v-else-if="column.key === 'action'">
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
      </template>
    </BasicTable>
    <ClaimModal @register="registerModal" @change="reloadTable" />
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import { CheckOutlined, CloseOutlined } from '@ant-design/icons-vue';
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
      CheckOutlined,
      CloseOutlined,
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
