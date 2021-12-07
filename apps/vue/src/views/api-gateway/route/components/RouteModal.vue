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
      :model="routeRef"
      :rules="formRules"
      :label-col="{ span: 6 }"
      :wrapper-col="{ span: 18 }"
    >
      <Tabs v-model:activeKey="activedTabRef">
        <TabPane key="info" :tab="L('Basic')" forceRender>
          <FormItem name="appId" :label="L('DisplayName:AppId')">
            <Select
              v-model:value="routeRef.appId"
              :options="appIdOptions"
              :disabled="routeRef.reRouteId !== undefined"
            />
          </FormItem>
          <FormItem name="reRouteName" :label="L('DisplayName:Name')">
            <Input v-model:value="routeRef.reRouteName" />
          </FormItem>
          <FormItem name="requestIdKey" :label="L('DisplayName:RequestIdKey')">
            <Input v-model:value="routeRef.requestIdKey" />
          </FormItem>
          <FormItem name="serviceName" :label="L('DisplayName:ServiceName')">
            <Input v-model:value="routeRef.serviceName" />
          </FormItem>
          <FormItem name="serviceNamespace" :label="L('DisplayName:Namespace')">
            <Input v-model:value="routeRef.serviceNamespace" />
          </FormItem>
          <FormItem name="timeout" :label="L('DisplayName:Timeout')">
            <InputNumber style="width: 100%" v-model:value="routeRef.timeout" />
          </FormItem>
          <FormItem name="priority" :label="L('DisplayName:Priority')">
            <InputNumber style="width: 100%" v-model:value="routeRef.priority" />
          </FormItem>
          <FormItem :label="L('DisplayName:CaseSensitive')">
            <Checkbox v-model:checked="routeRef.reRouteIsCaseSensitive">
              {{ L('DisplayName:CaseSensitive') }}
            </Checkbox>
          </FormItem>
          <FormItem :label="L('DisplayName:DangerousAcceptAnyServerCertificateValidator')">
            <Checkbox v-model:checked="routeRef.dangerousAcceptAnyServerCertificateValidator">
              {{ L('DisplayName:DangerousAcceptAnyServerCertificateValidator') }}
            </Checkbox>
          </FormItem>
        </TabPane>
        <TabPane key="forward" :tab="L('Forward')" forceRender>
          <FormItem name="downstreamScheme" :label="L('DisplayName:DownstreamScheme')">
            <Input v-model:value="routeRef.downstreamScheme" />
          </FormItem>
          <FormItem name="downstreamHttpVersion" :label="L('DisplayName:DownstreamHttpVersion')">
            <Input v-model:value="routeRef.downstreamHttpVersion" />
          </FormItem>
          <FormItem name="upstreamPathTemplate" :label="L('DisplayName:UpstreamPathTemplate')">
            <Input v-model:value="routeRef.upstreamPathTemplate" />
          </FormItem>
          <FormItem name="downstreamPathTemplate" :label="L('DisplayName:DownstreamPathTemplate')">
            <Input v-model:value="routeRef.downstreamPathTemplate" />
          </FormItem>
          <FormItem name="key" :label="L('DisplayName:AggregateKey')">
            <Input v-model:value="routeRef.key" />
          </FormItem>
          <FormItem name="upstreamHttpMethod" :label="L('DisplayName:UpstreamHttpMethod')">
            <Select
              mode="multiple"
              v-model:value="routeRef.upstreamHttpMethod"
              :options="httpMethods"
            />
          </FormItem>
          <FormItem name="downstreamHttpMethod" :label="L('DisplayName:DownstreamHttpMethod')">
            <Input v-model:value="routeRef.downstreamHttpMethod" />
          </FormItem>
          <FormItem name="downstreamHostAndPorts" :label="L('DisplayName:DownstreamHostAndPorts')">
            <Select
              mode="tags"
              :open="false"
              :value="getHostAndPorts"
              @change="onHostAndPortsChange"
            />
          </FormItem>
        </TabPane>
        <TabPane key="http" :tab="L('HttpOptions')" forceRender>
          <FormItem
            :name="['httpHandlerOptions', 'maxConnectionsPerServer']"
            :label="L('DisplayName:MaxConnectionsPerServer')"
          >
            <InputNumber
              style="width: 100%"
              v-model:value="routeRef.httpHandlerOptions.maxConnectionsPerServer"
            />
          </FormItem>
          <FormItem :name="['httpHandlerOptions', 'useProxy']" :label="L('DisplayName:UseProxy')">
            <RadioGroup
              v-model:value="routeRef.httpHandlerOptions.useProxy"
              :options="radioOptions"
            />
          </FormItem>
          <FormItem
            :name="['httpHandlerOptions', 'useTracing']"
            :label="L('DisplayName:UseTracing')"
          >
            <RadioGroup
              v-model:value="routeRef.httpHandlerOptions.useTracing"
              :options="radioOptions"
            />
          </FormItem>
          <FormItem
            :name="['httpHandlerOptions', 'allowAutoRedirect']"
            :label="L('DisplayName:AllowAutoRedirect')"
          >
            <RadioGroup
              v-model:value="routeRef.httpHandlerOptions.allowAutoRedirect"
              :options="radioOptions"
            />
          </FormItem>
          <FormItem
            :name="['httpHandlerOptions', 'useCookieContainer']"
            :label="L('DisplayName:UseCookieContainer')"
          >
            <RadioGroup
              buttonStyle="solid"
              v-model:value="routeRef.httpHandlerOptions.useCookieContainer"
              :options="radioOptions"
            />
          </FormItem>
        </TabPane>
        <TabPane key="processing" :tab="L('RequestProcessing')" forceRender>
          <FormItem name="addClaimsToRequest" :label="L('DisplayName:AddClaimsToRequest')">
            <Select
              mode="tags"
              :open="false"
              :value="getDictionaryValue('addClaimsToRequest')"
              @select="(val) => onDictionarySelect('addClaimsToRequest', val)"
              @deselect="(val) => onDictionaryUnSelect('addClaimsToRequest', val)"
            />
          </FormItem>
          <FormItem name="addHeadersToRequest" :label="L('DisplayName:AddHeadersToRequest')">
            <Select
              mode="tags"
              :open="false"
              :value="getDictionaryValue('addHeadersToRequest')"
              @select="(val) => onDictionarySelect('addHeadersToRequest', val)"
              @deselect="(val) => onDictionaryUnSelect('addHeadersToRequest', val)"
            />
          </FormItem>
          <FormItem
            name="upstreamHeaderTransform"
            :label="L('DisplayName:UpstreamHeaderTransform')"
          >
            <Select
              mode="tags"
              :open="false"
              :value="getDictionaryValue('upstreamHeaderTransform')"
              @select="(val) => onDictionarySelect('upstreamHeaderTransform', val)"
              @deselect="(val) => onDictionaryUnSelect('upstreamHeaderTransform', val)"
            />
          </FormItem>
          <FormItem
            name="downstreamHeaderTransform"
            :label="L('DisplayName:DownstreamHeaderTransform')"
          >
            <Select
              mode="tags"
              :open="false"
              :value="getDictionaryValue('downstreamHeaderTransform')"
              @select="(val) => onDictionarySelect('downstreamHeaderTransform', val)"
              @deselect="(val) => onDictionaryUnSelect('downstreamHeaderTransform', val)"
            />
          </FormItem>
          <FormItem
            name="changeDownstreamPathTemplate"
            :label="L('DisplayName:ChangeDownstreamPathTemplate')"
          >
            <Select
              mode="tags"
              :open="false"
              :value="getDictionaryValue('changeDownstreamPathTemplate')"
              @select="(val) => onDictionarySelect('changeDownstreamPathTemplate', val)"
              @deselect="(val) => onDictionaryUnSelect('changeDownstreamPathTemplate', val)"
            />
          </FormItem>
          <FormItem name="routeClaimsRequirement" :label="L('DisplayName:RouteClaimsRequirement')">
            <Select
              mode="tags"
              :open="false"
              :value="getDictionaryValue('routeClaimsRequirement')"
              @select="(val) => onDictionarySelect('routeClaimsRequirement', val)"
              @deselect="(val) => onDictionaryUnSelect('routeClaimsRequirement', val)"
            />
          </FormItem>
          <FormItem name="delegatingHandlers" :label="L('DisplayName:DelegatingHandlers')">
            <Select
              mode="tags"
              :open="false"
              :value="getDictionaryValue('delegatingHandlers')"
              @select="(val) => onDictionarySelect('delegatingHandlers', val)"
              @deselect="(val) => onDictionaryUnSelect('delegatingHandlers', val)"
            />
          </FormItem>
        </TabPane>
        <TabPane key="rateLimit" :tab="L('RateLimit')" forceRender>
          <FormItem :name="['rateLimitOptions', 'limit']" :label="L('DisplayName:Limit')">
            <InputNumber style="width: 100%" v-model:value="routeRef.rateLimitOptions.limit" />
          </FormItem>
          <FormItem :name="['rateLimitOptions', 'period']" :label="L('DisplayName:Period')">
            <Input v-model:value="routeRef.rateLimitOptions.period" />
          </FormItem>
          <FormItem
            :name="['rateLimitOptions', 'periodTimespan']"
            :label="L('DisplayName:PeriodTimespan')"
          >
            <InputNumber
              style="width: 100%"
              v-model:value="routeRef.rateLimitOptions.periodTimespan"
            />
          </FormItem>
          <FormItem
            :name="['rateLimitOptions', 'clientWhitelist']"
            :label="L('DisplayName:ClientWhitelist')"
          >
            <Select
              mode="tags"
              :open="false"
              :tokenSeparators="[',']"
              v-model:value="routeRef.rateLimitOptions.clientWhitelist"
            />
          </FormItem>
        </TabPane>
        <TabPane key="qos" :tab="L('Qos')" forceRender>
          <FormItem :name="['qoSOptions', 'timeoutValue']" :label="L('DisplayName:TimeoutValue')">
            <InputNumber style="width: 100%" v-model:value="routeRef.qoSOptions.timeoutValue" />
          </FormItem>
          <FormItem
            :name="['qoSOptions', 'durationOfBreak']"
            :label="L('DisplayName:DurationOfBreak')"
          >
            <InputNumber style="width: 100%" v-model:value="routeRef.qoSOptions.durationOfBreak" />
          </FormItem>
          <FormItem
            :name="['qoSOptions', 'exceptionsAllowedBeforeBreaking']"
            :label="L('DisplayName:ExceptionsAllowedBeforeBreaking')"
          >
            <InputNumber
              style="width: 100%"
              v-model:value="routeRef.qoSOptions.exceptionsAllowedBeforeBreaking"
            />
          </FormItem>
        </TabPane>
        <TabPane key="loadBalancer" :tab="L('LoadBalancer')" forceRender>
          <FormItem
            :name="['loadBalancerOptions', 'type']"
            :label="L('DisplayName:LoadBalancerType')"
          >
            <Select v-model:value="routeRef.loadBalancerOptions.type" :options="balancerOptions" />
          </FormItem>
          <FormItem :name="['loadBalancerOptions', 'key']" :label="L('DisplayName:PollingKey')">
            <Input v-model:value="routeRef.loadBalancerOptions.key" />
          </FormItem>
          <FormItem :name="['loadBalancerOptions', 'expiry']" :label="L('DisplayName:Expiry')">
            <InputNumber style="width: 100%" v-model:value="routeRef.loadBalancerOptions.expiry" />
          </FormItem>
        </TabPane>
        <TabPane key="securityOptions" :tab="L('SecurityOptions')" forceRender>
          <FormItem
            :name="['authenticationOptions', 'authenticationProviderKey']"
            :label="L('DisplayName:AuthenticationProviderKey')"
          >
            <Input v-model:value="routeRef.authenticationOptions.authenticationProviderKey" />
          </FormItem>
          <FormItem
            :name="['authenticationOptions', 'allowedScopes']"
            :label="L('DisplayName:AllowedScopes')"
          >
            <Select
              mode="tags"
              :open="false"
              :tokenSeparators="[',']"
              v-model:value="routeRef.authenticationOptions.allowedScopes"
            />
          </FormItem>
          <FormItem
            :name="['securityOptions', 'ipAllowedList']"
            :label="L('DisplayName:IpAllowedList')"
          >
            <Select
              mode="tags"
              :open="false"
              :tokenSeparators="[',']"
              v-model:value="routeRef.securityOptions.ipAllowedList"
            />
          </FormItem>
          <FormItem
            :name="['securityOptions', 'ipBlockedList']"
            :label="L('DisplayName:IpBlockedList')"
          >
            <Select
              mode="tags"
              :open="false"
              :tokenSeparators="[',']"
              v-model:value="routeRef.securityOptions.ipBlockedList"
            />
          </FormItem>
        </TabPane>
      </Tabs>
    </Form>
  </BasicModal>
