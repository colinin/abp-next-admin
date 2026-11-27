<script setup lang="ts">
import type { AbpEditorOpenContext } from '@abp/components/crud/modal';
import type { EntityTypeInfoModel } from '@abp/data-protection';
import type { PrintDesignerInstance } from '@abp/print-designer';

import type {
  BaseFormComponentType,
  VbenFormProps,
  VbenFormSchema,
} from '@vben/common-ui';
import type {
  VxeGridListeners,
  VxeGridPropTypes,
} from '@vben/plugins/vxe-table';

import type { BookDto, CreateUpdateBookDto } from '../../types/books';

import {
  computed,
  h,
  nextTick,
  onMounted,
  reactive,
  ref,
  useTemplateRef,
} from 'vue';

import { globalShareState } from '@vben/common-ui';
import { createIconifyIcon } from '@vben/icons';
import { $t } from '@vben/locales';

import { useAbpEditorModal } from '@abp/components/crud/modal';
import { useAbpDataTable } from '@abp/components/crud/table';
import { DataAccessOperation } from '@abp/data-protection';
import { PrintDesigner } from '@abp/print-designer';

import { useAuthorsApi } from '../../api/useAuthorsApi';
import { useBooksApi } from '../../api/useBooksApi';
import { BookPermissions } from '../../constants/permissions';
import { BookType } from '../../types/books';

defineOptions({
  name: 'BookTable',
});

const PlusOutlined = createIconifyIcon('ant-design:plus-outlined');
const PrinterOutlined = createIconifyIcon('ant-design:printer-outlined');
const ButtonComponent = computed(
  () => globalShareState.getComponents()?.Button,
);

const printDesigner = useTemplateRef<PrintDesignerInstance>('print-designer');
const selectedKeys = ref<string[]>([]);
const {
  deleteApi,
  getApi,
  getEntityInfoApi,
  getPagedListApi,
  createApi,
  updateApi,
} = useBooksApi();
const { getPagedListApi: getAuthorsApi } = useAuthorsApi();

const defaultFormSchema: VbenFormSchema<
  BaseFormComponentType,
  Record<string, never>,
  CreateUpdateBookDto
>[] = [
  {
    component: 'Input',
    fieldName: 'name',
    label: $t('Demo.DisplayName:Name'),
    rules: 'required',
  },
  {
    component: 'Select',
    componentProps: {
      options: [
        { label: $t('Demo.BookType:Undefined'), value: BookType.Undefined },
        { label: $t('Demo.BookType:Adventure'), value: BookType.Adventure },
        { label: $t('Demo.BookType:Biography'), value: BookType.Biography },
        { label: $t('Demo.BookType:Dystopia'), value: BookType.Dystopia },
        { label: $t('Demo.BookType:Fantastic'), value: BookType.Fantastic },
        { label: $t('Demo.BookType:Horror'), value: BookType.Horror },
        { label: $t('Demo.BookType:Science'), value: BookType.Science },
        {
          label: $t('Demo.BookType:ScienceFiction'),
          value: BookType.ScienceFiction,
        },
        { label: $t('Demo.BookType:Poetry'), value: BookType.Poetry },
      ],
    },
    fieldName: 'type',
    label: $t('Demo.DisplayName:Type'),
    rules: 'selectRequired',
  },
  {
    component: 'DatePicker',
    componentProps: {
      valueFormat: 'YYYY-MM-DD',
    },
    fieldName: 'publishDate',
    label: $t('Demo.DisplayName:PublishDate'),
    rules: 'selectRequired',
  },
  {
    component: 'InputNumber',
    fieldName: 'price',
    label: $t('Demo.DisplayName:Price'),
    rules: 'required',
  },
  {
    component: 'ApiSelect',
    componentProps: {
      api: getAuthorsApi,
      labelField: 'name',
      resultField: 'items',
      valueField: 'id',
    },
    fieldName: 'authorId',
    label: $t('Demo.DisplayName:AuthorId'),
    rules: 'selectRequired',
  },
];

const formOptions: VbenFormProps = {
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
  showCollapseButton: false,
  submitOnEnter: true,
};

const actionColumn = {
  align: 'center' as const,
  field: 'action',
  fixed: 'right' as const,
  slots: { default: 'action' },
  title: $t('AbpUi.Actions'),
  width: 180,
};

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
    cellType: 'date',
    formatter: 'AutoFormatDate',
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
    tableApi.refresh();
  },
};

