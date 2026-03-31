<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { NotificationSendRecordDto } from '../../types/send-records';

import { h, ref } from 'vue';

import { confirm } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import {
  formatToDateTime,
  useLocalization,
  useLocalizationSerializer,
} from '@abp/core';
import { useMessage, useVbenVxeGrid } from '@abp/ui';
import { DeleteOutlined } from '@ant-design/icons-vue';
import { Button, Tag, Tooltip } from 'ant-design-vue';

import { useNotificationDefinitionsApi } from '../../api/useNotificationDefinitionsApi';
import { useNotificationsApi } from '../../api/useNotificationsApi';
import { useNotificationSendRecordsApi } from '../../api/useNotificationSendRecordsApi';
import { useNotificationSerializer } from '../../hooks';
import { NotificationSendState } from '../../types/send-records';

defineOptions({
  name: 'NotificationSendRecordTable',
});

const SendMessageIcon = createIconifyIcon('ant-design:send-outlined');
interface SendStateInfo {
  color: string;
  label: string;
}

const { getAssignableProvidersApi } = useNotificationsApi();
const { deleteApi, reSendApi, getPagedListApi } =
  useNotificationSendRecordsApi();
const { getListApi: getNotificationDefinitionsApi } =
  useNotificationDefinitionsApi();

const message = useMessage();
const { Lr } = useLocalization();
const { deserialize: deserializeNotification } = useNotificationSerializer();
const { deserialize: deserializeLocalizableString } =
  useLocalizationSerializer();

const selectedKeys = ref<string[]>([]);
const sendStateMap: { [key: number]: SendStateInfo } = {
  [NotificationSendState.None]: {
    label: $t('Notifications.NotificationSendState:None'),
    color: '',
  },
  [NotificationSendState.Disabled]: {
    label: $t('Notifications.NotificationSendState:Disabled'),
    color: 'orange',
  },
  [NotificationSendState.Sent]: {
    label: $t('Notifications.NotificationSendState:Sent'),
    color: 'green',
  },
  [NotificationSendState.Failed]: {
    label: $t('Notifications.NotificationSendState:Failed'),
    color: 'red',
  },
};
const formOptions: VbenFormProps = {
  collapsed: true,
  collapsedRows: 3,
  commonConfig: {
    componentProps: {
      class: 'w-full',
    },
  },
  fieldMappingTime: [
    [
      'sendTime',
      ['beginSendTime', 'endSendTime'],
      ['YYYY-MM-DD 00:00:00', 'YYYY-MM-DD 23:59:59'],
    ],
  ],
  schema: [
    {
      component: 'ApiSelect',
      fieldName: 'provider',
      label: $t('Notifications.DisplayName:Provider'),
      componentProps: {
        allowClear: true,
        api: getAssignableProvidersApi,
        resultField: 'items',
        lableField: 'name',
        valueField: 'name',
      },
    },
    {
      component: 'Select',
      componentProps: {
        allowClear: true,
        options: [
          {
            label: $t('Notifications.NotificationSendState:None'),
            value: NotificationSendState.None,
          },
          {
            label: $t('Notifications.NotificationSendState:Sent'),
            value: NotificationSendState.Sent,
          },
          {
            label: $t('Notifications.NotificationSendState:Failed'),
            value: NotificationSendState.Failed,
          },
          {
            label: $t('Notifications.NotificationSendState:Disabled'),
            value: NotificationSendState.Disabled,
          },
        ],
      },
      fieldName: 'state',
      label: $t('WebhooksManagement.DisplayName:State'),
    },
    {
      component: 'ApiSelect',
      fieldName: 'notificationName',
      label: $t('Notifications.DisplayName:NotificationName'),
      componentProps: {
        allowClear: true,
        api: onLoadNotiferDefines,
        resultField: 'items',
        labelField: 'displayName',
        valueField: 'name',
      },
    },
    {
      component: 'RangePicker',
      fieldName: 'sendTime',
      label: $t('Notifications.DisplayName:SendTime'),
    },
  ],
  // 控制表单是否显示折叠按钮
  showCollapseButton: true,
  // 按下回车时是否提交表单
  submitOnEnter: true,
};

