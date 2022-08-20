<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    :loading="loading"
    :showOkBtn="!loading"
    :showCancelBtn="!loading"
    :title="L('OrganizationUnit:SelectRoles')"
    :width="800"
    @visible-change="handleVisibleChange"
    @ok="handleSubmit"
  >
    <BasicTable ref="tableRef" @register="registerTable">
      <template #toolbar>
        <InputSearch :placeholder="L('Search')" v-model:value="filter" @search="handleSearch" />
      </template>
    </BasicTable>
  </BasicModal>
</template>

<script lang="ts" setup>
  import { computed, ref, unref } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { Input } from 'ant-design-vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { BasicTable, BasicColumn, useTable, TableActionType } from '/@/components/Table';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import { addRoles, getUnaddedRoleList } from '/@/api/identity/organization-units';

  const InputSearch = Input.Search;

  const emits = defineEmits(['change', 'register']);
  const props = defineProps({
    ouId: { type: String },
  });

  const { createMessage } = useMessage();
  const { L } = useLocalization('AbpIdentity');
  const loading = ref(false);
  const filter = ref('');
  const tableRef = ref<Nullable<TableActionType>>(null);
  const [registerModal, { closeModal }] = useModalInner();
  const dataColumns: BasicColumn[] = [
    {
      title: 'id',
      dataIndex: 'id',
      width: 1,
      ifShow: false,
    },
    {
      title: L('RoleName'),
      dataIndex: 'name',
      align: 'left',
      width: 'auto',
      sorter: true,
    },
  ];
  const requestApi = computed(() => {
    return (request) => {
      request.id = unref(props).ouId;
      request.filter = unref(filter);
      formatPagedRequest(request);
    };
  });
  const [registerTable] = useTable({
    rowKey: 'id',
    columns: dataColumns,
    api: getUnaddedRoleList,
    beforeFetch: requestApi,
    pagination: true,
    striped: false,
    useSearchForm: false,
    showTableSetting: false,
    bordered: true,
    showIndexColumn: false,
    canResize: false,
    immediate: true,
    rowSelection: { type: 'checkbox' },
  });

  function handleSearch() {
    const tableEl = unref(tableRef);
    tableEl?.reload();
    tableEl?.clearSelectedRowKeys();
  }

  function handleVisibleChange(visible) {
    if (visible) {
      handleSearch();
    }
  }

  function handleSubmit() {
    const tableEl = unref(tableRef);
    const selectRows = tableEl?.getSelectRows();
    if (selectRows) {
      loading.value = true;
      addRoles(
        props.ouId!,
        selectRows.map((x) => x.id),
      )
        .then(() => {
          createMessage.success(L('Successful'));
          emits('change');
          closeModal();
        })
        .finally(() => {
          loading.value = false;
        });
    }
  }
</script>
