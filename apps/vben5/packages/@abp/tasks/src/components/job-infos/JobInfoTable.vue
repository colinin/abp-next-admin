<script setup lang="ts">
import type { SortOrder } from '@abp/core';
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface';

import type { VbenFormProps } from '@vben/common-ui';

import type { BackgroundJobInfoDto } from '../../types/job-infos';

import { defineAsyncComponent, h, ref } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenDrawer } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { formatToDateTime } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  EllipsisOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import {
  Button,
  Dropdown,
  Menu,
  MenuItem,
  message,
  Modal,
  Tag,
} from 'ant-design-vue';

import { useJobInfosApi } from '../../api/useJobInfosApi';
import { BackgroundJobsPermissions } from '../../constants/permissions';
import { useJobEnumsMap } from '../../hooks/useJobEnumsMap';
import { JobSource, JobStatus, JobType } from '../../types/job-infos';

defineOptions({
  name: 'GdprTable',
});

const PauseIcon = createIconifyIcon('material-symbols:pause-outline');
const ResumeIcon = createIconifyIcon('material-symbols:resume-outline');
const StartIcon = createIconifyIcon('solar:restart-circle-outline');
const StopIcon = createIconifyIcon('solar:stop-circle-outline');
const TriggerIcon = createIconifyIcon('grommet-icons:trigger');

const allowPauseStatus = [
  JobStatus.Queuing,
  JobStatus.Running,
  JobStatus.FailedRetry,
];
const allowResumeStatus = [JobStatus.Paused, JobStatus.Stopped];
const allowTriggerStatus = [
  JobStatus.Queuing,
  JobStatus.Running,
  JobStatus.Completed,
  JobStatus.FailedRetry,
];
const allowStartStatus = [
  JobStatus.None,
  JobStatus.Stopped,
  JobStatus.FailedRetry,
];
const allowStopStatus = [JobStatus.Queuing, JobStatus.Running];

const selectedKeys = ref<string[]>([]);

const { hasAccessByCodes } = useAccess();
const {
  bulkDeleteApi,
  bulkStartApi,
  bulkStopApi,
  cancel,
  deleteApi,
  getPagedListApi,
  pauseApi,
  resumeApi,
  startApi,
  stopApi,
  triggerApi,
} = useJobInfosApi();
const {
  jobPriorityColor,
  jobPriorityMap,
  jobSourceMap,
  jobStatusColor,
  jobStatusMap,
  jobTypeMap,
} = useJobEnumsMap();

