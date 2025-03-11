<script setup lang="ts">
import type { FormInstance } from 'ant-design-vue';

import type { NotificationDefinitionDto } from '../../../types';

import {
  computed,
  defineAsyncComponent,
  ref,
  toValue,
  useTemplateRef,
} from 'vue';

import { useVbenModal } from '@vben/common-ui';
import { $t } from '@vben/locales';

import { PropertyTable } from '@abp/ui';
import { Form, Input, message, Select, Tabs, Textarea } from 'ant-design-vue';

import { useNotificationDefinitionsApi } from '../../../api';
import { useNotificationsApi } from '../../../api/useNotificationsApi';
import { NotificationContentType, NotificationSeverity } from '../../../types';
import { useEnumMaps } from './useEnumMaps';

const FormItem = Form.Item;
const TabPane = Tabs.TabPane;

const MarkdownEditor = defineAsyncComponent(() =>
  import('@abp/components/vditor').then((module) => module.MarkdownEditor),
);

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
const { getApi } = useNotificationDefinitionsApi();

const getMarkdownToolbar = computed(() => {
  if (
    notification.value?.providers &&
    notification.value.providers.some((x) =>
      x.toLocaleLowerCase().includes('wechat'),
    )
  ) {
    // 微信仅支持部分markdown语法
    // see: https://developer.work.weixin.qq.com/document/path/96458#%E6%94%AF%E6%8C%81%E7%9A%84markdown%E8%AF%AD%E6%B3%95
    return ['headings', 'bold', 'italic', 'strike', 'link', 'code', 'quote'];
  }
  return undefined;
});

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
  const notificationDto = await getApi(dto.name);
  notification.value = {
    ...notificationDto,
    displayName: dto.displayName,
  };
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
            <MarkdownEditor
              v-if="
                notification?.contentType === NotificationContentType.Markdown
              "
              v-model="formModel.message"
              :height="300"
              :toolbar="getMarkdownToolbar"
            />
            <Textarea v-else v-model:value="formModel.message" />
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
