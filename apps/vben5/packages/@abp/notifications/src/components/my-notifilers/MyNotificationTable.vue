<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface';

import type { VbenFormProps } from '@vben/common-ui';

import type { Notification } from '../../types/notifications';

import { defineAsyncComponent, h, ref } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { formatToDateTime } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import { DeleteOutlined, DownOutlined } from '@ant-design/icons-vue';
import { Button, Dropdown, Menu, message, Modal } from 'ant-design-vue';

import { useMyNotifilersApi } from '../../api/useMyNotifilersApi';
import { useNotificationSerializer } from '../../hooks';
import {
  NotificationReadState,
  NotificationType,
} from '../../types/notifications';

defineOptions({
  name: 'MyNotificationTable',
});
const { cancel, deleteMyNotifilerApi, getMyNotifilersApi, markReadStateApi } =
  useMyNotifilersApi();

const { deserialize } = useNotificationSerializer();

const MenuItem = Menu.Item;

const ReadIcon = createIconifyIcon('ic:outline-mark-email-read');
const UnReadIcon = createIconifyIcon('ic:outline-mark-email-unread');
const BookMarkIcon = createIconifyIcon('material-symbols:bookmark-outline');

const selectedKeys = ref<string[]>([]);

const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: false,
  schema: [
    {
      component: 'Select',
      componentProps: {
        allowClear: true,
        options: [
          {
            label: $t('Notifications.Read'),
            value: NotificationReadState.Read,
          },
          {
            label: $t('Notifications.UnRead'),
            value: NotificationReadState.UnRead,
          },
        ],
      },
      defaultValue: NotificationReadState.UnRead,
      fieldName: 'readState',
      label: $t('Notifications.Notifications:State'),
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
  ],
  // 控制表单是否显示折叠按钮
  showCollapseButton: true,
  // 按下回车时是否提交表单
  submitOnEnter: true,
};

const gridOptions: VxeGridProps<Notification> = {
  columns: [
    {
      align: 'center',
      type: 'checkbox',
      width: 40,
    },
    {
      align: 'left',
      field: 'type',
      formatter({ cellValue }) {
        const type = cellValue as NotificationType;
        switch (type) {
          case NotificationType.Application: {
            return $t('Notifications.NotificationType:Application');
          }
          case NotificationType.ServiceCallback: {
            return $t('Notifications.NotificationType:ServiceCallback');
          }
          case NotificationType.System: {
            return $t('Notifications.NotificationType:System');
          }
          case NotificationType.User: {
            return $t('Notifications.NotificationType:User');
          }
        }
      },
      minWidth: 50,
      title: $t('Notifications.Notifications:Type'),
    },
    {
      align: 'left',
      field: 'creationTime',
      formatter({ cellValue }) {
        return cellValue ? formatToDateTime(cellValue) : '';
      },
      minWidth: 120,
      title: $t('Notifications.Notifications:SendTime'),
    },
    {
      align: 'left',
      field: 'title',
      minWidth: 150,
      slots: { default: 'title' },
      title: $t('Notifications.Notifications:Title'),
    },
    {
      align: 'left',
      field: 'message',
      minWidth: 120,
      slots: { default: 'message' },
      title: $t('Notifications.Notifications:Content'),
    },
    {
      field: 'action',
      fixed: 'right',
      slots: { default: 'action' },
      title: $t('AbpUi.Actions'),
      width: 200,
    },
  ],
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page }, formValues) => {
        const { totalCount, items } = await getMyNotifilersApi({
          maxResultCount: page.pageSize,
          skipCount: (page.currentPage - 1) * page.pageSize,
          ...formValues,
        });
        return {
          totalCount,
          items: items.map((item) => {
            const notification = deserialize(item);
            return {
              ...notification,
              id: item.id,
              state: item.state,
            };
          }),
        };
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
    // import: true,
    refresh: true,
    zoom: true,
  },
};

const gridEvents: VxeGridListeners<Notification> = {
  checkboxAll: (params) => {
    selectedKeys.value = params.records.map((record) => record.id);
  },
  checkboxChange: (params) => {
    selectedKeys.value = params.records.map((record) => record.id);
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const [NotificationModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./MyNotificationModal.vue'),
  ),
});

