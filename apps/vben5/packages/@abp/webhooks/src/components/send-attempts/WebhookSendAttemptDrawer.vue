<script setup lang="ts">
import type { TenantDto } from '@abp/saas';

import type { WebhookSendRecordDto, WebhookSubscriptionDto } from '../../types';

import { ref } from 'vue';

import { useAccess } from '@vben/access';
import { useVbenDrawer } from '@vben/common-ui';

import { CodeEditor } from '@abp/components/codeeditor';
import { Tinymce } from '@abp/components/tinymce';
import { formatToDateTime, isNullOrWhiteSpace } from '@abp/core';
import { useHttpStatusCodeMap } from '@abp/request';
import { useTenantsApi } from '@abp/saas';
import {
  Checkbox,
  DatePicker,
  Form,
  Input,
  InputPassword,
  Tabs,
  Tag,
  Textarea,
} from 'ant-design-vue';

import { useSendAttemptsApi, useSubscriptionsApi } from '../../api';
import { WebhookSubscriptionPermissions } from '../../constants/permissions';

const FormItem = Form.Item;
const TabPane = Tabs.TabPane;

type TabKey = 'basic' | 'event' | 'subscriber';

const activeTabKey = ref<TabKey>('basic');
const formModel = ref<WebhookSendRecordDto>();
const webhookSubscription = ref<WebhookSubscriptionDto>();
const webhookTenant = ref<TenantDto>();

const { hasAccessByCodes } = useAccess();
const { getApi } = useSendAttemptsApi();
const { getApi: getTenantApi } = useTenantsApi();
const { getApi: getSubscriptionApi } = useSubscriptionsApi();
const { getHttpStatusColor, httpStatusCodeMap } = useHttpStatusCodeMap();

const [Drawer, drawerApi] = useVbenDrawer({
  class: 'w-1/2',
  async onOpenChange(isOpen) {
    formModel.value = undefined;
    webhookTenant.value = undefined;
    webhookSubscription.value = undefined;
    if (isOpen) {
      await onInit();
    }
  },
});

async function onInit() {
  const dto = drawerApi.getData<WebhookSendRecordDto>();
  if (isNullOrWhiteSpace(dto.id)) {
    return;
  }
  const [sendRecordDto, subscriptionDto, tenantDto] = await Promise.all([
    getApi(dto.id),
    onInitSubscription(dto.webhookSubscriptionId),
    onInitTenant(dto.tenantId),
  ]);
  formModel.value = sendRecordDto;
  webhookSubscription.value = subscriptionDto;
  webhookTenant.value = tenantDto;
}

async function onInitSubscription(subscriptionId: string) {
  if (!hasAccessByCodes([WebhookSubscriptionPermissions.Default])) {
    return undefined;
  }
  return await getSubscriptionApi(subscriptionId);
}

async function onInitTenant(tenantId?: string) {
  // TODO: 公开saas模块常量?
  if (isNullOrWhiteSpace(tenantId) || !hasAccessByCodes(['AbpSaas.Tenants'])) {
    return undefined;
  }
  return await getTenantApi(tenantId);
}
</script>

