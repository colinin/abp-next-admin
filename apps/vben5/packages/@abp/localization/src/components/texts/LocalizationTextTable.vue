<script setup lang="ts">
import type { VxeGridListeners, VxeGridProps } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { LanguageDto, ResourceDto } from '../../types';
import type { TextDifferenceDto, TextDto } from '../../types/texts';

import { defineAsyncComponent, h, onMounted, reactive, ref } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { sortby, useAbpStore } from '@abp/core';
import { useVbenVxeGrid } from '@abp/ui';
import { EditOutlined, PlusOutlined } from '@ant-design/icons-vue';
import { Button, Select } from 'ant-design-vue';

import { useLanguagesApi } from '../../api/useLanguagesApi';
import { useLocalizationsApi } from '../../api/useLocalizationsApi';
import { useResourcesApi } from '../../api/useResourcesApi';
import { useTextsApi } from '../../api/useTextsApi';
import { TextsPermissions } from '../../constants/permissions';

defineOptions({
  name: 'LocalizationTextTable',
});

const dataSource = ref<TextDifferenceDto[]>([]);
const resources = ref<ResourceDto[]>([]);
const languages = ref<LanguageDto[]>([]);
const targetValueOptions = reactive([
  {
    label: $t('AbpLocalization.DisplayName:Any'),
    value: 'false',
  },
  {
    label: $t('AbpLocalization.DisplayName:OnlyNull'),
    value: 'true',
  },
]);

const abpStore = useAbpStore();
const { getListApi } = useTextsApi();
const { getListApi: getLanguagesApi } = useLanguagesApi();
const { getListApi: getResourcesApi } = useResourcesApi();
const { getLocalizationApi } = useLocalizationsApi();

const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: false,
  collapsedRows: 2,
  commonConfig: {
    // 所有表单项
    componentProps: {
      class: 'w-full',
    },
  },
  handleReset: onReset,
  async handleSubmit(params) {
    await onGet(params);
  },
  schema: [
    {
      component: 'Input',
      fieldName: 'cultureName',
      formItemClass: 'items-baseline',
      label: $t('AbpLocalization.DisplayName:CultureName'),
      rules: 'selectRequired',
    },
    {
      component: 'Input',
      fieldName: 'targetCultureName',
      formItemClass: 'items-baseline',
      label: $t('AbpLocalization.DisplayName:TargetCultureName'),
      rules: 'selectRequired',
    },
    {
      component: 'Input',
      fieldName: 'resourceName',
      label: $t('AbpLocalization.DisplayName:ResourceName'),
    },
    {
      component: 'Input',
      fieldName: 'onlyNull',
      label: $t('AbpLocalization.DisplayName:TargetValue'),
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

const gridOptions: VxeGridProps<TextDifferenceDto> = {
  columns: [
    {
      align: 'left',
      field: 'key',
      minWidth: 150,
      sortable: true,
      title: $t('AbpLocalization.DisplayName:Key'),
    },
    {
      align: 'left',
      field: 'value',
      minWidth: 150,
      sortable: true,
      title: $t('AbpLocalization.DisplayName:Value'),
    },
    {
      align: 'left',
      field: 'targetValue',
      minWidth: 150,
      sortable: true,
      title: $t('AbpLocalization.DisplayName:TargetValue'),
    },
    {
      align: 'left',
      field: 'resourceName',
      minWidth: 150,
      sortable: true,
      title: $t('AbpLocalization.DisplayName:ResourceName'),
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

const gridEvents: VxeGridListeners<TextDifferenceDto> = {
  sortChange: () => {
    gridApi.query();
  },
};

const [Grid, gridApi] = useVbenVxeGrid({
  formOptions,
  gridEvents,
  gridOptions,
});

const [LocalizationTextModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(
    () => import('./LocalizationTextModal.vue'),
  ),
});

async function onInit() {
  const [languageRes, resourceRes] = await Promise.all([
    getLanguagesApi(),
    getResourcesApi(),
  ]);
  languages.value = languageRes.items;
  resources.value = resourceRes.items;
}

function onFieldChange(fieldName: string, value?: string) {
  gridApi.formApi.setFieldValue(fieldName, value);
}

async function onGet(input: Record<string, string>) {
  try {
    gridApi.setLoading(true);
    const { items } = await getListApi(input as any);
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

async function onCreate() {
  const input = await gridApi.formApi.getValues();
  modalApi.setData({
    cultureName: input.targetCultureName,
    resourceName: input.resourceName,
  });
  modalApi.open();
}

function onUpdate(row: TextDifferenceDto) {
  modalApi.setData(row);
  modalApi.open();
}

async function onChange(data: TextDto) {
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

onMounted(onInit);
</script>

<template>
  <Grid :table-title="$t('AbpLocalization.Texts')">
    <template #form-cultureName="{ modelValue }">
      <Select
        class="w-full"
        allow-clear
        :options="languages"
        :value="modelValue"
        :field-names="{ label: 'displayName', value: 'cultureName' }"
        @change="(value) => onFieldChange('cultureName', value?.toString())"
      />
    </template>
    <template #form-targetCultureName="{ modelValue }">
      <Select
        class="w-full"
        allow-clear
        :options="languages"
        :value="modelValue"
        :field-names="{ label: 'displayName', value: 'cultureName' }"
        @change="
          (value) => onFieldChange('targetCultureName', value?.toString())
        "
      />
    </template>
    <template #form-resourceName="{ modelValue }">
      <Select
        class="w-full"
        allow-clear
        :options="resources"
        :value="modelValue"
        :field-names="{ label: 'displayName', value: 'name' }"
        @change="(value) => onFieldChange('resourceName', value?.toString())"
      />
    </template>
    <template #form-onlyNull="{ modelValue }">
      <Select
        class="w-full"
        allow-clear
        :options="targetValueOptions"
        :value="modelValue"
        @change="(value) => onFieldChange('onlyNull', value?.toString())"
      />
    </template>
    <template #toolbar-tools>
      <Button
        :icon="h(PlusOutlined)"
        type="primary"
        v-access:code="[TextsPermissions.Create]"
        @click="onCreate"
      >
        {{ $t('LocalizationManagement.Text:AddNew') }}
      </Button>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          :icon="h(EditOutlined)"
          block
          type="link"
          v-access:code="[TextsPermissions.Update]"
          @click="onUpdate(row)"
        >
          {{ $t('AbpUi.Edit') }}
        </Button>
      </div>
    </template>
  </Grid>
  <LocalizationTextModal @change="onChange" />
</template>

<style scoped></style>
