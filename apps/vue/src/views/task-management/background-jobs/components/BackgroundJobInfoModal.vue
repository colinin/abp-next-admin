<template>
  <BasicModal
    @register="registerModal"
    :width="800"
    :height="400"
    :title="modalTitle"
    :help-message="modalTips"
    :mask-closable="false"
    @ok="handleSubmit"
  >
    <Form
      ref="formElRef"
      :colon="false"
      label-align="left"
      layout="horizontal"
      :model="modelRef"
      :rules="modelRules"
    >
      <Tabs v-model:activeKey="activeKey">
        <TabPane key="basic" :tab="L('BasicInfo')">
          <FormItem name="isEnabled" :labelCol="{ span: 4 }" :wrapperCol="{ span: 18 }" :label="L('DisplayName:IsEnabled')">
            <Checkbox v-model:checked="modelRef.isEnabled">{{ L('DisplayName:IsEnabled') }}</Checkbox>
          </FormItem>
          <FormItem name="group" required :label="L('DisplayName:Group')">
            <Input :disabled="isEditModal" v-model:value="modelRef.group" autocomplete="off" />
          </FormItem>
          <FormItem name="name" required :label="L('DisplayName:Name')">
            <Input :disabled="isEditModal" v-model:value="modelRef.name" autocomplete="off" />
          </FormItem>
          <FormItem name="type" required :label="L('DisplayName:Type')" :extra="L('Description:Type')">
            <Textarea :disabled="isEditModal" v-model:value="modelRef.type" :auto-size="{ minRows: 3, maxRows: 6 }" />
          </FormItem>
          <FormItem name="beginTime" :label="L('DisplayName:BeginTime')">
            <DatePicker style="width: 100%;" v-model:value="modelRef.beginTime" />
          </FormItem>
          <FormItem name="endTime" :label="L('DisplayName:EndTime')">
            <DatePicker style="width: 100%;" v-model:value="modelRef.endTime" />
          </FormItem>
          <FormItem name="jobType" :label="L('DisplayName:JobType')" :help="L('Description:JobType')">
            <Select :options="jopTypeOptions" v-model:value="modelRef.jobType" :default-value="JobType.Once" />
          </FormItem>
          <FormItem name="cron" v-if="modelRef.jobType === JobType.Period" :label="L('DisplayName:Cron')" :help="L('Description:Cron')">
            <Input v-model:value="modelRef.cron" autocomplete="off" />
          </FormItem>
          <FormItem name="interval" v-if="modelRef.jobType !== JobType.Period" :label="L('DisplayName:Interval')" :help="L('Description:Interval')">
            <InputNumber style="width: 100%;" v-model:value="modelRef.interval" />
          </FormItem>
          <FormItem name="maxCount" :label="L('DisplayName:MaxCount')" :help="L('Description:MaxCount')">
            <InputNumber style="width: 100%;" v-model:value="modelRef.maxCount" />
          </FormItem>
          <FormItem name="maxTryCount" :label="L('DisplayName:MaxTryCount')" :help="L('Description:MaxTryCount')">
            <InputNumber style="width: 100%;" v-model:value="modelRef.maxTryCount" />
          </FormItem>
          <FormItem name="priority" :label="L('DisplayName:Priority')" :help="L('Description:Priority')">
            <Select :options="jobPriorityOptions" v-model:value="modelRef.priority" :default-value="JobPriority.Normal" />
          </FormItem>
          <FormItem name="lockTimeOut" :label="L('DisplayName:LockTimeOut')" :help="L('Description:LockTimeOut')">
            <InputNumber style="width: 100%;" v-model:value="modelRef.lockTimeOut" />
          </FormItem>
          <FormItem name="description" :label="L('DisplayName:Description')">
            <Textarea v-model:value="modelRef.description" :row="3" />
          </FormItem>
          <FormItem :label="L('DisplayName:Result')">
            <Textarea readonly v-model:value="modelRef.result" :auto-size="{ minRows: 6, maxRows: 12 }" />
          </FormItem>
        </TabPane>
        <TabPane key="paramters" :tab="L('Paramters')">
          <BasicTable @register="registerTable" :data-source="jobArgs">
            <template #toolbar>
              <Button type="primary" @click="handleAddNewArg">{{ L('BackgroundJobs:AddNewArg') }}
              </Button>
            </template>
            <template #action="{ record }">
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
          </BasicTable>
        </TabPane>
      </Tabs>
    </Form>
    <BasicModal
      :title="L('BackgroundJobs:Paramter')"
      @register="registerParamModal"
      @ok="handleSaveParam"
    >
      <BasicForm @register="registerParamForm" />
    </BasicModal>
  </BasicModal>
</template>

