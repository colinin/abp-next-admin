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
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'action'">
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
      </template>
    </BasicTable>
    <ClaimEditModal
      :identity="identityRef"
      :create-api="createApi"
      :update-api="updateApi"
      @register="registerEditModal"
      @change="reload"
    />
  </BasicModal>
</template>

<script lang="ts" setup>
  import { ref, nextTick } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicModal, useModal, useModalInner } from '/@/components/Modal';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { IdentityClaim } from '/@/api/identity/model/claimModel';
  import ClaimEditModal from './ClaimEditModal.vue';
  import { ListResultDto } from '/@/api/model/baseModel';
  import { isFunction } from '/@/utils/is';

  const { createConfirm, createMessage } = useMessage();
  const { L } = useLocalization('AbpIdentity');
  const props = defineProps({
    fetchApi: {
      type: Function as PropType<(...args: any) => Promise<ListResultDto<IdentityClaim>>>,
      required: true,
    },
    createApi: {
      type: Function as PropType<(...args: any) => Promise<void>>,
      required: true,
    },
    updateApi: {
      type: Function as PropType<(...args: any) => Promise<void>>,
      required: true,
    },
    deleteApi: {
      type: Function as PropType<(...args: any) => Promise<void>>,
      required: true,
    },
  });
  const identityRef = ref('');
  const [registerTable, { reload }] = useTable({
    rowKey: 'id',
    title: L('ManageClaim'),
    columns: [
      {
        title: 'id',
        dataIndex: 'id',
        width: 1,
        ifShow: false,
      },
      {
        title: L('DisplayName:ClaimType'),
        dataIndex: 'claimType',
        align: 'left',
        width: 150,
        sorter: (last, next) => {
          return last.claimType.localeCompare(next.claimType);
        },
      },
      {
        title: L('DisplayName:ClaimValue'),
        dataIndex: 'claimValue',
        align: 'left',
        width: 'auto',
        sorter: (last, next) => {
          return last.claimValue.localeCompare(next.claimValue);
        },
      }
    ],
    api: fetchClaims,
    pagination: false,
    striped: false,
    useSearchForm: false,
    showTableSetting: true,
    bordered: true,
    showIndexColumn: false,
    canResize: false,
    immediate: false,
    actionColumn: {
      width: 160,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });
  const [registerModal] = useModalInner((data) => {
    identityRef.value = data.id;
    nextTick(reload);
  });
  const [registerEditModal, { openModal: openEditModal }] = useModal();

  function fetchClaims() {
    if (isFunction(props.fetchApi)) {
      const params = {
        id: identityRef.value,
      };
      return props.fetchApi(params);
    }
    return new Promise((resolve) => resolve({ items: []}))
  }

  function handleAddNew() {
    openEditModal(true, {});
  }

  function handleEdit(claim) {
    openEditModal(true, claim);
  }

  function handleDelete(claim) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessageWithFormat', claim.claimType),
      onOk: () => {
        if (isFunction(props.deleteApi)) {
          props.deleteApi(identityRef.value, claim).then(() => {
            createMessage.success(L('Successful'));
            reload();
          });
        }
      },
    });
  }
</script>
