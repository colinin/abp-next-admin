<script setup lang="ts">
import type { PropertyInfo } from '@abp/ui';
import type { FormInstance } from 'ant-design-vue';

import type { NotificationGroupDefinitionDto } from '../../../types';
import type {
  NotificationDefinitionDto,
  NotificationProviderDto,
  NotificationTemplateDto,
} from '../../../types/definitions';

import { defineEmits, defineOptions, ref, toValue, useTemplateRef } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { useLocalization, useLocalizationSerializer } from '@abp/core';
import { LocalizableInput, PropertyTable } from '@abp/ui';
import { Checkbox, Form, Input, message, Select, Tabs } from 'ant-design-vue';

import {
  useNotificationGroupDefinitionsApi,
  useNotificationsApi,
} from '../../../api';
import { useNotificationDefinitionsApi } from '../../../api/useNotificationDefinitionsApi';
import {
  NotificationContentType,
  NotificationLifetime,
  NotificationType,
} from '../../../types';
import { useEnumMaps } from './useEnumMaps';

defineOptions({
  name: 'NotificationDefinitionModal',
});
const emits = defineEmits<{
  (event: 'change', data: NotificationDefinitionDto): void;
}>();

const FormItem = Form.Item;
const TabPane = Tabs.TabPane;

type TabKeys = 'basic' | 'props';

const defaultModel: NotificationDefinitionDto = {
  allowSubscriptionToClients: false,
  contentType: NotificationContentType.Text,
  displayName: '',
  extraProperties: {},
  groupName: '',
  isStatic: false,
  name: '',
  notificationLifetime: NotificationLifetime.Persistent,
  notificationType: NotificationType.Application,
};

const isEditModel = ref(false);
const activeTab = ref<TabKeys>('basic');
const form = useTemplateRef<FormInstance>('form');
const formModel = ref<NotificationDefinitionDto>({ ...defaultModel });
const assignableProviders = ref<NotificationProviderDto[]>([]);
const assignableTemplates = ref<NotificationTemplateDto[]>([]);
const notificationGroups = ref<NotificationGroupDefinitionDto[]>([]);

const {
  notificationContentTypeOptions,
  notificationLifetimeOptions,
  notificationTypeOptions,
} = useEnumMaps();
const { cancel, createApi, getApi, updateApi } =
  useNotificationDefinitionsApi();
const { getAssignableProvidersApi, getAssignableTemplatesApi } =
  useNotificationsApi();
const { getListApi: getGroupDefinitionsApi } =
  useNotificationGroupDefinitionsApi();
