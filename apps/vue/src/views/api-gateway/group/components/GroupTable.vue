<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button type="primary" @click="handleAddNew">{{ L('Group:AddNew') }}</a-button>
      </template>
      <template #active="{ record }">
        <Switch :checked="record.isActive" disabled />
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              auth: 'ApiGateway.RouteGroup.Update',
              label: L('Edit'),
              icon: 'ant-design:edit-outlined',
              onClick: handleEdit.bind(null, record),
            },
            {
              auth: 'ApiGateway.RouteGroup.Delete',
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
      @register="registerModal"
      :save-changes="handleSaveChanges"
      :form-items="formItems"
      :title="formTitle"
    />
  </div>
</template>

<script lang="ts">
  import { computed, defineComponent, ref, unref } from 'vue';
  import { cloneDeep } from 'lodash-es';
  import { Modal, Switch } from 'ant-design-vue';
  import { useModal } from '/@/components/Modal';
  import { BasicModalForm } from '/@/components/ModalForm';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import { create, deleteByAppId, getList, update } from '/@/api/api-gateway/group';
  import {
    RouteGroup,
    CreateRouteGroup,
    UpdateRouteGroup,
  } from '/@/api/api-gateway/model/groupModel';
  import { getDataColumns } from './TableData';
  import { getSearchFormSchemas, getModalFormSchemas } from './ModalData';
  export default defineComponent({
    name: 'GroupTable',
    components: { BasicModalForm, BasicTable, Switch, TableAction },
    setup() {
      const { L } = useLocalization('ApiGateway');
      const formModel = ref<Nullable<RouteGroup>>(null);
      const formItems = getModalFormSchemas();
      const formTitle = computed(() => {
        const model = unref(formModel);
        if (model && model.id) {
          return L('Group:EditBy', [model.name] as Recordable);
        }
        return L('Group:AddNew');
      });
      const [registerModal, { openModal }] = useModal();
      const [registerTable, { reload: reloadTable }] = useTable({
        rowKey: 'id',
        title: L('Groups'),
        columns: getDataColumns(),
        api: getList,
        beforeFetch: formatPagedRequest,
        pagination: true,
        striped: false,
        useSearchForm: true,
        showTableSetting: true,
        bordered: true,
        showIndexColumn: false,
        canResize: false,
        rowSelection: { type: 'checkbox' },
        formConfig: getSearchFormSchemas(),
        actionColumn: {
          width: 160,
          title: L('Actions'),
          dataIndex: 'action',
          slots: { customRender: 'action' },
        },
      });

      return {
        L,
        formModel,
        formItems,
        formTitle,
        registerModal,
        openModal,
        registerTable,
        reloadTable,
      };
    },
    methods: {
      handleAddNew() {
        this.formModel = {} as RouteGroup;
        this.openModal(true, {}, true);
      },
      handleEdit(record: Recordable) {
        this.formModel = cloneDeep(record) as RouteGroup;
        this.openModal(true, record, true);
      },
      handleDelete(record: Recordable) {
        Modal.warning({
          title: this.L('AreYouSure'),
          content: this.L('ItemWillBeDeletedMessageWithFormat', [record.name] as Recordable),
          okCancel: true,
          onOk: () => {
            deleteByAppId(record.appId).then(() => {
              this.reloadTable();
            });
          },
        });
      },
      handleSaveChanges(val) {
        const api: Promise<RouteGroup> = val.id
          ? update(cloneDeep(val) as UpdateRouteGroup)
          : create(cloneDeep(val) as CreateRouteGroup);
        return api.then(() => {
          this.reloadTable();
        });
      },
    },
  });
</script>
