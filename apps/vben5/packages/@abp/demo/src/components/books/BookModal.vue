<script setup lang="ts">
import type { EntityTypeInfoModel } from '@abp/data-protection';

import type { FormSchema } from '../../../../../@core/ui-kit/form-ui/src/types';
import type { BookDto, CreateUpdateBookDto } from '../../types/books';

import { nextTick, ref } from 'vue';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { DataAccessOperation } from '@abp/data-protection';
import { message } from 'ant-design-vue';

import { useAuthorsApi } from '../../api/useAuthorsApi';
import { useBooksApi } from '../../api/useBooksApi';
import { BookType } from '../../types/books';

const emits = defineEmits<{
  (event: 'change', data: BookDto): void;
}>();
const { getPagedListApi } = useAuthorsApi();
const { createApi, getApi, getEntityInfoApi, updateApi } = useBooksApi();

const allowShowFields = ref<string[]>([]);
const defaultFormSchema: FormSchema[] = [
  {
    component: 'Input',
    fieldName: 'id',
    formItemClass: 'hidden',
    label: 'id',
  },
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
        {
          label: $t('Demo.BookType:Undefined'),
          value: BookType.Undefined,
        },
        {
          label: $t('Demo.BookType:Adventure'),
          value: BookType.Adventure,
        },
        {
          label: $t('Demo.BookType:Biography'),
          value: BookType.Biography,
        },
        {
          label: $t('Demo.BookType:Dystopia'),
          value: BookType.Dystopia,
        },
        {
          label: $t('Demo.BookType:Fantastic'),
          value: BookType.Fantastic,
        },
        {
          label: $t('Demo.BookType:Horror'),
          value: BookType.Horror,
        },
        {
          label: $t('Demo.BookType:Science'),
          value: BookType.Science,
        },
        {
          label: $t('Demo.BookType:ScienceFiction'),
          value: BookType.ScienceFiction,
        },
        {
          label: $t('Demo.BookType:Poetry'),
          value: BookType.Poetry,
        },
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
      api: getPagedListApi,
      labelField: 'name',
      resultField: 'items',
      valueField: 'id',
    },
    fieldName: 'authorId',
    label: $t('Demo.DisplayName:AuthorId'),
    rules: 'selectRequired',
  },
];

const [Form, formApi] = useVbenForm({
  commonConfig: {
    colon: true,
    controlClass: 'w-full',
  },
  handleSubmit: onSubmit,
  schema: [],
  showDefaultActions: false,
});

const [Modal, modalApi] = useVbenModal({
  async onConfirm() {
    await formApi.validateAndSubmitForm();
  },
  async onOpenChange(isOpen) {
    if (isOpen) {
      await onInit();
      await onGet();
    }
  },
});

async function onInit() {
  const { id } = modalApi.getData<BookDto>();
  if (!id) {
    nextTick(() => {
      formApi.setState({
        schema: [...defaultFormSchema],
      });
    });
    return;
  }
  const [entityReadRule, entityWriteRule] = await Promise.all([
    getEntityInfoApi({
      operation: DataAccessOperation.Read,
    }),
    getEntityInfoApi({
      operation: DataAccessOperation.Write,
    }),
  ]);
  updateSchema(entityReadRule, entityWriteRule);
}

function updateSchema(
  readRule: EntityTypeInfoModel,
  writeRule: EntityTypeInfoModel,
) {
  const readProps = readRule.properties.map((x) => x.javaScriptName);
  const writeProps = new Set(writeRule.properties.map((x) => x.javaScriptName));
  const schemas = defaultFormSchema
    .filter((schema) => readProps.includes(schema.fieldName))
    .map((schema) => {
      return {
        ...schema,
        disabled: !writeProps.has(schema.fieldName),
      };
    });
  allowShowFields.value = readProps;

  nextTick(() => {
    formApi.setState({
      schema: schemas,
    });
  });
}

async function onGet() {
  formApi.resetForm();
  const { id } = modalApi.getData<BookDto>();
  if (id) {
    const dto = await getApi(id);
    allowShowFields.value.forEach((field) => {
      formApi.setFieldValue(field, Reflect.get(dto, field));
    });
  }
}

async function onSubmit(values: Record<string, any>) {
  const input = values as CreateUpdateBookDto;
  try {
    modalApi.setState({ submitting: true });
    const api = values.id ? updateApi(values.id, input) : createApi(input);
    const dto = await api;
    message.success($t('AbpUi.SavedSuccessfully'));
    emits('change', dto);
    modalApi.close();
  } finally {
    modalApi.setState({ submitting: false });
  }
}
</script>

<template>
  <Modal :title="$t('Demo.Book')">
    <Form />
  </Modal>
</template>

<style scoped></style>
