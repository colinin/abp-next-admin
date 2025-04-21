<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface';

import type { VbenFormProps } from '@vben/common-ui';

import type { WebhookGroupDefinitionDto } from '../../../types/groups';

import { h, onMounted, reactive, ref } from 'vue';

import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { useLocalization, useLocalizationSerializer } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  EllipsisOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, Dropdown, Menu, message, Modal } from 'ant-design-vue';

import { useWebhookGroupDefinitionsApi } from '../../../api/useWebhookGroupDefinitionsApi';
import { GroupDefinitionsPermissions } from '../../../constants/permissions';

defineOptions({
  name: 'WebhookGroupDefinitionTable',
});

const MenuItem = Menu.Item;

const WebhookIcon = createIconifyIcon('material-symbols:webhook');

const permissionGroups = ref<WebhookGroupDefinitionDto[]>([]);
const pageState = reactive({
  current: 1,
  size: 10,
  total: 0,
});

const { Lr } = useLocalization();
const { deserialize } = useLocalizationSerializer();
const { deleteApi, getListApi } = useWebhookGroupDefinitionsApi();

const formOptions: VbenFormProps = {
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
  showCollapseButton: true,
  submitOnEnter: true,
};

const gridOptions: VxeGridProps<WebhookGroupDefinitionDto> = {
  columns: [
    {
      align: 'left',
      field: 'name',
      minWidth: 150,
      title: $t('WebhooksManagement.DisplayName:Name'),
    },
    {
      align: 'left',
      field: 'displayName',
      minWidth: 150,
      title: $t('WebhooksManagement.DisplayName:DisplayName'),
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
  toolbarConfig: {
    custom: true,
    export: true,
    refresh: false,
    zoom: true,
  },
};

const gridEvents: VxeGridListeners<WebhookGroupDefinitionDto> = {
  pageChange(params) {
    pageState.current = params.currentPage;
    pageState.size = params.pageSize;
    onPageChange();
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

async function onGet(input?: Record<string, string>) {
  try {
    gridApi.setLoading(true);
    const { items } = await getListApi(input);
    pageState.total = items.length;
    permissionGroups.value = items.map((item) => {
      const localizableString = deserialize(item.displayName);
      return {
        ...item,
        displayName: Lr(localizableString.resourceName, localizableString.name),
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
  const items = permissionGroups.value.slice(
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

function onCreate() {}

function onUpdate(_row: WebhookGroupDefinitionDto) {}

function onMenuClick(_row: WebhookGroupDefinitionDto, info: MenuInfo) {
  switch (info.key) {
    case 'permissions': {
      break;
    }
  }
}

function onDelete(row: WebhookGroupDefinitionDto) {
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
  <Grid :table-title="$t('WebhooksManagement.GroupDefinitions')">
    <template #toolbar-tools>
      <Button
        :icon="h(PlusOutlined)"
        type="primary"
        v-access:code="[GroupDefinitionsPermissions.Create]"
        @click="onCreate"
      >
        {{ $t('WebhooksManagement.GroupDefinitions:AddNew') }}
      </Button>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <div :class="`${row.isStatic ? 'w-full' : 'basis-1/3'}`">
          <Button
            :icon="h(EditOutlined)"
            block
            type="link"
            v-access:code="[GroupDefinitionsPermissions.Update]"
            @click="onUpdate(row)"
          >
            {{ $t('AbpUi.Edit') }}
          </Button>
        </div>
        <template v-if="!row.isStatic">
          <div class="basis-1/3">
            <Button
              :icon="h(DeleteOutlined)"
              block
              danger
              type="link"
              v-access:code="[GroupDefinitionsPermissions.Delete]"
              @click="onDelete(row)"
            >
              {{ $t('AbpUi.Delete') }}
            </Button>
          </div>
          <div class="basis-1/3">
            <Dropdown>
              <template #overlay>
                <Menu @click="(info) => onMenuClick(row, info)">
                  <MenuItem key="webhooks" :icon="h(WebhookIcon)">
                    {{ $t('WebhooksManagement.Webhooks:AddNew') }}
                  </MenuItem>
                </Menu>
              </template>
              <Button :icon="h(EllipsisOutlined)" type="link" />
            </Dropdown>
          </div>
        </template>
      </div>
    </template>
  </Grid>
</template>

<style scoped></style>
