<template>
  <div>
    <BasicTable @register="registerTable">
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'level'">
          <Tag :color="LogLevelColor[record.level]" @click="handleFilter('level', record.level)">{{
            LogLevelLabel[record.level]
          }}</Tag>
        </template>
        <template v-else-if="column.key === 'application'">
          <a
            class="link"
            href="javaScript:void(0);"
            @click="handleFilter('application', record.fields.application)"
            >{{ record.fields.application }}</a
          >
        </template>
        <template v-else-if="column.key === 'machineName'">
          <a
            class="link"
            href="javaScript:void(0);"
            @click="handleFilter('machineName', record.fields.machineName)"
            >{{ record.fields.machineName }}</a
          >
        </template>
        <template v-else-if="column.key === 'environment'">
          <a
            class="link"
            href="javaScript:void(0);"
            @click="handleFilter('environment', record.fields.environment)"
            >{{ record.fields.environment }}</a
          >
        </template>
        <template v-else-if="column.key === 'connectionId'">
          <a
            class="link"
            href="javaScript:void(0);"
            @click="handleFilter('connectionId', record.fields.connectionId)"
            >{{ record.fields.connectionId }}</a
          >
        </template>
        <template v-else-if="column.key === 'correlationId'">
          <a
            class="link"
            href="javaScript:void(0);"
            @click="handleFilter('correlationId', record.fields.correlationId)"
            >{{ record.fields.correlationId }}</a
          >
        </template>
        <template v-else-if="column.key === 'requestId'">
          <a
            class="link"
            href="javaScript:void(0);"
            @click="handleFilter('requestId', record.fields.requestId)"
            >{{ record.fields.requestId }}</a
          >
        </template>
        <template v-else-if="column.key === 'requestPath'">
          <a
            class="link"
            href="javaScript:void(0);"
            @click="handleFilter('requestPath', record.fields.requestPath)"
            >{{ record.fields.requestPath }}</a
          >
        </template>
        <template v-else-if="column.key === 'processId'">
          <a
            class="link"
            href="javaScript:void(0);"
            @click="handleFilter('processId', record.fields.processId)"
            >{{ record.fields.processId }}</a
          >
        </template>
        <template v-else-if="column.key === 'threadId'">
          <a
            class="link"
            href="javaScript:void(0);"
            @click="handleFilter('threadId', record.fields.threadId)"
            >{{ record.fields.threadId }}</a
          >
        </template>
        <template v-else-if="column.key === 'context'">
          <a
            class="link"
            href="javaScript:void(0);"
            @click="handleFilter('context', record.fields.level)"
            >{{ record.fields.level }}</a
          >
        </template>
        <template v-else-if="column.key === 'action'">
          <TableAction
            :actions="[
              {
                color: 'success',
                label: L('ShowLogDialog'),
                icon: 'ant-design:search-outlined',
                onClick: handleShow.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <LoggingModal @register="registerModal" />
  </div>
</template>

<script lang="ts" setup>
  import { Tag } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useModal } from '/@/components/Modal';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { LogLevelColor, LogLevelLabel } from '../datas/typing';
  import { getDataColumns } from '../datas/TableData';
  import { getSearchFormSchemas } from '../datas/ModalData';
  import { getList } from '/@/api/logging/logs';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import LoggingModal from './LoggingModal.vue';

  const { L } = useLocalization('AbpAuditLogging');
  const [registerModal, { openModal }] = useModal();
  const [registerTable, { getForm }] = useTable({
    rowKey: 'fields.id',
    title: L('Logging'),
    columns: getDataColumns(),
    api: getList,
    beforeFetch: formatPagedRequest,
    pagination: true,
    striped: false,
    useSearchForm: true,
    showTableSetting: true,
    bordered: true,
    showIndexColumn: false,
    canResize: true,
    immediate: true,
    formConfig: getSearchFormSchemas(),
    scroll: { x: 'max-content', y: '100%' },
    actionColumn: {
      width: 180,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });

  function handleFilter(field: string, value: any) {
    const form = getForm();
    const setField: Recordable = {};
    setField[`${field}`] = value;
    form?.setFieldsValue(setField);
    form?.submit();
  }

  function handleShow(record) {
    openModal(true, record);
  }
</script>

<style lang="less" scoped>
  .link {
    cursor: pointer;
    margin-left: 5px;
  }
</style>
