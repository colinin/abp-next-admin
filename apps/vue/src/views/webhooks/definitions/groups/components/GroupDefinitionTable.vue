<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <Button
          v-auth="['AbpWebhooks.GroupDefinitions.Create']"
          type="primary"
          @click="handleAddNew"
        >
          {{ L('GroupDefinitions:AddNew') }}
        </Button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'displayName'">
          <span>{{ getDisplayName(record.displayName) }}</span>
        </template>
        <template v-else-if="column.key === 'action'">
          <TableAction
            :stop-button-propagation="true"
            :actions="[
              {
                auth: 'AbpWebhooks.GroupDefinitions.Update',
                label: L('Edit'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEdit.bind(null, record),
              },
              {
                auth: 'AbpWebhooks.GroupDefinitions.Delete',
                label: L('Delete'),
                color: 'error',
                icon: 'ant-design:delete-outlined',
                ifShow: !record.isStatic,
                onClick: handleDelete.bind(null, record),
              },
            ]"
            :dropDownActions="[
              {
                ifShow: !record.isStatic,
                auth: 'AbpWebhooks.Definitions.Create',
                label: L('Webhooks:AddNew'),
                icon: 'ant-design:edit-outlined',
                onClick: handleAddWebhook.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <GroupDefinitionModal @register="registerModal" @change="fetch" />
    <WebhookDefinitionModal @register="registerWebhookModal" />
  </div>
</template>

<script lang="ts" setup>
  import { computed, onMounted } from 'vue';
  import { Button } from 'ant-design-vue';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getDataColumns } from '../datas/TableData';
  import { useModal } from '/@/components/Modal';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
  import { GetListAsyncByInput, DeleteAsyncByName } from '/@/api/webhooks/definitions/groups';
  import { getSearchFormSchemas } from '../datas/ModalData';
  import GroupDefinitionModal from './GroupDefinitionModal.vue';
  import WebhookDefinitionModal from '../../webhooks/components/WebhookDefinitionModal.vue';

  const { deserialize } = useLocalizationSerializer();
  const { L, Lr } = useLocalization(['WebhooksManagement', 'AbpUi']);
  const { createConfirm, createMessage } = useMessage();
  const [registerModal, { openModal }] = useModal();
  const [registerWebhookModal, { openModal: openWebhookModal }] = useModal();
  const [registerTable, { setLoading, getForm, setTableData }] = useTable({
    rowKey: 'name',
    title: L('GroupDefinitions'),
    columns: getDataColumns(),
    pagination: true,
    striped: false,
    useSearchForm: true,
    showIndexColumn: false,
    showTableSetting: true,
    tableSetting: {
      redo: false,
    },
    formConfig: {
      labelWidth: 100,
      submitFunc: fetch,
      schemas: getSearchFormSchemas(),
    },
    bordered: true,
    canResize: true,
    immediate: false,
    actionColumn: {
      width: 120,
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

  function fetch() {
    const form = getForm();
    return form.validate().then(() => {
      setLoading(true);
      setTableData([]);
      var input = form.getFieldsValue();
      GetListAsyncByInput(input)
        .then((res) => {
          setTableData(res.items);
        })
        .finally(() => {
          setLoading(false);
        });
    });
  }

  function handleAddNew() {
    openModal(true, {});
  }

  function handleEdit(record) {
    openModal(true, record);
  }

  function handleAddWebhook(record) {
    openWebhookModal(true, { groupName: record.name });
  }

  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeleteOrRestoreMessage'),
      onOk: () => {
        return DeleteAsyncByName(record.name).then(() => {
          createMessage.success(L('Successful'));
          fetch();
        });
      },
    });
  }
</script>