const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: true,
  collapsedRows: 2,
  fieldMappingTime: [
    [
      'time',
      ['beginTime', 'endTime'],
      ['YYYY-MM-DD HH:mm:ss', 'YYYY-MM-DD HH:mm:ss'],
    ],
    [
      'lastRunTime',
      ['beginLastRunTime', 'endLastRunTime'],
      ['YYYY-MM-DD HH:mm:ss', 'YYYY-MM-DD HH:mm:ss'],
    ],
  ],
  schema: [
    {
      component: 'Input',
      fieldName: 'group',
      label: $t('TaskManagement.DisplayName:Group'),
    },
    {
      component: 'Input',
      fieldName: 'name',
      label: $t('TaskManagement.DisplayName:Name'),
    },
    {
      component: 'RangePicker',
      componentProps: {
        showTime: true,
      },
      fieldName: 'lastRunTime',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('TaskManagement.DisplayName:LastRunTime'),
    },
    {
      component: 'Select',
      componentProps: {
        allowClear: true,
        options: [
          {
            label: jobStatusMap[JobStatus.None],
            value: JobStatus.None,
          },
          {
            label: jobStatusMap[JobStatus.Running],
            value: JobStatus.Running,
          },
          {
            label: jobStatusMap[JobStatus.Completed],
            value: JobStatus.Completed,
          },
          {
            label: jobStatusMap[JobStatus.FailedRetry],
            value: JobStatus.FailedRetry,
          },
          {
            label: jobStatusMap[JobStatus.Paused],
            value: JobStatus.Paused,
          },
          {
            label: jobStatusMap[JobStatus.Queuing],
            value: JobStatus.Queuing,
          },
          {
            label: jobStatusMap[JobStatus.Stopped],
            value: JobStatus.Stopped,
          },
        ],
      },
      defaultValue: JobStatus.Running,
      fieldName: 'status',
      label: $t('TaskManagement.DisplayName:Status'),
    },
    {
      component: 'Select',
      componentProps: {
        allowClear: true,
        options: [
          {
            label: jobSourceMap[JobSource.None],
            value: JobSource.None,
          },
          {
            label: jobSourceMap[JobSource.User],
            value: JobSource.User,
          },
          {
            label: jobSourceMap[JobSource.System],
            value: JobSource.System,
          },
        ],
      },
      defaultValue: JobSource.User,
      fieldName: 'source',
      label: $t('TaskManagement.DisplayName:Source'),
    },
    {
      component: 'RangePicker',
      componentProps: {
        showTime: true,
      },
      fieldName: 'time',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('TaskManagement.DisplayName:BeginTime'),
    },
    {
      component: 'Select',
      componentProps: {
        allowClear: true,
        options: [
          {
            label: jobTypeMap[JobType.Once],
            value: JobType.Once,
          },
          {
            label: jobTypeMap[JobType.Period],
            value: JobType.Period,
          },
          {
            label: jobTypeMap[JobType.Persistent],
            value: JobType.Persistent,
          },
        ],
      },
      fieldName: 'jobType',
      label: $t('TaskManagement.DisplayName:JobType'),
    },
    {
      component: 'Input',
      componentProps: {
        allowClear: true,
      },
      fieldName: 'filter',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('AbpUi.Search'),
    },
    {
      component: 'Checkbox',
      componentProps: {
        render: () => {
          return h('span', $t('TaskManagement.DisplayName:IsAbandoned'));
        },
      },
      fieldName: 'isAbandoned',
      label: $t('TaskManagement.DisplayName:IsAbandoned'),
    },
  ],
  // 控制表单是否显示折叠按钮
  showCollapseButton: true,
  submitOnChange: true,
  // 按下回车时是否提交表单
  submitOnEnter: true,
  wrapperClass: 'grid-cols-4',
};
const gridOptions: VxeGridProps<BackgroundJobInfoDto> = {
  columns: [
    {
      align: 'center',
      fixed: 'left',
      type: 'checkbox',
    },
    {
      align: 'left',
      field: 'group',
      fixed: 'left',
      sortable: true,
      title: $t('TaskManagement.DisplayName:Group'),
      width: 150,
    },
    {
      align: 'left',
      field: 'name',
      fixed: 'left',
      slots: { default: 'name' },
      sortable: true,
      title: $t('TaskManagement.DisplayName:Name'),
      width: 300,
    },
    {
      align: 'left',
      field: 'description',
      sortable: true,
      title: $t('TaskManagement.DisplayName:Description'),
      width: 350,
    },
    {
      align: 'left',
      field: 'creationTime',
      formatter({ cellValue }) {
        return cellValue ? formatToDateTime(cellValue) : cellValue;
      },
      sortable: true,
      title: $t('TaskManagement.DisplayName:CreationTime'),
      width: 150,
    },
    {
      align: 'left',
      field: 'status',
      slots: { default: 'status' },
      sortable: true,
      title: $t('TaskManagement.DisplayName:Status'),
      width: 100,
    },
    {
      align: 'left',
      field: 'result',
      sortable: true,
      title: $t('TaskManagement.DisplayName:Result'),
      width: 200,
    },
    {
      align: 'left',
      field: 'lastRunTime',
      formatter({ cellValue }) {
        return cellValue ? formatToDateTime(cellValue) : cellValue;
      },
      sortable: true,
      title: $t('TaskManagement.DisplayName:LastRunTime'),
      width: 150,
    },
    {
      align: 'left',
      field: 'nextRunTime',
      formatter({ cellValue }) {
        return cellValue ? formatToDateTime(cellValue) : cellValue;
      },
      sortable: true,
      title: $t('TaskManagement.DisplayName:NextRunTime'),
      width: 150,
    },
    {
      align: 'left',
      field: 'jobType',
      slots: { default: 'type' },
      sortable: true,
      title: $t('TaskManagement.DisplayName:JobType'),
      width: 150,
    },
    {
      align: 'left',
      field: 'priority',
      slots: { default: 'priority' },
      sortable: true,
      title: $t('TaskManagement.DisplayName:Priority'),
      width: 150,
    },
    {
      align: 'left',
      field: 'cron',
      sortable: true,
      title: $t('TaskManagement.DisplayName:Cron'),
      width: 150,
    },
    {
      align: 'left',
      field: 'triggerCount',
      sortable: true,
      title: $t('TaskManagement.DisplayName:TriggerCount'),
      width: 100,
    },
    {
      align: 'left',
      field: 'tryCount',
      sortable: true,
      title: $t('TaskManagement.DisplayName:TryCount'),
      width: 150,
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      width: 220,
    },
  ],
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page }, formValues) => {
        return await getPagedListApi({
          maxResultCount: page.pageSize,
          skipCount: (page.currentPage - 1) * page.pageSize,
          ...formValues,
        });
      },
    },
    response: {
      total: 'totalCount',
      list: 'items',
    },
  },
  toolbarConfig: {
    refresh: true,
  },
};
const gridEvents: VxeGridListeners<BackgroundJobInfoDto> = {
  checkboxAll: (params) => {
    selectedKeys.value = params.records.map((x) => x.id);
  },
  checkboxChange: (params) => {
    selectedKeys.value = params.records.map((x) => x.id);
  },
  sortChange: onSort,
};
const [JobInfoDrawer, jobDrawerApi] = useVbenDrawer({
  connectedComponent: defineAsyncComponent(() => import('./JobInfoDrawer.vue')),
});
const [JobInfoDetailDrawer, jobDetailDrawerApi] = useVbenDrawer({
  connectedComponent: defineAsyncComponent(
    () => import('./JobInfoDetailDrawer.vue'),
  ),
});
const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

