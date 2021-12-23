<template>
  <div>
    <BasicTable @register="registerTable">
      <template #level="{ record }">
        <Tag :color="LogLevelColor[record.level]">{{ LogLevelLabel[record.level] }}</Tag>
      </template>
      <template #action="{ record }">
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
    </BasicTable>
    <LoggingModal @register="registerModal" />
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import { Tag } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useModal } from '/@/components/Modal';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { LogLevelColor, LogLevelLabel } from '../datas/typing';
  import { getDataColumns } from '../datas/TableData';
  import { getSearchFormSchemas } from '../datas/ModalData';
  import { getList } from '/@/api/logging/logging';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import LoggingModal from './LoggingModal.vue';

  export default defineComponent({
    name: 'LoggingTable',
    components: { BasicTable, Tag, TableAction, LoggingModal },
    setup() {
      const { L } = useLocalization('AbpAuditLogging');
      const [registerModal, { openModal }] = useModal();
      const [registerTable] = useTable({
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
        canResize: false,
        immediate: true,
        formConfig: getSearchFormSchemas(),
        scroll: { x: true },
        actionColumn: {
          width: 180,
          title: L('Actions'),
          dataIndex: 'action',
          slots: { customRender: 'action' },
        },
      });

      function handleShow(record) {
        openModal(true, record);
      }

      return {
        L,
        registerModal,
        registerTable,
        LogLevelColor,
        LogLevelLabel,
        handleShow,
      };
    },
  });
</script>
