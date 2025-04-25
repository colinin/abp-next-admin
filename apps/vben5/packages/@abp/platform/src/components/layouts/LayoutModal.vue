<script setup lang="ts">
import type {
  LayoutCreateDto,
  LayoutDto,
  LayoutUpdateDto,
} from '../../types/layouts';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { isNullOrWhiteSpace, listToTree } from '@abp/core';
import { message } from 'ant-design-vue';

import { useDataDictionariesApi, useLayoutsApi } from '../../api';

const emits = defineEmits<{
  (event: 'change', data: LayoutDto): void;
}>();

const { createApi, getApi, updateApi } = useLayoutsApi();
const {
  getAllApi: getDataDictionariesApi,
  getByNameApi: getDataDictionaryByNameApi,
} = useDataDictionariesApi();

const [Form, formApi] = useVbenForm({
  commonConfig: {
    componentProps: {
      class: 'w-full',
    },
  },
  handleSubmit: onSubmit,
  schema: [
    {
      component: 'Input',
      dependencies: {
        show: false,
        triggerFields: ['name'],
      },
      fieldName: 'id',
    },
    {
      component: 'ApiSelect',
      componentProps: {
        allowClear: true,
        api: () => getDataDictionaryByNameApi('UI Framework'),
        labelField: 'displayName',
        resultField: 'items',
        valueField: 'name',
      },
      dependencies: {
        if: (values) => {
          return !values?.id;
        },
        triggerFields: ['id'],
      },
      fieldName: 'framework',
      label: $t('AppPlatform.DisplayName:UIFramework'),
      rules: 'selectRequired',
    },
    {
      component: 'ApiTreeSelect',
      componentProps: {
        allowClear: true,
        api: async () => {
          const { items } = await getDataDictionariesApi();
          return listToTree(items, {
            id: 'id',
            pid: 'parentId',
          });
        },
        labelField: 'displayName',
        valueField: 'id',
        childrenField: 'children',
      },
      dependencies: {
        if: (values) => {
          return !values?.id;
        },
        triggerFields: ['id'],
      },
      fieldName: 'dataId',
      label: $t('AppPlatform.DisplayName:LayoutConstraint'),
      rules: 'selectRequired',
    },
    {
      component: 'Input',
      fieldName: 'name',
      label: $t('AppPlatform.DisplayName:Name'),
      rules: 'required',
    },
    {
      component: 'Input',
      fieldName: 'displayName',
      label: $t('AppPlatform.DisplayName:DisplayName'),
      rules: 'required',
    },
    {
      component: 'Input',
      fieldName: 'path',
      label: $t('AppPlatform.DisplayName:Path'),
      rules: 'required',
    },
    {
      component: 'Input',
      fieldName: 'redirect',
      label: $t('AppPlatform.DisplayName:Redirect'),
    },
    {
      component: 'Textarea',
      componentProps: {
        autoSize: {
          minRows: 3,
        },
      },
      fieldName: 'description',
      label: $t('AppPlatform.DisplayName:Description'),
    },
  ],
  showDefaultActions: false,
});
const [Modal, modalApi] = useVbenModal({
  closeOnClickModal: false,
  closeOnPressEscape: false,
  draggable: true,
  onConfirm: async () => {
    await formApi.validateAndSubmitForm();
  },
  onOpenChange: async (isOpen) => {
    if (isOpen) {
      await onGet();
    }
  },
});

async function onGet() {
  formApi.resetForm();
  const { id } = modalApi.getData<LayoutDto>();
  if (isNullOrWhiteSpace(id)) {
    modalApi.setState({ title: $t('AppPlatform.Layout:AddNew') });
    return;
  }
  try {
    modalApi.setState({ loading: true });
    const dto = await getApi(id);
    formApi.setValues(dto, false);
    modalApi.setState({
      title: `${$t('AppPlatform.Layout:Edit')} - ${dto.displayName}`,
    });
  } finally {
    modalApi.setState({ loading: false });
  }
}

async function onSubmit(values: Record<string, any>) {
  try {
    modalApi.setState({ submitting: true });
    const api = values.id
      ? updateApi(values.id, values as LayoutUpdateDto)
      : createApi(values as LayoutCreateDto);
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
  <Modal>
    <Form />
  </Modal>
</template>

<style scoped></style>
