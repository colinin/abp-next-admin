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
      label-align="right"
      :label-col="{ span: 4 }"
      :wrapper-col="{ span: 18 }"
      :model="modelRef"
    >
      <Tabs v-model:activeKey="activeKey">
        <TabPane key="basic" :tab="L('BasicInfo')">
          <FormItem :label="L('DisplayName:TenantId')">
            <Input readonly :value="getTenant" />
          </FormItem>
          <FormItem :label="L('DisplayName:SendExactSameData')">
            <Checkbox disabled v-model:checked="modelRef.sendExactSameData">{{ L('DisplayName:SendExactSameData') }}</Checkbox>
          </FormItem>
          <FormItem :label="L('DisplayName:CreationTime')">
            <Input readonly :value="getDateTime(modelRef.creationTime)" />
          </FormItem>
          <FormItem :label="L('DisplayName:RequestHeaders')">
            <CodeEditor readonly style="height: 300px;" :mode="MODE.JSON" v-model:value="modelRef.requestHeaders" />
          </FormItem>
          <FormItem :label="L('DisplayName:ResponseStatusCode')">
            <Tag v-if="modelRef.responseStatusCode" :color="getHttpStatusColor(modelRef.responseStatusCode)">{{ httpStatusCodeMap[modelRef.responseStatusCode] }}</Tag>
          </FormItem>
          <FormItem :label="L('DisplayName:ResponseHeaders')">
            <CodeEditor readonly style="height: 300px;" :mode="MODE.JSON" v-model:value="modelRef.responseHeaders" />
          </FormItem>
          <FormItem :label="L('DisplayName:Response')">
            <TextArea readonly v-model:value="modelRef.response" :auto-size="{ minRows: 10 }" />
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
            <CodeEditor readonly style="height: 300px;" :mode="MODE.JSON" v-model:value="modelRef.webhookEvent.data" />
          </FormItem>
        </TabPane>

        <TabPane v-if="subscriptionRef.id" key="subscription" :tab="L('Subscriptions')">
          <FormItem :label="L('DisplayName:IsActive')">
            <Checkbox disabled v-model:checked="subscriptionRef.isActive">{{ L('DisplayName:IsActive') }}</Checkbox>
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
            <TextArea readonly :value="getWebhooks(subscriptionRef.webhooks)" :auto-size="{ minRows: 5, maxRows: 10 }" />
          </FormItem>
          <FormItem name="headers" :label="L('DisplayName:Headers')">
            <CodeEditor readonly style="height: 300px;" :mode="MODE.JSON" v-model:value="subscriptionRef.headers" />
          </FormItem>
        </TabPane>
      </Tabs>
    </Form>
  </BasicModal>
</template>

<script lang="ts" setup>
  import { ref, computed, watch } from 'vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import {
    Checkbox,
    Form,
    Tabs,
    Tag,
    Input,
    InputPassword
  } from 'ant-design-vue';
  import { CodeEditor, MODE } from '/@/components/CodeEditor';
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
  const TextArea = Input.TextArea;

  const { L } = useLocalization('WebhooksManagement');
  const formElRef = ref<any>();
  const activeKey = ref('basic');
  const tenantName = ref('');
  const modelRef = ref<WebhookSendAttempt>(getDefaultModel());
  const subscriptionRef = ref<WebhookSubscription>(getDefaultSubscription());
  const [registerModal] = useModalInner((model) => {
    activeKey.value = 'basic';
    fetchModel(model.id);
  });
  const getDateTime = computed(() => {
    return (date?: Date) => {
      return date ? formatToDateTime(date) : '';
    }
  });
  const getWebhooks = computed(() => {
    return (webhooks: string[]) => {
      return webhooks.reduce((hook, p) => hook + p + '\n', '');
    }
  })
  const getTenant = computed(() => {
    return tenantName.value ?? modelRef.value.tenantId;
  })

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
    }
  )

  function fetchModel(id: string) {
    if (!id) {
      modelRef.value = getDefaultModel();
      return;
    }
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

  function getDefaultModel() : WebhookSendAttempt {
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
    }
  }

  function getDefaultSubscription() : WebhookSubscription {
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
