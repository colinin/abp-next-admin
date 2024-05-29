<template>
  <div>
    <BasicTable @register="registerTable">
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'url'">
          <Tag
            :color="httpStatusCodeColor(record.httpStatusCode)"
            @click="handleFilter('httpStatusCode', record.httpStatusCode)"
            >{{ record.httpStatusCode }}</Tag
          >
          <Tag
            style="margin-left: 5px"
            :color="httpMethodColor(record.httpMethod)"
            @click="handleFilter('httpMethod', record.httpMethod)"
            >{{ record.httpMethod }}</Tag
          >
          <a class="link" href="javaScript:void(0);" @click="handleFilter('url', record.url)">{{
            record.url
          }}</a>
        </template>
        <template v-else-if="column.key === 'applicationName'">
          <a
            class="link"
            href="javaScript:void(0);"
            @click="handleFilter('applicationName', record.applicationName)"
            >{{ record.applicationName }}</a
          >
        </template>
        <template v-else-if="column.key === 'userName'">
          <a
            class="link"
            href="javaScript:void(0);"
            @click="handleFilter('userName', record.userName)"
            >{{ record.userName }}</a
          >
        </template>
        <template v-else-if="column.key === 'clientIpAddress'">
          <a
            class="link"
            href="javaScript:void(0);"
            @click="handleFilter('clientIpAddress', record.clientIpAddress)"
            >{{ record.clientIpAddress }}</a
          >
        </template>
        <template v-else-if="column.key === 'clientId'">
          <a
            class="link"
            href="javaScript:void(0);"
            @click="handleFilter('clientId', record.clientId)"
            >{{ record.clientId }}</a
          >
        </template>
        <template v-else-if="column.key === 'correlationId'">
          <a
            class="link"
            href="javaScript:void(0);"
            @click="handleFilter('correlationId', record.correlationId)"
            >{{ record.correlationId }}</a
          >
        </template>
        <template v-else-if="column.key === 'action'">
          <TableAction
            :actions="[
              {
                auth: 'AbpAuditing.AuditLog',
                color: 'success',
                label: L('ShowLogDialog'),
                icon: 'ant-design:search-outlined',
                onClick: handleShow.bind(null, record),
              },
              {
                auth: 'AbpAuditing.AuditLog.Delete',
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
    <AuditLogModal @register="registerModal" />
  </div>
</template>

<script lang="ts" setup>
  import { Tag } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getDataColumns } from './TableData';
  import { getSearchFormSchemas } from './ModalData';
  import { useModal } from '/@/components/Modal';
  import { useAuditLog } from '../hooks/useAuditLog';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { deleteById, getList } from '/@/api/auditing/audit-log';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import AuditLogModal from './AuditLogModal.vue';

  const { createMessage, createConfirm } = useMessage();
  const { L } = useLocalization('AbpAuditLogging');
  const [registerTable, { reload, getForm }] = useTable({
    rowKey: 'id',
    title: L('AuditLog'),
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
    immediate: true,
    formConfig: getSearchFormSchemas(),
    scroll: { x: 'max-content', y: '100%' },
    actionColumn: {
      width: 180,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });
  const [registerModal, { openModal }] = useModal();
  const { httpMethodColor, httpStatusCodeColor } = useAuditLog();

  function handleFilter(field: string, value: any) {
    const form = getForm();
    const setField: Recordable = {};
    setField[`${field}`] = value;
    form?.setFieldsValue(setField);
    form?.submit();
  }

  function handleShow(record) {
    openModal(true, record, true);
  }

  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeletedMessage'),
      okCancel: true,
      onOk: () => {
        return deleteById(record.id).then(() => {
          createMessage.success(L('SuccessfullyDeleted'));
          reload();
        });
      },
    });
  }
</script>

<style lang="less" scoped>
  .link {
    cursor: pointer;
    margin-left: 5px;
  }
</style>
