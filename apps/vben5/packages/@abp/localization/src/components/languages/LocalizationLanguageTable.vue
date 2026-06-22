<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { LanguageDto } from '../../types/languages';

import { defineAsyncComponent, h } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useAbpStore } from '@abp/core';
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

const abpStore = useAbpStore();
const { deleteApi, getPagedListApi } = useLanguagesApi();
const { getLocalizationApi } = useLocalizationsApi();

const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: false,
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