function onSort(params: { field: string; order: SortOrder }) {
  const sorting = params.order ? `${params.field} ${params.order}` : undefined;
  gridApi.query({ sorting });
}

function onShow(row: BackgroundJobInfoDto) {
  jobDetailDrawerApi.setData(row);
  jobDetailDrawerApi.open();
}

function onCreate() {
  jobDrawerApi.setData({});
  jobDrawerApi.open();
}

function onEdit(row: BackgroundJobInfoDto) {
  jobDrawerApi.setData(row);
  jobDrawerApi.open();
}

function onDelete(row: BackgroundJobInfoDto) {
  Modal.confirm({
    centered: true,
    content: $t('AbpUi.ItemWillBeDeletedMessage'),
    onCancel: () => {
      cancel();
    },
    onOk: async () => {
      gridApi.setLoading(true);
      try {
        await deleteApi(row.id);
        message.success($t('AbpUi.DeletedSuccessfully'));
        await gridApi.query();
      } finally {
        gridApi.setLoading(false);
      }
    },
    title: $t('AbpUi.AreYouSure'),
  });
}

function onBulkDelete() {
  Modal.confirm({
    centered: true,
    content: $t('TaskManagement.MultipleSelectJobsWillBeDeletedMessage'),
    async onOk() {
      gridApi.setLoading(true);
      try {
        await bulkDeleteApi({
          jobIds: selectedKeys.value,
        });
        message.success($t('AbpUi.SavedSuccessfully'));
        await gridApi.query();
      } finally {
        gridApi.setLoading(false);
      }
    },
    title: $t('AbpUi.AreYouSure'),
  });
}

function onBulkStop() {
  Modal.confirm({
    centered: true,
    content: $t('TaskManagement.SelectJobWillBeStopMessage'),
    async onOk() {
      gridApi.setLoading(true);
      try {
        await bulkStopApi({
          jobIds: selectedKeys.value,
        });
        message.success($t('AbpUi.SavedSuccessfully'));
        await gridApi.query();
      } finally {
        gridApi.setLoading(false);
      }
    },
    title: $t('AbpUi.AreYouSure'),
  });
}

function onBulkStart() {
  Modal.confirm({
    centered: true,
    content: $t('TaskManagement.SelectJobWillBeStartMessage'),
    async onOk() {
      gridApi.setLoading(true);
      try {
        await bulkStartApi({
          jobIds: selectedKeys.value,
        });
        message.success($t('AbpUi.SavedSuccessfully'));
        await gridApi.query();
      } finally {
        gridApi.setLoading(false);
      }
    },
    title: $t('AbpUi.AreYouSure'),
  });
}

function onMenuClick(row: BackgroundJobInfoDto, info: MenuInfo) {
  switch (info.key) {
    case 'pause': {
      onJobStatusChange(
        row,
        pauseApi,
        $t('TaskManagement.SelectJobWillBePauseMessage'),
      );
      break;
    }
    case 'resume': {
      onJobStatusChange(
        row,
        resumeApi,
        $t('TaskManagement.SelectJobWillBeResumeMessage'),
      );
      break;
    }
    case 'start': {
      onJobStatusChange(
        row,
        startApi,
        $t('TaskManagement.SelectJobWillBeStartMessage'),
      );
      break;
    }
    case 'stop': {
      onJobStatusChange(
        row,
        stopApi,
        $t('TaskManagement.SelectJobWillBeStopMessage'),
      );
      break;
    }
    case 'trigger': {
      onJobStatusChange(
        row,
        triggerApi,
        $t('TaskManagement.SelectJobWillBeTriggerMessage'),
      );
      break;
    }
  }
}

function onJobStatusChange(
  job: BackgroundJobInfoDto,
  api: (jobId: string) => Promise<void>,
  warningMsg: string,
) {
  Modal.confirm({
    centered: true,
    content: warningMsg,
    async onOk() {
      gridApi.setLoading(true);
      try {
        await api(job.id);
        message.success($t('AbpUi.SavedSuccessfully'));
        await gridApi.query();
      } finally {
        gridApi.setLoading(false);
      }
    },
    title: $t('AbpUi.AreYouSure'),
  });
}
</script>

