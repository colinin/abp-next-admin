<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <Button
          v-auth="['FeatureManagement.GroupDefinitions.Create']"
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
                auth: 'FeatureManagement.GroupDefinitions.Update',
                label: L('Edit'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEdit.bind(null, record),
              },
              {
                auth: 'FeatureManagement.GroupDefinitions.Delete',
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
                auth: 'FeatureManagement.Definitions.Create',
                label: L('FeatureDefinitions:AddNew'),
                icon: 'ant-design:edit-outlined',
                onClick: handleAddFeature.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <GroupDefinitionModal @register="registerModal" @change="fetch" />
    <FeatureDefinitionModal @register="registerFeatureModal" />
  </div>
</template>

<script lang="ts" setup>
  import { cloneDeep } from 'lodash-es';
  import { computed, reactive, onMounted } from 'vue';
  import { Button } from 'ant-design-vue';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getDataColumns } from '../datas/TableData';
  import { useModal } from '/@/components/Modal';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
  import { FeatureGroupDefinitionDto } from '/@/api/feature-management/definitions/groups/model';
  import { getList, deleteByName } from '/@/api/feature-management/definitions/groups';
  import { getSearchFormSchemas } from '../datas/ModalData';
  import GroupDefinitionModal from './GroupDefinitionModal.vue';
  import FeatureDefinitionModal from '../../features/components/FeatureDefinitionModal.vue';

  interface State {
    groups: FeatureGroupDefinitionDto[];
  }

  const state = reactive<State>({
    groups: [],
  });
  const { deserialize } = useLocalizationSerializer();
  const { L, Lr } = useLocalization(['AbpFeatureManagement', 'AbpUi']);
  const { createConfirm, createMessage } = useMessage();
  const [registerModal, { openModal }] = useModal();
  const [registerFeatureModal, { openModal: openFeatureModal }] = useModal();
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
      state.groups = [];
      var input = form.getFieldsValue();
      getList(input)
        .then((res) => {
          state.groups = res.items;
        })
        .finally(() => {
          setTableData(state.groups);
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

  function handleAddFeature(record) {
    openFeatureModal(true, {
      groupName: record.name,
      groups: cloneDeep(state.groups),
    });
  }

  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeleteOrRestoreMessage'),
      onOk: () => {
        return deleteByName(record.name).then(() => {
          createMessage.success(L('Successful'));
          fetch();
        });
      },
    });
  }
</script>
