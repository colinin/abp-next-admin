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
          v-auth="['PermissionManagement.Definitions.Create']"
          type="primary"
          @click="handleAddNew"
        >
          {{ L('PermissionDefinitions:AddNew') }}
        </Button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'displayName'">
          <span>{{ getDisplayName(record.displayName) }}</span>
        </template>
      </template>
      <template #expandedRowRender="{ record }">
        <BasicTable @register="registerSubTable" :data-source="record.permissions">
          <template #bodyCell="{ column, record }">
            <template v-if="column.key === 'displayName'">
              <span>{{ getDisplayName(record.displayName) }}</span>
            </template>
            <template v-else-if="column.key === 'multiTenancySide'">
              <Tag color="blue">{{ multiTenancySidesMap[record.multiTenancySide] }}</Tag>
            </template>
            <template v-else-if="column.key === 'providers'">
              <Tag v-for="provider in record.providers" color="blue" style="margin: 5px">{{
                providersMap[provider]
              }}</Tag>
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
                    auth: 'PermissionManagement.Definitions.Update',
                    label: L('Edit'),
                    icon: 'ant-design:edit-outlined',
                    onClick: handleEdit.bind(null, record),
                  },
                  {
                    auth: 'PermissionManagement.Definitions.Delete',
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
      </template>
    </BasicTable>
    <PermissionDefinitionModal @register="registerModal" @change="fetch" />
  </div>
</template>

<script lang="ts" setup>
  import { cloneDeep } from 'lodash-es';
  import { computed, reactive, onMounted } from 'vue';
  import { Button, Form, Select, Tag } from 'ant-design-vue';
  import { CheckOutlined, CloseOutlined } from '@ant-design/icons-vue';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getDataColumns } from '../datas/TableData';
  import { useModal } from '/@/components/Modal';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
  import { GetListAsyncByInput as getGroupDefinitions } from '/@/api/permission-management/definitions/groups';
  import { PermissionGroupDefinitionDto } from '/@/api/permission-management/definitions/groups/model';
  import {
    GetListAsyncByInput,
    DeleteAsyncByName,
  } from '/@/api/permission-management/definitions/permissions';
  import { multiTenancySidesMap, providersMap } from '../../typing';
  import { getSearchFormSchemas } from '../datas/ModalData';
  import { listToTree } from '/@/utils/helper/treeHelper';
  import { sorter } from '/@/utils/table';
  import PermissionDefinitionModal from './PermissionDefinitionModal.vue';

  const FormItem = Form.Item;
  interface Permission {
    groupName: string;
    name: string;
    displayName: string;
    parentName?: string;
    isEnabled: boolean;
    isStatic: boolean;
    providers: string[];
    stateCheckers: string[];
    children: Permission[];
  }
  interface PermissionGroup {
    name: string;
    displayName: string;
    permissions: Permission[];
  }
  interface State {
    groups: PermissionGroupDefinitionDto[];
  }

  const { deserialize } = useLocalizationSerializer();
  const { L, Lr } = useLocalization(['AbpPermissionManagement', 'AbpUi']);
  const { createConfirm, createMessage } = useMessage();
  const [registerModal, { openModal }] = useModal();
  const [registerTable, { setLoading, getForm, setTableData }] = useTable({
    rowKey: 'name',
    title: L('PermissionDefinitions'),
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
      width: 100,
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
          const permissionGroupData: PermissionGroup[] = [];
          state.groups.forEach((group) => {
            const groupData: PermissionGroup = {
              name: group.name,
              displayName: group.displayName,
              permissions: [],
            };
            const permissionTree = listToTree(res.items.filter((item) => item.groupName === group.name), {
              id: 'name',
              pid: 'parentName',
            });
            permissionTree.forEach((tk) => {
              groupData.permissions.push(tk);
            });
            permissionGroupData.push(groupData);
          });
          setTableData(permissionGroupData);
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
    openModal(true, {
      groups: cloneDeep(state.groups),
    });
  }

  function handleEdit(record) {
    openModal(true, {
      record: record,
      groups: cloneDeep(state.groups),
    });
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
