<script setup lang="ts">
import type { VxeGridProps } from '@abp/ui';
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface';

import type { VbenFormProps } from '@vben/common-ui';

import type { EmailMessageDto } from '../../../types/messages';

import { defineAsyncComponent, h } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenModal } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { formatToDateTime } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  EllipsisOutlined,
} from '@ant-design/icons-vue';
import { Button, Dropdown, Menu, message, Modal, Tag } from 'ant-design-vue';

import { useEmailMessagesApi } from '../../../api/useEmailMessagesApi';
import { EmailMessagesPermissions } from '../../../constants/permissions';
import { MessageStatus } from '../../../types/messages';

defineOptions({
  name: 'EmailMessageTable',
});

const MenuItem = Menu.Item;
const SendMessageIcon = createIconifyIcon('ant-design:send-outlined');

const { hasAccessByCodes } = useAccess();
const { deleteApi, getPagedListApi, sendApi } = useEmailMessagesApi();

const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: true,
  collapsedRows: 2,
  // 所有表单项共用，可单独在表单内覆盖
  commonConfig: {
    // 在label后显示一个冒号
    colon: true,
    // 所有表单项
    componentProps: {
      class: 'w-full',
    },
  },
  fieldMappingTime: [
    ['sendTime', ['beginSendTime', 'endSendTime'], 'YYYY-MM-DD'],
  ],
  schema: [
    {
      component: 'Select',
      componentProps: {
        options: [
          {
            label: $t('AppPlatform.MessageStatus:Pending'),
            value: MessageStatus.Pending,
          },
          {
            label: $t('AppPlatform.MessageStatus:Sent'),
            value: MessageStatus.Sent,
          },
          {
            label: $t('AppPlatform.MessageStatus:Failed'),
            value: MessageStatus.Failed,
          },
        ],
      },
      fieldName: 'status',
      label: $t('AppPlatform.DisplayName:Status'),
    },
    {
      component: 'Input',
      fieldName: 'emailAddress',
      label: $t('AppPlatform.DisplayName:Receiver'),
    },
    {
      component: 'RangePicker',
      fieldName: 'sendTime',
      label: $t('AppPlatform.DisplayName:SendTime'),
    },
    {
      component: 'Input',
      fieldName: 'filter',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('AbpUi.Search'),
    },
  ],
  // 控制表单是否显示折叠按钮
  showCollapseButton: true,
  // 按下回车时是否提交表单
  submitOnEnter: true,
};

const gridOptions: VxeGridProps<EmailMessageDto> = {
  columns: [
    {
      align: 'left',
      field: 'provider',
      minWidth: 180,
      title: $t('AppPlatform.DisplayName:Provider'),
    },
    {
      align: 'left',
      field: 'status',
      minWidth: 150,
      slots: { default: 'status' },
      title: $t('AppPlatform.DisplayName:Status'),
    },
    {
      align: 'left',
      field: 'sendTime',
      formatter: ({ cellValue }) => {
        return cellValue ? formatToDateTime(cellValue) : cellValue;
      },
      minWidth: 150,
      title: $t('AppPlatform.DisplayName:SendTime'),
    },
    {
      align: 'center',
      field: 'sendCount',
      minWidth: 100,
      title: $t('AppPlatform.DisplayName:SendCount'),
    },
    {
      align: 'left',
      field: 'subject',
      minWidth: 180,
      title: $t('AppPlatform.DisplayName:Subject'),
    },
    {
      align: 'left',
      field: 'content',
      minWidth: 220,
      title: $t('AppPlatform.DisplayName:Content'),
    },
    {
      align: 'left',
      field: 'from',
      minWidth: 220,
      title: $t('AppPlatform.DisplayName:From'),
    },
    {
      align: 'left',
      field: 'receiver',
      minWidth: 150,
      title: $t('AppPlatform.DisplayName:Receiver'),
    },
    {
      align: 'left',
      field: 'creationTime',
      formatter: ({ cellValue }) => {
        return cellValue ? formatToDateTime(cellValue) : cellValue;
      },
      minWidth: 200,
      title: $t('AppPlatform.DisplayName:CreationTime'),
    },
    {
      align: 'left',
      field: 'reason',
      minWidth: 150,
      title: $t('AppPlatform.DisplayName:Reason'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      visible: hasAccessByCodes([EmailMessagesPermissions.Delete]),
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
    custom: true,
    export: true,
    refresh: true,
    zoom: true,
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridOptions,
});

const [EmailMessageModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./EmailMessageModal.vue'),
  ),
});

function onUpdate(row: EmailMessageDto) {
  // console.warn('onUpdate is not implementation!', row);
  modalApi.setData(row);
  modalApi.open();
}

function onDelete(row: EmailMessageDto) {
  Modal.confirm({
    afterClose: () => {
      gridApi.setLoading(false);
    },
    centered: true,
    content: `${$t('AbpUi.ItemWillBeDeletedMessage')}`,
    onOk: async () => {
      try {
        gridApi.setLoading(true);
        await deleteApi(row.id);
        message.success($t('AbpUi.DeletedSuccessfully'));
        gridApi.query();
      } finally {
        gridApi.setLoading(false);
      }
    },
    title: $t('AbpUi.AreYouSure'),
  });
}

async function onSend(row: EmailMessageDto) {
  Modal.confirm({
    centered: true,
    content: `${$t('AppPlatform.MessageWillBeReSendWarningMessage')}`,
    onOk: async () => {
      try {
        gridApi.setLoading(true);
        await sendApi(row.id);
        message.success($t('AppPlatform.SuccessfullySent'));
        gridApi.query();
      } finally {
        gridApi.setLoading(false);
      }
    },
    title: $t('AbpUi.AreYouSure'),
  });
}
async function onMenuClick(row: EmailMessageDto, info: MenuInfo) {
  switch (info.key) {
    case 'send': {
      await onSend(row);
    }
  }
}
</script>

<template>
  <Grid :table-title="$t('AppPlatform.EmailMessages')">
    <template #status="{ row }">
      <Tag v-if="row.status === MessageStatus.Pending" color="warning">
        {{ $t('AppPlatform.MessageStatus:Pending') }}
      </Tag>
      <Tag v-else-if="row.status === MessageStatus.Sent" color="success">
        {{ $t('AppPlatform.MessageStatus:Sent') }}
      </Tag>
      <Tag v-else-if="row.status === MessageStatus.Failed" color="error">
        {{ $t('AppPlatform.MessageStatus:Failed') }}
      </Tag>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          :icon="h(EditOutlined)"
          block
          type="link"
          @click="onUpdate(row)"
        >
          {{ $t('AbpUi.Edit') }}
        </Button>
        <Button
          :icon="h(DeleteOutlined)"
          block
          danger
          type="link"
          v-access:code="[EmailMessagesPermissions.Delete]"
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
        <Dropdown>
          <template #overlay>
            <Menu @click="(info) => onMenuClick(row, info)">
              <MenuItem
                v-if="hasAccessByCodes([EmailMessagesPermissions.SendMessage])"
                key="send"
              >
                <div class="flex flex-row items-center gap-[4px]">
                  <SendMessageIcon color="green" />
                  {{ $t('AppPlatform.SendMessage') }}
                </div>
              </MenuItem>
            </Menu>
          </template>
          <Button :icon="h(EllipsisOutlined)" type="link" />
        </Dropdown>
      </div>
    </template>
  </Grid>
  <EmailMessageModal />
</template>

<style lang="scss" scoped></style>
