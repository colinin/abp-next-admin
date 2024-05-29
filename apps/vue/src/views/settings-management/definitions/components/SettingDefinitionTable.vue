<template>
  <div>
    <BasicTable @register="registerTable">
      <template #toolbar>
        <Button
          v-auth="['SettingManagement.Definition.Create']"
          type="primary"
          @click="handleAddNew"
        >
          {{ L('Definition:AddNew') }}
        </Button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'displayName'">
          <span>{{ getDisplayName(record.displayName) }}</span>
        </template>
        <template v-else-if="column.key === 'description'">
          <span>{{ getDisplayName(record.description) }}</span>
        </template>
        <template v-else-if="column.key === 'isVisibleToClients'">
          <CheckOutlined v-if="record.isVisibleToClients" class="enable" />
          <CloseOutlined v-else class="disable" />
        </template>
        <template v-else-if="column.key === 'isInherited'">
          <CheckOutlined v-if="record.isInherited" class="enable" />
          <CloseOutlined v-else class="disable" />
        </template>
        <template v-else-if="column.key === 'isEncrypted'">
          <CheckOutlined v-if="record.isEncrypted" class="enable" />
          <CloseOutlined v-else class="disable" />
        </template>
        <template v-else-if="column.key === 'isStatic'">
          <CheckOutlined v-if="record.isStatic" class="enable" />
          <CloseOutlined v-else class="disable" />
        </template>
        <template v-else-if="column.key === 'providers'">
          <Tag v-for="provider in record.providers" style="margin-left: 5px" color="blue">{{
            provider
          }}</Tag>
        </template>
        <template v-else-if="column.key === 'action'">
          <TableAction
            :stop-button-propagation="true"
            :actions="[
              {
                auth: 'SettingManagement.Definition.Update',
                label: L('Edit'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEdit.bind(null, record),
              },
              {
                auth: 'SettingManagement.Definition.DeleteOrRestore',
                label: L('DeleteOrRestore'),
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
    <SettingDefinitionModal @register="registerModal" @change="fetch" />
  </div>
</template>

<script lang="ts" setup>
  import { computed, onMounted } from 'vue';
  import { Button, Tag } from 'ant-design-vue';
  import { CheckOutlined, CloseOutlined } from '@ant-design/icons-vue';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { getDataColumns } from '../datas/TableData';
  import { useModal } from '/@/components/Modal';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useLocalizationSerializer } from '/@/hooks/abp/useLocalizationSerializer';
  import {
    GetListAsyncByInput,
    DeleteOrRestoreAsyncByName,
  } from '/@/api/settings-management/definitions';
  import { getSearchFormSchemas } from '../datas/ModalData';
  import SettingDefinitionModal from './SettingDefinitionModal.vue';

  const { deserialize } = useLocalizationSerializer();
  const { L, Lr } = useLocalization(['AbpSettingManagement', 'AbpUi']);
  const { createConfirm, createMessage } = useMessage();
  const [registerModal, { openModal }] = useModal();
  const [registerTable, { setLoading, getForm, setTableData }] = useTable({
    rowKey: 'name',
    title: L('Settings'),
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
      width: 200,
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

  function handleDelete(record) {
    createConfirm({
      iconType: 'warning',
      title: L('AreYouSure'),
      content: L('ItemWillBeDeleteOrRestoreMessage'),
      onOk: () => {
        return DeleteOrRestoreAsyncByName(record.name).then(() => {
          createMessage.success(L('Successful'));
          fetch();
        });
      },
    });
  }
</script>
