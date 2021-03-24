<template>
  <el-form
    ref="formGlobal"
    label-width="100px"
    :model="globalConfiguration"
    :rules="globalConfigurationRules"
  >
    <el-tabs>
      <el-tab-pane :label="$t('apiGateWay.basicOptions')">
        <el-form-item
          prop="appId"
          :label="$t('apiGateWay.appId')"
        >
          <el-select
            v-model="globalConfiguration.appId"
            :disabled="hasEdit"
            class="global-select"
            :placeholder="$t('pleaseSelectBy', {name: $t('apiGateWay.appId')})"
          >
            <el-option
              v-for="item in routeGroupAppIdOptions"
              :key="item.appId"
              :label="item.appName"
              :value="item.appId"
            />
          </el-select>
        </el-form-item>
        <el-form-item
          prop="baseUrl"
          :label="$t('apiGateWay.baseUrl')"
        >
          <el-input
            v-model="globalConfiguration.baseUrl"
            :placeholder="$t('pleaseInputBy', {key: $t('apiGateWay.baseUrl')})"
          />
        </el-form-item>
        <el-form-item
          prop="requestIdKey"
          :label="$t('apiGateWay.requestIdKey')"
        >
          <el-input
            v-model="globalConfiguration.requestIdKey"
            :placeholder="$t('pleaseInputBy', {key: $t('apiGateWay.requestIdKey')})"
          />
        </el-form-item>
        <el-form-item
          prop="downstreamScheme"
          :label="$t('apiGateWay.downstreamScheme')"
        >
          <el-input
            v-model="globalConfiguration.downstreamScheme"
            :placeholder="$t('pleaseInputBy', {key: $t('apiGateWay.downstreamScheme')})"
          />
        </el-form-item>
        <el-form-item
          prop="downstreamHttpVersion"
          :label="$t('apiGateWay.downstreamHttpVersion')"
        >
          <el-input
            v-model="globalConfiguration.downstreamHttpVersion"
          />
        </el-form-item>
      </el-tab-pane>
      <el-tab-pane :label="$t('apiGateWay.httpOptions')">
        <el-form-item
          prop="httpHandlerOptions.maxConnectionsPerServer"
          :label="$t('apiGateWay.maxConnectionsPerServer')"
          label-width="180px"
        >
          <el-input
            v-model="globalConfiguration.httpHandlerOptions.maxConnectionsPerServer"
            type="number"
          />
        </el-form-item>
        <el-row>
          <el-col :span="6">
            <el-form-item
              :label="$t('apiGateWay.useProxy')"
            >
              <el-switch
                v-model="globalConfiguration.httpHandlerOptions.useProxy"
                active-color="#13ce66"
                inactive-color="#ff4949"
              />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item
              :label="$t('apiGateWay.useTracing')"
            >
              <el-switch
                v-model="globalConfiguration.httpHandlerOptions.useTracing"
                active-color="#13ce66"
                inactive-color="#ff4949"
              />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item
              label-width="120px"
              :label="$t('apiGateWay.allowAutoRedirect')"
            >
              <el-switch
                v-model="globalConfiguration.httpHandlerOptions.allowAutoRedirect"
                active-color="#13ce66"
                inactive-color="#ff4949"
              />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item
              label-width="120px"
              :label="$t('apiGateWay.useCookieContainer')"
            >
              <el-switch
                v-model="globalConfiguration.httpHandlerOptions.useCookieContainer"
                active-color="#13ce66"
                inactive-color="#ff4949"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-tab-pane>
      <el-tab-pane :label="$t('apiGateWay.rateLimitOptions')">
        <el-form-item
          prop="rateLimitOptions.clientIdHeader"
          label-width="150px"
          :label="$t('apiGateWay.clientIdHeader')"
        >
          <el-input
            v-model="globalConfiguration.rateLimitOptions.clientIdHeader"
          />
        </el-form-item>
        <el-form-item
          prop="rateLimitOptions.httpStatusCode"
          label-width="150px"
          :label="$t('apiGateWay.httpStatusCode')"
        >
          <el-input
            v-model="globalConfiguration.rateLimitOptions.httpStatusCode"
            type="number"
          />
        </el-form-item>
        <el-row>
          <el-col :span="16">
            <el-form-item
              prop="rateLimitOptions.rateLimitCounterPrefix"
              label-width="150px"
              :label="$t('apiGateWay.rateLimitCounterPrefix')"
            >
              <el-input
                v-model="globalConfiguration.rateLimitOptions.rateLimitCounterPrefix"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item
              prop="rateLimitOptions.disableRateLimitHeaders"
              label-width="150px"
              :label="$t('apiGateWay.disableRateLimitHeaders')"
            >
              <el-switch v-model="globalConfiguration.rateLimitOptions.disableRateLimitHeaders" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item
          prop="rateLimitOptions.quotaExceededMessage"
          label-width="150px"
          :label="$t('apiGateWay.quotaExceededMessage')"
        >
          <el-input
            v-model="globalConfiguration.rateLimitOptions.quotaExceededMessage"
            type="textarea"
          />
        </el-form-item>
      </el-tab-pane>
      <el-tab-pane :label="$t('apiGateWay.qoSOptions')">
        <el-form-item
          prop="qoSOptions.timeoutValue"
          :label="$t('apiGateWay.timeoutValue')"
        >
          <el-input
            v-model="globalConfiguration.qoSOptions.timeoutValue"
            type="number"
          />
        </el-form-item>
        <el-form-item
          prop="qoSOptions.durationOfBreak"
          :label="$t('apiGateWay.durationOfBreak')"
        >
          <el-input
            v-model="globalConfiguration.qoSOptions.durationOfBreak"
            type="number"
          />
        </el-form-item>
        <el-form-item
          prop="qoSOptions.exceptionsAllowedBeforeBreaking"
          label-width="150px"
          :label="$t('apiGateWay.exceptionsAllowedBeforeBreaking')"
        >
          <el-input
            v-model="globalConfiguration.qoSOptions.exceptionsAllowedBeforeBreaking"
            type="number"
          />
        </el-form-item>
      </el-tab-pane>
      <el-tab-pane :label="$t('apiGateWay.loadBalancerOptions')">
        <el-form-item
          prop="loadBalancerOptions.type"
          :label="$t('apiGateWay.loadBalancerType')"
        >
          <el-select
            v-model="globalConfiguration.loadBalancerOptions.type"
            clearable
            class="global-select"
            :placeholder="$t('pleaseSelectBy', {key: $t('apiGateWay.loadBalancerType')})"
          >
            <el-option
              v-for="provider in loadBalancerProviders"
              :key="provider.type"
              :label="provider.displayName"
              :value="provider.type"
            />
          </el-select>
        </el-form-item>
        <el-form-item
          prop="loadBalancerOptions.expiry"
          :label="$t('apiGateWay.durationOfBreak')"
        >
          <el-input
            v-model="globalConfiguration.loadBalancerOptions.expiry"
            type="number"
          />
        </el-form-item>
        <el-form-item
          prop="loadBalancerOptions.key"
          :label="$t('apiGateWay.loadBalancerKey')"
        >
          <el-input
            v-model="globalConfiguration.loadBalancerOptions.key"
          />
        </el-form-item>
      </el-tab-pane>
      <el-tab-pane :label="$t('apiGateWay.serviceDiscovery')">
        <el-form-item
          prop="serviceDiscoveryProvider.type"
          :label="$t('apiGateWay.discoverType')"
        >
          <el-select
            v-model="globalConfiguration.serviceDiscoveryProvider.type"
            class="global-select"
            :placeholder="$t('pleaseSelectBy', {key: $t('apiGateWay.discoverType')})"
          >
            <el-option
              :label="$t('none')"
              value=""
            />
            <el-option
              label="Consul"
              value="Consul"
            />
            <el-option
              label="Zookeeper"
              value="Zookeeper"
            />
            <el-option
              label="Eureka"
              value="Eureka"
            />
          </el-select>
        </el-form-item>
        <el-form-item
          prop="serviceDiscoveryProvider.host"
          :label="$t('apiGateWay.discoverHost')"
        >
          <el-input
            v-model="globalConfiguration.serviceDiscoveryProvider.host"
          />
        </el-form-item>
        <el-form-item
          prop="serviceDiscoveryProvider.port"
          :label="$t('apiGateWay.discoverPort')"
        >
          <el-input
            v-model="globalConfiguration.serviceDiscoveryProvider.port"
            type="number"
          />
        </el-form-item>
        <el-form-item
          prop="serviceDiscoveryProvider.token"
          :label="$t('apiGateWay.discoverToken')"
        >
          <el-input
            v-model="globalConfiguration.serviceDiscoveryProvider.token"
          />
        </el-form-item>
        <el-form-item
          prop="serviceDiscoveryProvider.configurationKey"
          :label="$t('apiGateWay.configurationKey')"
        >
          <el-input
            v-model="globalConfiguration.serviceDiscoveryProvider.configurationKey"
          />
        </el-form-item>
        <el-form-item
          prop="serviceDiscoveryProvider.pollingInterval"
          :label="$t('apiGateWay.pollingInterval')"
        >
          <el-input
            v-model="globalConfiguration.serviceDiscoveryProvider.pollingInterval"
            type="number"
          />
        </el-form-item>
        <el-form-item
          prop="serviceDiscoveryProvider.namespace"
          :label="$t('apiGateWay.namespace')"
        >
          <el-input
            v-model="globalConfiguration.serviceDiscoveryProvider.namespace"
          />
        </el-form-item>
        <el-form-item
          prop="serviceDiscoveryProvider.scheme"
          :label="$t('apiGateWay.discoverScheme')"
        >
          <el-input
            v-model="globalConfiguration.serviceDiscoveryProvider.scheme"
          />
        </el-form-item>
      </el-tab-pane>
    </el-tabs>

    <el-form-item>
      <el-button
        class="cancel"
        style="width:100px"
        @click="onCancel"
      >
        {{ $t('table.cancel') }}
      </el-button>
      <el-button
        class="confirm"
        type="primary"
        style="width:100px"
        @click="onSubmitEdit('formGlobal')"
      >
        {{ $t('table.confirm') }}
      </el-button>
    </el-form-item>
  </el-form>
