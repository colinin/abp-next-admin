<script setup lang="ts">
import type { VxeGridProps } from 'vxe-table';

import type { Action, AuditLogDto } from '../../types/audit-logs';

import { ref } from 'vue';

import { useVbenDrawer } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { formatToDateTime } from '@abp/core';
import { CodeEditor, MODE, useVbenVxeGrid } from '@abp/ui';
import { Descriptions, Tabs, Tag } from 'ant-design-vue';

import { getApi } from '../../api/audit-logs';
import { useAuditlogs } from '../../hooks/useAuditlogs';
import EntityChangeTable from '../entity-changes/EntityChangeTable.vue';

defineOptions({
  name: 'AuditLogDrawer',
});

const TabPane = Tabs.TabPane;
const DescriptionsItem = Descriptions.Item;

const activedTab = ref('basic');
const auditLogModel = ref<AuditLogDto>({} as AuditLogDto);

const { getHttpMethodColor, getHttpStatusCodeColor } = useAuditlogs();
const [Drawer, drawerApi] = useVbenDrawer({
  class: 'w-auto',
  onCancel() {
    drawerApi.close();
  },
  onConfirm: async () => {},
  onOpenChange: async (isOpen: boolean) => {
    if (isOpen) {
      try {
        auditLogModel.value = {} as AuditLogDto;
        drawerApi.setState({ loading: true });
        const dto = drawerApi.getData<AuditLogDto>();
        await onGet(dto.id);
      } finally {
        drawerApi.setState({ loading: false });
      }
    }
  },
  title: $t('AbpAuditLogging.AuditLog'),
});
/** 调用方法表格配置 */
const actionsGridOptions: VxeGridProps<Action> = {
  border: true,
  columns: [
    {
      align: 'left',
      field: 'parameters',
      slots: {
        content: 'parameters',
      },
      type: 'expand',
    },
    {
      align: 'left',
      field: 'serviceName',
      sortable: true,
      title: $t('AbpAuditLogging.ServiceName'),
      width: 'auto',
    },
    {
      align: 'left',
      field: 'methodName',
      sortable: true,
      title: $t('AbpAuditLogging.MethodName'),
      width: 150,
    },
    {
      align: 'left',
      field: 'executionTime',
      formatter: ({ cellValue }) => {
        return cellValue ? formatToDateTime(cellValue) : cellValue;
      },
      sortable: true,
      title: $t('AbpAuditLogging.ExecutionTime'),
      width: 200,
    },
    {
      align: 'left',
      field: 'executionDuration',
      sortable: true,
      title: $t('AbpAuditLogging.ExecutionDuration'),
      width: 150,
    },
  ],
  expandConfig: {
    padding: true,
    trigger: 'row',
  },
  exportConfig: {},
  keepSource: true,
  pagerConfig: {
    enabled: false,
  },
  proxyConfig: {
    ajax: {
      query: () => {
        return Promise.resolve(auditLogModel.value.actions);
      },
    },
    response: {
      list: ({ data }) => {
        return data;
      },
    },
  },
  toolbarConfig: {
    enabled: false,
  },
};
/** 调用方法表格 */
const [ActionsGrid] = useVbenVxeGrid({
  gridOptions: actionsGridOptions,
});
/** 查询审计日志 */
async function onGet(id: string) {
  const dto = await getApi(id);
  auditLogModel.value = dto;
}
</script>

<template>
  <Drawer>
    <div style="width: 800px">
      <Tabs v-model="activedTab">
        <TabPane key="basic" :tab="$t('AbpAuditLogging.Operation')">
          <Descriptions :colon="false" :column="2" bordered size="small">
            <DescriptionsItem :label="$t('AbpAuditLogging.ApplicationName')">
              {{ auditLogModel.applicationName }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.ExecutionTime')">
              {{ formatToDateTime(auditLogModel.executionTime) }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.UserName')">
              {{ auditLogModel.userName }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.HttpMethod')">
              <Tag :color="getHttpMethodColor(auditLogModel.httpMethod)">
                {{ auditLogModel.httpMethod }}
              </Tag>
            </DescriptionsItem>
            <DescriptionsItem
              :label="$t('AbpAuditLogging.RequestUrl')"
              :span="2"
            >
              {{ auditLogModel.url }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.HttpStatusCode')">
              <Tag
                :color="getHttpStatusCodeColor(auditLogModel.httpStatusCode)"
              >
                {{ auditLogModel.httpStatusCode }}
              </Tag>
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.ExecutionDuration')">
              {{ auditLogModel.executionDuration }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.ClientId')">
              {{ auditLogModel.clientId }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.ClientIpAddress')">
              {{ auditLogModel.clientIpAddress }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.ClientName')">
              {{ auditLogModel.clientName }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.CorrelationId')">
              {{ auditLogModel.correlationId }}
            </DescriptionsItem>
            <DescriptionsItem
              :label="$t('AbpAuditLogging.BrowserInfo')"
              :label-style="{ width: '110px' }"
              :span="2"
            >
              {{ auditLogModel.browserInfo }}
            </DescriptionsItem>
            <DescriptionsItem :label="$t('AbpAuditLogging.Comments')" :span="2">
              {{ auditLogModel.comments }}
            </DescriptionsItem>
            <DescriptionsItem
              :label="$t('AbpAuditLogging.Exception')"
              :span="2"
            >
              {{ auditLogModel.exceptions }}
            </DescriptionsItem>
            <DescriptionsItem
              :label="$t('AbpAuditLogging.Additional')"
              :span="2"
            >
              {{ auditLogModel.extraProperties }}
            </DescriptionsItem>
          </Descriptions>
        </TabPane>
        <TabPane
          v-if="auditLogModel.actions?.length"
          key="opera"
          :tab="`${$t('AbpAuditLogging.InvokeMethod')}(${auditLogModel.actions?.length})`"
        >
          <ActionsGrid>
            <template #parameters="{ row }">
              <Descriptions :colon="false" :column="1" bordered size="small">
                <DescriptionsItem :label="$t('AbpAuditLogging.Parameters')">
                  <CodeEditor
                    :mode="MODE.JSON"
                    :value="row.parameters"
                    readonly
                  />
                </DescriptionsItem>
                <DescriptionsItem :label="$t('AbpAuditLogging.Additional')">
                  <CodeEditor
                    :mode="MODE.JSON"
                    :value="row.extraProperties"
                    readonly
                  />
                </DescriptionsItem>
              </Descriptions>
            </template>
          </ActionsGrid>
        </TabPane>
        <TabPane
          v-if="auditLogModel.entityChanges?.length"
          key="changes"
          :tab="`${$t('AbpAuditLogging.EntitiesChanged')}(${auditLogModel.entityChanges?.length})`"
        >
          <EntityChangeTable :data="auditLogModel.entityChanges" />
        </TabPane>
      </Tabs>
    </div>
  </Drawer>
</template>

<style scoped></style>
