<script setup lang="ts">
import type {
  TemplateCreateDto,
  TemplateDto,
  TemplateUpdateDto,
} from '../../types/templates';

import { useVbenForm, useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { getModalData, isNullOrWhiteSpace, useNotifications } from '@abp/core';

import { useTemplatesApi } from '../../api/useTemplatesApi';
const emits = defineEmits<{
  (event: 'change' | 'new', data: TemplateDto): void;
}>();

const { toastSuccess } = useNotifications();

const { createApi, getApi, updateApi } = useTemplatesApi();

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
      component: 'Input',
      fieldName: 'name',
      label: $t('PrintDesigner.DisplayName:Name'),
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
      label: $t('PrintDesigner.DisplayName:Description'),
    },
  ],
  showDefaultActions: false,
});
const [Modal, modalApi] = useVbenModal<TemplateDto>({
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
  const { id } = getModalData(modalApi);
  if (isNullOrWhiteSpace(id)) {
    modalApi.setState({ title: $t('PrintDesigner.Templates:Create') });
    return;
  }
  try {
    modalApi.setState({ loading: true });
    const dto = await getApi(id);
    formApi.setValues(dto, false);
    modalApi.setState({
      title: `${$t('PrintDesigner.Templates:Edit')} - ${dto.name}`,
    });
  } finally {
    modalApi.setState({ loading: false });
  }
}

async function onSubmit(values: Record<string, any>) {
  try {
    modalApi.setState({ submitting: true });
    const api = values.id
      ? updateApi(values.id, values as TemplateUpdateDto)
      : createApi(values as TemplateCreateDto);
    const dto = await api;
    toastSuccess($t('AbpUi.SavedSuccessfully'));
    if (!values.id) {
      emits('new', dto);
    }
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
