<script setup lang="ts">
import type { VbenFormProps, VxeGridListeners, VxeGridProps } from '@abp/ui';
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface';

import type { NotificationDefinitionDto } from '../../../types/definitions';
import type { NotificationGroupDefinitionDto } from '../../../types/groups';

import { defineAsyncComponent, h, onMounted, reactive, ref } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenModal } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import {
  listToTree,
  useLocalization,
  useLocalizationSerializer,
} from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  CheckOutlined,
  CloseOutlined,
  DeleteOutlined,
  EditOutlined,
  EllipsisOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, Dropdown, Menu, message, Modal, Tag } from 'ant-design-vue';
import { VxeGrid } from 'vxe-table';

import { useNotificationDefinitionsApi } from '../../../api/useNotificationDefinitionsApi';
import { useNotificationGroupDefinitionsApi } from '../../../api/useNotificationGroupDefinitionsApi';
import {
  NotificationDefinitionsPermissions,
  NotificationPermissions,
} from '../../../constants/permissions';
import { useEnumMaps } from './useEnumMaps';

defineOptions({
  name: 'NotificationDefinitionTable',
});

interface DefinitionItem {
  allowSubscriptionToClients: boolean;
  children: DefinitionItem[];
  contentType: string;
  description?: string;
  displayName: string;
  groupName: string;
  isStatic: boolean;
  name: string;
  notificationLifetime: string;
  notificationType: string;
  providers?: string[];
  template?: string;
}
interface DefinitionGroup {
  displayName: string;
  items: DefinitionItem[];
  name: string;
}

const MenuItem = Menu.Item;
const SendIcon = createIconifyIcon('ant-design:send-outlined');

const { Lr } = useLocalization();
const { hasAccessByCodes } = useAccess();
const { deserialize } = useLocalizationSerializer();
const {
  notificationContentTypeMap,
  notificationLifetimeMap,
  notificationTypeMap,
} = useEnumMaps();
const { getListApi: getGroupsApi } = useNotificationGroupDefinitionsApi();
const { deleteApi, getListApi: getDefinitionsApi } =
  useNotificationDefinitionsApi();

const definitionGroups = ref<DefinitionGroup[]>([]);
const pageState = reactive({
  current: 1,
  size: 10,
  total: 0,
});

const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: false,
  handleReset: onReset,
  async handleSubmit(params) {
    pageState.current = 1;
    await onGet(params);
  },
  schema: [
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

const gridOptions: VxeGridProps<NotificationGroupDefinitionDto> = {
  columns: [
    {
      align: 'center',
      type: 'seq',
      width: 50,
    },
    {
      align: 'left',
      field: 'group',
      slots: { content: 'group' },
      type: 'expand',
      width: 50,
    },
    {
      align: 'left',
      field: 'name',
      minWidth: 150,
      title: $t('Notifications.DisplayName:Name'),
    },
    {
      align: 'left',
      field: 'displayName',
      minWidth: 150,
      title: $t('Notifications.DisplayName:DisplayName'),
    },
  ],
  expandConfig: {
    padding: true,
    trigger: 'row',
  },
  exportConfig: {},
  keepSource: true,
  toolbarConfig: {
    custom: true,
    export: true,
    refresh: false,
    zoom: true,
  },
};
const subGridColumns: VxeGridProps<NotificationDefinitionDto>['columns'] = [
  {
    align: 'center',
    type: 'seq',
    width: 50,
  },
  {
    align: 'left',
    field: 'name',
    minWidth: 150,
    title: $t('Notifications.DisplayName:Name'),
    treeNode: true,
  },
  {
    align: 'left',
    field: 'displayName',
    minWidth: 120,
    title: $t('Notifications.DisplayName:DisplayName'),
  },
  {
    align: 'left',
    field: 'description',
    minWidth: 120,
    title: $t('Notifications.DisplayName:Description'),
  },
  {
    align: 'left',
    field: 'allowSubscriptionToClients',
    minWidth: 120,
    slots: { default: 'allowSubscriptionToClients' },
    title: $t('Notifications.DisplayName:AllowSubscriptionToClients'),
  },
  {
    align: 'left',
    field: 'template',
    minWidth: 150,
    title: $t('Notifications.DisplayName:Template'),
  },
  {
    align: 'left',
    field: 'notificationLifetime',
    minWidth: 150,
    title: $t('Notifications.DisplayName:NotificationLifetime'),
  },
  {
    align: 'left',
    field: 'notificationType',
    minWidth: 150,
    title: $t('Notifications.DisplayName:NotificationType'),
  },
  {
    align: 'left',
    field: 'contentType',
    minWidth: 150,
    title: $t('Notifications.DisplayName:ContentType'),
  },
  {
    align: 'left',
    field: 'providers',
    minWidth: 150,
    slots: { default: 'providers' },
    title: $t('Notifications.DisplayName:Providers'),
  },
  {
    field: 'action',
    fixed: 'right',
    slots: { default: 'action' },
    title: $t('AbpUi.Actions'),
    width: 220,
  },
];

const gridEvents: VxeGridListeners<NotificationGroupDefinitionDto> = {
  pageChange(params) {
    pageState.current = params.currentPage;
    pageState.size = params.pageSize;
    onPageChange();
  },
};

const [GroupGrid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const [NotificationDefinitionModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./NotificationDefinitionModal.vue'),
  ),
});
const [NotificationSendModal, sendModalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./NotificationSendModal.vue'),
  ),
});

