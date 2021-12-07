<template>
  <BasicModal
    v-bind="$attrs"
    @register="registerModal"
    :title="formTitle"
    :width="800"
    :min-height="400"
    @ok="handleSubmit"
    @cancel="handleCancel"
  >
    <Form
      ref="formElRef"
      :model="modelRef"
      :rules="formRules"
      :label-col="{ span: 6 }"
      :wrapper-col="{ span: 18 }"
    >
      <Tabs v-model:activeKey="activedTabRef">
        <TabPane key="info" :tab="L('Basic')" forceRender>
          <FormItem name="appId" :label="L('DisplayName:AppId')">
            <Select
              v-model:value="modelRef.appId"
              :options="appIdOptions"
              :disabled="modelRef.itemId !== undefined"
            />
          </FormItem>
          <FormItem name="baseUrl" :label="L('DisplayName:BaseUrl')">
            <Input v-model:value="modelRef.baseUrl" />
          </FormItem>
          <FormItem name="requestIdKey" :label="L('DisplayName:RequestIdKey')">
            <Input v-model:value="modelRef.requestIdKey" />
          </FormItem>
          <FormItem name="downstreamScheme" :label="L('DisplayName:DownstreamScheme')">
            <Input v-model:value="modelRef.downstreamScheme" />
          </FormItem>
          <FormItem name="downstreamHttpVersion" :label="L('DisplayName:DownstreamHttpVersion')">
            <Input v-model:value="modelRef.downstreamHttpVersion" />
          </FormItem>
        </TabPane>
        <TabPane key="httpOptions" :tab="L('HttpOptions')" forceRender>
          <FormItem
            name="httpHandlerOptions.maxConnectionsPerServer"
            :label="L('DisplayName:MaxConnectionsPerServer')"
          >
            <InputNumber
              style="width: 100%"
              v-model:value="modelRef.httpHandlerOptions.maxConnectionsPerServer"
            />
          </FormItem>
          <FormItem :name="['httpHandlerOptions', 'useProxy']" :label="L('DisplayName:UseProxy')">
            <RadioGroup
              v-model:value="modelRef.httpHandlerOptions.useProxy"
              :options="radioOptions"
            />
          </FormItem>
          <FormItem
            :name="['httpHandlerOptions', 'useTracing']"
            :label="L('DisplayName:UseTracing')"
          >
            <RadioGroup
              v-model:value="modelRef.httpHandlerOptions.useTracing"
              :options="radioOptions"
            />
          </FormItem>
          <FormItem
            :name="['httpHandlerOptions', 'allowAutoRedirect']"
            :label="L('DisplayName:AllowAutoRedirect')"
          >
            <RadioGroup
              v-model:value="modelRef.httpHandlerOptions.allowAutoRedirect"
              :options="radioOptions"
            />
          </FormItem>
          <FormItem
            :name="['httpHandlerOptions', 'useCookieContainer']"
            :label="L('DisplayName:UseCookieContainer')"
          >
            <RadioGroup
              v-model:value="modelRef.httpHandlerOptions.useCookieContainer"
              :options="radioOptions"
            />
          </FormItem>
        </TabPane>
        <TabPane key="rateLimit" :tab="L('RateLimit')" forceRender>
          <FormItem
            :name="['rateLimitOptions', 'disableRateLimitHeaders']"
            :label="L('DisplayName:DisableRateLimitHeaders')"
          >
            <RadioGroup
              v-model:value="modelRef.rateLimitOptions.disableRateLimitHeaders"
              :options="radioOptions"
            />
          </FormItem>
          <FormItem
            :name="['rateLimitOptions', 'clientIdHeader']"
            :label="L('DisplayName:ClientIdHeader')"
          >
            <Input v-model:value="modelRef.rateLimitOptions.clientIdHeader" />
          </FormItem>
          <FormItem
            :name="['rateLimitOptions', 'httpStatusCode']"
            :label="L('DisplayName:HttpStatusCode')"
          >
            <InputNumber
              style="width: 100%"
              v-model:value="modelRef.rateLimitOptions.httpStatusCode"
            />
          </FormItem>
          <FormItem
            :name="['rateLimitOptions', 'rateLimitCounterPrefix']"
            :label="L('DisplayName:RateLimitCounterPrefix')"
          >
            <Input v-model:value="modelRef.rateLimitOptions.rateLimitCounterPrefix" />
          </FormItem>
          <FormItem
            :name="['rateLimitOptions', 'quotaExceededMessage']"
            :label="L('DisplayName:QuotaExceededMessage')"
          >
            <TextArea
              v-model:value="modelRef.rateLimitOptions.quotaExceededMessage"
              :auto-size="{ minRows: 5, maxRows: 10 }"
            />
          </FormItem>
        </TabPane>
        <TabPane key="qos" :tab="L('Qos')" forceRender>
          <FormItem :name="['qoSOptions', 'timeoutValue']" :label="L('DisplayName:TimeoutValue')">
            <InputNumber style="width: 100%" v-model:value="modelRef.qoSOptions.timeoutValue" />
          </FormItem>
          <FormItem
            :name="['qoSOptions', 'durationOfBreak']"
            :label="L('DisplayName:DurationOfBreak')"
          >
            <InputNumber style="width: 100%" v-model:value="modelRef.qoSOptions.durationOfBreak" />
          </FormItem>
          <FormItem
            :name="['qoSOptions', 'exceptionsAllowedBeforeBreaking']"
            :label="L('DisplayName:ExceptionsAllowedBeforeBreaking')"
          >
            <InputNumber
              style="width: 100%"
              v-model:value="modelRef.qoSOptions.exceptionsAllowedBeforeBreaking"
            />
          </FormItem>
        </TabPane>
        <TabPane key="LoadBalancer" :tab="L('LoadBalancer')" forceRender>
          <FormItem
            :name="['loadBalancerOptions', 'type']"
            :label="L('DisplayName:LoadBalancerType')"
          >
            <Select v-model:value="modelRef.loadBalancerOptions.type" :options="balancerOptions" />
          </FormItem>
          <FormItem :name="['loadBalancerOptions', 'key']" :label="L('DisplayName:PollingKey')">
            <Input v-model:value="modelRef.loadBalancerOptions.key" />
          </FormItem>
          <FormItem :name="['loadBalancerOptions', 'expiry']" :label="L('DisplayName:Expiry')">
            <InputNumber style="width: 100%" v-model:value="modelRef.loadBalancerOptions.expiry" />
          </FormItem>
        </TabPane>
        <TabPane key="ServiceDiscovery" :tab="L('ServiceDiscovery')" forceRender>
          <FormItem
            :name="['serviceDiscoveryProvider', 'type']"
            :label="L('DisplayName:DiscoveryType')"
          >
            <Select
              v-model:value="modelRef.serviceDiscoveryProvider.type"
              :options="discoveryOptions"
            />
          </FormItem>
          <FormItem :name="['serviceDiscoveryProvider', 'host']" :label="L('DisplayName:Host')">
            <Input v-model:value="modelRef.serviceDiscoveryProvider.host" />
          </FormItem>
          <FormItem :name="['serviceDiscoveryProvider', 'port']" :label="L('DisplayName:Port')">
            <InputNumber
              style="width: 100%"
              v-model:value="modelRef.serviceDiscoveryProvider.port"
            />
          </FormItem>
          <FormItem :name="['serviceDiscoveryProvider', 'token']" :label="L('DisplayName:Token')">
            <Input v-model:value="modelRef.serviceDiscoveryProvider.token" />
          </FormItem>
          <FormItem
            :name="['serviceDiscoveryProvider', 'configurationKey']"
            :label="L('DisplayName:ConfigurationKey')"
          >
            <Input v-model:value="modelRef.serviceDiscoveryProvider.configurationKey" />
          </FormItem>
          <FormItem
            :name="['serviceDiscoveryProvider', 'namespace']"
            :label="L('DisplayName:Namespace')"
          >
            <Input v-model:value="modelRef.serviceDiscoveryProvider.namespace" />
          </FormItem>
          <FormItem :name="['serviceDiscoveryProvider', 'scheme']" :label="L('DisplayName:Scheme')">
            <Input v-model:value="modelRef.serviceDiscoveryProvider.scheme" />
          </FormItem>
          <FormItem
            :name="['serviceDiscoveryProvider', 'pollingInterval']"
            :label="L('DisplayName:PollingInterval')"
          >
            <InputNumber
              style="width: 100%"
              v-model:value="modelRef.serviceDiscoveryProvider.pollingInterval"
            />
          </FormItem>
        </TabPane>
      </Tabs>
    </Form>
  </BasicModal>
