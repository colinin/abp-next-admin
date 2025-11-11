<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';
import type { MenuInfo } from 'ant-design-vue/es/menu/src/interface';

import type { VbenFormProps } from '@vben/common-ui';

import type { TextTemplateDefinitionDto } from '../../types/definitions';

import { defineAsyncComponent, h, onMounted, ref } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { sortby, useLocalization, useLocalizationSerializer } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  CheckOutlined,
  CloseOutlined,
  DeleteOutlined,
  EditOutlined,
  EllipsisOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, Dropdown, Menu, message, Modal } from 'ant-design-vue';

import { useTemplateDefinitionsApi } from '../../api/useTemplateDefinitionsApi';
import { WebhookDefinitionsPermissions } from '../../constants/permissions';

defineOptions({
  name: 'TemplateDefinitionTable',
});

const MenuItem = Menu.Item;

const textTemplates = ref<TextTemplateDefinitionDto[]>([]);

const { Lr } = useLocalization();
const { hasAccessByCodes } = useAccess();
const { deserialize } = useLocalizationSerializer();
const { deleteApi, getListApi } = useTemplateDefinitionsApi();

const formOptions: VbenFormProps = {
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
    {
      component: 'Checkbox',
      fieldName: 'isLayout',
      label: $t('AbpTextTemplating.DisplayName:IsLayout'),
    },
  ],
  showCollapseButton: true,
  submitOnEnter: true,
};

const gridOptions: VxeGridProps<TextTemplateDefinitionDto> = {
  columns: [
    {
      align: 'left',
      field: 'name',
      minWidth: 150,
      sortable: true,
      title: $t('AbpTextTemplating.DisplayName:Name'),
    },
    {
      align: 'left',
      field: 'displayName',
      minWidth: 150,
      sortable: true,
      title: $t('AbpTextTemplating.DisplayName:DisplayName'),
    },
    {
      align: 'center',
      field: 'isStatic',
      minWidth: 150,
      slots: { default: 'isStatic' },
      sortable: true,
      title: $t('AbpTextTemplating.DisplayName:IsStatic'),
    },
    {
      align: 'center',
      field: 'isInlineLocalized',
      minWidth: 150,
      slots: { default: 'isInlineLocalized' },
      sortable: true,
      title: $t('AbpTextTemplating.DisplayName:IsInlineLocalized'),
    },
    {
      align: 'center',
      field: 'isLayout',
      minWidth: 150,
      slots: { default: 'isLayout' },
      sortable: true,
      title: $t('AbpTextTemplating.DisplayName:IsLayout'),
    },
    {
      align: 'left',
      field: 'layout',
      minWidth: 150,
      sortable: true,
      title: $t('AbpTextTemplating.DisplayName:Layout'),
    },
    {
      align: 'left',
      field: 'defaultCultureName',
      minWidth: 150,
      sortable: true,
      title: $t('AbpTextTemplating.DisplayName:DefaultCultureName'),
    },
    {
      align: 'left',
      field: 'localizationResourceName',
      minWidth: 150,
      sortable: true,
      title: $t('AbpTextTemplating.LocalizationResource'),
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
      query: async ({ page, sort }) => {
        let items = sortby(textTemplates.value, sort.field);
        if (sort.order === 'desc') {
          items = items.reverse();
        }
        const result = {
          totalCount: textTemplates.value.length,
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

const gridEvents: VxeGridListeners<TextTemplateDefinitionDto> = {
  sortChange: () => {
    gridApi.query();
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const [TemplateDefinitionModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./TemplateDefinitionModal.vue'),
  ),
});
const [TemplateContentModal, contentModalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./TemplateContentModal.vue'),
  ),
});

async function onGet(input?: Record<string, string>) {
  try {
    gridApi.setLoading(true);
    const { items } = await getListApi(input);
    textTemplates.value = items.map((item) => {
      const localizableString = deserialize(item.displayName);
      return {
        ...item,
        displayName: Lr(localizableString.resourceName, localizableString.name),
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
  modalApi.setData({});
  modalApi.open();
}

function onUpdate(row: TextTemplateDefinitionDto) {
  modalApi.setData(row);
  modalApi.open();
}

function onMenuClick(row: TextTemplateDefinitionDto, info: MenuInfo) {
  switch (info.key) {
    case 'contents': {
      contentModalApi.setData(row);
      contentModalApi.open();
      break;
    }
  }
}

function onDelete(row: TextTemplateDefinitionDto) {
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
  <Grid :table-title="$t('AbpTextTemplating.TextTemplates')">
    <template #toolbar-tools>
      <Button
        :icon="h(PlusOutlined)"
        type="primary"
        v-access:code="[WebhookDefinitionsPermissions.Create]"
        @click="onCreate"
      >
        {{ $t('AbpTextTemplating.TextTemplates:AddNew') }}
      </Button>
    </template>
    <template #isStatic="{ row }">
      <div class="flex flex-row justify-center">
        <CheckOutlined v-if="row.isStatic" class="text-green-500" />
        <CloseOutlined v-else class="text-red-500" />
      </div>
    </template>
    <template #isInlineLocalized="{ row }">
      <div class="flex flex-row justify-center">
        <CheckOutlined v-if="row.isInlineLocalized" class="text-green-500" />
        <CloseOutlined v-else class="text-red-500" />
      </div>
    </template>
    <template #isLayout="{ row }">
      <div class="flex flex-row justify-center">
        <CheckOutlined v-if="row.isLayout" class="text-green-500" />
        <CloseOutlined v-else class="text-red-500" />
      </div>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          :icon="h(EditOutlined)"
          block
          type="link"
          v-access:code="[WebhookDefinitionsPermissions.Update]"
          @click="onUpdate(row)"
        >
          {{ $t('AbpUi.Edit') }}
        </Button>
        <Button
          v-if="
            !row.isStatic &&
            hasAccessByCodes([WebhookDefinitionsPermissions.Delete])
          "
          :icon="h(DeleteOutlined)"
          block
          danger
          type="link"
          v-access:code="[WebhookDefinitionsPermissions.Delete]"
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
        <Dropdown>
          <template #overlay>
            <Menu @click="(info) => onMenuClick(row, info)">
              <MenuItem key="contents" :icon="h(EditOutlined)">
                {{ $t('AbpTextTemplating.EditContents') }}
              </MenuItem>
            </Menu>
          </template>
          <Button :icon="h(EllipsisOutlined)" type="link" />
        </Dropdown>
      </div>
    </template>
  </Grid>
  <TemplateDefinitionModal @change="() => onGet()" />
  <TemplateContentModal />
</template>

<style scoped></style>