async function onGet(input?: Record<string, string>) {
  try {
    gridApi.setLoading(true);
    const groupRes = await getGroupsApi(input);
    const definitionRes = await getDefinitionsApi(input);
    pageState.total = groupRes.items.length;
    definitionGroups.value = groupRes.items.map((group) => {
      const localizableGroup = deserialize(group.displayName);
      const definitions = definitionRes.items
        .filter((definition) => definition.groupName === group.name)
        .map((definition) => {
          const displayName = deserialize(definition.displayName);
          const description = deserialize(definition.displayName);
          return {
            ...definition,
            contentType: notificationContentTypeMap[definition.contentType],
            description: Lr(description.resourceName, description.name),
            displayName: Lr(displayName.resourceName, displayName.name),
            notificationLifetime:
              notificationLifetimeMap[definition.notificationLifetime],
            notificationType: notificationTypeMap[definition.notificationType],
          };
        });
      return {
        ...group,
        displayName: Lr(localizableGroup.resourceName, localizableGroup.name),
        items: listToTree(definitions, {
          id: 'name',
          pid: 'parentName',
        }),
      };
    });
    onPageChange();
  } finally {
    gridApi.setLoading(false);
  }
}

async function onReset() {
  await gridApi.formApi.resetForm();
  const input = await gridApi.formApi.getValues();
  await onGet(input);
}

function onPageChange() {
  const items = definitionGroups.value.slice(
    (pageState.current - 1) * pageState.size,
    pageState.current * pageState.size,
  );
  gridApi.setGridOptions({
    data: items,
    pagerConfig: {
      currentPage: pageState.current,
      pageSize: pageState.size,
      total: pageState.total,
    },
  });
}

function onCreate() {
  modalApi.setData({});
  modalApi.open();
}

function onUpdate(row: NotificationDefinitionDto) {
  modalApi.setData(row);
  modalApi.open();
}

function onDelete(row: NotificationDefinitionDto) {
  Modal.confirm({
    centered: true,
    content: `${$t('AbpUi.ItemWillBeDeletedMessageWithFormat', [row.name])}`,
    onOk: async () => {
      await deleteApi(row.name);
      message.success($t('AbpUi.SuccessfullyDeleted'));
      onGet();
    },
    title: $t('AbpUi.AreYouSure'),
  });
}

function onMenuClick(row: NotificationDefinitionDto, info: MenuInfo) {
  switch (info.key) {
    case 'send': {
      sendModalApi.setData(row);
      sendModalApi.open();
    }
  }
}

onMounted(onGet);
</script>

<template>
  <GroupGrid :table-title="$t('Notifications.NotificationDefinitions')">
    <template #toolbar-tools>
      <Button
        :icon="h(PlusOutlined)"
        type="primary"
        v-access:code="[NotificationDefinitionsPermissions.Create]"
        @click="onCreate"
      >
        {{ $t('Notifications.NotificationDefinitions:AddNew') }}
      </Button>
    </template>
    <template #group="{ row: group }">
      <VxeGrid
        :columns="subGridColumns"
        :data="group.items"
        :tree-config="{
          trigger: 'row',
          rowField: 'name',
          childrenField: 'children',
        }"
      >
        <template #allowSubscriptionToClients="{ row }">
          <div class="flex flex-row justify-center">
            <CheckOutlined
              v-if="row.allowSubscriptionToClients"
              class="text-green-500"
            />
            <CloseOutlined v-else class="text-red-500" />
          </div>
        </template>
        <template #providers="{ row }">
          <div class="flex flex-row justify-center gap-1">
            <template v-for="provider in row.providers" :key="provider">
              <Tag color="blue">{{ provider }}</Tag>
            </template>
          </div>
        </template>
        <template #action="{ row: definition }">
          <div class="flex flex-row">
            <Button
              :icon="h(EditOutlined)"
              block
              type="link"
              v-access:code="[NotificationDefinitionsPermissions.Update]"
              @click="onUpdate(definition)"
            >
              {{ $t('AbpUi.Edit') }}
            </Button>
            <Button
              v-if="!definition.isStatic"
              :icon="h(DeleteOutlined)"
              block
              danger
              type="link"
              v-access:code="[NotificationDefinitionsPermissions.Delete]"
              @click="onDelete(definition)"
            >
              {{ $t('AbpUi.Delete') }}
            </Button>
            <Dropdown v-if="hasAccessByCodes([NotificationPermissions.Create])">
              <template #overlay>
                <Menu @click="(info) => onMenuClick(definition, info)">
                  <MenuItem key="send" :icon="h(SendIcon)">
                    {{ $t('Notifications.Notifications:Send') }}
                  </MenuItem>
                </Menu>
              </template>
              <Button :icon="h(EllipsisOutlined)" type="link" />
            </Dropdown>
          </div>
        </template>
      </VxeGrid>
    </template>
  </GroupGrid>
  <NotificationDefinitionModal @change="() => onGet()" />
  <NotificationSendModal />
</template>

<style scoped></style>
