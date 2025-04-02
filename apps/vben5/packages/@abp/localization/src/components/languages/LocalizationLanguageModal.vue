<script setup lang="ts">
import type { FormInstance } from 'ant-design-vue';

import type { LanguageDto } from '../../types/languages';

import { defineEmits, defineOptions, ref, toValue, useTemplateRef } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { Form, Input, message } from 'ant-design-vue';

import { useLanguagesApi } from '../../api/useLanguagesApi';

defineOptions({
  name: 'LocalizationLanguageModal',
});
const emits = defineEmits<{
  (event: 'change', data: LanguageDto): void;
}>();

const FormItem = Form.Item;

const defaultModel = {} as LanguageDto;

const form = useTemplateRef<FormInstance>('form');
const formModel = ref<LanguageDto>({ ...defaultModel });

const { cancel, createApi, getApi, updateApi } = useLanguagesApi();
const [Modal, modalApi] = useVbenModal({
  class: 'w-1/2',
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onClosed() {
    cancel('LocalizationLanguageModal has closed!');
  },
  onConfirm: onSubmit,
  onOpenChange: async (isOpen: boolean) => {
    if (isOpen) {
      formModel.value = { ...defaultModel };
      modalApi.setState({
        title: $t('LocalizationManagement.Language:AddNew'),
      });
      const { cultureName } = modalApi.getData<LanguageDto>();
      cultureName && (await onGet(cultureName));
    }
  },
  title: $t('LocalizationManagement.Language:AddNew'),
});
async function onGet(name: string) {
  try {
    modalApi.setState({ loading: true });
    const dto = await getApi(name);
    formModel.value = dto;
    modalApi.setState({
      title: `${$t('AbpLocalization.Languages')} - ${dto.cultureName}`,
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
    const api = input.id
      ? updateApi(input.cultureName, input)
      : createApi(input);
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
        :label="$t('AbpLocalization.DisplayName:CultureName')"
        name="cultureName"
        required
      >
        <Input v-model:value="formModel.cultureName" autocomplete="off" />
      </FormItem>
      <FormItem
        :label="$t('AbpLocalization.DisplayName:UiCultureName')"
        name="uiCultureName"
      >
        <Input v-model:value="formModel.uiCultureName" autocomplete="off" />
      </FormItem>
      <FormItem
        :label="$t('AbpLocalization.DisplayName:DisplayName')"
        name="displayName"
        required
      >
        <Input v-model:value="formModel.displayName" autocomplete="off" />
      </FormItem>
    </Form>
  </Modal>
</template>

<style scoped></style>