<template>
  <Grid :table-title="$t('TaskManagement.BackgroundJobs')">
    <template #toolbar-tools>
      <div class="flex flex-row gap-4">
        <Button
          v-if="selectedKeys.length > 0"
          :icon="h(StartIcon)"
          ghost
          type="primary"
          v-access:code="[BackgroundJobsPermissions.Start]"
          @click="onBulkStart"
        >
          {{ $t('TaskManagement.BackgroundJobs:Start') }}
        </Button>
        <Button
          v-if="selectedKeys.length > 0"
          :icon="h(StopIcon)"
          danger
          ghost
          type="primary"
          v-access:code="[BackgroundJobsPermissions.Stop]"
          @click="onBulkStop"
        >
          {{ $t('TaskManagement.BackgroundJobs:Pause') }}
        </Button>
        <Button
          :icon="h(PlusOutlined)"
          type="primary"
          v-access:code="[BackgroundJobsPermissions.Create]"
          @click="onCreate"
        >
          {{ $t('TaskManagement.BackgroundJobs:AddNew') }}
        </Button>
        <Button
          v-if="selectedKeys.length > 0"
          :icon="h(DeleteOutlined)"
          danger
          type="primary"
          v-access:code="[BackgroundJobsPermissions.Delete]"
          @click="onBulkDelete"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
      </div>
    </template>
    <template #name="{ row }">
      <Button type="link" @click="onShow(row)">{{ row.name }}</Button>
    </template>
    <template #status="{ row }">
      <Tag :color="jobStatusColor[row.status]">
        {{ jobStatusMap[row.status] }}
      </Tag>
    </template>
    <template #type="{ row }">
      <Tag color="blue">
        {{ jobTypeMap[row.jobType] }}
      </Tag>
    </template>
    <template #priority="{ row }">
      <Tag :color="jobPriorityColor[row.priority]">
        {{ jobPriorityMap[row.priority] }}
      </Tag>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          v-if="hasAccessByCodes([BackgroundJobsPermissions.Update])"
          :icon="h(EditOutlined)"
          block
          type="link"
          @click="onEdit(row)"
        >
          {{ $t('AbpUi.Edit') }}
        </Button>
        <Button
          v-if="
            hasAccessByCodes([BackgroundJobsPermissions.Delete]) &&
            (row.source !== JobSource.System ||
              hasAccessByCodes([BackgroundJobsPermissions.ManageSystemJobs]))
          "
          :icon="h(DeleteOutlined)"
          block
          danger
          type="link"
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
        <Dropdown>
          <template #overlay>
            <Menu @click="(info) => onMenuClick(row, info)">
              <MenuItem
                v-if="
                  allowPauseStatus.includes(row.status) &&
                  hasAccessByCodes([BackgroundJobsPermissions.Pause])
                "
                key="pause"
                :icon="h(PauseIcon)"
              >
                {{ $t('TaskManagement.BackgroundJobs:Pause') }}
              </MenuItem>
              <MenuItem
                v-if="
                  allowResumeStatus.includes(row.status) &&
                  hasAccessByCodes([BackgroundJobsPermissions.Resume])
                "
                key="resume"
                :icon="h(ResumeIcon)"
              >
                {{ $t('TaskManagement.BackgroundJobs:Resume') }}
              </MenuItem>
              <MenuItem
                v-if="
                  allowTriggerStatus.includes(row.status) &&
                  hasAccessByCodes([BackgroundJobsPermissions.Trigger])
                "
                key="trigger"
                :icon="h(TriggerIcon)"
              >
                {{ $t('TaskManagement.BackgroundJobs:Trigger') }}
              </MenuItem>
              <MenuItem
                v-if="
                  allowStartStatus.includes(row.status) &&
                  hasAccessByCodes([BackgroundJobsPermissions.Start])
                "
                key="start"
                :icon="h(StartIcon)"
              >
                {{ $t('TaskManagement.BackgroundJobs:Start') }}
              </MenuItem>
              <MenuItem
                v-if="
                  allowStopStatus.includes(row.status) &&
                  hasAccessByCodes([BackgroundJobsPermissions.Stop])
                "
                key="stop"
                :icon="h(StopIcon)"
              >
                {{ $t('TaskManagement.BackgroundJobs:Stop') }}
              </MenuItem>
            </Menu>
          </template>
          <Button :icon="h(EllipsisOutlined)" type="link" />
        </Dropdown>
      </div>
    </template>
  </Grid>
  <JobInfoDrawer />
  <JobInfoDetailDrawer />
</template>

<style lang="scss" scoped></style>
