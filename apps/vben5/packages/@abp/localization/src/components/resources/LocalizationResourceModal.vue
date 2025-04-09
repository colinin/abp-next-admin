<script setup lang="ts">
import type { FormInstance } from 'ant-design-vue';

import type { ResourceDto } from '../../types/resources';

import { defineEmits, defineOptions, ref, toValue, useTemplateRef } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { Checkbox, Form, Input, message } from 'ant-design-vue';

import { useResourcesApi } from '../../api/useResourcesApi';

defineOptions({
  name: 'LocalizationResourceModal',
});
const emits = defineEmits<{
  (event: 'change', data: ResourceDto): void;
}>();

const FormItem = Form.Item;

const defaultModel = {
  displayName: '',
  enable: true,
  name: '',
} as ResourceDto;

const form = useTemplateRef<FormInstance>('form');
const formModel = ref<ResourceDto>({ ...defaultModel });

const { cancel, createApi, getApi, updateApi } = useResourcesApi();
const [Modal, modalApi] = useVbenModal({
  class: 'w-1/2',
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onClosed() {
    cancel('LocalizationResourceModal has closed!');
  },
  onConfirm: onSubmit,
  onOpenChange: async (isOpen: boolean) => {
    if (isOpen) {
      formModel.value = { ...defaultModel };
      modalApi.setState({
        title: $t('LocalizationManagement.Resource:AddNew'),
      });
      const { name } = modalApi.getData<ResourceDto>();
      name && (await onGet(name));
    }
  },
  title: $t('LocalizationManagement.Resource:AddNew'),
});
async function onGet(name: string) {
  try {
    modalApi.setState({ loading: true });
    const dto = await getApi(name);
    formModel.value = dto;
    modalApi.setState({
      title: `${$t('AbpLocalization.Resources')} - ${dto.name}`,
    });
  } finally {
    modalApi.setState({ loading: false });
  }
}
async function onSubmit() {
  await form.value?.validate();
  try {
    modalApi.setState({ submitting: true });
    const input = toValue(formModel);
    const api = input.id ? updateApi(input.name, input) : createApi(input);
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
    <Form
      ref="form"
      :label-col="{ span: 6 }"
      :model="formModel"
      :wrapper-col="{ span: 18 }"
    >
      <FormItem
        :label="$t('LocalizationManagement.DisplayName:Enable')"
        name="enable"
      >
        <Checkbox v-model:checked="formModel.enable">
          {{ $t('LocalizationManagement.DisplayName:Enable') }}
        </Checkbox>
      </FormItem>
      <FormItem
        :label="$t('AbpLocalization.DisplayName:ResourceName')"
        name="name"
        required
      >
        <Input v-model:value="formModel.name" autocomplete="off" />
      </FormItem>
      <FormItem
        :label="$t('AbpLocalization.DisplayName:DisplayName')"
        name="displayName"
        required
      >
        <Input v-model:value="formModel.displayName" autocomplete="off" />
      </FormItem>
      <FormItem
        :label="$t('AbpLocalization.DisplayName:Description')"
        name="description"
      >
        <Input v-model:value="formModel.description" autocomplete="off" />
      </FormItem>
    </Form>
  </Modal>
</template>

<style scoped></style>
