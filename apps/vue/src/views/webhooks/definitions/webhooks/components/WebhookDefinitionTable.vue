<template>
  <div>
    <BasicTable @register="registerTable">
      <template #form-groupName="{ model, field }">
        <FormItem name="groupName">
          <Select v-model:value="model[field]" :options="getGroupOptions" />
        </FormItem>
      </template>
      <template #toolbar>
        <Button
          v-auth="['AbpWebhooks.Definitions.Create']"
          type="primary"
          @click="handleAddNew"
        >
          {{ L('Webhooks:AddNew') }}
        </Button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'groupName'">
          <span>{{ getGroupDisplayName(record.groupName) }}</span>
        </template>
        <template v-else-if="column.key === 'displayName'">
          <span>{{ getDisplayName(record.displayName) }}</span>
        </template>
        <template v-else-if="column.key === 'description'">
          <span>{{ getDisplayName(record.description) }}</span>
        </template>
        <template v-else-if="column.key === 'isEnabled'">
          <CheckOutlined v-if="record.isEnabled" class="enable" />
          <CloseOutlined v-else class="disable" />
        </template>
        <template v-else-if="column.key === 'isStatic'">
          <CheckOutlined v-if="record.isStatic" class="enable" />
          <CloseOutlined v-else class="disable" />
        </template>
        <template v-else-if="column.key === 'action'">
          <TableAction
            :stop-button-propagation="true"
            :actions="[
              {
                auth: 'AbpWebhooks.Definitions.Update',
                label: L('Edit'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEdit.bind(null, record),
              },
              {
                auth: 'AbpWebhooks.Definitions.Delete',
                label: L('Delete'),
                color: 'error',
                icon: 'ant-design:delete-outlined',
                ifShow: !record.isStatic,
                onClick: handleDelete.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <WebhookDefinitionModal @register="registerModal" @change="fetch" />
  </div>
</template>

<script lang="ts" setup>
  import { computed, reactive, onMounted } from 'vue';
  import { Button, Form, Select } from 'ant-design-vue';
  import { CheckOutlined, CloseOutlined } from '@ant-design/icons-vue';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getDataColumns } from '../datas/TableData';
  import { useModal } from '/@/components/Modal';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
  import { GetListAsyncByInput as getGroupDefinitions } from '/@/api/webhooks/definitions/groups';
  import { WebhookGroupDefinitionDto } from '/@/api/webhooks/definitions/groups/model';
  import { GetListAsyncByInput, DeleteAsyncByName } from '/@/api/webhooks/definitions/webhooks';
  import { getSearchFormSchemas } from '../datas/ModalData';
  import WebhookDefinitionModal from './WebhookDefinitionModal.vue';

  const FormItem = Form.Item;
  interface State {
    groups: WebhookGroupDefinitionDto[],
  }

  const { deserialize } = useLocalizationSerializer();
  const { L, Lr } = useLocalization(['WebhooksManagement', 'AbpUi']);
  const { createConfirm, createMessage } = useMessage();
  const [registerModal, { openModal }] = useModal();
  const [registerTable, { setLoading, getForm, setTableData }] = useTable({
    rowKey: 'name',
    title: L('WebhookDefinitions'),
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
  const state = reactive<State>({
    groups: [],
  });
  const getGroupOptions = computed(() => {
    return state.groups.map((group) => {
      const info = deserialize(group.displayName);
      return {
        label: Lr(info.resourceName, info.name),
        value: group.name,
      };
    });
  });
  const getGroupDisplayName = computed(() => {
    return (groupName: string) => {
      const group = state.groups.find(x => x.name === groupName);
      if (!group) return groupName;
      const info = deserialize(group.displayName);
      return Lr(info.resourceName, info.name);
    };
  });
  const getDisplayName = computed(() => {
    return (displayName?: string) => {
      if (!displayName) return displayName;
      const info = deserialize(displayName);
      return Lr(info.resourceName, info.name);
    };
  });

  onMounted(() => {
    fetch();
    fetchGroups();
  });

  function fetch() {
    const form = getForm();
    return form.validate().then(() => {
      setLoading(true);
      setTableData([]);
      var input = form.getFieldsValue();
      GetListAsyncByInput(input).then((res) => {
        setTableData(res.items);
      }).finally(() => {
        setLoading(false);
      });
    });
  }

  function fetchGroups() {
    getGroupDefinitions({}).then((res) => {
      state.groups = res.items;
    });
  }

  function handleAddNew() {
    openModal(true, {});
  }

  function handleEdit(record) {
    openModal(true, record);
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
