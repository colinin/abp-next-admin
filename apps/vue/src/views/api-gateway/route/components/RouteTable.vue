<template>
  <div class="content">
    <BasicTable @register="registerTable">
      <template #toolbar>
        <a-button
          v-if="hasPermission('ApiGateway.Route.Create')"
          type="primary"
          @click="handleAddNew"
          >{{ L('Route:AddNew') }}</a-button
        >
      </template>
      <template #methods="{ record }">
        <Tag
          style="margin-right: 10px; margin-bottom: 5px"
          v-for="method in record.upstreamHttpMethod"
          :key="method"
          :color="HttpMethods[method]"
        >
          {{ method }}
        </Tag>
      </template>
      <template #hosts="{ record }">
        <Tag
          style="margin-bottom: 5px"
          v-for="host in record.downstreamHostAndPorts"
          :key="host"
          color="blue"
        >
          {{ `${host.host}:${host.port}` }}
        </Tag>
      </template>
      <template #action="{ record }">
        <TableAction
          :actions="[
            {
              auth: 'ApiGateway.Route.Update',
              label: L('Edit'),
              icon: 'ant-design:edit-outlined',
              onClick: handleEdit.bind(null, record),
            },
            {
              auth: 'ApiGateway.Route.Delete',
              color: 'error',
              label: L('Delete'),
              icon: 'ant-design:delete-outlined',
              onClick: handleDelete.bind(null, record),
            },
          ]"
        />
      </template>
    </BasicTable>
    <RouteModal @register="registerModal" @change="handleChange" />
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { usePermission } from '/@/hooks/web/usePermission';
  import { Modal, Tag } from 'ant-design-vue';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import RouteModal from './RouteModal.vue';
  import { useModal } from '/@/components/Modal';
  import { deleteById, getList } from '/@/api/api-gateway/route';
  import { HttpMethods } from '/@/api/api-gateway/model/basicModel';
  import { getDataColumns } from './TableData';
  import { getSearchFormSchemas } from './ModalData';
  // import { formatPagedRequest } from '/@/utils/http/abp/helper';
  export default defineComponent({
    name: 'RouteTable',
    components: { BasicTable, RouteModal, Tag, TableAction },
    setup() {
      const { L } = useLocalization('ApiGateway');

      const { hasPermission } = usePermission();
      const [registerModal, { openModal }] = useModal();
      const [registerTable, { reload: reloadTable }] = useTable({
        rowKey: 'reRouteId',
        title: L('Routes'),
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

      function handleChange(route) {
        reloadTable({ searchInfo: { appId: route.appId } });
      }

      function handleAddNew() {
        openModal(true, {}, true);
      }

      function handleEdit(record) {
        openModal(true, record, true);
      }

      function handleDelete(record) {
        Modal.warning({
          title: L('AreYouSure'),
          content: L('ItemWillBeDeletedMessageWithFormat', [record.reRouteName] as Recordable),
          okCancel: true,
          onOk: () => {
            deleteById(record.reRouteId).then(() => {
              reloadTable({ searchInfo: { appId: record.appId } });
            });
          },
        });
      }

      return {
        L,
        registerTable,
        reloadTable,
        registerModal,
        openModal,
        hasPermission,
        HttpMethods,
        handleAddNew,
        handleEdit,
        handleDelete,
        handleChange,
      };
    },
  });
</script>
