<script setup lang="ts">
import type { FormInstance } from 'ant-design-vue/lib/form';

import type {
  BackgroundJobDefinitionDto,
  BackgroundJobInfoDto,
} from '../../types/job-infos';

import { reactive, ref, useTemplateRef } from 'vue';

import { useVbenDrawer } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { formatToDate } from '@abp/core';
import { PropertyTable } from '@abp/ui';
import {
  Checkbox,
  DatePicker,
  Form,
  FormItem,
  Input,
  InputNumber,
  message,
  Select,
  Tabs,
  Textarea,
} from 'ant-design-vue';

import { useJobInfosApi } from '../../api/useJobInfosApi';
import { JobPriority, JobType } from '../../types/job-infos';

defineOptions({
  name: 'JobInfoDrawer',
});
const emits = defineEmits<{
  (event: 'change', data: BackgroundJobInfoDto): void;
}>();
const TabPane = Tabs.TabPane;
interface JobParamter {
  key: string;
  value: any;
}

const activedTab = ref('basic');
const jobTypeOptions = reactive([
  {
    label: $t('BackgroundTasks.JobType:Once'),
    value: JobType.Once,
  },
  {
    label: $t('BackgroundTasks.JobType:Period'),
    value: JobType.Period,
  },
  {
    label: $t('BackgroundTasks.JobType:Persistent'),
    value: JobType.Persistent,
  },
]);
const jobPriorityOptions = reactive([
  {
    label: $t('BackgroundTasks.Priority:Low'),
    value: JobPriority.Low,
  },
  {
    label: $t('BackgroundTasks.Priority:BelowNormal'),
    value: JobPriority.BelowNormal,
  },
  {
    label: $t('BackgroundTasks.Priority:Normal'),
    value: JobPriority.Normal,
  },
  {
    label: $t('BackgroundTasks.Priority:AboveNormal'),
    value: JobPriority.AboveNormal,
  },
  {
    label: $t('BackgroundTasks.Priority:High'),
    value: JobPriority.High,
  },
]);
const jobDefinetions = ref<BackgroundJobDefinitionDto[]>([]);
const form = useTemplateRef<FormInstance>('form');
const formModel = ref<BackgroundJobInfoDto>({} as BackgroundJobInfoDto);
const { createApi, getApi, getDefinitionsApi, updateApi } = useJobInfosApi();
const [Drawer, drawerApi] = useVbenDrawer({
  class: 'w-auto',
  onCancel() {
    drawerApi.close();
  },
  onConfirm: async () => {
    await onSubmit();
  },
  onOpenChange: async (isOpen: boolean) => {
    if (isOpen) {
      try {
        formModel.value = {
          beginTime: formatToDate(new Date()),
          isEnabled: true,
          jobType: JobType.Once,
          priority: JobPriority.Normal,
        } as BackgroundJobInfoDto;
        drawerApi.setState({ loading: true });
        const dto = drawerApi.getData<BackgroundJobInfoDto>();
        if (dto?.id) {
          await onGet(dto.id);
        } else {
          await onInitDefaultJobs();
          drawerApi.setState({
            title: $t('TaskManagement.BackgroundJobs:AddNew'),
          });
        }
      } finally {
        drawerApi.setState({ loading: false });
      }
    }
  },
  title: $t('BackgroundJobs:Edit'),
});
async function onGet(id: string) {
  const dto = await getApi(id);
  formModel.value = dto;
  drawerApi.setState({
    title: `${$t('TaskManagement.BackgroundJobs:Edit')} - ${dto.name}`,
  });
}

async function onInitDefaultJobs() {
  const { items } = await getDefinitionsApi();
  jobDefinetions.value = items;
}

async function onSubmit() {
  await form.value?.validate();
  try {
    drawerApi.setState({ confirmLoading: true });
    const api = formModel.value?.id
      ? updateApi(formModel.value.id, formModel.value)
      : createApi(formModel.value);
    const dto = await api;
    message.success($t('AbpUi.SavedSuccessfully'));
    emits('change', dto);
    drawerApi.close();
  } finally {
    drawerApi.setState({ confirmLoading: false });
  }
}

function onJobDefineChange(jobName: string) {
  formModel.value.args = {};
  const jobDefine = jobDefinetions.value.find((x) => x.name === jobName);
  if (jobDefine) {
    const params: Record<string, any> = {};
    jobDefine.paramters.forEach((param) => {
      params[param.name] = undefined;
    });
    formModel.value.args = params;
    formModel.value.type = jobDefine.name;
  }
}

function onEditParam(record: JobParamter) {
  formModel.value.args[record.key] = record.value;
}

function onDeleteParam(record: JobParamter) {
  delete formModel.value.args[record.key];
}
</script>