const { Lr } = useLocalization();
const { deserialize } = useLocalizationSerializer();
const [Modal, modalApi] = useVbenModal({
  class: 'w-1/2',
  draggable: true,
  fullscreenButton: false,
  onCancel() {
    modalApi.close();
  },
  onClosed() {
    cancel('Notifications.NotificationDefinitionModal has closed!');
  },
  onConfirm: async () => {
    await form.value?.validate();
    const input = toValue(formModel);
    const api = isEditModel.value
      ? updateApi(formModel.value.name, input)
      : createApi(input);
    try {
      modalApi.setState({ submitting: true });
      const res = await api;
      emits('change', res);
      message.success($t('AbpUi.SavedSuccessfully'));
      modalApi.close();
    } finally {
      modalApi.setState({ submitting: false });
    }
  },
  onOpenChange: async (isOpen: boolean) => {
    if (isOpen) {
      isEditModel.value = false;
      activeTab.value = 'basic';
      formModel.value = { ...defaultModel };
      modalApi.setState({
        loading: true,
        showConfirmButton: true,
        title: $t('Notifications.NotificationDefinitions:AddNew'),
      });
      try {
        const { groupName, name } =
          modalApi.getData<NotificationDefinitionDto>();
        await onInit(groupName);
        name && (await onGet(name));
      } finally {
        modalApi.setState({ loading: false });
      }
    }
  },
  title: $t('Notifications.NotificationDefinitions:AddNew'),
});
async function onGet(name: string) {
  isEditModel.value = true;
  const dto = await getApi(name);
  formModel.value = dto;
  modalApi.setState({
    showConfirmButton: !dto.isStatic,
    title: `${$t('Notifications.NotificationDefinitions')} - ${dto.name}`,
  });
}
async function onInit(groupName?: string) {
  const [getGroupsRes, getTemplateRes, getProviderRes] = await Promise.all([
    getGroupDefinitionsApi({ filter: groupName }),
    getAssignableTemplatesApi(),
    getAssignableProvidersApi(),
  ]);
  assignableProviders.value = getProviderRes.items;
  assignableTemplates.value = getTemplateRes.items;
  notificationGroups.value = getGroupsRes.items.map((group) => {
    const displayName = deserialize(group.displayName);
    const description = deserialize(group.description);
    return {
      ...group,
      description: Lr(description.resourceName, description.name),
      displayName: Lr(displayName.resourceName, displayName.name),
    };
  });
  if (getGroupsRes.items.length === 1) {
    formModel.value.groupName = getGroupsRes.items[0]!.name;
  }
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
        <TabPane key="basic" :tab="$t('Notifications.BasicInfo')">
          <FormItem
            name="groupName"
            :label="$t('Notifications.DisplayName:GroupName')"
            required
          >
            <Select
              :disabled="formModel.isStatic"
              :allow-clear="true"
              v-model:value="formModel.groupName"
              :options="notificationGroups"
              :field-names="{ label: 'displayName', value: 'name' }"
            />
          </FormItem>
          <FormItem
            :label="$t('Notifications.DisplayName:Name')"
            name="name"
            required
          >
            <Input
              v-model:value="formModel.name"
              :disabled="formModel.isStatic"
              autocomplete="off"
            />
          </FormItem>
          <FormItem
            :label="$t('Notifications.DisplayName:DisplayName')"
            name="displayName"
            required
          >
            <LocalizableInput
              v-model:value="formModel.displayName"
              :disabled="formModel.isStatic"
            />
          </FormItem>
          <FormItem
            name="description"
            :label="$t('Notifications.DisplayName:Description')"
          >
            <LocalizableInput
              :disabled="formModel.isStatic"
              :allow-clear="true"
              v-model:value="formModel.description"
            />
          </FormItem>
          <FormItem
            name="allowSubscriptionToClients"
            :label="$t('Notifications.DisplayName:AllowSubscriptionToClients')"
            :extra="$t('Notifications.Description:AllowSubscriptionToClients')"
          >
            <Checkbox
              :disabled="formModel.isStatic"
              v-model:checked="formModel.allowSubscriptionToClients"
            >
              {{ $t('Notifications.DisplayName:AllowSubscriptionToClients') }}
            </Checkbox>
          </FormItem>
          <FormItem
            name="notificationType"
            :label="$t('Notifications.DisplayName:NotificationType')"
            :extra="$t('Notifications.Description:NotificationType')"
          >
            <Select
              :disabled="formModel.isStatic"
              :allow-clear="true"
              v-model:value="formModel.notificationType"
              :options="notificationTypeOptions"
            />
          </FormItem>
          <FormItem
            name="notificationLifetime"
            :label="$t('Notifications.DisplayName:NotificationLifetime')"
            :extra="$t('Notifications.Description:NotificationLifetime')"
          >
            <Select
              :disabled="formModel.isStatic"
              :allow-clear="true"
              v-model:value="formModel.notificationLifetime"
              :options="notificationLifetimeOptions"
            />
          </FormItem>
          <FormItem
            name="contentType"
            :label="$t('Notifications.DisplayName:ContentType')"
            :extra="$t('Notifications.Description:ContentType')"
          >
            <Select
              :disabled="formModel.isStatic"
              :allow-clear="true"
              v-model:value="formModel.contentType"
              :options="notificationContentTypeOptions"
            />
          </FormItem>
          <FormItem
            name="providers"
            :label="$t('Notifications.DisplayName:Providers')"
            :extra="$t('Notifications.Description:Providers')"
          >
            <Select
              :disabled="formModel.isStatic"
              :allow-clear="true"
              mode="multiple"
              v-model:value="formModel.providers"
              :options="assignableProviders"
              :field-names="{ label: 'name', value: 'name' }"
            />
          </FormItem>
          <FormItem
            name="template"
            :label="$t('Notifications.DisplayName:Template')"
            :extra="$t('Notifications.Description:Template')"
          >
            <Select
              :disabled="formModel.isStatic"
              :allow-clear="true"
              v-model:value="formModel.template"
              :options="assignableTemplates"
              :field-names="{ label: 'name', value: 'name' }"
            />
          </FormItem>
        </TabPane>
        <!-- 属性 -->
        <TabPane key="props" :tab="$t('Notifications.Properties')">
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
