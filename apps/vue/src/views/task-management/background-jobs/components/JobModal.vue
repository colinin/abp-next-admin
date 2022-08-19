<template>
  <BasicModal
    @register="registerModal"
    :width="1000"
    :height="600"
    :title="modalTitle"
    :help-message="modalTips"
    :mask-closable="false"
    @ok="handleSubmit"
  >
    <Form
      ref="formElRef"
      :colon="false"
      label-align="left"
      layout="vertical"
      :model="modelRef"
      :rules="modelRules"
    >
      <Tabs
        v-model:activeKey="activeKey"
        :style="tabsStyle.style"
        :tabBarStyle="tabsStyle.tabBarStyle"
      >
        <TabPane key="basic" :tab="L('BasicInfo')">
          <FormItem
            name="isEnabled"
            :labelCol="{ span: 4 }"
            :wrapperCol="{ span: 18 }"
            :label="L('DisplayName:IsEnabled')"
          >
            <Checkbox v-model:checked="modelRef.isEnabled">{{
              L('DisplayName:IsEnabled')
            }}</Checkbox>
          </FormItem>
          <FormItem name="group" required :label="L('DisplayName:Group')">
            <Input :disabled="isEditModal" v-model:value="modelRef.group" autocomplete="off" />
          </FormItem>
          <FormItem name="name" required :label="L('DisplayName:Name')">
            <Input :disabled="isEditModal" v-model:value="modelRef.name" autocomplete="off" />
          </FormItem>
          <FormItem
            name="type"
            required
            :label="L('DisplayName:Type')"
            :extra="L('Description:Type')"
          >
            <TextArea
              :disabled="isEditModal"
              v-model:value="modelRef.type"
              :auto-size="{ minRows: 3, maxRows: 6 }"
            />
          </FormItem>
          <FormItem name="beginTime" :label="L('DisplayName:BeginTime')">
            <DatePicker
              style="width: 100%"
              :value="getDate('beginTime')"
              @change="(val) => dateChange('beginTime', val, 'YYYY-MM-DD 00:00:00')"
            />
          </FormItem>
          <FormItem name="endTime" :label="L('DisplayName:EndTime')">
            <DatePicker
              style="width: 100%"
              :value="getDate('endTime')"
              @change="(val) => dateChange('endTime', val, 'YYYY-MM-DD 23:59:59')"
            />
          </FormItem>
          <FormItem
            name="jobType"
            :label="L('DisplayName:JobType')"
            :help="L('Description:JobType')"
          >
            <Select
              :options="jopTypeOptions"
              v-model:value="modelRef.jobType"
              :default-value="JobType.Once"
            />
          </FormItem>
          <FormItem
            name="cron"
            v-if="modelRef.jobType === JobType.Period"
            :label="L('DisplayName:Cron')"
            :help="L('Description:Cron')"
          >
            <Input v-model:value="modelRef.cron" autocomplete="off" />
          </FormItem>
          <FormItem
            name="interval"
            v-if="modelRef.jobType !== JobType.Period"
            :label="L('DisplayName:Interval')"
            :help="L('Description:Interval')"
          >
            <InputNumber style="width: 100%" v-model:value="modelRef.interval" />
          </FormItem>
          <FormItem
            name="maxCount"
            :label="L('DisplayName:MaxCount')"
            :help="L('Description:MaxCount')"
          >
            <InputNumber style="width: 100%" v-model:value="modelRef.maxCount" />
          </FormItem>
          <FormItem
            name="maxTryCount"
            :label="L('DisplayName:MaxTryCount')"
            :help="L('Description:MaxTryCount')"
          >
            <InputNumber style="width: 100%" v-model:value="modelRef.maxTryCount" />
          </FormItem>
          <FormItem
            name="priority"
            :label="L('DisplayName:Priority')"
            :help="L('Description:Priority')"
          >
            <Select
              :options="jobPriorityOptions"
              v-model:value="modelRef.priority"
              :default-value="JobPriority.Normal"
            />
          </FormItem>
          <FormItem
            name="lockTimeOut"
            :label="L('DisplayName:LockTimeOut')"
            :help="L('Description:LockTimeOut')"
          >
            <InputNumber style="width: 100%" v-model:value="modelRef.lockTimeOut" />
          </FormItem>
          <FormItem name="description" :label="L('DisplayName:Description')">
            <TextArea v-model:value="modelRef.description" :row="3" />
          </FormItem>
          <FormItem :label="L('DisplayName:Result')">
            <TextArea
              readonly
              v-model:value="modelRef.result"
              :auto-size="{ minRows: 6, maxRows: 12 }"
            />
          </FormItem>
        </TabPane>
        <TabPane key="paramters" :tab="L('Paramters')">
          <JobParamter :isEditModal="isEditModal" :args="modelRef.args" @args-reset="handleArgsChange" />
        </TabPane>
        <TabPane v-if="isEditModal" key="actions" :tab="L('Job:Actions')">
          <JobAction :jobId="modelRef.id" />
        </TabPane>
      </Tabs>
    </Form>
  </BasicModal>
