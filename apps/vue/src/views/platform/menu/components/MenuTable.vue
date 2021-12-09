<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button type="primary" @click="handleAddNew">{{ L('Menu:AddNew') }}</a-button>
      </template>
      <template #action="{ record }">
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
    </BasicTable>
    <MenuDrawer @register="registerDrawer" @change="handleChange" :framework="useFramework" />
  </div>
</template>

<script lang="ts">
  import { defineComponent, ref } from 'vue';

  import { Modal } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import { useDrawer } from '/@/components/Drawer';
  import MenuDrawer from './MenuDrawer.vue';
  import { getDataColumns } from './TableData';
  import { getSearchFormSchemas } from './ModalData';
  import { getAll, getById, deleteById } from '/@/api/platform/menu';
  import { listToTree } from '/@/utils/helper/treeHelper';
  export default defineComponent({
    name: 'MenuTable',
    components: {
      BasicTable,
      TableAction,
      MenuDrawer,
    },
    setup() {
      const { L } = useLocalization('AppPlatform', 'AbpUi');
      const useFramework = ref('');
      const [registerTable, { reload: reloadTable }] = useTable({
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
          slots: { customRender: 'action' },
        },
      });

      const [registerDrawer, { openDrawer }] = useDrawer();

      return {
        L,
        registerTable,
        reloadTable,
        registerDrawer,
        openDrawer,
        useFramework,
      };
    },
    methods: {
      handleAddNew() {
        this.openDrawer(true, {}, true);
      },
      handleEdit(record: Recordable) {
        getById(record.id).then((menu) => {
          this.openDrawer(true, menu, true);
        });
      },
      handleDelete(record: Recordable) {
        Modal.warning({
          title: this.L('AreYouSure'),
          content: this.L('ItemWillBeDeletedMessageWithFormat', [record.displayName] as Recordable),
          okCancel: true,
          onOk: () => {
            deleteById(record.id).then(() => {
              this.reloadTable();
            });
          },
        });
      },
      handleChange() {
        this.reloadTable();
      },
    },
  });
</script>