const [Table, tableApi] = useAbpDataTable<BookDto>({
  actionColumn: false,
  dropdownActions: (row) => [
    {
      onClick: () => onPrint(row),
      icon: h(PrinterOutlined),
      text: $t('Demo.Print'),
    },
  ],
  delete: (dto) => deleteApi(dto.id),
  deleteAuth: BookPermissions.Delete,
  deleteWarnMessage: (row) =>
    $t('AbpUi.ItemWillBeDeletedMessageWithFormat', [row.name]),
  edit: (row) => bookModalApi.open(row),
  editAuth: BookPermissions.Update,
  formOptions,
  gridEvents,
  gridOptions: {
    columns: [...baseColumns, actionColumn],
    exportConfig: {},
    height: 'auto',
    keepSource: true,
    toolbarConfig: {
      refresh: true,
      refreshOptions: {
        code: 'query',
      },
    },
  },
  load: async ({ pager, searchValues, sorting }) =>
    await getPagedListApi({
      maxResultCount: pager.maxResultCount,
      skipCount: pager.skipCount,
      sorting,
      ...searchValues,
    }),
});

async function onEditorOpen(
  context: AbpEditorOpenContext<BookDto, CreateUpdateBookDto>,
) {
  const { dto, formApi, mode } = context;
  if (mode === 'create' || !dto) {
    formApi.setState({ schema: defaultFormSchema });
    await nextTick();
    return;
  }

  const [readRule, writeRule] = await Promise.all([
    getEntityInfoApi({ operation: DataAccessOperation.Read }),
    getEntityInfoApi({ operation: DataAccessOperation.Write }),
  ]);
  updateSchema(formApi, readRule, writeRule);
  await nextTick();
}

function updateSchema(
  formApi: AbpEditorOpenContext<BookDto, CreateUpdateBookDto>['formApi'],
  readRule: EntityTypeInfoModel,
  writeRule: EntityTypeInfoModel,
) {
  const readProps = new Set(readRule.properties.map((x) => x.javaScriptName));
  const writeProps = new Set(writeRule.properties.map((x) => x.javaScriptName));
  const schema: typeof defaultFormSchema = defaultFormSchema
    .filter((item) => readProps.has(item.fieldName ?? ''))
    .map((item) => ({
      ...item,
      disabled: !writeProps.has(item.fieldName ?? ''),
    }));
  formApi.setState({ schema });
}

const [BookEditorModal, bookModalApi] = useAbpEditorModal<
  BookDto,
  CreateUpdateBookDto
>({
  create: createApi,
  formOptions: {
    commonConfig: {
      colon: true,
      controlClass: 'w-full',
    },
    schema: defaultFormSchema,
  },
  get: getApi,
  idKey: (dto) => dto.id,
  onOpen: onEditorOpen,
  refresh: async () => {
    await tableApi.refresh();
  },
  titleKey: (dto) => dto.name,
  update: updateApi,
});

function onCreate() {
  void bookModalApi.create();
}

async function onPrint(record: BookDto) {
  if (!printDesigner.value) {
    return;
  }
  try {
    tableApi.setLoading(true);
    const designer = printDesigner.value;
    await designer.printWithName({
      templateName: '书籍标签',
      variables: record,
    });
  } finally {
    tableApi.setLoading(false);
  }
}

async function onPrintMany() {
  if (!printDesigner.value) {
    return;
  }
  try {
    tableApi.setLoading(true);
    const designer = printDesigner.value;
    const data = tableApi.grid.getCheckboxRecords();
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
    tableApi.setLoading(false);
  }
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
    tableApi.setGridOptions({
      columns: [
        ...baseColumns.filter((x) => !x.field || allowProperties.has(x.field)),
        actionColumn,
      ],
    });
  });
}

onMounted(onInit);
</script>

<template>
  <Table :table-title="$t('Demo.Book')">
    <template #toolbar-tools>
      <div class="flex flex-row gap-2">
        <component
          v-access:code="[BookPermissions.Create]"
          :is="ButtonComponent"
          type="primary"
          @click="onCreate"
        >
          <PlusOutlined />
          {{ $t('Demo.NewBook') }}
        </component>
        <component
          v-if="selectedKeys.length > 0"
          :is="ButtonComponent"
          ghost
          type="primary"
          @click="onPrintMany"
        >
          <PrinterOutlined />
          {{ $t('Demo.Print') }}
        </component>
      </div>
    </template>
  </Table>
  <BookEditorModal />
  <PrintDesigner
    class="pointer-events-none fixed overflow-hidden !w-[1px] !h-[1px]"
    ref="print-designer"
    headless
  />
</template>

<style scoped></style>
