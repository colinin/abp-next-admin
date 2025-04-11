<script setup lang="ts">
import type { LogDto, LogLevel } from '../../types/loggings';

import { ref } from 'vue';

import { useVbenDrawer } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { formatToDateTime } from '@abp/core';
import { Descriptions, Tabs, Tag } from 'ant-design-vue';

import { useLoggingsApi } from '../../api/useLoggingsApi';

defineOptions({
  name: 'LoggingDrawer',
});

defineProps<{
  logLevelOptions: { color: string; label: string; value: LogLevel }[];
}>();
const TabPane = Tabs.TabPane;
const DescriptionsItem = Descriptions.Item;

const activedTab = ref('basic');
const logModel = ref<LogDto>({} as LogDto);

const { getApi } = useLoggingsApi();
const [Drawer, drawerApi] = useVbenDrawer({
  class: 'w-auto',
  onCancel() {
    drawerApi.close();
  },
  onConfirm: async () => {
    drawerApi.close();
  },
  onOpenChange: async (isOpen: boolean) => {
    if (isOpen) {
      try {
        logModel.value = {} as LogDto;
        drawerApi.setState({ loading: true });
        const dto = drawerApi.getData<LogDto>();
        await onGet(dto.fields.id);
      } finally {
        drawerApi.setState({ loading: false });
      }
    }
  },
  title: $t('AbpAuditLogging.AuditLog'),
});
async function onGet(id: string) {
  const dto = await getApi(id);
  logModel.value = dto;
}
</script>

<template>
  <Drawer>
    <div style="width: 800px">
      <Tabs v-model="activedTab">
        <TabPane key="basic" :tab="$t('AbpAuditLogging.Operation')">
          <Descriptions
            :colon="false"
            :column="1"
            bordered
            size="small"
            :label-style="{ minWidth: '110px' }"
          >
            <DescriptionsItem :label="$t('AbpAuditLogging.TimeStamp')">
              {{ formatToDateTime(logModel.timeStamp) }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.Level')">
              <Tag :color="logLevelOptions[logModel.level]?.color">
                {{ logLevelOptions[logModel.level]?.label }}
              </Tag>
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.Message')" :span="2">
              {{ logModel.message }}
            </DescriptionsItem>
          </Descriptions>
        </TabPane>
        <TabPane key="fields" :tab="$t('AbpAuditLogging.Fields')">
          <Descriptions
            :colon="false"
            :column="1"
            bordered
            size="small"
            :label-style="{ minWidth: '110px' }"
          >
            <DescriptionsItem :label="$t('AbpAuditLogging.ApplicationName')">
              {{ logModel.fields.application }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.MachineName')">
              {{ logModel.fields.machineName }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.Environment')">
              {{ logModel.fields.environment }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.ProcessId')">
              {{ logModel.fields.processId }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.ThreadId')">
              {{ logModel.fields.threadId }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.Context')">
              {{ logModel.fields.context }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.ActionId')">
              {{ logModel.fields.actionId }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.MethodName')">
              {{ logModel.fields.actionName }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.RequestId')">
              {{ logModel.fields.requestId }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.RequestPath')">
              {{ logModel.fields.requestPath }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.ConnectionId')">
              {{ logModel.fields.connectionId }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.CorrelationId')">
              {{ logModel.fields.correlationId }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.ClientId')">
              {{ logModel.fields.clientId }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.UserId')">
              {{ logModel.fields.userId }}
            </DescriptionsItem>
          </Descriptions>
        </TabPane>
      </Tabs>
    </div>
  </Drawer>
</template>

<style scoped></style>