</template>

<script lang="ts">
  import { defineComponent, ref } from 'vue';

  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { Checkbox, Form, Tabs, Input, InputNumber, Select, RadioGroup } from 'ant-design-vue';
  import { Input as BInput } from '/@/components/Input';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useGlobalModal } from '../hooks/useGlobalModal';

  export default defineComponent({
    name: 'GlobalModal',
    components: {
      BasicModal,
      Checkbox,
      Form,
      FormItem: Form.Item,
      Input: BInput,
      TextArea: Input.TextArea,
      Tabs,
      TabPane: Tabs.TabPane,
      InputNumber,
      Select,
      RadioGroup,
    },
    emits: ['change', 'register'],
    setup(_, { emit }) {
      const { L } = useLocalization('ApiGateway');
      const appIdRef = ref('');
      const formElRef = ref<any>(null);
      const activedTabRef = ref('info');

      const [registerModal, modalMethods] = useModalInner((args) => {
        activedTabRef.value = 'info';
        appIdRef.value = args.appId;
      });

      const {
        modelRef,
        formTitle,
        formRules,
        radioOptions,
        appIdOptions,
        balancerOptions,
        discoveryOptions,
        handleCancel,
        handleSubmit,
      } = useGlobalModal({ emit, appIdRef, formElRef, modalMethods });

      return {
        L,
        modelRef,
        formTitle,
        formElRef,
        formRules,
        activedTabRef,
        radioOptions,
        appIdOptions,
        balancerOptions,
        discoveryOptions,
        registerModal,
        handleCancel,
        handleSubmit,
      };
    },
  });
</script>
