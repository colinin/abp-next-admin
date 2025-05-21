<script setup lang="ts">
import type { PropertyInfo } from '@abp/ui';
import type { FormInstance } from 'ant-design-vue';

import type { WebhookGroupDefinitionDto } from '../../../types/groups';

import { defineEmits, defineOptions, ref, toValue, useTemplateRef } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { LocalizableInput, PropertyTable } from '@abp/ui';
import { Form, Input, message, Tabs } from 'ant-design-vue';

import { useWebhookGroupDefinitionsApi } from '../../../api/useWebhookGroupDefinitionsApi';

defineOptions({
  name: 'WebhookGroupDefinitionModal',
});
const emits = defineEmits<{
  (event: 'change', data: WebhookGroupDefinitionDto): void;
}>();

const FormItem = Form.Item;
const TabPane = Tabs.TabPane;

type TabKeys = 'basic' | 'props';

const defaultModel = {} as WebhookGroupDefinitionDto;

const isEditModel = ref(false);
const activeTab = ref<TabKeys>('basic');
const form = useTemplateRef<FormInstance>('form');
const formModel = ref<WebhookGroupDefinitionDto>({ ...defaultModel });

const { cancel, createApi, getApi, updateApi } =
  useWebhookGroupDefinitionsApi();
const [Modal, modalApi] = useVbenModal({
  class: 'w-1/2',
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onClosed() {
    cancel('WebhookGroupDefinitionModal has closed!');
  },
  onConfirm: async () => {
    await form.value?.validate();
    const api = isEditModel.value
      ? updateApi(formModel.value.name, toValue(formModel))
      : createApi(toValue(formModel));
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
      activeTab.value = 'basic';
      formModel.value = { ...defaultModel };
      modalApi.setState({
        showConfirmButton: true,
        title: $t('WebhooksManagement.GroupDefinitions:AddNew'),
      });
      try {
        modalApi.setState({ loading: true });
        const { name } = modalApi.getData<WebhookGroupDefinitionDto>();
        name && (await onGet(name));
      } finally {
        modalApi.setState({ loading: false });
      }
    }
  },
  title: $t('WebhooksManagement.GroupDefinitions:AddNew'),
});
async function onGet(name: string) {
  isEditModel.value = true;
  const dto = await getApi(name);
  formModel.value = dto;
  modalApi.setState({
    showConfirmButton: !dto.isStatic,
    title: `${$t('WebhooksManagement.GroupDefinitions')} - ${dto.name}`,
  });
}
function onPropChange(prop: PropertyInfo) {
  formModel.value.extraProperties ??= {};
  formModel.value.extraProperties[prop.key] = prop.value;
}
function onPropDelete(prop: PropertyInfo) {
  formModel.value.extraProperties ??= {};
  delete formModel.value.extraProperties[prop.key];
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
      <Tabs v-model:active-key="activeTab">
        <!-- 基本信息 -->
        <TabPane key="basic" :tab="$t('WebhooksManagement.BasicInfo')">
          <FormItem
            :label="$t('WebhooksManagement.DisplayName:Name')"
            name="name"
            required
          >
            <Input
              v-model:value="formModel.name"
              :disabled="formModel.isStatic || isEditModel"
              autocomplete="off"
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
        </TabPane>
        <!-- 属性 -->
        <TabPane key="props" :tab="$t('WebhooksManagement.Properties')">
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
