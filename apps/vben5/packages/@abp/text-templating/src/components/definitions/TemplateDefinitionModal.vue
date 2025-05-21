<script setup lang="ts">
import type { ResourceDto } from '@abp/localization';
import type { PropertyInfo } from '@abp/ui';
import type { FormInstance } from 'ant-design-vue';

import type { TextTemplateDefinitionDto } from '../../types';

import { computed, ref, useTemplateRef } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import {
  isNullOrWhiteSpace,
  useAbpStore,
  useLocalization,
  useLocalizationSerializer,
} from '@abp/core';
import { useResourcesApi } from '@abp/localization';
import { LocalizableInput, PropertyTable } from '@abp/ui';
import { Checkbox, Form, Input, message, Select, Tabs } from 'ant-design-vue';

import { useTemplateDefinitionsApi } from '../../api';

defineOptions({
  name: 'TemplateDefinitionModal',
});
const emits = defineEmits<{
  (event: 'change', data: TextTemplateDefinitionDto): void;
}>();

const FormItem = Form.Item;
const TabPane = Tabs.TabPane;

type TabKey = 'basic' | 'properties';

const activeTabKey = ref<TabKey>('basic');
const isEditModel = ref(false);
const form = useTemplateRef<FormInstance>('form');
const formModel = ref<TextTemplateDefinitionDto>();
const textTemplateLayouts = ref<TextTemplateDefinitionDto[]>([]);
const textTemplateResources = ref<ResourceDto[]>([]);

const abpStore = useAbpStore();

const { Lr } = useLocalization();
const { hasAccessByCodes } = useAccess();
const { deserialize } = useLocalizationSerializer();
const { getListApi: getResourcesApi } = useResourcesApi();
const { cancel, createApi, getApi, getListApi, updateApi } =
  useTemplateDefinitionsApi();

const getLanguageOptions = computed(() => {
  const languages = abpStore.application?.localization.languages;
  if (!languages) {
    return [];
  }
  return languages.map((language) => {
    return {
      label: language.displayName,
      value: language.cultureName,
    };
  });
});

const [Modal, modalApi] = useVbenModal({
  class: 'w-1/2',
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onClosed() {
    cancel('TemplateDefinitionModal has closed!');
  },
  onConfirm: async () => {
    await form.value?.validate();
    const api = isEditModel.value
      ? updateApi(formModel.value!.name, formModel.value!)
      : createApi(formModel.value!);
    modalApi.setState({ submitting: true });
    api
      .then((res) => {
        message.success($t('AbpUi.SavedSuccessfully'));
        emits('change', res);
        modalApi.close();
      })
      .finally(() => {
        modalApi.setState({ submitting: false });
      });
  },
  onOpenChange: async (isOpen: boolean) => {
    if (isOpen) {
      isEditModel.value = false;
      activeTabKey.value = 'basic';
      formModel.value = undefined;
      await Promise.all([onGet(), onInitLayouts(), onInitResources()]);
    }
  },
  title: $t('AbpTextTemplating.TextTemplates:AddNew'),
});

async function onGet() {
  const { name } = modalApi.getData<TextTemplateDefinitionDto>();
  if (isNullOrWhiteSpace(name)) {
    modalApi.setState({ title: $t('AbpTextTemplating.TextTemplates:AddNew') });
    formModel.value = {
      displayName: '',
      extraProperties: {},
      isInlineLocalized: false,
      isLayout: false,
      isStatic: false,
      name: '',
    };
    return;
  }
  try {
    modalApi.setState({ loading: true });
    formModel.value = await getApi(name);
    modalApi.setState({
      title: `${$t('AbpTextTemplating.TextTemplates')} - ${name}`,
    });
  } finally {
    modalApi.setState({ loading: false });
  }
}

async function onInitLayouts() {
  const { items } = await getListApi({
    isLayout: true,
  });
  textTemplateLayouts.value = items.map((item) => {
    const localizableString = deserialize(item.displayName);
    return {
      ...item,
      displayName: Lr(localizableString.resourceName, localizableString.name),
    };
  });
}

