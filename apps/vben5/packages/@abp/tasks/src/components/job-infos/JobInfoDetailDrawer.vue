<script setup lang="ts">
import type { BackgroundJobInfoDto, BackgroundJobLogDto } from '../../types';

import { ref } from 'vue';

import { useVbenDrawer } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { formatToDateTime } from '@abp/core';
import { PropertyTable } from '@abp/ui';
import {
  Button,
  Checkbox,
  Descriptions,
  DescriptionsItem,
  Empty,
  List,
  ListItem,
  ListItemMeta,
  message,
  Popconfirm,
  TabPane,
  Tabs,
  Tag,
} from 'ant-design-vue';

import { useJobInfosApi } from '../../api/useJobInfosApi';
import { useJobLogsApi } from '../../api/useJobLogsApi';
import { useJobEnumsMap } from '../../hooks/useJobEnumsMap';
import { JobType } from '../../types';

const DeleteOutlined = createIconifyIcon('ant-design:delete-outlined');
const SuccessIcon = createIconifyIcon('grommet-icons:status-good');
const FailedIcon = createIconifyIcon('grommet-icons:status-warning');

const { getApi } = useJobInfosApi();
const { deleteApi: deleteJobLogApi, getPagedListApi: getJobLogsApi } =
  useJobLogsApi();
const {
  jobPriorityColor,
  jobPriorityMap,
  jobSourceMap,
  jobStatusColor,
  jobStatusMap,
  jobTypeMap,
} = useJobEnumsMap();

const activeTabKey = ref('basic');
const jobInfo = ref<BackgroundJobInfoDto>();
const jobLogs = ref<BackgroundJobLogDto[]>([]);
const paginationInfo = ref({
  current: 1,
  pageSize: 10,
  show: true,
  totalCount: 0,
});
const [Drawer, drawerApi] = useVbenDrawer({
  class: 'w-1/2',
  onCancel() {
    drawerApi.close();
  },
  onClosed() {
    activeTabKey.value = 'basic';
  },
  async onOpenChange(isOpen) {
    if (isOpen) {
      await onGet();
    }
  },
  showConfirmButton: false,
});

async function onTabChange(activeKey: string) {
  if (activeKey === 'logs') {
    await onGetLogs(1);
  }
}

function onSizeChange() {}

async function onGet() {
  drawerApi.setState({ loading: true });
  try {
    jobLogs.value = [];
    const { id } = drawerApi.getData<BackgroundJobInfoDto>();
    jobInfo.value = await getApi(id);
  } finally {
    drawerApi.setState({ loading: false });
  }
}

async function onGetLogs(page?: number) {
  drawerApi.setState({ loading: true });
  try {
    const pageNumber = page ?? 1;
    paginationInfo.value.current = pageNumber;
    const { id } = drawerApi.getData<BackgroundJobInfoDto>();
    const jobLogResult = await getJobLogsApi({
      jobId: id,
      maxResultCount: paginationInfo.value.pageSize,
      skipCount: (pageNumber - 1) * paginationInfo.value.pageSize,
    });
    jobLogs.value = jobLogResult.items;
    paginationInfo.value.totalCount = jobLogResult.totalCount;
  } finally {
    drawerApi.setState({ loading: false });
  }
}

async function onDeleteLog(jobLog: BackgroundJobLogDto) {
  drawerApi.setState({ loading: true });
  try {
    await deleteJobLogApi(jobLog.id);
    message.success($t('AbpUi.DeletedSuccessfully'));
    await onGetLogs(paginationInfo.value.current);
  } finally {
    drawerApi.setState({ loading: false });
  }
}
</script>

