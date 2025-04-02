<script setup lang="ts">
import type { FormInstance } from 'ant-design-vue';

import type { LanguageDto } from '../../types/languages';
import type { ResourceDto } from '../../types/resources';
import type {
  GetTextByKeyInput,
  TextDifferenceDto,
  TextDto,
} from '../../types/texts';

import {
  defineEmits,
  defineOptions,
  onMounted,
  ref,
  toValue,
  useTemplateRef,
} from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { Form, Input, message, Select, Textarea } from 'ant-design-vue';

import { useLanguagesApi } from '../../api/useLanguagesApi';
import { useResourcesApi } from '../../api/useResourcesApi';
import { useTextsApi } from '../../api/useTextsApi';

defineOptions({
  name: 'LocalizationTextModal',
});
const emits = defineEmits<{
  (event: 'change', data: TextDto): void;
}>();

const FormItem = Form.Item;

const defaultModel = {} as TextDto;

const isEditModal = ref(false);
const form = useTemplateRef<FormInstance>('form');
const formModel = ref<TextDto>({ ...defaultModel });
const resources = ref<ResourceDto[]>([]);
const languages = ref<LanguageDto[]>([]);

const { getListApi: getLanguagesApi } = useLanguagesApi();
const { getListApi: getResourcesApi } = useResourcesApi();
const { cancel, getApi, setApi } = useTextsApi();

const [Modal, modalApi] = useVbenModal({
  class: 'w-1/2',
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onClosed() {
    cancel('LocalizationTextModal has closed!');
  },
  onConfirm: onSubmit,
  onOpenChange: async (isOpen: boolean) => {
    if (isOpen) {
      isEditModal.value = false;
      modalApi.setState({
        title: $t('LocalizationManagement.Text:AddNew'),
      });
      const modalData = modalApi.getData<TextDifferenceDto>();
      formModel.value = { ...modalData };
      await onGet();
    }
  },
  title: $t('LocalizationManagement.Text:AddNew'),
});

async function onInit() {
  const [languageRes, resourceRes] = await Promise.all([
    getLanguagesApi(),
    getResourcesApi(),
  ]);
  languages.value = languageRes.items;
  resources.value = resourceRes.items;
}

async function onGet() {
  const dto = modalApi.getData<TextDifferenceDto>();
  if (dto.targetCultureName) {
    isEditModal.value = true;
    await onLoad({
      cultureName: dto.targetCultureName,
      key: dto.key,
      resourceName: dto.resourceName,
    });
  }
}

async function onLanguageChange(value: string) {
  const dto = modalApi.getData<TextDifferenceDto>();
  if (dto.targetCultureName) {
    await onLoad({
      cultureName: value,
      key: dto.key,
      resourceName: dto.resourceName,
    });
  }
}

async function onLoad(input: GetTextByKeyInput) {
  try {
    modalApi.setState({ loading: true });
    const textDto = await getApi(input);
    formModel.value = textDto;
    modalApi.setState({
      title: `${$t('AbpLocalization.Texts')} - ${textDto.key}`,
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
    await setApi(input);
    message.success($t('AbpUi.SavedSuccessfully'));
    emits('change', input);
    modalApi.close();
  } finally {
    modalApi.setState({ submitting: false });
  }
}

onMounted(onInit);
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
        <Select
          v-model:value="formModel.cultureName"
          :options="languages"
          :field-names="{ label: 'displayName', value: 'cultureName' }"
          @change="(value) => onLanguageChange(value!.toString())"
        />
      </FormItem>
      <FormItem
        :label="$t('AbpLocalization.DisplayName:ResourceName')"
        name="resourceName"
        required
      >
        <Select
          v-model:value="formModel.resourceName"
          :options="resources"
          :disabled="isEditModal"
          :field-names="{ label: 'displayName', value: 'name' }"
        />
      </FormItem>
      <FormItem
        :label="$t('AbpLocalization.DisplayName:Key')"
        name="key"
        required
      >
        <Input
          :disabled="isEditModal"
          v-model:value="formModel.key"
          autocomplete="off"
        />
      </FormItem>
      <FormItem
        :label="$t('AbpLocalization.DisplayName:Value')"
        name="value"
        required
      >
        <Textarea
          :auto-size="{ minRows: 3 }"
          v-model:value="formModel.value"
          autocomplete="off"
        />
      </FormItem>
    </Form>
  </Modal>
</template>

<style scoped></style>