const gridOptions: VxeGridProps<NotificationSendRecordDto> = {
  columns: [
    {
      align: 'center',
      type: 'checkbox',
      width: 40,
    },
    {
      align: 'left',
      field: 'provider',
      minWidth: 150,
      sortable: true,
      title: $t('Notifications.DisplayName:Provider'),
    },
    {
      align: 'left',
      field: 'notificationName',
      formatter: ({ row }) => {
        return row.notification.name;
      },
      minWidth: 150,
      sortable: true,
      title: $t('Notifications.DisplayName:NotificationName'),
    },
    {
      align: 'left',
      field: 'title',
      formatter: ({ row }) => {
        const notification = deserializeNotification(row.notification);
        return notification.title;
      },
      minWidth: 150,
      title: $t('Notifications.Notifications:Title'),
    },
    {
      align: 'left',
      field: 'sendTime',
      minWidth: 150,
      sortable: true,
      title: $t('Notifications.DisplayName:SendTime'),
      formatter: ({ cellValue }) => {
        return formatToDateTime(cellValue);
      },
    },
    {
      align: 'left',
      field: 'state',
      minWidth: 150,
      sortable: true,
      slots: { default: 'state' },
      title: $t('Notifications.DisplayName:SendState'),
    },
    {
      align: 'left',
      field: 'userName',
      minWidth: 150,
      sortable: true,
      title: $t('Notifications.DisplayName:UserName'),
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
      query: async ({ page, sort }, formValues) => {
        const sorting = sort.order ? `${sort.field} ${sort.order}` : undefined;
        return await getPagedListApi({
          sorting,
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
    custom: true,
    export: true,
    refresh: {
      code: 'query',
    },
    zoom: true,
  },
};

const gridEvents: VxeGridListeners<NotificationSendRecordDto> = {
  checkboxAll: (params) => {
    selectedKeys.value = params.records.map((record) => record.id);
  },
  checkboxChange: (params) => {
    selectedKeys.value = params.records.map((record) => record.id);
  },
  sortChange: () => {
    gridApi.query();
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

async function onLoadNotiferDefines() {
  const { items } = await getNotificationDefinitionsApi();
  items.forEach((item) => {
    if (item.displayName) {
      const localizableString = deserializeLocalizableString(item.displayName);
      item.displayName = Lr(
        localizableString.resourceName,
        localizableString.name,
      );
    }
  });
  return items;
}

function onDelete(row: NotificationSendRecordDto) {
  confirm({
    beforeClose: async ({ isConfirm }) => {
      if (isConfirm) {
        await deleteApi(row.id);
        message.success($t('AbpUi.DeletedSuccessfully'));
        await gridApi.query();
      }
      return true;
    },
    centered: true,
    content: $t('Notifications.SelectedSendRecordWillBeDeleteMessage'),
    icon: 'warning',
    title: $t('AbpUi.AreYouSure'),
  });
}

async function onSend(row: NotificationSendRecordDto) {
  confirm({
    beforeClose: async ({ isConfirm }) => {
      if (isConfirm) {
        try {
          gridApi.setLoading(true);
          await reSendApi(row.id);
          message.success($t('Notifications.SendSuccessfully'));
          await gridApi.query();
        } finally {
          gridApi.setLoading(false);
        }
      }
      return true;
    },
    centered: true,
    content: $t('Notifications.SelectedSendRecordWillBeReSendMessage'),
    icon: 'question',
    title: $t('AbpUi.AreYouSure'),
  });
}
</script>

<template>
  <Grid :table-title="$t('Notifications.SendRecords')">
    <template #state="{ row }">
      <Tooltip v-if="row.reason" placement="top">
        <template #title>
          <span>{{ row.reason }}</span>
        </template>
        <Tag :color="sendStateMap[row.state]?.color">
          {{ sendStateMap[row.state]?.label }}
        </Tag>
      </Tooltip>
      <Tag v-else :color="sendStateMap[row.state]?.color">
        {{ sendStateMap[row.state]?.label }}
      </Tag>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          :icon="h(DeleteOutlined)"
          danger
          type="link"
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
        <Button :icon="h(SendMessageIcon)" type="link" @click="onSend(row)">
          {{ $t('Notifications.Resend') }}
        </Button>
      </div>
    </template>
  </Grid>
</template>

<style lang="scss" scoped></style>
