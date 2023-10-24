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
          v-auth="['FeatureManagement.Definitions.Create']"
          type="primary"
          @click="handleAddNew"
        >
          {{ L('FeatureDefinitions:AddNew') }}
        </Button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'displayName'">
          <span>{{ getGroupDisplayName(record.displayName) }}</span>
        </template>
      </template>
      <template #expandedRowRender="{ record }">
        <BasicTable @register="registerSubTable" :data-source="record.features">
          <template #bodyCell="{ column, record }">
            <template v-if="column.key === 'groupName'">
              <span>{{ getGroupDisplayName(record.groupName) }}</span>
            </template>
            <template v-else-if="column.key === 'parentName'">
              <span>{{ getDisplayName(record.parentName) }}</span>
            </template>
            <template v-else-if="column.key === 'displayName'">
              <span>{{ getDisplayName(record.displayName) }}</span>
            </template>
            <template v-else-if="column.key === 'description'">
              <span>{{ getDisplayName(record.description) }}</span>
            </template>
            <template v-else-if="column.key === 'isVisibleToClients'">
              <CheckOutlined v-if="record.isVisibleToClients" class="enable" />
              <CloseOutlined v-else class="disable" />
            </template>
            <template v-else-if="column.key === 'isAvailableToHost'">
              <CheckOutlined v-if="record.isAvailableToHost" class="enable" />
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
                    auth: 'FeatureManagement.Definitions.Update',
                    label: L('Edit'),
                    icon: 'ant-design:edit-outlined',
                    onClick: handleEdit.bind(null, record),
                  },
                  {
                    auth: 'FeatureManagement.Definitions.Delete',
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
    <FeatureDefinitionModal @register="registerModal" @change="fetch" />
  </div>
</template>

<script lang="ts" setup>
  import { cloneDeep } from 'lodash-es';
  import { computed, reactive, onMounted } from 'vue';
  import { Button, Form, Select } from 'ant-design-vue';
  import { CheckOutlined, CloseOutlined } from '@ant-design/icons-vue';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getDataColumns } from '../datas/TableData';
  import { useModal } from '/@/components/Modal';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
  import { GetListAsyncByInput as getGroupDefinitions } from '/@/api/feature-management/definitions/groups';
  import { FeatureGroupDefinitionDto } from '/@/api/feature-management/definitions/groups/model';
  import { GetListAsyncByInput, DeleteAsyncByName } from '/@/api/feature-management/definitions/features';
  import { getSearchFormSchemas } from '../datas/ModalData';
  import { listToTree } from '/@/utils/helper/treeHelper';
  import { groupBy } from '/@/utils/array';
  import { sorter } from '/@/utils/table';
  import FeatureDefinitionModal from './FeatureDefinitionModal.vue';

  const FormItem = Form.Item;
  interface Feature {
    name: string;
    groupName: string;
    displayName: string;
    parentName?: string;
    description?: string;
    defaultValue?: string;
    valueType: string;
    isStatic: boolean;
    isVisibleToClients: boolean;
    isAvailableToHost: boolean;
    allowedProviders: string[];
    children: Feature[];
  }
  interface FeatureGroup {
    name: string;
    displayName: string;
    features: Feature[];
  }
  interface State {
    groups: FeatureGroupDefinitionDto[];
  }

  const { deserialize } = useLocalizationSerializer();
  const { L, Lr } = useLocalization(['AbpFeatureManagement', 'AbpUi']);
  const { createConfirm, createMessage } = useMessage();
  const [registerModal, { openModal }] = useModal();
  const [registerTable, { setLoading, getForm, setTableData }] = useTable({
    rowKey: 'name',
    title: L('FeatureDefinitions'),
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
      width: 150,
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
        const featureGroup = groupBy(res.items, 'groupName');
        const featureGroupData: FeatureGroup[] = [];
        Object.keys(featureGroup).forEach((gk) => {
          const groupData: FeatureGroup = {
            name: gk,
            displayName: gk,
            features: [],
          };
          const featureTree = listToTree(featureGroup[gk], {
            id: 'name',
            pid: 'parentName',
          });
          featureTree.forEach((tk) => {
            groupData.features.push(tk);
          });
          featureGroupData.push(groupData);
        });
        setTableData(featureGroupData);
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