</template>

<script lang="ts">
  import { defineComponent, ref } from 'vue';
  import { Checkbox, Form, Tabs, InputNumber, Select, RadioGroup } from 'ant-design-vue';
  import { useLocalization } from '/@/hooks/abp/useLocalization';
  import { Input } from '/@/components/Input';
  import { ApiSelect } from '/@/components/Form';
  import { BasicModal, useModalInner } from '/@/components/Modal';
  import { useRouteModal } from '../hooks/useRouteModal';

  export default defineComponent({
    name: 'RouteModal',
    components: {
      ApiSelect,
      BasicModal,
      Checkbox,
      Form,
      FormItem: Form.Item,
      Tabs,
      TabPane: Tabs.TabPane,
      Input,
      InputNumber,
      Select,
      RadioGroup,
    },
    emits: ['change', 'register'],
    setup(_, { emit }) {
      const { L } = useLocalization('ApiGateway');
      const routeIdRef = ref('');
      const formElRef = ref(null);
      const activedTabRef = ref('info');

      const [registerModal, modalMethods] = useModalInner((args) => {
        activedTabRef.value = 'info';
        routeIdRef.value = args.reRouteId;
      });

      const {
        routeRef,
        formRules,
        formTitle,
        httpMethods,
        radioOptions,
        appIdOptions,
        balancerOptions,
        getHostAndPorts,
        onHostAndPortsChange,
        getDictionaryValue,
        onDictionarySelect,
        onDictionaryUnSelect,
        handleCancel,
        handleSubmit,
      } = useRouteModal({ emit, formElRef, routeIdRef, modalMethods });

      return {
        L,
        routeRef,
        formElRef,
        formRules,
        formTitle,
        httpMethods,
        radioOptions,
        appIdOptions,
        activedTabRef,
        balancerOptions,
        getHostAndPorts,
        onHostAndPortsChange,
        getDictionaryValue,
        onDictionarySelect,
        onDictionaryUnSelect,
        registerModal,
        handleCancel,
        handleSubmit,
      };
    },
  });
</script>
