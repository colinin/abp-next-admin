<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    :title="L('ManageClaim')"
    :width="800"
    :showCancelBtn="false"
    :showOkBtn="false"
  >
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button type="primary" @click="handleAddNew">{{ L('AddClaim') }}</a-button>
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              auth: 'AbpIdentity.Users.ManageClaims',
              label: L('Edit'),
              icon: 'ant-design:edit-outlined',
              onClick: handleEdit.bind(null, record),
            },
            {
              auth: 'AbpIdentity.Users.ManageClaims',
              color: 'error',
              label: L('Delete'),
              icon: 'ant-design:delete-outlined',
              onClick: handleDelete.bind(null, record),
            },
          ]"
        />
      </template>
    </BasicTable>
    <BasicModalForm
      @register="registerClaimForm"
      :save-changes="handleSaveChanges"
      :form-items="formSchemas"
      :title="L('ManageClaim')"
    />
  </BasicModal>
</template>

<script lang="ts">
  import { defineComponent, ref } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { BasicTable, TableAction } from '/@/components/Table';
  import { BasicModalForm } from '/@/components/ModalForm';
  import { useClaim } from '../hooks/useClaim';

  export default defineComponent({
    name: 'RoleClaimModal',
    components: { BasicModal, BasicTable, BasicModalForm, TableAction },
    setup() {
      const { L } = useLocalization('AbpIdentity');
      const roleIdRef = ref('');
      const [registerModal] = useModalInner((val) => {
        roleIdRef.value = val.id;
      });
      const {
        formSchemas,
        registerClaimForm,
        openClaimForm,
        registerTable,
        handleDelete,
        handleSaveChanges,
      } = useClaim({ roleIdRef });

      return {
        L,
        formSchemas,
        registerModal,
        registerTable,
        registerClaimForm,
        openClaimForm,
        handleDelete,
        handleSaveChanges,
      };
    },
    methods: {
      handleAddNew() {
        this.openClaimForm({});
      },
      handleEdit(record) {
        this.openClaimForm(record);
      },
    },
  });
</script>
