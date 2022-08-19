<template>
  <BasicModal
    @register="registerModal"
    :width="900"
    :height="500"
    :title="L('SendAttempts')"
    :mask-closable="false"
  >
    <Form
      ref="formElRef"
      :colon="true"
      :model="modelRef"
      layout="vertical"
    >
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
            <CodeEditorX readonly :mode="MODE.JSON" v-model="modelRef.requestHeaders" />
          </FormItem>
          <FormItem :label="L('DisplayName:ResponseStatusCode')">
            <Tag
              v-if="modelRef.responseStatusCode"
              :color="getHttpStatusColor(modelRef.responseStatusCode)"
              >{{ httpStatusCodeMap[modelRef.responseStatusCode] }}</Tag
            >
          </FormItem>
          <FormItem :label="L('DisplayName:ResponseHeaders')">
            <CodeEditorX readonly :mode="MODE.JSON" v-model="modelRef.responseHeaders" />
          </FormItem>
          <FormItem :label="L('DisplayName:Response')">
            <CodeEditorX readonly v-model="modelRef.response" />
          </FormItem>
        </TabPane>

        <TabPane v-if="modelRef.webhookEvent" key="event" :tab="L('WebhookEvent')">
          <FormItem :label="L('DisplayName:TenantId')">
            <Input readonly :value="getTenant" />
          </FormItem>
          <FormItem :label="L('DisplayName:WebhookEventId')">
            <Input readonly :value="modelRef.webhookEventId" />
          </FormItem>
          <FormItem :label="L('DisplayName:WebhookName')">
            <Input readonly :value="modelRef.webhookEvent.webhookName" />
          </FormItem>
          <FormItem :label="L('DisplayName:CreationTime')">
            <Input readonly :value="getDateTime(modelRef.webhookEvent.creationTime)" />
          </FormItem>
          <FormItem :label="L('DisplayName:Data')">
            <CodeEditorX readonly :mode="MODE.JSON" v-model="modelRef.webhookEvent.data" />
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
            <template v-for="(webhook) in subscriptionRef.webhooks" :key="webhook">
              <Tag color="#1c6a8f">{{ webhook }}</Tag>
            </template>
          </FormItem>
          <FormItem name="headers" :label="L('DisplayName:Headers')">
            <CodeEditorX readonly :mode="MODE.JSON" v-model="subscriptionRef.headers" />
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
  import { Checkbox, Form, Tabs, Tag, Input, InputPassword } from 'ant-design-vue';
  import { CodeEditorX, MODE } from '/@/components/CodeEditor';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { findTenantById } from '/@/api/multi-tenancy/tenants';
  import { getById } from '/@/api/webhooks/send-attempts';
  import { getById as getSubscription } from '/@/api/webhooks/subscriptions';
  import { WebhookSendAttempt } from '/@/api/webhooks/model/sendAttemptsModel';
  import { WebhookSubscription } from '/@/api/webhooks/model/subscriptionsModel';
  import { httpStatusCodeMap, getHttpStatusColor } from '../../typing';
  import { formatToDateTime } from '/@/utils/dateUtil';

  const FormItem = Form.Item;
  const TabPane = Tabs.TabPane;

  const { L } = useLocalization('WebhooksManagement');
  const formElRef = ref<any>();
  const activeKey = ref('basic');
  const tenantName = ref('');
  const tabsStyle = useTabsStyle();
  const modelRef = ref<WebhookSendAttempt>(getDefaultModel());
  const subscriptionRef = ref<WebhookSubscription>(getDefaultSubscription());
  const [registerModal] = useModalInner((model) => {
    activeKey.value = 'basic';
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
    getById(id).then((res) => {
      modelRef.value = res;
      fetchSubscription(res.webhookSubscriptionId);
    });
  }

  function fetchSubscription(id: string) {
    getSubscription(id).then((res) => {
      subscriptionRef.value = res;
    });
  }

  function getDefaultModel(): WebhookSendAttempt {
    return {
      id: '',
      webhookEventId: '',
      webhookSubscriptionId: '',
      webhookEvent: {
        tenantId: undefined,
        webhookName: '',
        data: '{}',
        creationTime: new Date(),
      },
      response: '',
      responseStatusCode: undefined,
      creationTime: new Date(),
      lastModificationTime: undefined,
      requestHeaders: {},
      responseHeaders: {},
      sendExactSameData: false,
    };
  }

  function getDefaultSubscription(): WebhookSubscription {
    return {
      id: '',
      webhooks: [],
      webhookUri: '',
      headers: {},
      secret: '',
      isActive: true,
      creatorId: '',
      creationTime: new Date(),
    };
  }
</script>
