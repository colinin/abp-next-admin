<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <Button
          v-auth="['AbpTextTemplating.TextTemplateDefinitions.Create']"
          type="primary"
          @click="handleAddNew"
          >{{ L('TextTemplates:AddNew') }}
        </Button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'displayName'">
          <span>{{ getDisplayName(record.displayName) }}</span>
        </template>
        <template v-else-if="column.key === 'isStatic'">
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
            v-auth="[
              'AbpTextTemplating.TextTemplateDefinitions.Update',
              'AbpTextTemplating.TextTemplateDefinitions.Delete',
              'AbpTextTemplating.TextTemplateContents.Update',
            ]"
            :stop-button-propagation="true"
            :actions="[
              {
                auth: 'AbpTextTemplating.TextTemplateDefinitions.Update',
                label: L('Edit'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEdit.bind(null, record),
              },
              {
                ifShow: !record.isStatic,
                auth: 'AbpTextTemplating.TextTemplateDefinitions.Delete',
                label: L('Delete'),
                color: 'error',
                icon: 'ant-design:delete-outlined',
                onClick: handleDelete.bind(null, record),
              },
            ]"
            :dropDownActions="[
              {
                auth: 'AbpTextTemplating.TextTemplateContents.Update',
                label: L('EditContents'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEditContent.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <TemplateContentModal @register="registerContentModal" />
    <TemplateDefinitionModal @register="registerEditModal" @change="fetch" />
  </div>
</template>

<script lang="ts" setup>
  import { computed, onMounted } from 'vue';
  import { Button } from 'ant-design-vue';
  import { CheckOutlined, CloseOutlined } from '@ant-design/icons-vue';
  import { useModal } from '/@/components/Modal';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getDataColumns } from '../datas/TableData';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
  import { GetListAsyncByInput, DeleteAsyncByName } from '/@/api/text-templating/definitions';
  import { TextTemplateDefinitionDto } from '/@/api/text-templating/definitions/model';
  import TemplateContentModal from './TemplateContentModal.vue';
  import TemplateDefinitionModal from './TemplateDefinitionModal.vue';

  const { deserialize } = useLocalizationSerializer();
  const { L, Lr } = useLocalization(['AbpTextTemplating', 'AbpUi']);
  const { createConfirm, createMessage } = useMessage();
  const [registerEditModal, { openModal: openEditModal }] = useModal();
  const [registerContentModal, { openModal: openContentModal }] = useModal();
  const [registerTable, { getForm, setLoading, setTableData }] = useTable({
    rowKey: 'name',
    title: L('TextTemplates'),
    columns: getDataColumns(),
    pagination: true,
    striped: false,
    useSearchForm: true,
    formConfig: {
      labelWidth: 100,
      schemas: [
        {
          field: 'filter',
          component: 'Input',
          label: L('Search'),
          colProps: { span: 18 },
        },
        {
          field: 'isLayout',
          component: 'Checkbox',
          label: L('DisplayName:IsLayout'),
          colProps: { span: 6 },
          renderComponentContent: L('DisplayName:IsLayout'),
        },
      ],
      submitFunc: fetch,
    },
    showIndexColumn: false,
    showTableSetting: true,
    bordered: true,
    canResize: true,
    immediate: false,
    actionColumn: {
      width: 150,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });
  const getDisplayName = computed(() => {
    return (displayName?: string) => {
      if (!displayName) return displayName;
      const info = deserialize(displayName);
      return Lr(info.resourceName, info.name);
    };
  });

  onMounted(fetch);

  async function fetch() {
    try {
      const form = getForm();
      const input = await form.validate();
      setLoading(true);
      setTableData([]);
      const { items } = await GetListAsyncByInput(input);
      setTableData(items);
    } finally {
      setLoading(false);
    }
  }

  function handleAddNew() {
    openEditModal(true, {});
  }

  function handleEdit(record: TextTemplateDefinitionDto) {
    openEditModal(true, record);
  }

  function handleEditContent(record: TextTemplateDefinitionDto) {
    openContentModal(true, record);
  }

  function handleDelete(record: TextTemplateDefinitionDto) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: record.isStatic
        ? L('RestoreTemplateToDefaultMessage')
        : L('ItemWillBeDeletedMessage'),
      onOk: () => {
        return DeleteAsyncByName(record.name).then(() => {
          createMessage.success(record.isStatic ? L('TemplateUpdated') : L('SuccessfullyDeleted'));
          fetch();
        });
      },
    });
  }
</script>