/** 点击行标记已读 */
async function onClickRead(row: Notification) {
  modalApi.setData(row);
  modalApi.open();
  await _onRead([row.id], NotificationReadState.Read);
}

/** 行标记阅读状态 */
async function onRowRead(row: Notification, info: MenuInfo) {
  switch (info.key) {
    case 'read': {
      return await _onRead([row.id], NotificationReadState.Read);
    }
    case 'un-read': {
      return await _onRead([row.id], NotificationReadState.UnRead);
    }
  }
}

/** 批量标记阅读状态 */
async function onBulkRead(info: MenuInfo) {
  switch (info.key) {
    case 'read': {
      await _onRead(selectedKeys.value, NotificationReadState.Read);
      break;
    }
    case 'un-read': {
      await _onRead(selectedKeys.value, NotificationReadState.UnRead);
      break;
    }
  }
  selectedKeys.value = [];
}

async function _onRead(idList: string[], state: NotificationReadState) {
  try {
    gridApi.setLoading(true);
    await markReadStateApi({
      idList,
      state,
    });
    await gridApi.query();
  } finally {
    gridApi.setLoading(false);
  }
}

const onDelete = (row: Notification) => {
  Modal.confirm({
    centered: true,
    content: $t('AbpUi.ItemWillBeDeletedMessageWithFormat', [row.title]),
    onCancel: () => {
      cancel();
    },
    onOk: async () => {
      await deleteMyNotifilerApi(row.id);
      message.success($t('AbpUi.DeletedSuccessfully'));
      await gridApi.query();
    },
    title: $t('AbpUi.AreYouSure'),
  });
};
</script>

<template>
  <Grid :table-title="$t('Notifications.Notifications')">
    <template #toolbar-tools>
      <Dropdown v-if="selectedKeys.length > 0">
        <template #overlay>
          <Menu @click="(info) => onBulkRead(info)">
            <MenuItem key="read">
              <div class="flex flex-row items-center gap-[4px]">
                <ReadIcon color="#00DD00" />
                {{ $t('Notifications.Read') }}
              </div>
            </MenuItem>
            <MenuItem key="un-read">
              <div class="flex flex-row items-center gap-[4px]">
                <UnReadIcon color="#FF7744" />
                {{ $t('Notifications.UnRead') }}
              </div>
            </MenuItem>
          </Menu>
        </template>
        <Button>
          <div class="flex flex-row items-center gap-[4px]">
            <BookMarkIcon />
            {{ $t('Notifications.MarkAs') }}
            <DownOutlined />
          </div>
        </Button>
      </Dropdown>
    </template>
    <template #title="{ row }">
      <div class="flex flex-row items-center gap-[4px]">
        <ReadIcon
          v-if="row.state === NotificationReadState.Read"
          class="size-5"
          color="#00DD00"
        />
        <UnReadIcon v-else class="size-5" color="#FF7744" />
        <a href="javascript:(0);" @click="onClickRead(row)">{{ row.title }}</a>
      </div>
    </template>
    <template #message="{ row }">
      <a href="javascript:(0);" @click="onClickRead(row)">{{ row.message }}</a>
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
        <Dropdown>
          <template #overlay>
            <Menu @click="(info) => onRowRead(row, info)">
              <MenuItem key="read">
                <div class="flex flex-row items-center gap-[4px]">
                  <ReadIcon color="#00DD00" />
                  {{ $t('Notifications.Read') }}
                </div>
              </MenuItem>
              <MenuItem key="un-read">
                <div class="flex flex-row items-center gap-[4px]">
                  <UnReadIcon color="#FF7744" />
                  {{ $t('Notifications.UnRead') }}
                </div>
              </MenuItem>
            </Menu>
          </template>
          <Button type="link">
            <div class="flex flex-row items-center gap-[4px]">
              <BookMarkIcon />
              {{ $t('Notifications.MarkAs') }}
              <DownOutlined />
            </div>
          </Button>
        </Dropdown>
      </div>
    </template>
  </Grid>
  <NotificationModal />
</template>

<style lang="scss" scoped></style>