<script lang="ts" setup>
  import { computed, ref, reactive, unref, nextTick } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useValidation } from '/@/hooks/abp/useValidation';
  import { useMessage } from '/@/hooks/web/useMessage';
  import {
    Button,
    Checkbox,
    DatePicker,
    Form,
    Select,
    Tabs,
    Input,
    InputNumber,
    Textarea,
  } from 'ant-design-vue';
  import { BasicForm, useForm } from '/@/components/Form';
  import { BasicModal, useModal, useModalInner } from '/@/components/Modal';
  import { BasicTable, BasicColumn, TableAction, useTable } from '/@/components/Table';
  import { getById, create, update } from '/@/api/task-management/backgroundJobInfo';
  import { JobType, JobPriority, BackgroundJobInfo } from '/@/api/task-management/model/backgroundJobInfoModel';
  import { JobTypeMap, JobPriorityMap } from '../datas/typing';

  const FormItem = Form.Item;
  const TabPane = Tabs.TabPane;

  const emit = defineEmits(['change', 'register']);
  const { L } = useLocalization('TaskManagement');
  const { ruleCreator } = useValidation();
  const { createMessage } = useMessage();
  const formElRef = ref<any>();
  const activeKey = ref('basic');
  const modelRef = ref<BackgroundJobInfo>({
    id: '',
    isEnabled: true,
    priority: JobPriority.Normal,
    jobType: JobType.Once,
    args: {},
  } as BackgroundJobInfo);
  const [registerModal, { closeModal, changeOkLoading }] = useModalInner((model) => {
    activeKey.value = 'basic';
    fetchModel(model.id);
  });
  const [registerParamModal, { openModal: openParamModal, closeModal: closeParamModal }] = useModal();
  const [registerParamForm, { resetFields: resetParamFields, setFieldsValue: setParamFields, validate }] = useForm({
    labelAlign: 'left',
    labelWidth: 120,
    schemas: [
      {
        field: 'key',
        component: 'Input',
        label: L('DisplayName:Key'),
        required: true,
        colProps: { span: 24 },
        componentProps: {
           autocomplete: "off"
        },
      },
      {
        field: 'value',
        component: 'InputTextArea',
        label: L('DisplayName:Value'),
        required: true,
        colProps: { span: 24 },
      },
    ],
    showActionButtonGroup: false,
  });
  const columns: BasicColumn[] = [
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
  ];
  const [registerTable] = useTable({
    rowKey: 'key',
    columns: columns,
    pagination: false,
    maxHeight: 300,
    actionColumn: {
      width: 180,
      title: L('Actions'),
      dataIndex: 'action',
      slots: { customRender: 'action' },
    },
  });
  const isEditModal = computed(() => {
    if (modelRef.value.id) {
      return true;
    }
    return false;
  });
  const modalTitle = computed(() => {
    if (!isEditModal.value) {
      return L('BackgroundJobs:AddNew');
    }
    if (modelRef.value.isAbandoned) {
      return `${L('BackgroundJobs:Edit')} - ${L('DisplayName:IsAbandoned')}`;
    }
    return L('BackgroundJobs:Edit');
  });
  const modalTips = computed(() => {
    if (modelRef.value.isAbandoned) {
      return L('Description:IsAbandoned');
    }
    return '';
  });
  const modelRules = reactive({
    group: ruleCreator.fieldRequired({
      name: 'Group',
      resourceName: 'TaskManagement',
      prefix: 'DisplayName',
    }),
    name: ruleCreator.fieldRequired({
      name: 'Name',
      resourceName: 'TaskManagement',
      prefix: 'DisplayName',
    }),
    type: ruleCreator.fieldRequired({
      name: 'Type',
      resourceName: 'TaskManagement',
      prefix: 'DisplayName',
    }),
    beginTime: ruleCreator.fieldRequired({
      name: 'BeginTime',
      resourceName: 'TaskManagement',
      prefix: 'DisplayName',
      type: 'date',
    }),
  });
  const jopTypeOptions = reactive([
    { label: JobTypeMap[JobType.Once], value: JobType.Once },
    { label: JobTypeMap[JobType.Period], value: JobType.Period },
    { label: JobTypeMap[JobType.Persistent], value: JobType.Persistent },
  ]);
  const jobPriorityOptions = reactive([
    { label: JobPriorityMap[JobPriority.Low], value: JobPriority.Low },
    { label: JobPriorityMap[JobPriority.BelowNormal], value: JobPriority.BelowNormal },
    { label: JobPriorityMap[JobPriority.Normal], value: JobPriority.Normal },
    { label: JobPriorityMap[JobPriority.AboveNormal], value: JobPriority.AboveNormal },
    { label: JobPriorityMap[JobPriority.High], value: JobPriority.High },
  ]);
  const jobArgs = computed(() => {
    const model = unref(modelRef);
    if (!model.args) return [];
    return Object.keys(model.args).map((key) => {
      return {
        key: key,
        value: model.args[key],
      };
    });
  });

  function fetchModel(id: string) {
    if (!id) {
      resetFields();
      return;
    }
    getById(id).then((res) => {
      modelRef.value = res;
    });
  }

  function resetFields() {
    nextTick(() => {
      modelRef.value = {
        id: '',
        isEnabled: true,
        priority: JobPriority.Normal,
        jobType: JobType.Once,
        args: {},
      } as BackgroundJobInfo;
    });
  }

  function handleSubmit() {
    const formEl = unref(formElRef);
    formEl?.validate().then(() => {
      changeOkLoading(true);
      const model = unref(modelRef);
      const api = isEditModal.value
        ? update(model.id, Object.assign(model))
        : create(Object.assign(model));
      api.then(() => {
        createMessage.success(L('Successful'));
        formEl?.resetFields();
        closeModal();
        emit('change');
      }).finally(() => {
        changeOkLoading(false);
      });
    });
  }

  function handleAddNewArg() {
    openParamModal(true);
    nextTick(() => {
      resetParamFields();
    });
  }

  function handleSaveParam() {
    validate().then((input) => {
      const model = unref(modelRef);
      model.args ??= {};
      model.args[input.key] = input.value;
      resetParamFields();
      closeParamModal();
    });
  }

  function handleEditParam(record) {
    openParamModal(true);
    nextTick(() => {
      setParamFields(record);
    });
  }

  function handleDeleteParam(record) {
    const model = unref(modelRef);
    model.args ??= {};
    delete model.args[record.key]
  }
</script>
