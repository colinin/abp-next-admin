<script setup lang="ts">
import type {
  DataCreateDto,
  DataDto,
  DataUpdateDto,
} from '../../types/dataDictionaries';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { listToTree } from '@abp/core';
import { message } from 'ant-design-vue';

import { useDataDictionariesApi } from '../../api';

const emits = defineEmits<{
  (event: 'change', data: DataDto): void;
}>();

const { createApi, getAllApi, getApi, updateApi } = useDataDictionariesApi();

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
      component: 'ApiTreeSelect',
      componentProps: {
        allowClear: true,
        api: async () => {
          const { items } = await getAllApi();
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
        disabled: (values) => {
          return !!values?.id;
        },
        triggerFields: ['id'],
      },
      fieldName: 'parentId',
      label: $t('AppPlatform.DisplayName:ParentData'),
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
  try {
    modalApi.setState({ loading: true });
    formApi.resetForm();
    const { displayName, id, parentId } = modalApi.getData<DataDto>();
    let title = $t('AppPlatform.Data:AddNew');
    if (id) {
      const dto = await getApi(id);
      formApi.setValues(dto);
      title = `${$t('AppPlatform.Data:Edit')} - ${dto.name}`;
    } else if (parentId) {
      title = `${$t('AppPlatform.Data:AddChildren')} - ${displayName}`;
      formApi.setFieldValue('parentId', parentId);
    }
    modalApi.setState({ title });
  } finally {
    modalApi.setState({ loading: false });
  }
}

async function onSubmit(values: Record<string, any>) {
  try {
    modalApi.setState({ submitting: true });
    const api = values.id
      ? updateApi(values.id, values as DataUpdateDto)
      : createApi(values as DataCreateDto);
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
