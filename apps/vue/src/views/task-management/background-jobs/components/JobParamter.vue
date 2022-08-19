<template>
  <div>
    <FormItem v-if="!isEditModal" :label="L('BackgroundJobs')">
      <Select :options="getJobDefinitionOptions" @change="handleJobChange" :allow-clear="true" />
    </FormItem>
    <BasicTable @register="registerTable" :data-source="getArgs">
      <template #toolbar>
        <Button type="primary" @click="handleAddNewArg"
          >{{ L('BackgroundJobs:AddNewArg') }}
        </Button>
      </template>
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'action'">
          <TableAction
            :stop-button-propagation="true"
            :actions="[
              {
                label: L('Edit'),
                icon: 'ant-design:edit-outlined',
                onClick: handleEditParam.bind(null, record),
              },
              {
                color: 'error',
                label: L('Delete'),
                icon: 'ant-design:delete-outlined',
                onClick: handleDeleteParam.bind(null, record),
              },
            ]"
          />
        </template>
      </template>
    </BasicTable>
    <BasicModal
      :title="L('BackgroundJobs:Paramter')"
      @register="registerParamModal"
      @ok="handleSaveParam"
    >
      <BasicForm @register="registerParamForm" />
    </BasicModal>
  </div>
</template>

<script lang="ts" setup>
  import { computed, ref, nextTick, onMounted } from 'vue';
  import { Button, Form, Select } from 'ant-design-vue';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModal } from '/@/components/Modal';
  import { BasicTable, TableAction, useTable } from '/@/components/Table';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { getDefinitions } from '/@/api/task-management/backgroundJobInfo';
  import { BackgroundJobDefinition } from '/@/api/task-management/model/backgroundJobInfoModel';

  const FormItem = Form.Item;

  const props = defineProps({
    isEditModal: {
      type: Boolean,
      default: false,
    },
    args: {
      type: Object as PropType<ExtraPropertyDictionary>,
      default: {},
    }
  });
  const emits = defineEmits(['args-reset']);
  const { L } = useLocalization(['TaskManagement', 'AbpUi']);
  const jobDefinitions = ref<BackgroundJobDefinition[]>([]);
  const getJobDefinitionOptions = computed(() => {
    return jobDefinitions.value.map((job) => {
      return {
        label: job.displayName,
        value: job.name,
        paramters: job.paramters,
      };
    });
  });
  const [registerTable] = useTable({
    rowKey: 'key',
    columns: [
      {
        title: L('DisplayName:Key'),
        dataIndex: 'key',
        align: 'left',
        width: 200,
        sorter: true,
      },
      {
        title: L('DisplayName:Value'),
        dataIndex: 'value',
        align: 'left',
        width: 300,
        sorter: true,
      },
    ],
    pagination: false,
    actionColumn: {
      width: 180,
      title: L('Actions'),
      dataIndex: 'action',
    },
  });
  const [registerParamModal, { openModal: openParamModal, closeModal: closeParamModal }] =
    useModal();
  const [
    registerParamForm,
    { resetFields: resetParamFields, setFieldsValue: setParamFields, validate },
  ] = useForm({
    labelAlign: 'left',
    labelWidth: 120,
    schemas: [
      {
        field: 'oldKey',
        component: 'Input',
        label: 'oldKey',
        show: false,
        colProps: { span: 24 },
      },
      {
        field: 'key',
        component: 'Input',
        label: L('DisplayName:Key'),
        required: true,
        colProps: { span: 24 },
        componentProps: {
          autocomplete: 'off',
        },
      },
      {
        field: 'value',
        component: 'InputTextArea',
        label: L('DisplayName:Value'),
        required: true,
        colProps: { span: 24 },
        componentProps: {
          autoSize: {
            minRows: 5,
          },
        },
      },
    ],
    showActionButtonGroup: false,
  });
  const getArgs = computed(() => {
    if (!props.args) return [];
    return Object.keys(props.args).map((key) => {
      return {
        key: key,
        value: props.args[key],
      };
    });
  });

  onMounted(fetchDefinitionJobs);

  function fetchDefinitionJobs() {
    getDefinitions().then((res) => {
      jobDefinitions.value = res.items;
    });
  }

  function handleJobChange(key, job: BackgroundJobDefinition) {
    if (key) {
      const args: ExtraPropertyDictionary = {};
      job.paramters.forEach((p) => {
        args[p.name] = '';
      });
      emits('args-reset', args);
    }
  }

  function handleAddNewArg() {
    openParamModal(true);
    nextTick(() => {
      resetParamFields();
    });
  }

  function handleSaveParam() {
    validate().then((input) => {
      const args = props.args;
      if (input.oldKey && input.key !== input.oldKey) {
        delete args[input.oldKey];
      }
      args[input.key] = input.value;
      emits('args-reset', args);
      resetParamFields();
      closeParamModal();
    });
  }

  function handleEditParam(record) {
    openParamModal(true);
    nextTick(() => {
      setParamFields(Object.assign({ oldKey: record.key }, record));
    });
  }

  function handleDeleteParam(record) {
    const args = props.args;
    delete args[record.key];
    emits('args-reset', args);
  }
</script>
