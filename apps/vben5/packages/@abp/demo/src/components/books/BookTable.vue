<script setup lang="ts">
import type { PrintDesignerInstance } from '@abp/print-designer';
import type { VxeGridListeners, VxeGridProps, VxeGridPropTypes } from '@abp/ui';

import type { VbenFormProps } from '@vben/common-ui';

import type { BookDto } from '../../types/books';

import {
  defineAsyncComponent,
  h,
  nextTick,
  onMounted,
  reactive,
  ref,
  useTemplateRef,
} from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { DataAccessOperation } from '@abp/data-protection';
import { PrintDesigner } from '@abp/print-designer';
import { useVbenVxeGrid } from '@abp/ui';
import {
  DeleteOutlined,
  EditOutlined,
  PlusOutlined,
  PrinterOutlined,
} from '@ant-design/icons-vue';
import { Button, message, Modal } from 'ant-design-vue';

import { useBooksApi } from '../../api/useBooksApi';
import { BookPermissions } from '../../constants/permissions';

defineOptions({
  name: 'BookTable',
});

const printDesigner = useTemplateRef<PrintDesignerInstance>('print-designer');
const selectedKeys = ref<string[]>([]);

const { deleteApi, getEntityInfoApi, getPagedListApi } = useBooksApi();
const formOptions: VbenFormProps = {
  // 默认展开
  collapsed: false,
  resetButtonOptions: {
    show: false,
  },
  schema: [
    {
      component: 'Input',
      componentProps: {
        allowClear: true,
        autocomplete: 'off',
      },
      fieldName: 'filter',
      formItemClass: 'col-span-2 items-baseline',
      label: $t('AbpUi.Search'),
    },
  ],
  // 控制表单是否显示折叠按钮
  showCollapseButton: false,
  // 按下回车时是否提交表单
  submitOnEnter: true,
};
const actionColumns = reactive<VxeGridPropTypes.Columns<BookDto>>([
  {
    field: 'action',
    fixed: 'right',
    slots: { default: 'action' },
    title: $t('AbpUi.Actions'),
    width: 180,
  },
]);
const baseColumns = reactive<VxeGridPropTypes.Columns<BookDto>>([
  {
    align: 'center',
    type: 'checkbox',
  },
  {
    align: 'left',
    field: 'name',
    minWidth: 150,
    sortable: true,
    title: $t('Demo.DisplayName:Name'),
  },
  {
    align: 'left',
    field: 'authorName',
    minWidth: 150,
    title: $t('Demo.DisplayName:AuthorId'),
  },
  {
    align: 'left',
    field: 'publishDate',
    minWidth: 200,
    sortable: true,
    title: $t('Demo.DisplayName:PublishDate'),
  },
  {
    align: 'left',
    field: 'price',
    minWidth: 150,
    sortable: true,
    title: $t('Demo.DisplayName:Price'),
  },
]);

const gridOptions: VxeGridProps<BookDto> = {
  columns: [...baseColumns, ...actionColumns],
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
    refresh: true,
    refreshOptions: {
      code: 'query',
    },
  },
};

const gridEvents: VxeGridListeners<BookDto> = {
  checkboxAll: (params) => {
    selectedKeys.value = params.records.map((x) => x.id);
  },
  checkboxChange: (params) => {
    selectedKeys.value = params.records.map((x) => x.id);
  },
  dataChange: () => {
    selectedKeys.value = [];
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
const [BookModal, modalApi] = useVbenModal({
  connectedComponent: defineAsyncComponent(() => import('./BookModal.vue')),
});

function onCreate() {
  modalApi.setData({});
  modalApi.open();
}

async function onPrint(record: BookDto) {
  if (!printDesigner.value) {
    return;
  }
  try {
    gridApi.setLoading(true);
    const designer = printDesigner.value;
    await designer.previewWithName({
      templateName: '书籍标签',
      variables: record,
    });
  } finally {
    gridApi.setLoading(false);
  }
}

async function onPrintMany() {
  if (!printDesigner.value) {
    return;
  }
  try {
    gridApi.setLoading(true);
    const designer = printDesigner.value;
    const data = gridApi.grid.getCheckboxRecords();
    await designer.printBatchWithName(
      data.map((d) => {
        return {
          variables: d,
        };
      }),
      {
        templateName: '书籍标签',
        label: {
          language: 'auto',
        },
      },
    );
  } finally {
    gridApi.setLoading(false);
  }
}

function onEdit(record: BookDto) {
  modalApi.setData(record);
  modalApi.open();
}

async function onDelete(record: BookDto) {
  Modal.confirm({
    centered: true,
    content: $t('AbpUi.ItemWillBeDeletedMessageWithFormat', [record.name]),
    maskClosable: true,
    onOk: async () => {
      await deleteApi(record.id);
      message.success($t('AbpUi.DeletedSuccessfully'));
      await gridApi.query();
    },
    title: $t('AbpUi.AreYouSure'),
  });
}

async function onInit() {
  const entityRule = await getEntityInfoApi({
    operation: DataAccessOperation.Read,
  });
  if (!entityRule.properties?.length) {
    return;
  }
  const allowProperties = new Set(
    entityRule.properties.map((x) => x.javaScriptName),
  );
  nextTick(() => {
    gridApi.setGridOptions({
      columns: [
        ...baseColumns.filter((x) => !x.field || allowProperties.has(x.field)),
        ...actionColumns,
      ],
    });
  });
}

onMounted(onInit);
</script>

<template>
  <Grid :table-title="$t('Demo.Book')">
    <template #toolbar-tools>
      <div class="flex flex-row gap-2">
        <Button
          type="primary"
          v-access:code="[BookPermissions.Create]"
          :icon="h(PlusOutlined)"
          @click="onCreate"
        >
          {{ $t('Demo.NewBook') }}
        </Button>
        <Button
          v-if="selectedKeys.length > 0"
          type="primary"
          :icon="h(PrinterOutlined)"
          ghost
          @click="onPrintMany"
        >
          {{ $t('Demo.Print') }}
        </Button>
      </div>
    </template>
    <template #action="{ row }">
      <div class="flex flex-row">
        <Button
          :icon="h(PrinterOutlined)"
          type="link"
          block
          @click="onPrint(row)"
        >
          {{ $t('Demo.Print') }}
        </Button>
        <Button
          v-access:code="[BookPermissions.Update]"
          :icon="h(EditOutlined)"
          type="link"
          block
          @click="onEdit(row)"
        >
          {{ $t('AbpUi.Edit') }}
        </Button>
        <Button
          v-access:code="[BookPermissions.Delete]"
          :icon="h(DeleteOutlined)"
          type="link"
          danger
          block
          @click="onDelete(row)"
        >
          {{ $t('AbpUi.Delete') }}
        </Button>
      </div>
    </template>
  </Grid>
  <BookModal @change="() => gridApi.query()" />
  <PrintDesigner ref="print-designer" headless />
</template>

<style lang="scss" scoped></style>