</template>

<script lang="ts">
import { checkPermission } from '@/utils/permission'
import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'
import ApiGatewayService, {
  RouteGroupAppIdDto,
  LoadBalancerDescriptor,
  GlobalConfigurationDto,
  GlobalConfigurationCreateDto,
  GlobalConfigurationUpdateDto
}
  from '@/api/apigateway'

@Component({
  name: 'GlobalCreateOrEditForm',
  methods: {
    checkPermission
  }
})
export default class extends Mixins(LocalizationMiXin) {
  @Prop({ default: '' })
  private appId!: string

  private globalConfiguration: GlobalConfigurationDto
  private routeGroupAppIdOptions: RouteGroupAppIdDto[]
  private loadBalancerProviders: LoadBalancerDescriptor[]

  constructor() {
    super()
    this.globalConfiguration = new GlobalConfigurationDto()
    this.routeGroupAppIdOptions = new Array<RouteGroupAppIdDto>()
    this.loadBalancerProviders = new Array<LoadBalancerDescriptor>()
  }

  get hasEdit() {
    if (this.globalConfiguration.itemId) {
      return true
    }
    return false
  }

  private globalConfigurationRules = {
    appId: [
      { required: true, message: this.l('pleaseSelectBy', { key: this.l('apiGateWay.appId') }), trigger: 'blur' }
    ],
    baseUrl: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('apiGateWay.baseUrl') }), trigger: 'blur' }
    ]
  }

  mounted() {
    ApiGatewayService.getRouteGroupAppIds().then(appKeys => {
      this.routeGroupAppIdOptions = appKeys.items
    })
    ApiGatewayService.getLoadBalancerProviders().then(res => {
      this.loadBalancerProviders = res.items
    })
  }

  @Watch('appId', { immediate: true })
  private handleAppIdChanged(appId: string) {
    if (appId) {
      this.handleGetGlobalConfiguration()
    } else {
      this.globalConfiguration = new GlobalConfigurationDto()
    }
  }

  private handleGetGlobalConfiguration() {
    ApiGatewayService.getGlobalConfigurationByAppId(this.appId).then(global => {
      this.globalConfiguration = global
    })
  }

  private onSubmitEdit(form: string) {
    const formGlobal = this.$refs[form] as any
    formGlobal.validate(async(valid: boolean) => {
      if (valid) {
        if (this.globalConfiguration.itemId) {
          const updateGlobalDto = new GlobalConfigurationUpdateDto()
          updateGlobalDto.setBasicGlobalConfiguration(this.globalConfiguration)
          updateGlobalDto.itemId = this.globalConfiguration.itemId
          this.globalConfiguration = await ApiGatewayService.updateGlobalConfiguration(updateGlobalDto)
        } else {
          const createGlobalDto = new GlobalConfigurationCreateDto()
          createGlobalDto.appId = this.globalConfiguration.appId
          createGlobalDto.setBasicGlobalConfiguration(this.globalConfiguration)
          this.globalConfiguration = await ApiGatewayService.createGlobalConfiguration(createGlobalDto)
        }
        this.$message('successful')
        formGlobal.resetFields()
        this.$emit('closed', true)
      }
    })
  }

  private onCancel() {
    this.globalConfiguration = new GlobalConfigurationDto()
    const formGlobal = this.$refs.formGlobal as any
    formGlobal.resetFields()
    this.$emit('closed', false)
  }
}
</script>

<style lang="scss" scoped>
.global-select {
  width: 100%;
}
.confirm {
  position: absolute;
  right: 10px;
}
.cancel {
  position: absolute;
  right: 120px;
}
</style>
