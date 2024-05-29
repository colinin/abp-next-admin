<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button v-auth="['Platform.Menu.Create']" type="primary" @click="handleAddNew">{{
          L('Menu:AddNew')
        }}</a-button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'displayName'">
          <Icon v-if="record.meta.icon" style="margin-right: 10px" :icon="record.meta.icon" />
          <span>{{ record.displayName }}</span>
        </template>
        <template v-else-if="column.key === 'action'">
          <TableAction
            :actions="[
              {
                auth: 'Platform.Menu.Create',
                label: L('Menu:AddNew'),
                icon: 'ant-design:plus-outlined',
                onClick: handleAddNew.bind(null, record),
              },
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
  import { Icon } from '/@/components/Icon';
  import { useDrawer } from '/@/components/Drawer';
  import { getDataColumns } from './TableData';
  import { getSearchFormSchemas } from './ModalData';
  import { getAll, getById, deleteById } from '/@/api/platform/menus';
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
      width: 280,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });

  const [registerDrawer, { openDrawer }] = useDrawer();

  function handleAddNew(record?: Recordable) {
    openDrawer(true, {
      layoutId: record?.layoutId,
      parentId: record?.id,
    });
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
