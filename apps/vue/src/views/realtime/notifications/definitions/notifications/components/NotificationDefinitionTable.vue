<template>
  <div>
    <BasicTable @register="registerTable">
      <template #form-groupName="{ model, field }">
        <FormItem name="groupName">
          <Select v-model:value="model[field]" :options="getGroupOptions" />
        </FormItem>
      </template>
      <template #form-notificationType="{ model, field }">
        <FormItem name="notificationType">
          <Select v-model:value="model[field]" :options="notificationTypeOptions" />
        </FormItem>
      </template>
      <template #form-notificationLifetime="{ model, field }">
        <FormItem name="notificationLifetime">
          <Select v-model:value="model[field]" :options="notificationLifetimeOptions" />
        </FormItem>
      </template>
      <template #form-contentType="{ model, field }">
        <FormItem name="contentType">
          <Select v-model:value="model[field]" :options="notificationContentTypeOptions" />
        </FormItem>
      </template>
      <template #form-template="{ model, field }">
        <FormItem name="template">
          <Select v-model:value="model[field]" :options="getAvailableTemplateOptions" />
        </FormItem>
      </template>
      <template #toolbar>
        <Button v-auth="['Notifications.Definitions.Create']" type="primary" @click="handleAddNew">
          {{ L('NotificationDefinitions:AddNew') }}
        </Button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'displayName'">
          <span>{{ getDisplayName(record.displayName) }}</span>
        </template>
      </template>
      <template #expandedRowRender="{ record }">
        <BasicTable @register="registerSubTable" :data-source="record.notifications">
          <template #bodyCell="{ column, record }">
            <template v-if="column.key === 'displayName'">
              <span>{{ getDisplayName(record.displayName) }}</span>
            </template>
            <template v-else-if="column.key === 'description'">
              <span>{{ getDisplayName(record.description) }}</span>
            </template>
            <template v-else-if="column.key === 'allowSubscriptionToClients'">
              <CheckOutlined v-if="record.allowSubscriptionToClients" class="enable" />
              <CloseOutlined v-else class="disable" />
            </template>
            <template v-else-if="column.key === 'notificationLifetime'">
              <span>{{ notificationLifetimeMap[record.notificationLifetime] }}</span>
            </template>
            <template v-else-if="column.key === 'notificationType'">
              <span>{{ notificationTypeMap[record.notificationType] }}</span>
            </template>
            <template v-else-if="column.key === 'contentType'">
              <span>{{ notificationContentTypeMap[record.contentType] }}</span>
            </template>
            <template v-else-if="column.key === 'providers'">
              <Tag v-for="provider in record.providers" color="blue" style="margin-right: 5px">{{
                provider
              }}</Tag>
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
                    auth: 'Notifications.Definitions.Update',
                    label: L('Edit'),
                    icon: 'ant-design:edit-outlined',
                    onClick: handleEdit.bind(null, record),
                  },
                  {
                    auth: 'Notifications.Definitions.Delete',
                    label: L('Delete'),
                    color: 'error',
                    icon: 'ant-design:delete-outlined',
                    ifShow: !record.isStatic,
                    onClick: handleDelete.bind(null, record),
                  },
                ]"
                :dropDownActions="[
                  {
                    label: L('Notifications:Send'),
                    icon: 'ant-design:edit-outlined',
                    onClick: handleSendNotification.bind(null, record),
                  },
                ]"
              />
            </template>
          </template>
        </BasicTable>
      </template>
    </BasicTable>
    <NotificationDefinitionModal @register="registerModal" @change="fetch" />
    <NotificationSendModal @register="registerSendModal" />
  </div>
</template>

<script lang="ts" setup>
  import { computed, reactive, onMounted } from 'vue';
  import { Button, Form, Select, Tag } from 'ant-design-vue';
  import { CheckOutlined, CloseOutlined } from '@ant-design/icons-vue';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getDataColumns } from '../datas/TableData';
  import { useModal } from '/@/components/Modal';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
  import { useNotificationDefinition } from '../hooks/useNotificationDefinition';
  import { GetListAsyncByInput as getGroupDefinitions } from '/@/api/realtime/notifications/definitions/groups';
  import { NotificationGroupDefinitionDto } from '/@/api/realtime/notifications/definitions/groups/model';
  import {
    GetListAsyncByInput,
    DeleteAsyncByName,
  } from '/@/api/realtime/notifications/definitions/notifications';
  import { NotificationDefinitionDto } from '/@/api/realtime/notifications/definitions/notifications/model';
  import { getSearchFormSchemas } from '../datas/ModalData';
  import { sorter } from '/@/utils/table';
  import NotificationDefinitionModal from './NotificationDefinitionModal.vue';
  import NotificationSendModal from './NotificationSendModal.vue';

  const FormItem = Form.Item;
  interface NotificationGroup {
    name: string;
    displayName: string;
    notifications: NotificationDefinitionDto[];
  }
  interface State {
    groups: NotificationGroupDefinitionDto[];
  }

  const { deserialize } = useLocalizationSerializer();
  const { L, Lr } = useLocalization(['Notifications', 'AbpUi']);
  const {
    notificationTypeMap,
    notificationTypeOptions,
    notificationLifetimeMap,
    notificationLifetimeOptions,
    notificationContentTypeMap,
    notificationContentTypeOptions,
    getAvailableTemplateOptions,
  } = useNotificationDefinition();
  const { createConfirm, createMessage } = useMessage();
  const [registerModal, { openModal }] = useModal();
  const [registerSendModal, { openModal: openSendModal }] = useModal();
  const [registerTable, { setLoading, getForm, setTableData }] = useTable({
    rowKey: 'name',
    title: L('NotificationDefinitions'),
    columns: [
      {
        title: L('DisplayName:DisplayName'),
        dataIndex: 'displayName',
        align: 'left',
        width: 180,
        resizable: true,
        sorter: (a, b) => sorter(a, b, 'displayName'),
      },
    ],
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
  });
  const [registerSubTable] = useTable({
    rowKey: 'name',
    columns: getDataColumns(),
    pagination: false,
    striped: false,
    useSearchForm: false,
    showIndexColumn: true,
    immediate: false,
    scroll: { x: 2000, y: 900 },
    actionColumn: {
      width: 180,
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
  const getDisplayName = computed(() => {
    return (displayName?: string) => {
      if (!displayName) return displayName;
      const info = deserialize(displayName);
      return Lr(info.resourceName, info.name);
    };
  });

  onMounted(() => {
    fetchGroups().then(fetch);
  });

  function fetch() {
    const form = getForm();
    return form.validate().then(() => {
      setLoading(true);
      setTableData([]);
      var input = form.getFieldsValue();
      GetListAsyncByInput(input)
        .then((res) => {
          const definitionGroupData: NotificationGroup[] = [];
          state.groups.forEach((group) => {
            const groupData: NotificationGroup = {
              name: group.name,
              displayName: group.displayName,
              notifications: [],
            };
            groupData.notifications.push(...res.items.filter((item) => item.groupName === group.name));
            definitionGroupData.push(groupData);
          });
          setTableData(definitionGroupData);
        })
        .finally(() => {
          setLoading(false);
        });
    });
  }

  function fetchGroups() {
    return getGroupDefinitions({}).then((res) => {
      state.groups = res.items;
    });
  }

  function handleAddNew() {
    openModal(true, {});
  }

  function handleEdit(record: NotificationDefinitionDto) {
    openModal(true, record);
  }

  function handleDelete(record: NotificationDefinitionDto) {
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

  function handleSendNotification(record: NotificationDefinitionDto) {
    openSendModal(true, record);
  }
</script>
