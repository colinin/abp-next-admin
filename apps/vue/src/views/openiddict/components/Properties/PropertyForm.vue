<template>
  <Card :title="L('Propertites')">
    <BasicTable @register="registerTable" :data-source="dataSource">
      <template #toolbar>
        <Button type="primary" @click="handleAddNew">{{ L('Propertites:New') }}</Button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'action'">
          <TableAction
            :actions="[
              {
                color: 'error',
                icon: 'ant-design:delete-outlined',
                label: L('Delete'),
                onClick: handleDelete.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <BasicModal v-bind="$attrs" @register="registerModal" @ok="handleSubmit" :title="title">
      <BasicForm @register="registerForm" />
    </BasicModal>
  </Card>
</template>

<script lang="ts" setup>
  import { computed, ref } from 'vue';
  import { Button, Card } from 'ant-design-vue';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModal } from '/@/components/Modal';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { useLocalization } from '/@/hooks/abp/useLocalization';

  const emits = defineEmits(['create', 'delete']);
  const props = defineProps({
    properties: {
      type: Object as PropType<Dictionary<string, string>>,
      default: () => [],
    },
  });

  const { L } = useLocalization(['AbpOpenIddict', 'AbpUi']);
  const title = ref('');
  const [registerForm, { validate, resetFields }] = useForm({
    labelWidth: 120,
    showActionButtonGroup: false,
    schemas: [
      {
        field: 'key',
        component: 'Input',
        label: L('Propertites:Key'),
        colProps: { span: 24 },
        required: true,
      },
      {
        field: 'value',
        component: 'Input',
        label: L('Propertites:Value'),
        colProps: { span: 24 },
        required: true,
      },
    ],
  });
  const dataSource = computed(() => {
    if (!props.properties) return [];
    return Object.keys(props.properties).map((key) => {
      return {
        key: key,
        value: props.properties[key],
      };
    });
  });
  const [registerTable] = useTable({
    rowKey: 'key',
    columns: [
      {
        title: L('Propertites:Key'),
        dataIndex: 'key',
        align: 'left',
        width: 180,
        sorter: true,
      },
      {
        title: L('Propertites:Value'),
        dataIndex: 'value',
        align: 'left',
        width: 180,
        sorter: true,
      },
    ],
    pagination: false,
    showTableSetting: true,
    maxHeight: 230,
    actionColumn: {
      width: 100,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });
  const [registerModal, { openModal, closeModal }] = useModal();

  function handleAddNew() {
    title.value = L('Propertites:New');
    openModal(true);
  }

  function handleDelete(record) {
    emits('delete', record);
  }

  function handleSubmit() {
    validate().then((input) => {
      emits('create', input);
      resetFields();
      closeModal();
    });
  }
</script>
