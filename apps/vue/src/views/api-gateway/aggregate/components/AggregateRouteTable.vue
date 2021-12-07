<template>
  <div class="content">
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button
          v-if="hasPermission('ApiGateway.AggregateRoute.Create')"
          type="primary"
          @click="handleAddNew"
          >{{ L('AggregateRoute:AddNew') }}</a-button
        >
      </template>
      <template #keys="{ record }">
        <Tag
          style="margin-right: 10px; margin-bottom: 5px"
          v-for="routeKey in record.reRouteKeys"
          :key="routeKey"
          color="blue"
        >
          {{ routeKey }}
        </Tag>
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              auth: 'ApiGateway.AggregateRoute.Update',
              label: L('Edit'),
              icon: 'ant-design:edit-outlined',
              onClick: handleEdit.bind(null, record),
            },
            {
              auth: 'ApiGateway.AggregateRoute.Delete',
              color: 'error',
              label: L('Delete'),
              icon: 'ant-design:delete-outlined',
              onClick: handleDelete.bind(null, record),
            },
          ]"
          :dropDownActions="[
            {
              auth: 'ApiGateway.AggregateRoute.ManageRouteConfig',
              label: L('AggregateRoute:ManageRouteConfig'),
              icon: 'ant-design:radius-setting-outlined',
              onClick: handleManageConfig.bind(null, record),
            },
          ]"
        />
      </template>
    </BasicTable>
    <BasicModalForm
      @register="registerEditModal"
      :save-changes="handleSaveChanges"
      :form-items="formItems"
      :title="formTitle"
      :width="600"
    />
    <AggregateRouteConfigModal @register="registerConfigModal" :routeId="routeId" />
  </div>
</template>

<script lang="ts">
  import { computed, defineComponent, ref, unref } from 'vue';
  import { cloneDeep } from 'lodash-es';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { Modal, Tag } from 'ant-design-vue';
  import { useModal } from '/@/components/Modal';
  import { BasicModalForm } from '/@/components/ModalForm';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import { create, deleteById, getList, update } from '/@/api/api-gateway/aggregate';
  import { getDataColumns } from './TableData';
  import { getSearchFormSchemas, getModalFormSchemas } from './ModalData';
  import {
    AggregateRoute,
    CreateAggregateRoute,
    UpdateAggregateRoute,
  } from '/@/api/api-gateway/model/aggregateModel';
  import AggregateRouteConfigModal from './AggregateRouteConfigModal.vue';

  export default defineComponent({
    name: 'AggregateRouteTable',
    components: { AggregateRouteConfigModal, BasicModalForm, BasicTable, TableAction, Tag },
    setup() {
      const { L } = useLocalization('ApiGateway');
      const { hasPermission } = usePermission();
      const routeId = ref<string>('');
      const formModel = ref<Nullable<AggregateRoute>>(null);
      const formItems = getModalFormSchemas();
      const formTitle = computed(() => {
        const model = unref(formModel);
        if (model && model.reRouteId) {
          return L('AggregateRoute:EditBy', [model.name] as Recordable);
        }
        return L('AggregateRoute:AddNew');
      });
      const [registerEditModal, { openModal: openEditModal }] = useModal();
      const [registerConfigModal, { openModal: openConfigModal }] = useModal();
      const [registerTable, { reload: reloadTable }] = useTable({
        rowKey: 'reRouteId',
        title: L('AggregateRoutes'),
        columns: getDataColumns(),
        api: getList,
        // beforeFetch: formatPagedRequest,
        pagination: true,
        striped: false,
        useSearchForm: true,
        showTableSetting: true,
        bordered: true,
        showIndexColumn: false,
        canResize: false,
        immediate: false,
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
        routeId,
        formItems,
        formModel,
        formTitle,
        registerEditModal,
        openEditModal,
        registerConfigModal,
        openConfigModal,
        hasPermission,
        registerTable,
        reloadTable,
      };
    },
    methods: {
      handleAddNew() {
        this.formModel = {} as AggregateRoute;
        this.openEditModal(true, {}, true);
      },
      handleEdit(record) {
        this.formModel = cloneDeep(record) as AggregateRoute;
        this.openEditModal(true, record, true);
      },
      handleDelete(record) {
        Modal.warning({
          title: this.L('AreYouSure'),
          content: this.L('ItemWillBeDeletedMessageWithFormat', [record.name] as Recordable),
          okCancel: true,
          onOk: () => {
            deleteById(record.reRouteId).then(() => {
              this.reloadTable();
            });
          },
        });
      },
      handleManageConfig(record) {
        this.routeId = record.reRouteId;
        this.openConfigModal(true, {}, true);
      },
      handleSaveChanges(val) {
        const api: Promise<AggregateRoute> = val.reRouteId
          ? update(
              cloneDeep(Object.assign(val, { routeId: val.reRouteId })) as UpdateAggregateRoute,
            )
          : create(cloneDeep(val) as CreateAggregateRoute);
        return api.then(() => {
          this.reloadTable();
        });
      },
    },
  });
</script>