async function onInitResources() {
  // TODO: 是否暴露localization、模块常量
  if (!hasAccessByCodes(['LocalizationManagement.Resource'])) {
    return;
  }
  const { items } = await getResourcesApi();
  textTemplateResources.value = items;
}

function onIsInlineLocalizedChange(isInlineLocalized: boolean) {
  if (isInlineLocalized) {
    formModel.value!.localizationResourceName = undefined;
  }
}

function onIsLayoutChange(isLayout: boolean) {
  if (isLayout) {
    formModel.value!.layout = undefined;
  }
}

function onPropChange(prop: PropertyInfo) {
  formModel.value!.extraProperties ??= {};
  formModel.value!.extraProperties[prop.key] = prop.value;
}

function onPropDelete(prop: PropertyInfo) {
  formModel.value!.extraProperties ??= {};
  delete formModel.value!.extraProperties[prop.key];
}
</script>

<template>
  <Modal>
    <Form
      v-if="formModel"
      ref="form"
      :label-col="{ span: 6 }"
      :model="formModel"
      :wrapper-col="{ span: 18 }"
    >
      <Tabs v-model:active-key="activeTabKey">
        <TabPane key="basic" :tab="$t('AbpTextTemplating.BasicInfo')">
          <FormItem
            name="name"
            required
            :label="$t('AbpTextTemplating.DisplayName:Name')"
          >
            <Input
              allow-clear
              autocomplete="off"
              :disabled="!isEditModel || formModel.isStatic"
              v-model:value="formModel.name"
            />
          </FormItem>
          <FormItem
            :label="$t('WebhooksManagement.DisplayName:DisplayName')"
            name="displayName"
            required
          >
            <LocalizableInput
              v-model:value="formModel.displayName"
              :disabled="formModel.isStatic"
            />
          </FormItem>
          <FormItem
            name="isInlineLocalized"
            :label="$t('AbpTextTemplating.DisplayName:IsInlineLocalized')"
          >
            <Checkbox
              :disabled="formModel.isStatic"
              v-model:checked="formModel.isInlineLocalized"
              @change="(e) => onIsInlineLocalizedChange(e.target.checked)"
            >
              {{ $t('AbpTextTemplating.DisplayName:IsInlineLocalized') }}
            </Checkbox>
          </FormItem>
          <FormItem
            v-if="!formModel.isInlineLocalized"
            name="defaultCultureName"
            :label="$t('AbpTextTemplating.DisplayName:DefaultCultureName')"
          >
            <Select
              allow-clear
              :disabled="formModel.isStatic"
              v-model:value="formModel.defaultCultureName"
              :options="getLanguageOptions"
            />
          </FormItem>
          <FormItem
            v-if="formModel.isInlineLocalized"
            name="localizationResourceName"
            :label="$t('AbpTextTemplating.LocalizationResource')"
          >
            <Select
              allow-clear
              :disabled="formModel.isStatic"
              v-model:value="formModel.localizationResourceName"
              :options="textTemplateResources"
              :field-names="{
                label: 'displayName',
                value: 'name',
              }"
            />
          </FormItem>
          <FormItem
            name="isLayout"
            :label="$t('AbpTextTemplating.DisplayName:IsLayout')"
          >
            <Checkbox
              :disabled="formModel.isStatic"
              v-model:checked="formModel.isLayout"
              @change="(e) => onIsLayoutChange(e.target.checked)"
            >
              {{ $t('AbpTextTemplating.DisplayName:IsLayout') }}
            </Checkbox>
          </FormItem>
          <FormItem
            v-if="!formModel.isLayout"
            name="layout"
            :label="$t('AbpTextTemplating.DisplayName:Layout')"
          >
            <Select
              allow-clear
              :disabled="formModel.isStatic"
              v-model:value="formModel.layout"
              :options="textTemplateLayouts"
              :field-names="{
                label: 'displayName',
                value: 'name',
              }"
            />
          </FormItem>
        </TabPane>
        <TabPane key="properties" :tab="$t('AbpTextTemplating.Properties')">
          <PropertyTable
            :data="formModel.extraProperties"
            :disabled="formModel.isStatic"
            @change="onPropChange"
            @delete="onPropDelete"
          />
        </TabPane>
      </Tabs>
    </Form>
  </Modal>
</template>

<style scoped></style>