<template>
  <Drawer>
    <div style="width: 800px">
      <Form
        ref="form"
        :model="formModel"
        :label-col="{ span: 4 }"
        :wrapper-col="{ span: 18 }"
      >
        <Tabs v-model="activedTab">
          <TabPane key="basic" :tab="$t('TaskManagement.BasicInfo')">
            <FormItem
              v-if="!formModel.id"
              :label="$t('TaskManagement.BackgroundJobs')"
              :help="$t('TaskManagement.BackgroundJobs')"
            >
              <Select
                class="w-full"
                allow-clear
                :options="jobDefinetions"
                :field-names="{ label: 'displayName', value: 'name' }"
                @change="(val) => onJobDefineChange(val!.toString())"
              />
            </FormItem>
            <FormItem
              name="isEnabled"
              :label="$t('TaskManagement.DisplayName:IsEnabled')"
            >
              <Checkbox v-model:checked="formModel.isEnabled">
                {{ $t('TaskManagement.DisplayName:IsEnabled') }}
              </Checkbox>
            </FormItem>
            <FormItem
              name="group"
              required
              :label="$t('TaskManagement.DisplayName:Group')"
            >
              <Input
                v-model:value="formModel.group"
                allow-clear
                autocomplete="off"
              />
            </FormItem>
            <FormItem
              name="name"
              required
              :label="$t('TaskManagement.DisplayName:Name')"
            >
              <Input
                v-model:value="formModel.name"
                allow-clear
                autocomplete="off"
              />
            </FormItem>
            <FormItem
              name="type"
              required
              :label="$t('TaskManagement.DisplayName:Type')"
              :extra="$t('TaskManagement.Description:Type')"
            >
              <Textarea
                allow-clear
                v-model:value="formModel.type"
                :auto-size="{ minRows: 3 }"
              />
            </FormItem>
            <FormItem
              name="beginTime"
              required
              :label="$t('TaskManagement.DisplayName:BeginTime')"
            >
              <DatePicker
                class="w-full"
                value-format="YYYY-MM-DD"
                allow-clear
                v-model:value="formModel.beginTime"
              />
            </FormItem>
            <FormItem
              name="endTime"
              :label="$t('TaskManagement.DisplayName:EndTime')"
            >
              <DatePicker
                class="w-full"
                value-format="YYYY-MM-DD"
                allow-clear
                v-model:value="formModel.endTime"
              />
            </FormItem>
            <FormItem
              name="jobType"
              :label="$t('TaskManagement.DisplayName:JobType')"
              :extra="$t('TaskManagement.Description:JobType')"
            >
              <Select
                class="w-full"
                allow-clear
                v-model:value="formModel.jobType"
                :options="jobTypeOptions"
              />
            </FormItem>
            <FormItem
              v-if="formModel.jobType === JobType.Period"
              name="cron"
              required
              :label="$t('TaskManagement.DisplayName:Cron')"
              :extra="$t('TaskManagement.Description:Cron')"
            >
              <Input allow-clear v-model:value="formModel.cron" />
            </FormItem>
            <FormItem
              name="maxCount"
              :label="$t('TaskManagement.DisplayName:MaxCount')"
              :extra="$t('TaskManagement.Description:MaxCount')"
            >
              <InputNumber
                class="w-full"
                allow-clear
                v-model:value="formModel.maxCount"
              />
            </FormItem>
            <FormItem
              name="maxTryCount"
              :label="$t('TaskManagement.DisplayName:MaxTryCount')"
              :extra="$t('TaskManagement.Description:MaxTryCount')"
            >
              <InputNumber
                class="w-full"
                allow-clear
                v-model:value="formModel.maxTryCount"
              />
            </FormItem>
            <FormItem
              name="priority"
              :label="$t('TaskManagement.DisplayName:Priority')"
              :extra="$t('TaskManagement.Description:Priority')"
            >
              <Select
                class="w-full"
                allow-clear
                v-model:value="formModel.priority"
                :options="jobPriorityOptions"
              />
            </FormItem>
            <FormItem
              name="lockTimeOut"
              :label="$t('TaskManagement.DisplayName:LockTimeOut')"
              :extra="$t('TaskManagement.Description:LockTimeOut')"
            >
              <InputNumber
                class="w-full"
                allow-clear
                v-model:value="formModel.lockTimeOut"
              />
            </FormItem>
            <FormItem
              name="description"
              :label="$t('TaskManagement.DisplayName:Description')"
            >
              <Textarea
                allow-clear
                v-model:value="formModel.description"
                :auto-size="{ minRows: 3 }"
              />
            </FormItem>
          </TabPane>
          <TabPane key="paramters" :tab="$t('TaskManagement.Paramters')">
            <PropertyTable
              :data="formModel.args"
              @change="onEditParam"
              @delete="onDeleteParam"
            />
          </TabPane>
        </Tabs>
      </Form>
    </div>
  </Drawer>
</template>

<style scoped></style>
