<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { WebhookDefinitionDto } from '../../../types/definitions';
import type { WebhookGroupDefinitionDto } from '../../../types/groups';

import { defineAsyncComponent, h, onMounted, ref } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import {
  listToTree,
  sortby,
  useLocalization,
  useLocalizationSerializer,
} from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  CheckOutlined,
  CloseOutlined,
  DeleteOutlined,
  EditOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, message, Modal, Tag } from 'ant-design-vue';
import { VxeGrid } from 'vxe-table';

import { useWebhookDefinitionsApi } from '../../../api/useWebhookDefinitionsApi';
import { useWebhookGroupDefinitionsApi } from '../../../api/useWebhookGroupDefinitionsApi';
import { WebhookDefinitionsPermissions } from '../../../constants/permissions';

defineOptions({
  name: 'WebhookDefinitionTable',
});

interface DefinitionItem {
  children: DefinitionItem[];
  description?: string;
  displayName: string;
  groupName: string;
  isEnabled: boolean;
  isStatic: boolean;
  name: string;
  requiredFeatures?: string[];
}
interface DefinitionGroup {
  displayName: string;
  items: DefinitionItem[];
  name: string;
}

const { Lr } = useLocalization();
const { deserialize } = useLocalizationSerializer();
const { getListApi: getGroupsApi } = useWebhookGroupDefinitionsApi();
const { deleteApi, getListApi: getDefinitionsApi } = useWebhookDefinitionsApi();

const webhookGroups = ref<DefinitionGroup[]>([]);

const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: false,
  handleReset: onReset,
  async handleSubmit(params) {
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

const gridOptions: VxeGridProps<WebhookGroupDefinitionDto> = {
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
      sortable: true,
      title: $t('WebhooksManagement.DisplayName:Name'),
    },
    {
      align: 'left',
      field: 'displayName',
      minWidth: 150,
      sortable: true,
      title: $t('WebhooksManagement.DisplayName:DisplayName'),
    },
  ],
  expandConfig: {
    padding: true,
    trigger: 'row',
  },
  exportConfig: {},
  keepSource: true,
  proxyConfig: {
    ajax: {
      query: async ({ page, sort }) => {
        let items = sortby(webhookGroups.value, sort.field);
        if (sort.order === 'desc') {
          items = items.reverse();
        }
        const result = {
          totalCount: webhookGroups.value.length,
          items: items.slice(
            (page.currentPage - 1) * page.pageSize,
            page.currentPage * page.pageSize,
          ),
        };
        return new Promise((resolve) => {
          resolve(result);
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
    refresh: false,
    zoom: true,
  },
};
const subGridColumns: VxeGridProps<WebhookDefinitionDto>['columns'] = [
  {
    align: 'center',
    type: 'seq',
    width: 50,
  },
  {
    align: 'left',
    field: 'isEnabled',
    minWidth: 120,
    slots: { default: 'isEnabled' },
    sortable: true,
    title: $t('WebhooksManagement.DisplayName:IsEnabled'),
  },
  {
    align: 'left',
    field: 'name',
    minWidth: 150,
    sortable: true,
    title: $t('WebhooksManagement.DisplayName:Name'),
    treeNode: true,
  },
  {
    align: 'left',
    field: 'displayName',
    minWidth: 120,
    sortable: true,
    title: $t('WebhooksManagement.DisplayName:DisplayName'),
  },
  {
    align: 'left',
    field: 'description',
    minWidth: 120,
    sortable: true,
    title: $t('WebhooksManagement.DisplayName:Description'),
  },
  {
    align: 'left',
    field: 'requiredFeatures',
    minWidth: 150,
    slots: { default: 'requiredFeatures' },
    sortable: true,
    title: $t('WebhooksManagement.DisplayName:RequiredFeatures'),
  },
  {
    field: 'action',
    fixed: 'right',
    slots: { default: 'action' },
    title: $t('AbpUi.Actions'),
    width: 220,
  },
];

const gridEvents: VxeGridListeners<WebhookGroupDefinitionDto> = {
  sortChange: () => {
    gridApi.query();
  },
};

const [GroupGrid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const [WebhookDefinitionModal, groupModalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./WebhookDefinitionModal.vue'),
  ),
});

async function onGet(input?: Record<string, string>) {
  try {
    gridApi.setLoading(true);
    const groupRes = await getGroupsApi(input);
    const definitionRes = await getDefinitionsApi(input);
    webhookGroups.value = groupRes.items.map((group) => {
      const localizableGroup = deserialize(group.displayName);
      const definitions = definitionRes.items
        .filter((definition) => definition.groupName === group.name)
        .map((definition) => {
          const displayName = deserialize(definition.displayName);
          const description = deserialize(definition.displayName);
          return {
            ...definition,
            description: Lr(description.resourceName, description.name),
            displayName: Lr(displayName.resourceName, displayName.name),
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
    setTimeout(() => gridApi.reload(), 100);
  } finally {
    gridApi.setLoading(false);
  }
}

async function onReset() {
  await gridApi.formApi.resetForm();
  const input = await gridApi.formApi.getValues();
  await onGet(input);
}

function onCreate() {
  groupModalApi.setData({});
  groupModalApi.open();
}

function onUpdate(row: WebhookDefinitionDto) {
  groupModalApi.setData(row);
  groupModalApi.open();
}

function onDelete(row: WebhookDefinitionDto) {
  Modal.confirm({
    centered: true,
    content: `${$t('AbpUi.ItemWillBeDeletedMessageWithFormat', [row.name])}`,
    onOk: async () => {
      await deleteApi(row.name);
      message.success($t('AbpUi.DeletedSuccessfully'));
      onGet();
    },
    title: $t('AbpUi.AreYouSure'),
  });
}

onMounted(onGet);
</script>

<template>
  <GroupGrid :table-title="$t('WebhooksManagement.WebhookDefinitions')">
    <template #toolbar-tools>
      <Button
        :icon="h(PlusOutlined)"
        type="primary"
        v-access:code="[WebhookDefinitionsPermissions.Create]"
        @click="onCreate"
      >
        {{ $t('WebhooksManagement.Webhooks:AddNew') }}
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
        <template #isEnabled="{ row }">
          <div class="flex flex-row justify-center">
            <CheckOutlined v-if="row.isEnabled" class="text-green-500" />
            <CloseOutlined v-else class="text-red-500" />
          </div>
        </template>
        <template #requiredFeatures="{ row }">
          <div class="flex flex-row justify-center gap-1">
            <template v-for="feature in row.requiredFeatures" :key="feature">
              <Tag color="blue">{{ feature }}</Tag>
            </template>
          </div>
        </template>
        <template #action="{ row: definition }">
          <div class="flex flex-row">
            <Button
              :icon="h(EditOutlined)"
              block
              type="link"
              v-access:code="[WebhookDefinitionsPermissions.Update]"
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
              v-access:code="[WebhookDefinitionsPermissions.Delete]"
              @click="onDelete(definition)"
            >
              {{ $t('AbpUi.Delete') }}
            </Button>
          </div>
        </template>
      </VxeGrid>
    </template>
  </GroupGrid>
  <WebhookDefinitionModal @change="() => onGet()" />
</template>

<style scoped></style>
