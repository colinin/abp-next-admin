<template>
  <BasicModal
    @register="registerModal"
    :width="900"
    :height="500"
    :title="L('SendAttempts')"
    :mask-closable="false"
  >
    <Form ref="formElRef" :colon="true" :model="modelRef" layout="vertical">
      <Tabs
        v-model:activeKey="activeKey"
        :style="tabsStyle.style"
        :tabBarStyle="tabsStyle.tabBarStyle"
      >
        <TabPane key="basic" :tab="L('BasicInfo')">
          <FormItem :label="L('DisplayName:TenantId')">
            <Input readonly :value="getTenant" />
          </FormItem>
          <FormItem>
            <Checkbox disabled :checked="modelRef.sendExactSameData">{{
              L('DisplayName:SendExactSameData')
            }}</Checkbox>
          </FormItem>
          <FormItem :label="L('DisplayName:CreationTime')">
            <Input readonly :value="getDateTime(modelRef.creationTime)" />
          </FormItem>
          <FormItem :label="L('DisplayName:RequestHeaders')">
            <CodeEditor readonly :mode="MODE.JSON" :value="getJsonValue(modelRef.requestHeaders)" />
          </FormItem>
          <FormItem :label="L('DisplayName:ResponseStatusCode')">
            <Tag
              v-if="modelRef.responseStatusCode"
              :color="getHttpStatusColor(modelRef.responseStatusCode)"
              >{{ httpStatusCodeMap[modelRef.responseStatusCode] }}</Tag
            >
          </FormItem>
          <FormItem :label="L('DisplayName:ResponseHeaders')">
            <CodeEditor
              readonly
              :mode="MODE.JSON"
              :value="getJsonValue(modelRef.responseHeaders)"
            />
          </FormItem>
          <FormItem :label="L('DisplayName:Response')">
            <CodeEditor readonly :value="getJsonValue(modelRef.response)" />
          </FormItem>
        </TabPane>

        <TabPane v-if="modelRef.webhookEvent" key="event" :tab="L('WebhookEvent')">
          <FormItem :label="L('DisplayName:TenantId')">
            <Input readonly :value="getTenant" />
          </FormItem>
          <FormItem name="webhookEventId" :label="L('DisplayName:WebhookEventId')">
            <Input readonly :value="modelRef.webhookEventId" />
          </FormItem>
          <FormItem :name="['webhookEvent', 'webhookName']" :label="L('DisplayName:WebhookName')">
            <Input readonly :value="modelRef.webhookEvent.webhookName" />
          </FormItem>
          <FormItem :name="['webhookEvent', 'creationTime']" :label="L('DisplayName:CreationTime')">
            <Input readonly :value="getDateTime(modelRef.webhookEvent.creationTime)" />
          </FormItem>
          <FormItem :name="['webhookEvent', 'data']" :label="L('DisplayName:Data')">
            <CodeEditor readonly :mode="MODE.JSON" :value="modelRef.webhookEvent.data" />
          </FormItem>
        </TabPane>

        <TabPane v-if="subscriptionRef.id" key="subscription" :tab="L('Subscriptions')">
          <FormItem>
            <Checkbox disabled :checked="subscriptionRef.isActive">{{
              L('DisplayName:IsActive')
            }}</Checkbox>
          </FormItem>
          <FormItem :label="L('DisplayName:TenantId')">
            <Input readonly :value="getTenant" />
          </FormItem>
          <FormItem :label="L('DisplayName:WebhookSubscriptionId')">
            <Input readonly :value="modelRef.webhookSubscriptionId" />
          </FormItem>
          <FormItem :label="L('DisplayName:WebhookUri')">
            <Input readonly :value="subscriptionRef.webhookUri" />
          </FormItem>
          <FormItem :label="L('DisplayName:Description')">
            <Textarea
              readonly
              :value="subscriptionRef.description"
              :show-count="true"
              :auto-size="{ minRows: 3 }"
            />
          </FormItem>
          <FormItem :label="L('DisplayName:Secret')">
            <InputPassword readonly :value="subscriptionRef.secret" />
          </FormItem>
          <FormItem :label="L('DisplayName:CreationTime')">
            <Input readonly :value="getDateTime(subscriptionRef.creationTime)" />
          </FormItem>
          <FormItem :label="L('DisplayName:Webhooks')">
            <!-- <TextArea
              readonly
              :value="getWebhooks(subscriptionRef.webhooks)"
              :auto-size="{ minRows: 5, maxRows: 10 }"
            /> -->
            <template v-for="webhook in subscriptionRef.webhooks" :key="webhook">
              <Tag color="#1c6a8f">{{ webhook }}</Tag>
            </template>
          </FormItem>
          <FormItem name="headers" :label="L('DisplayName:Headers')">
            <CodeEditor readonly :mode="MODE.JSON" :value="getJsonValue(subscriptionRef.headers)" />
          </FormItem>
        </TabPane>
      </Tabs>
    </Form>
  </BasicModal>
</template>

<script lang="ts" setup>
  import { ref, unref, computed, watch } from 'vue';
  import { useTabsStyle } from '/@/hooks/component/useStyles';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { Checkbox, Form, Tabs, Tag, Input, InputPassword, Textarea } from 'ant-design-vue';
  import { CodeEditor, MODE } from '/@/components/CodeEditor';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { findTenantById } from '/@/api/multi-tenancy/tenants';
  import { GetAsyncById } from '/@/api/webhooks/send-attempts';
  import { GetAsyncById as getSubscription } from '/@/api/webhooks/subscriptions';
  import { WebhookSendAttempt } from '/@/api/webhooks/send-attempts/model';
  import { WebhookSubscription } from '/@/api/webhooks/subscriptions/model';
  import { httpStatusCodeMap, getHttpStatusColor } from '../../typing';
  import { formatToDateTime } from '/@/utils/dateUtil';
  import { isString } from '/@/utils/is';

  const FormItem = Form.Item;
  const TabPane = Tabs.TabPane;

  const { L } = useLocalization(['WebhooksManagement', 'AbpUi']);
  const formElRef = ref<any>();
  const activeKey = ref('basic');
  const tenantName = ref('');
  const tabsStyle = useTabsStyle();
  const modelRef = ref<WebhookSendAttempt>({} as WebhookSendAttempt);
  const subscriptionRef = ref<WebhookSubscription>({} as WebhookSubscription);
  const [registerModal] = useModalInner((model) => {
    activeKey.value = 'basic';
    modelRef.value = {} as WebhookSendAttempt;
    subscriptionRef.value = {} as WebhookSubscription;
    fetchModel(model.id);
  });
  const getDateTime = computed(() => {
    return (date?: Date) => {
      return date ? formatToDateTime(date) : '';
    };
  });
  const getTenant = computed(() => {
    return tenantName.value ?? modelRef.value.tenantId;
  });
  const getJsonValue = computed(() => {
    return (value?: Recordable | string) => {
      if (!value) return '{}';
      if (isString(value)) return value;
      return JSON.stringify(value);
    };
  });

  watch(
    () => modelRef.value.tenantId,
    (sentTenant) => {
      if (sentTenant) {
        findTenantById(sentTenant).then((res) => {
          if (res.success) {
            tenantName.value = res.name;
          }
        });
      }
    },
  );

  function fetchModel(id: string) {
    const formEl = unref(formElRef);
    formEl?.resetFields();
    GetAsyncById(id).then((res) => {
      modelRef.value = res;
      fetchSubscription(res.webhookSubscriptionId);
    });
  }

  function fetchSubscription(id: string) {
    getSubscription(id).then((res) => {
      subscriptionRef.value = res;
    });
  }
</script>
