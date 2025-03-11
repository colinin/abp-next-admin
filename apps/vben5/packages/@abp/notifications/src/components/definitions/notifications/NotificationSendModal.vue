<script setup lang="ts">
import type { FormInstance } from 'ant-design-vue';

import type {
  NotificationDefinitionDto,
  NotificationSeverity,
} from '../../../types';

import { ref, toValue, useTemplateRef } from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { PropertyTable } from '@abp/ui';
import { Form, Input, message, Select, Tabs, Textarea } from 'ant-design-vue';

import { useNotificationsApi } from '../../../api/useNotificationsApi';
import { useEnumMaps } from './useEnumMaps';

const FormItem = Form.Item;
const TabPane = Tabs.TabPane;

const defaultModel: {
  culture?: string;
  data: Record<string, any>;
  description?: string;
  message: string;
  name: string;
  severity?: NotificationSeverity;
  title: string;
} = {
  data: {},
  message: '',
  name: '',
  title: '',
};

const activeTabKey = ref('basic');
const formModel = ref({ ...defaultModel });
const form = useTemplateRef<FormInstance>('form');
const notification = ref<NotificationDefinitionDto>();

const { notificationSeverityOptions } = useEnumMaps();
const { sendNotiferApi } = useNotificationsApi();

const [Modal, modalApi] = useVbenModal({
  class: 'w-1/2',
  draggable: true,
  fullscreenButton: false,
  onConfirm: onSubmit,
  async onOpenChange(isOpen) {
    if (isOpen) {
      formModel.value = { ...defaultModel };
      await onInit();
    }
  },
});

async function onInit() {
  const dto = modalApi.getData<NotificationDefinitionDto>();
  notification.value = dto;
  formModel.value.name = dto.name;
}

async function onSubmit() {
  await form.value?.validate();
  try {
    modalApi.setState({ submitting: true });
    const input = toValue(formModel);
    await sendNotiferApi({
      culture: input.culture,
      data: {
        description: input.description,
        message: input.message,
        title: input.title,
      },
      name: input.name,
      severity: input.severity,
    });
    message.success($t('Notifications.SendSuccessfully'));
    modalApi.close();
  } finally {
    modalApi.setState({ submitting: false });
  }
}
</script>

<template>
  <Modal :title="$t('Notifications.Notifications:Send')">
    <Form
      ref="form"
      :label-col="{ span: 6 }"
      :model="formModel"
      :wrapper-col="{ span: 18 }"
    >
      <Tabs v-model:active-key="activeTabKey">
        <TabPane key="basic" :tab="$t('Notifications.BasicInfo')">
          <FormItem name="name" :label="$t('Notifications.DisplayName:Name')">
            <Input disabled :value="notification?.displayName" />
          </FormItem>
          <FormItem
            v-if="!notification?.template"
            name="title"
            :label="$t('Notifications.Notifications:Title')"
            required
          >
            <Textarea v-model:value="formModel.title" />
          </FormItem>
          <FormItem
            v-if="!notification?.template"
            name="message"
            :label="$t('Notifications.Notifications:Message')"
            required
          >
            <Textarea v-model:value="formModel.message" />
          </FormItem>
          <FormItem
            v-if="!notification?.template"
            name="description"
            :label="$t('Notifications.Notifications:Description')"
          >
            <Textarea v-model:value="formModel.description" />
          </FormItem>
          <FormItem
            name="severity"
            :label="$t('Notifications.Notifications:Severity')"
          >
            <Select
              allow-clear
              v-model:value="formModel.severity"
              :options="notificationSeverityOptions"
            />
          </FormItem>
        </TabPane>
        <TabPane key="props" :tab="$t('Notifications.Properties')">
          <PropertyTable :data="formModel.data" />
        </TabPane>
      </Tabs>
    </Form>
  </Modal>
</template>

<style scoped></style>
