<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { LanguageDto } from '../../types/languages';

import { defineAsyncComponent, h, onMounted, ref } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { sortby, useAbpStore } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  PlusOutlined,
} from '@ant-design/icons-vue';
import { Button, message, Modal } from 'ant-design-vue';

import { useLanguagesApi } from '../../api/useLanguagesApi';
import { useLocalizationsApi } from '../../api/useLocalizationsApi';
import { LanguagesPermissions } from '../../constants/permissions';

defineOptions({
  name: 'LocalizationLanguageTable',
});

const dataSource = ref<LanguageDto[]>([]);

const abpStore = useAbpStore();
const { deleteApi, getListApi } = useLanguagesApi();
const { getLocalizationApi } = useLocalizationsApi();

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

const gridOptions: VxeGridProps<LanguageDto> = {
  columns: [
    {
      align: 'left',
      field: 'cultureName',
      minWidth: 150,
      sortable: true,
      title: $t('AbpLocalization.DisplayName:CultureName'),
    },
    {
      align: 'left',
      field: 'displayName',
      minWidth: 150,
      sortable: true,
      title: $t('AbpLocalization.DisplayName:DisplayName'),
    },
    {
      align: 'left',
      field: 'uiCultureName',
      minWidth: 150,
      sortable: true,
      title: $t('AbpLocalization.DisplayName:UiCultureName'),
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
        let items = sortby(dataSource.value, sort.field);
        if (sort.order === 'desc') {
          items = items.reverse();
        }
        const result = {
          totalCount: dataSource.value.length,
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

const gridEvents: VxeGridListeners<LanguageDto> = {
  sortChange: () => {
    gridApi.query();
  },
};

const [LocalizationLanguageModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./LocalizationLanguageModal.vue'),
  ),
});

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

async function onGet(input?: Record<string, string>) {
  try {
    gridApi.setLoading(true);
    const { items } = await getListApi(input);
    dataSource.value = items;
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

function onUpdate(row: LanguageDto) {
  modalApi.setData(row);
  modalApi.open();
}

function onDelete(row: LanguageDto) {
  Modal.confirm({
    centered: true,
    content: `${$t('AbpUi.ItemWillBeDeletedMessageWithFormat', [row.cultureName])}`,
    onOk: async () => {
      await deleteApi(row.cultureName);
      message.success($t('AbpUi.DeletedSuccessfully'));
      onChange(row);
    },
    title: $t('AbpUi.AreYouSure'),
  });
}

async function onChange(data: LanguageDto) {
  const input = await gridApi.formApi.getValues();
  onGet(input);
  // 资源变化刷新本地多语言缓存
  const cultureName =
    abpStore.application!.localization.currentCulture.cultureName;
  if (data.cultureName === cultureName) {
    const localization = await getLocalizationApi({
      cultureName,
    });
    abpStore.setLocalization(localization);
  }
}

onMounted(onGet);
</script>

<template>
  <Grid :table-title="$t('AbpLocalization.Languages')">
    <template #toolbar-tools>
      <Button
        :icon="h(PlusOutlined)"
        type="primary"
        v-access:code="[LanguagesPermissions.Create]"
        @click="onCreate"
      >
        {{ $t('LocalizationManagement.Language:AddNew') }}
      </Button>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          :icon="h(EditOutlined)"
          block
          type="link"
          v-access:code="[LanguagesPermissions.Update]"
          @click="onUpdate(row)"
        >
          {{ $t('AbpUi.Edit') }}
        </Button>
        <Button
          v-if="!row.isStatic"
          :icon="h(DeleteOutlined)"
          block
          danger
          type="link"
          v-access:code="[LanguagesPermissions.Delete]"
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
      </div>
    </template>
  </Grid>
  <LocalizationLanguageModal @change="onChange" />
</template>

<style scoped></style>
