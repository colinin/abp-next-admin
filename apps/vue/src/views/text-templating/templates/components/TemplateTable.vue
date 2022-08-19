<template>
  <div>
    <BasicTable @register="registerTable">
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'isInlineLocalized'">
          <!-- <Switch readonly :checked="record.isInlineLocalized" /> -->
          <CheckOutlined v-if="record.isInlineLocalized" class="enable" />
          <CloseOutlined v-else class="disable" />
        </template>
        <template v-else-if="column.key === 'isLayout'">
          <!-- <Switch readonly :checked="record.isLayout" /> -->
          <CheckOutlined v-if="record.isLayout" class="enable" />
          <CloseOutlined v-else class="disable" />
        </template>
        <template v-else-if="column.key === 'action'">
          <TableAction
            :stop-button-propagation="true"
            :actions="[
              {
                label: L('EditContents'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEditContent.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <TemplateContentModal @register="registerModal" />
  </div>
</template>

<script lang="ts" setup>
  import { CheckOutlined, CloseOutlined } from '@ant-design/icons-vue';
  import { useModal } from '/@/components/Modal';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getDataColumns } from '../datas/TableData';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { getList } from '/@/api/text-templating/templates';
  import { TextTemplateDefinition } from '/@/api/text-templating/templates/model';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import TemplateContentModal from './TemplateContentModal.vue';

  const { L } = useLocalization('AbpTextTemplating');

  const [registerModal, { openModal }] = useModal();
  const [registerTable] = useTable({
    rowKey: 'id',
    title: L('TextTemplates'),
    columns: getDataColumns(),
    api: getList,
    beforeFetch: formatPagedRequest,
    pagination: true,
    striped: false,
    useSearchForm: false,
    showIndexColumn: false,
    showTableSetting: true,
    bordered: true,
    canResize: true,
    immediate: true,
    actionColumn: {
      width: 150,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });

  function handleEditContent(record: TextTemplateDefinition) {
    openModal(true, record);
  }
</script>
