<template>
  <div>
    <BasicTable @register="registerTable">
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'isStatic'">
          <CheckOutlined v-if="record.isStatic" class="enable" />
          <CloseOutlined v-else class="disable" />
        </template>
        <template v-else-if="column.key === 'isInlineLocalized'">
          <CheckOutlined v-if="record.isInlineLocalized" class="enable" />
          <CloseOutlined v-else class="disable" />
        </template>
        <template v-else-if="column.key === 'isLayout'">
          <CheckOutlined v-if="record.isLayout" class="enable" />
          <CloseOutlined v-else class="disable" />
        </template>
        <template v-else-if="column.key === 'action'">
          <TableAction
            v-auth="['AbpTextTemplating.TextTemplateDefinitions', 'AbpTextTemplating.TextTemplateDefinitions.Delete']"
            :stop-button-propagation="true"
            :actions="[
              {
                auth: 'AbpTextTemplating.TextTemplateDefinitions.Update',
                label: L('Update'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEdit.bind(null, record),
              },
              {
                auth: 'AbpTextTemplating.TextTemplateDefinitions.Delete',
                label: L('Delete'),
                color: 'error',
                icon: 'ant-design:delete-outlined',
                onClick: handleDelete.bind(null, record),
              },
            ]"
            :dropDownActions="[
              {
                auth: 'AbpTextTemplating.TextTemplateContent.Update',
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
  import { getSearchFormSchemas } from '../datas/ModalData';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { GetListAsyncByInput, DeleteAsyncByName } from '/@/api/text-templating/definitions';
  import { TextTemplateDefinitionDto } from '/@/api/text-templating/definitions/model';
  import { formatPagedRequest } from '/@/utils/http/abp/helper';
  import TemplateContentModal from './TemplateContentModal.vue';

  const { L } = useLocalization(['AbpTextTemplating', 'AbpUi']);
  const { createConfirm, createMessage } = useMessage();
  const [registerModal, { openModal }] = useModal();
  const [registerTable, { reload }] = useTable({
    rowKey: 'name',
    title: L('TextTemplates'),
    columns: getDataColumns(),
    api: GetListAsyncByInput,
    beforeFetch: formatPagedRequest,
    pagination: true,
    striped: false,
    useSearchForm: true,
    formConfig: getSearchFormSchemas(),
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

  function handleEdit(record: TextTemplateDefinitionDto) {
    console.log('This method is not implemented', record);
  }

  function handleEditContent(record: TextTemplateDefinitionDto) {
    openModal(true, record);
  }

  function handleDelete(record: TextTemplateDefinitionDto) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: record.isStatic ? L('RestoreTemplateToDefaultMessage') : L('ItemWillBeDeletedMessage'),
      onOk: () => {
        return DeleteAsyncByName(record.name).then(() => {
          createMessage.success(record.isStatic ? L('TemplateUpdated') : L('SuccessfullyDeleted'));
          reload();
        });
      },
    });
  }
</script>