</template>

<script lang="ts" setup>
  import dayjs from 'dayjs';
  import { computed, ref, reactive, unref } from 'vue';
  import { useTabsStyle } from '/@/hooks/component/useStyles';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { useValidation } from '/@/hooks/abp/useValidation';
  import { useMessage } from '/@/hooks/web/useMessage';
  import {
    Checkbox,
    DatePicker,
    Form,
    Select,
    Tabs,
    Input,
    InputNumber,
  } from 'ant-design-vue';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { getById, create, update } from '/@/api/task-management/backgroundJobInfo';
  import {
    JobType,
    JobPriority,
    JobSource,
    BackgroundJobInfo,
  } from '/@/api/task-management/model/backgroundJobInfoModel';
  import { JobTypeMap, JobPriorityMap } from '../datas/typing';
  import { formatToDate } from '/@/utils/dateUtil';
  import JobAction from './JobAction.vue';
  import JobParamter from './JobParamter.vue';

  const FormItem = Form.Item;
  const TabPane = Tabs.TabPane;
  const TextArea = Input.TextArea;

  const emit = defineEmits(['change', 'register']);
  const { L } = useLocalization(['TaskManagement', 'AbpUi']);
  const modelRef = ref<BackgroundJobInfo>(getDefaultModel());
  const { ruleCreator } = useValidation();
  const { createMessage } = useMessage();
  const copyJob = ref(false);
  const formElRef = ref<any>();
  const activeKey = ref('basic');
  const tabsStyle = useTabsStyle();
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
  const getDate = computed(() => {
    return (field: string) => {
      const model = unref(modelRef);
      if (model[field]) {
        return dayjs(model[field], 'YYYY-MM-DD');
      }
      return undefined;
    };
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
  const [registerModal, { closeModal, changeOkLoading }] = useModalInner((model) => {
    activeKey.value = 'basic';
    copyJob.value = model.copy ?? false;
    fetchModel(model.id);
  });

  function fetchModel(id: string) {
    if (!id) {
      modelRef.value = getDefaultModel();
      return;
    }
    getById(id).then((res) => {
      if (copyJob.value) {
        res.id = '';
        res.name = '';
        res.source = JobSource.User;
      }
      modelRef.value = res;
    });
  }

  function dateChange(field: string, value: string | dayjs.Dayjs, format?: string) {
    const model = unref(modelRef);
    model[field] = formatToDate(value, format);
  }

  function handleSubmit() {
    const formEl = unref(formElRef);
    formEl?.validate().then(() => {
      changeOkLoading(true);
      const model = unref(modelRef);
      const api = isEditModal.value
        ? update(model.id, model)
        : create(model);
      api
        .then(() => {
          createMessage.success(L('Successful'));
          formEl?.resetFields();
          closeModal();
          emit('change');
        })
        .finally(() => {
          changeOkLoading(false);
        });
    });
  }

  function handleArgsChange(args: ExtraPropertyDictionary) {
    const model = unref(modelRef);
    model.args = args;
  }

  function getDefaultModel() {
    return {
      id: '',
      isEnabled: true,
      priority: JobPriority.Normal,
      jobType: JobType.Once,
      source: JobSource.User,
      maxCount: 0,
      maxTryCount: 0,
      args: {},
    } as BackgroundJobInfo;
  }

</script>