<template>
  <Drawer :title="$t('TaskManagement.BackgroundJobDetail')">
    <Tabs
      v-if="jobInfo"
      v-model:active-key="activeTabKey"
      @change="(key) => onTabChange(key.toString())"
    >
      <TabPane key="basic" :tab="$t('TaskManagement.BasicInfo')">
        <Descriptions
          :colon="false"
          :column="2"
          bordered
          size="small"
          :label-style="{ minWidth: '120px' }"
        >
          <DescriptionsItem :label="$t('TaskManagement.DisplayName:Status')">
            <Tag :color="jobStatusColor[jobInfo.status]">
              {{ jobStatusMap[jobInfo.status] }}
            </Tag>
          </DescriptionsItem>
          <DescriptionsItem :label="$t('TaskManagement.DisplayName:Source')">
            <span>{{ jobSourceMap[jobInfo.source] }}</span>
          </DescriptionsItem>
          <DescriptionsItem :label="$t('TaskManagement.DisplayName:IsEnabled')">
            <Checkbox disabled :checked="jobInfo.isEnabled" />
          </DescriptionsItem>
          <DescriptionsItem :label="$t('TaskManagement.DisplayName:Priority')">
            <Tag :color="jobPriorityColor[jobInfo.priority]">
              {{ jobPriorityMap[jobInfo.priority] }}
            </Tag>
          </DescriptionsItem>
          <DescriptionsItem :label="$t('TaskManagement.DisplayName:Group')">
            <span>{{ jobInfo.group }}</span>
          </DescriptionsItem>
          <DescriptionsItem :label="$t('TaskManagement.DisplayName:Name')">
            <span>{{ jobInfo.name }}</span>
          </DescriptionsItem>
          <DescriptionsItem
            :label="$t('TaskManagement.DisplayName:Description')"
            :span="2"
          >
            <span>{{ jobInfo.description }}</span>
          </DescriptionsItem>
          <DescriptionsItem
            :label="$t('TaskManagement.DisplayName:Type')"
            :span="2"
          >
            <span>{{ jobInfo.type }}</span>
          </DescriptionsItem>
          <DescriptionsItem
            :label="$t('TaskManagement.DisplayName:CreationTime')"
          >
            <span>{{ formatToDateTime(jobInfo.creationTime) }}</span>
          </DescriptionsItem>
          <DescriptionsItem
            :label="$t('TaskManagement.DisplayName:LockTimeOut')"
          >
            <span>{{ jobInfo.lockTimeOut }}</span>
          </DescriptionsItem>
          <DescriptionsItem :label="$t('TaskManagement.DisplayName:BeginTime')">
            <span>{{ formatToDateTime(jobInfo.beginTime) }}</span>
          </DescriptionsItem>
          <DescriptionsItem :label="$t('TaskManagement.DisplayName:EndTime')">
            <span>{{
              jobInfo.endTime
                ? formatToDateTime(jobInfo.endTime)
                : jobInfo.endTime
            }}</span>
          </DescriptionsItem>
          <DescriptionsItem :label="$t('TaskManagement.DisplayName:JobType')">
            <span>{{ jobTypeMap[jobInfo.jobType] }}</span>
          </DescriptionsItem>
          <DescriptionsItem
            v-if="jobInfo.jobType === JobType.Period"
            :label="$t('TaskManagement.DisplayName:Cron')"
          >
            <span>{{ jobInfo.cron }}</span>
          </DescriptionsItem>
          <DescriptionsItem
            v-else
            :label="$t('TaskManagement.DisplayName:Interval')"
          >
            <span>{{ jobInfo.interval }}</span>
          </DescriptionsItem>
          <DescriptionsItem
            :label="$t('TaskManagement.DisplayName:LastRunTime')"
          >
            <span>{{
              jobInfo.lastRunTime
                ? formatToDateTime(jobInfo.lastRunTime)
                : jobInfo.lastRunTime
            }}</span>
          </DescriptionsItem>
          <DescriptionsItem
            :label="$t('TaskManagement.DisplayName:NextRunTime')"
          >
            <span>{{
              jobInfo.nextRunTime
                ? formatToDateTime(jobInfo.nextRunTime)
                : jobInfo.nextRunTime
            }}</span>
          </DescriptionsItem>
          <DescriptionsItem
            :label="$t('TaskManagement.DisplayName:TriggerCount')"
          >
            <span>{{ jobInfo.triggerCount }}</span>
          </DescriptionsItem>
          <DescriptionsItem :label="$t('TaskManagement.DisplayName:MaxCount')">
            <span>{{ jobInfo.maxCount }}</span>
          </DescriptionsItem>
          <DescriptionsItem :label="$t('TaskManagement.DisplayName:TryCount')">
            <span>{{ jobInfo.tryCount }}</span>
          </DescriptionsItem>
          <DescriptionsItem
            :label="$t('TaskManagement.DisplayName:MaxTryCount')"
          >
            <span>{{ jobInfo.maxTryCount }}</span>
          </DescriptionsItem>
          <DescriptionsItem :label="$t('TaskManagement.DisplayName:Result')">
            <span>{{ jobInfo.result }}</span>
          </DescriptionsItem>
        </Descriptions>
      </TabPane>
      <TabPane key="paramters" :tab="$t('TaskManagement.Paramters')">
        <PropertyTable :data="jobInfo.args" disabled />
      </TabPane>
      <TabPane key="logs" :tab="$t('TaskManagement.BackgroundJobLogs')">
        <List
          item-layout="vertical"
          size="default"
          bordered
          :data-source="jobLogs"
          :pagination="{
            current: paginationInfo.current,
            pageSize: paginationInfo.pageSize,
            total: paginationInfo.totalCount,
            showSizeChanger: paginationInfo.show,
            onChange: onGetLogs,
            onShowSizeChange: onSizeChange,
          }"
        >
          <template #renderItem="{ item }">
            <ListItem :key="item.id">
              <template #extra>
                <Popconfirm
                  placement="topLeft"
                  :title="$t('AbpUi.AreYouSure')"
                  :description="$t('AbpUi.ItemWillBeDeletedMessage')"
                  @confirm="onDeleteLog(item)"
                >
                  <Button block danger type="link">
                    <template #icon>
                      <DeleteOutlined class="inline size-5" />
                    </template>
                  </Button>
                </Popconfirm>
              </template>
              <ListItemMeta :description="item.runTime">
                <template #avatar>
                  <SuccessIcon
                    v-if="!item.exception"
                    class="size-8"
                    color="seagreen"
                  />
                  <FailedIcon v-else class="size-8" color="orangered" />
                </template>
                <template #title>
                  <span>{{ jobInfo.name }}</span>
                </template>
              </ListItemMeta>
              <h3 style="word-wrap: break-word; white-space: pre-line">
                {{ item.exception ?? item.message }}
              </h3>
            </ListItem>
          </template>
        </List>
      </TabPane>
    </Tabs>
    <Empty v-else />
  </Drawer>
</template>

<style scoped lang="scss">
:deep(.ant-list-item-main) {
  overflow: hidden;
}
</style>
