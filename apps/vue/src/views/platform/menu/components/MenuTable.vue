<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button type="primary" @click="handleAddNew">{{ L('Menu:AddNew') }}</a-button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'action'">
          <TableAction
            :actions="[
              {
                auth: 'Platform.Menu.Update',
                label: L('Edit'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEdit.bind(null, record),
              },
              {
                auth: 'Platform.Menu.Delete',
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
    <MenuDrawer @register="registerDrawer" @change="handleChange" :framework="useFramework" />
  </div>
</template>

<script lang="ts" setup>
  import { ref } from 'vue';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import { useDrawer } from '/@/components/Drawer';
  import { getDataColumns } from './TableData';
  import { getSearchFormSchemas } from './ModalData';
  import { getAll, getById, deleteById } from '/@/api/platform/menu';
  import { listToTree } from '/@/utils/helper/treeHelper';
  import MenuDrawer from './MenuDrawer.vue';

  const { createMessage, createConfirm } = useMessage();
  const { L } = useLocalization(['AppPlatform', 'AbpUi']);
  const useFramework = ref('');
  const [registerTable, { reload }] = useTable({
    rowKey: 'id',
    title: L('DisplayName:Menus'),
    columns: getDataColumns(),
    api: getAll,
    beforeFetch: (request) => {
      // 子组件需要此参数,拦截一下
      useFramework.value = request.framework;
      return request;
    },
    afterFetch: (result) => {
      return listToTree(result, {
        id: 'id',
        pid: 'parentId',
      });
    },
    pagination: false,
    striped: false,
    useSearchForm: true,
    showTableSetting: true,
    bordered: true,
    showIndexColumn: false,
    canResize: false,
    formConfig: getSearchFormSchemas(),
    actionColumn: {
      width: 160,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });

  const [registerDrawer, { openDrawer }] = useDrawer();
  
  function handleAddNew() {
    openDrawer(true, {});
  }

  function handleEdit(record: Recordable) {
    getById(record.id).then((menu) => {
      openDrawer(true, menu);
    });
  }

  function handleDelete(record: Recordable) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessageWithFormat', [record.displayName]),
      okCancel: true,
      onOk: () => {
        return deleteById(record.id).then(() => {
          createMessage.success(L('SuccessfullyDeleted'));
          reload();
        });
      },
    });
  }
    
  function handleChange() {
    reload();
  }
</script>