<template>
  <Drawer :title="$t('WebhooksManagement.SendAttempts')">
    <Form :model="formModel" layout="vertical">
      <Tabs v-if="formModel" v-model:active-key="activeTabKey">
        <TabPane key="basic" :tab="$t('WebhooksManagement.BasicInfo')">
          <FormItem
            v-if="webhookTenant"
            name="tenantId"
            :label="$t('WebhooksManagement.DisplayName:TenantId')"
          >
            <Input :value="webhookTenant.name" disabled />
          </FormItem>
          <FormItem name="sendExactSameData">
            <Checkbox :checked="formModel.sendExactSameData" disabled>
              {{ $t('WebhooksManagement.DisplayName:SendExactSameData') }}
            </Checkbox>
          </FormItem>
          <FormItem
            name="creationTime"
            :label="$t('WebhooksManagement.DisplayName:CreationTime')"
          >
            <DatePicker
              class="w-full"
              value-format="YYYY-MM-DD HH:mm:ss"
              :value="formModel.creationTime"
              disabled
              show-time
            />
          </FormItem>
          <FormItem
            name="requestHeaders"
            :label="$t('WebhooksManagement.DisplayName:RequestHeaders')"
          >
            <CodeEditor :value="formModel.requestHeaders" readonly />
          </FormItem>
          <FormItem
            v-if="formModel.responseStatusCode"
            name="responseStatusCode"
            :label="$t('WebhooksManagement.DisplayName:ResponseStatusCode')"
          >
            <Tag :color="getHttpStatusColor(formModel.responseStatusCode)">
              {{ httpStatusCodeMap[formModel.responseStatusCode] }}
            </Tag>
          </FormItem>
          <FormItem
            name="responseHeaders"
            :label="$t('WebhooksManagement.DisplayName:ResponseHeaders')"
          >
            <CodeEditor :value="formModel.responseHeaders" readonly />
          </FormItem>
          <FormItem
            name="response"
            :label="$t('WebhooksManagement.DisplayName:Response')"
          >
            <Tinymce
              :value="formModel.response"
              :toolbar="[]"
              :plugins="[]"
              readonly
            />
          </FormItem>
        </TabPane>
        <TabPane key="event" :tab="$t('WebhooksManagement.WebhookEvent')">
          <FormItem
            name="webhookEventId"
            :label="$t('WebhooksManagement.DisplayName:WebhookEventId')"
          >
            <Input :value="formModel.webhookEventId" disabled />
          </FormItem>
          <FormItem
            :name="['webhookEvent', 'webhookName']"
            :label="$t('WebhooksManagement.DisplayName:WebhookName')"
          >
            <Input :value="formModel.webhookEvent.webhookName" disabled />
          </FormItem>
          <FormItem
            :name="['webhookEvent', 'creationTime']"
            :label="$t('WebhooksManagement.DisplayName:CreationTime')"
          >
            <DatePicker
              class="w-full"
              value-format="YYYY-MM-DD HH:mm:ss"
              :value="formModel.webhookEvent.creationTime"
              disabled
              show-time
            />
          </FormItem>
          <FormItem
            :name="['webhookEvent', 'data']"
            :label="$t('WebhooksManagement.DisplayName:Data')"
          >
            <CodeEditor :value="formModel.webhookEvent.data" readonly />
          </FormItem>
        </TabPane>
        <TabPane
          v-if="webhookSubscription"
          key="subscriber"
          :tab="$t('WebhooksManagement.Subscriptions')"
        >
          <FormItem>
            <Checkbox :checked="webhookSubscription.isActive" disabled>
              {{ $t('WebhooksManagement.DisplayName:IsActive') }}
            </Checkbox>
          </FormItem>
          <FormItem
            :label="$t('WebhooksManagement.DisplayName:WebhookSubscriptionId')"
          >
            <Input :value="webhookSubscription.id" disabled />
          </FormItem>
          <FormItem :label="$t('WebhooksManagement.DisplayName:WebhookUri')">
            <Input :value="webhookSubscription.webhookUri" disabled />
          </FormItem>
          <FormItem :label="$t('WebhooksManagement.DisplayName:Description')">
            <Textarea
              :auto-size="{ minRows: 3 }"
              :value="webhookSubscription.description"
              disabled
            />
          </FormItem>
          <FormItem :label="$t('WebhooksManagement.DisplayName:Secret')">
            <InputPassword :value="webhookSubscription.secret" disabled />
          </FormItem>
          <FormItem :label="$t('WebhooksManagement.DisplayName:CreationTime')">
            <DatePicker
              class="w-full"
              value-format="YYYY-MM-DD HH:mm:ss"
              :value="formatToDateTime(webhookSubscription.creationTime)"
              disabled
              show-time
            />
          </FormItem>
          <FormItem :label="$t('WebhooksManagement.DisplayName:Webhooks')">
            <template
              v-for="webhook in webhookSubscription.webhooks"
              :key="webhook"
            >
              <Tag color="blue">
                {{ webhook }}
              </Tag>
            </template>
          </FormItem>
          <FormItem :label="$t('WebhooksManagement.DisplayName:Headers')">
            <CodeEditor :value="webhookSubscription.headers" readonly />
          </FormItem>
        </TabPane>
      </Tabs>
    </Form>
  </Drawer>
</template>

<style scoped></style>
