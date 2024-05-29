<template>
  <BasicTable @register="registerTable">
    <template #toolbar>
      <a-button v-if="isEnableNew" type="primary" @click="handleAppendItem">{{
        L('Data:AppendItem')
      }}</a-button>
    </template>
    <template #bodyCell="{ column, record }">
      <template v-if="column.key === 'allowBeNull'">
        <Switch :checked="record.allowBeNull" readonly />
      </template>
      <template v-else-if="column.key === 'action'">
        <TableAction
          :actions="[
            {
              auth: 'Platform.DataDictionary.Update',
              label: L('Edit'),
              icon: 'ant-design:edit-outlined',
              onClick: handleEdit.bind(null, record),
            },
            {
              auth: 'Platform.DataDictionary.ManageItems',
              color: 'error',
              label: L('Delete'),
              icon: 'ant-design:delete-outlined',
              onClick: handleDelete.bind(null, record),
            },
          ]"
        />
      </template>
    </template>
  </BasicTable>
  <DataItemModal @register="registerModal" @change="fetchItems(dataId)" />
</template>

<script lang="ts" setup>
  import { computed, ref, createVNode, watch } from 'vue';
  import { getDataColumns } from './TableData';
  import { Switch } from 'ant-design-vue';
  import { ExclamationCircleOutlined } from '@ant-design/icons-vue';
  import { BasicTable, useTable, TableAction } from '/@/components/Table';
  import { useModal } from '/@/components/Modal';
  import { useMessage } from '/@/hooks/web/useMessage';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { DataItem } from '/@/api/platform/datas/model';
  import { get, removeItem } from '/@/api/platform/datas';
  import DataItemModal from './DataItemModal.vue';

  const emits = defineEmits(['reload']);
  const props = defineProps({
    dataId: {
      type: String,
      retuired: true,
      default: '',
    },
  });

  const { createMessage, createConfirm } = useMessage();
  const { L } = useLocalization(['AppPlatform', 'AbpUi']);
  const dataItems = ref<DataItem[]>([]);
  const [registerTable] = useTable({
    rowKey: 'id',
    title: L('Data:Items'),
    columns: getDataColumns(),
    dataSource: dataItems,
    bordered: true,
    canResize: true,
    showTableSetting: true,
    rowSelection: { type: 'checkbox' },
    actionColumn: {
      width: 160,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });
  const [registerModal, { openModal }] = useModal();

  const isEnableNew = computed(() => {
    if (props.dataId && props.dataId !== '') {
      return true;
    }
    return false;
  });

  watch(
    () => props.dataId,
    (dataId) => {
      dataItems.value = [];
      if (dataId) {
        fetchItems(dataId);
      }
    },
    {
      immediate: true,
    },
  );

  function fetchItems(dataId: string) {
    get(dataId).then((res) => {
      dataItems.value = res.items;
    });
  }

  function handleAppendItem() {
    openModal(true, { dataId: props.dataId });
  }

  function handleEdit(record) {
    openModal(true, { dataId: props.dataId, ...record });
  }

  function handleDelete(record: Recordable) {
    createConfirm({
      iconType: 'error',
      title: L('AreYouSure'),
      icon: createVNode(ExclamationCircleOutlined),
      content: createVNode(
        'div',
        { style: 'color:red;' },
        L('ItemWillBeDeletedMessageWithFormat', [record.displayName] as Recordable),
      ),
      onOk: () => {
        return removeItem(props.dataId!, record.name).then(() => {
          createMessage.success(L('SuccessfullyDeleted'));
          emits('reload');
          fetchItems(props.dataId!);
        });
      },
    });
  }
</script>
