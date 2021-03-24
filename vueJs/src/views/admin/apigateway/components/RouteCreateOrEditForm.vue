<template>
  <el-form
    ref="formRoute"
    label-width="100px"
    :model="apiGateWayRoute"
    :rules="apiGateWayRouteRules"
  >
    <el-tabs v-model="activeTablePane">
      <el-tab-pane
        :label="$t('apiGateWay.basicOptions')"
        name="basicOptions"
      >
        <el-row>
          <el-col :span="24">
            <el-form-item
              v-if="!isEditRoute"
              prop="appId"
              :label="$t('apiGateWay.appId')"
            >
              <el-select
                v-model="apiGateWayRoute.appId"
                class="route-select"
                :placeholder="$t('pleaseSelectBy', {name: $t('apiGateWay.appId')})"
              >
                <el-option
                  v-for="item in appIdOptions"
                  :key="item.appId"
                  :label="item.appName"
                  :value="item.appId"
                />
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item
              prop="reRouteName"
              :label="$t('apiGateWay.reRouteName')"
            >
              <el-input
                v-model="apiGateWayRoute.reRouteName"
                :placeholder="$t('pleaseInputBy', {key: $t('apiGateWay.reRouteName')})"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item
              prop="requestIdKey"
              :label="$t('apiGateWay.requestIdKey')"
            >
              <el-input
                v-model="apiGateWayRoute.requestIdKey"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item
              prop="serviceName"
              :label="$t('apiGateWay.serviceName')"
            >
              <el-input
                v-model="apiGateWayRoute.serviceName"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item
              prop="serviceNamespace"
              :label="$t('apiGateWay.serviceNamespace')"
            >
              <el-input
                v-model="apiGateWayRoute.serviceNamespace"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item
              prop="timeout"
              :label="$t('apiGateWay.timeoutValue')"
            >
              <el-input
                v-model="apiGateWayRoute.timeout"
                type="number"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item
              prop="priority"
              :label="$t('apiGateWay.priority')"
            >
              <el-input
                v-model="apiGateWayRoute.priority"
                type="number"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="6">
            <el-form-item
              prop="reRouteIsCaseSensitive"
              :label="$t('apiGateWay.reRouteIsCaseSensitive')"
            >
              <el-switch v-model="apiGateWayRoute.reRouteIsCaseSensitive" />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item
              prop="dangerousAcceptAnyServerCertificateValidator"
              :label="$t('apiGateWay.dangerousAcceptAnyServerCertificateValidator')"
            >
              <el-switch v-model="apiGateWayRoute.dangerousAcceptAnyServerCertificateValidator" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-tab-pane>
      <el-tab-pane
        :label="$t('apiGateWay.routingForward')"
        name="routingForward"
      >
        <el-row>
          <el-col :span="12">
            <el-form-item
              prop="downstreamScheme"
              :label="$t('apiGateWay.downstreamScheme')"
              label-width="120px"
            >
              <el-input
                v-model="apiGateWayRoute.downstreamScheme"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item
              prop="downstreamHttpVersion"
              :label="$t('apiGateWay.downstreamHttpVersion')"
              label-width="120px"
            >
              <el-input
                v-model="apiGateWayRoute.downstreamHttpVersion"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item
          prop="upstreamPathTemplate"
          :label="$t('apiGateWay.upstreamPathTemplate')"
          label-width="120px"
        >
          <el-input
            v-model="apiGateWayRoute.upstreamPathTemplate"
            :placeholder="$t('pleaseInputBy', {key: $t('apiGateWay.upstreamPathTemplate')})"
          />
        </el-form-item>
        <el-form-item
          prop="downstreamPathTemplate"
          :label="$t('apiGateWay.downstreamPathTemplate')"
          label-width="120px"
        >
          <el-input
            v-model="apiGateWayRoute.downstreamPathTemplate"
            :placeholder="$t('pleaseInputBy', {key: $t('apiGateWay.downstreamPathTemplate')})"
          />
        </el-form-item>
        <el-row>
          <el-col :span="12">
            <el-form-item
              prop="downstreamHttpMethod"
              :label="$t('apiGateWay.downstreamHttpMethod')"
              label-width="120px"
            >
              <el-input
                v-model="apiGateWayRoute.downstreamHttpMethod"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item
              prop="key"
              :label="$t('apiGateWay.aggrefateKey')"
            >
              <el-input
                v-model="apiGateWayRoute.key"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item
          prop="upstreamHttpMethod"
          :label="$t('apiGateWay.upstreamHttpMethod')"
          label-width="120px"
        >
          <el-input-tag
            v-model="apiGateWayRoute.upstreamHttpMethod"
            :type-filter="httpMethodsFilter"
          />
        </el-form-item>
        <el-form-item
          prop="downstreamHostAndPorts"
          :label="$t('apiGateWay.downstreamHostAndPorts')"
          label-width="120px"
        >
          <host-and-port-input-tag
            v-model="apiGateWayRoute.downstreamHostAndPorts"
          />
        </el-form-item>
      </el-tab-pane>
      <el-tab-pane
        :label="$t('apiGateWay.requestProcessing')"
        name="requestProcessing"
      >
        <el-form-item
          prop="addClaimsToRequest"
          :label="$t('apiGateWay.addClaimsToRequest')"
          label-width="150px"
        >
          <dictionary-input-tag
            v-model="apiGateWayRoute.addClaimsToRequest"
          />
        </el-form-item>
        <el-form-item
          prop="addQueriesToRequest"
          :label="$t('apiGateWay.addQueriesToRequest')"
          label-width="150px"
        >
          <dictionary-input-tag
            v-model="apiGateWayRoute.addQueriesToRequest"
          />
        </el-form-item>
        <el-form-item
          prop="addHeadersToRequest"
          :label="$t('apiGateWay.addHeadersToRequest')"
          label-width="150px"
        >
          <dictionary-input-tag
            v-model="apiGateWayRoute.addHeadersToRequest"
          />
        </el-form-item>
        <el-form-item
          prop="upstreamHeaderTransform"
          :label="$t('apiGateWay.upstreamHeaderTransform')"
          label-width="150px"
        >
          <dictionary-input-tag
            v-model="apiGateWayRoute.upstreamHeaderTransform"
          />
        </el-form-item>
        <el-form-item
          prop="downstreamHeaderTransform"
          :label="$t('apiGateWay.downstreamHeaderTransform')"
          label-width="150px"
        >
          <dictionary-input-tag
            v-model="apiGateWayRoute.downstreamHeaderTransform"
          />
        </el-form-item>
        <el-form-item
          prop="changeDownstreamPathTemplate"
          :label="$t('apiGateWay.changeDownstreamPathTemplate')"
          label-width="150px"
        >
          <dictionary-input-tag
            v-model="apiGateWayRoute.changeDownstreamPathTemplate"
          />
        </el-form-item>
        <el-form-item
          prop="routeClaimsRequirement"
          :label="$t('apiGateWay.routeClaimsRequirement')"
          label-width="150px"
        >
          <dictionary-input-tag
            v-model="apiGateWayRoute.routeClaimsRequirement"
          />
        </el-form-item>
        <el-form-item
          prop="delegatingHandlers"
          :label="$t('apiGateWay.delegatingHandlers')"
          label-width="150px"
        >
          <el-input-tag
            v-model="apiGateWayRoute.delegatingHandlers"
          />
        </el-form-item>
      </el-tab-pane>
      <el-tab-pane
        :label="$t('apiGateWay.httpOptions')"
        name="httpOptions"
      >
        <el-form-item
          prop="httpHandlerOptions.maxConnectionsPerServer"
          :label="$t('apiGateWay.maxConnectionsPerServer')"
          label-width="180px"
        >
          <el-input
            v-model="apiGateWayRoute.httpHandlerOptions.maxConnectionsPerServer"
            type="number"
          />
        </el-form-item>
        <el-row>
          <el-col :span="12">
            <el-form-item
              prop="httpHandlerOptions.useProxy"
              :label="$t('apiGateWay.useProxy')"
            >
              <el-switch
                v-model="apiGateWayRoute.httpHandlerOptions.useProxy"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item
              prop="httpHandlerOptions.useTracing"
              :label="$t('apiGateWay.useTracing')"
            >
              <el-switch
                v-model="apiGateWayRoute.httpHandlerOptions.useTracing"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="12">
            <el-form-item
              prop="httpHandlerOptions.allowAutoRedirect"
              label-width="120px"
              :label="$t('apiGateWay.allowAutoRedirect')"
            >
              <el-switch
                v-model="apiGateWayRoute.httpHandlerOptions.allowAutoRedirect"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item
              label-width="120px"
              prop="httpHandlerOptions.useCookieContainer"
              :label="$t('apiGateWay.useCookieContainer')"
            >
              <el-switch
                v-model="apiGateWayRoute.httpHandlerOptions.useCookieContainer"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-tab-pane>
      <el-tab-pane
        :label="$t('apiGateWay.qoSOptions')"
        name="qoSOptions"
      >
        <el-form-item
          prop="qoSOptions.timeoutValue"
          :label="$t('apiGateWay.timeoutValue')"
        >
          <el-input
            v-model="apiGateWayRoute.qoSOptions.timeoutValue"
            type="number"
          />
        </el-form-item>
        <el-form-item
          prop="qoSOptions.durationOfBreak"
          :label="$t('apiGateWay.durationOfBreak')"
        >
          <el-input
            v-model="apiGateWayRoute.qoSOptions.durationOfBreak"
            type="number"
          />
        </el-form-item>
        <el-form-item
          prop="qoSOptions.exceptionsAllowedBeforeBreaking"
          label-width="150px"
          :label="$t('apiGateWay.exceptionsAllowedBeforeBreaking')"
        >
          <el-input
            v-model="apiGateWayRoute.qoSOptions.exceptionsAllowedBeforeBreaking"
            type="number"
          />
        </el-form-item>
      </el-tab-pane>
      <el-tab-pane
        :label="$t('apiGateWay.loadBalancerOptions')"
        name="loadBalancerOptions"
      >
        <el-form-item
          prop="loadBalancerOptions.type"
          :label="$t('apiGateWay.loadBalancerType')"
        >
          <el-select
            v-model="apiGateWayRoute.loadBalancerOptions.type"
            clearable
            class="route-select"
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
            v-model="apiGateWayRoute.loadBalancerOptions.expiry"
            type="number"
          />
        </el-form-item>
        <el-form-item
          prop="loadBalancerOptions.key"
          :label="$t('apiGateWay.loadBalancerKey')"
        >
          <el-input
            v-model="apiGateWayRoute.loadBalancerOptions.key"
          />
        </el-form-item>
      </el-tab-pane>
      <el-tab-pane
        :label="$t('apiGateWay.rateLimitOptions')"
        name="rateLimitOptions"
      >
        <el-row>
          <el-col :span="6">
            <el-form-item
              prop="rateLimitOptions.enableRateLimiting"
              :label="$t('apiGateWay.enableRateLimiting')"
            >
              <el-switch
                v-model="apiGateWayRoute.rateLimitOptions.enableRateLimiting"
              />
            </el-form-item>
          </el-col>
          <el-col :span="18">
            <el-form-item
              :label="$t('apiGateWay.periodTimespan')"
              prop="rateLimitOptions.periodTimespan"
              label-width="150px"
            >
              <el-input
                v-model="apiGateWayRoute.rateLimitOptions.periodTimespan"
                type="number"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item
          :label="$t('apiGateWay.rateLimitCount')"
          prop="rateLimitOptions.limit"
        >
          <el-input
            v-model="apiGateWayRoute.rateLimitOptions.limit"
            type="number"
          />
        </el-form-item>
        <el-form-item
          :label="$t('apiGateWay.period')"
          prop="rateLimitOptions.period"
        >
          <el-input
            v-model="apiGateWayRoute.rateLimitOptions.period"
          />
        </el-form-item>
        <el-form-item
          prop="rateLimitOptions.clientWhitelist"
          :label="$t('apiGateWay.clientWhitelist')"
        >
          <el-input-tag
            v-model="apiGateWayRoute.rateLimitOptions.clientWhitelist"
          />
        </el-form-item>
      </el-tab-pane>
      <el-tab-pane
        :label="$t('apiGateWay.authorization')"
        name="authorization"
      >
        <el-form-item
          prop="authenticationOptions.authenticationProviderKey"
          :label="$t('apiGateWay.authenticationProviderKey')"
        >
          <el-input
            v-model="apiGateWayRoute.authenticationOptions.authenticationProviderKey"
          />
        </el-form-item>
        <el-form-item
          prop="authenticationOptions.allowedScopes"
          :label="$t('apiGateWay.allowedScopes')"
        >
          <el-input-tag
            v-model="apiGateWayRoute.authenticationOptions.allowedScopes"
          />
        </el-form-item>
        <el-form-item
          prop="securityOptions.ipAllowedList"
          :label="$t('apiGateWay.ipAllowedList')"
        >
          <el-input-tag
            v-model="apiGateWayRoute.securityOptions.ipAllowedList"
          />
        </el-form-item>
        <el-form-item
          prop="securityOptions.ipBlockedList"
          :label="$t('apiGateWay.ipBlockedList')"
        >
          <el-input-tag
            v-model="apiGateWayRoute.securityOptions.ipBlockedList"
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
        @click="onSubmitEdit('formRoute')"
      >
        {{ $t('table.confirm') }}
      </el-button>
    </el-form-item>
  </el-form>
</template>

<script lang="ts">
import ElInputTag from '@/components/InputTag/index.vue'
import HostAndPortInputTag from './HostAndPortInputTag.vue'
import DictionaryInputTag from './DictionaryInputTag.vue'
import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'
import ApiGateWayService, { LoadBalancerDescriptor, RouteGroupAppIdDto, ReRouteDto, ReRouteCreateDto, ReRouteUpdateDto } from '@/api/apigateway'

@Component({
  name: 'RouteCreateOrEditForm',
  components: {
    ElInputTag,
    DictionaryInputTag,
    HostAndPortInputTag
  }
})
export default class extends Mixins(LocalizationMiXin) {
  @Prop({ default: '' })
  private routeId!: number

  @Prop({ default: () => new Array<RouteGroupAppIdDto>() })
  private appIdOptions!: RouteGroupAppIdDto[]

  private activeTablePane: string
  private apiGateWayRoute: ReRouteDto
  private loadBalancerProviders: LoadBalancerDescriptor[]
  private httpMethodsFilter: { [key: string]: string } = {
    GET: '',
    POST: 'success',
    PUT: 'warning',
    PATCH: 'warning',
    DELETE: 'danger'
  }

  get isEditRoute() {
    if (this.routeId) {
      return true
    }
    return false
  }

  private validateRequiredArrayValue = (rule: any, value: any[], callback: any, errorMessage: string) => {
    if (!value || !Array.isArray(value) || value.length === 0) {
      callback(new Error(errorMessage))
    } else {
      callback()
    }
  }

  private apiGateWayRouteRules = {
    appId: [
      { required: true, message: this.l('pleaseSelectBy', { key: this.l('apiGateWay.appId') }), trigger: 'blur' }
    ],
    reRouteName: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('apiGateWay.reRouteName') }), trigger: 'blur' }
    ],
    downstreamPathTemplate: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('apiGateWay.downstreamPathTemplate') }), trigger: 'blur' }
    ],
    upstreamPathTemplate: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('apiGateWay.upstreamPathTemplate') }), trigger: 'blur' }
    ],
    upstreamHttpMethod: [
      {
        required: true,
        validator: (rule: any, value: any[], callback: any) =>
          this.validateRequiredArrayValue(rule, value, callback, this.l('pleaseInputBy', { key: this.l('apiGateWay.upstreamHttpMethod') })),
        trigger: 'blur'
      }
    ],
    downstreamHostAndPorts: [
      {
        required: true,
        validator: (rule: any, value: any[], callback: any) =>
          this.validateRequiredArrayValue(rule, value, callback, this.l('pleaseInputBy', { key: this.l('apiGateWay.downstreamHostAndPorts') })),
        trigger: 'blur'
      }
    ]
  }

  constructor() {
    super()
    this.activeTablePane = 'basicOptions'
    this.apiGateWayRoute = new ReRouteDto()
    this.loadBalancerProviders = new Array<LoadBalancerDescriptor>()
  }

  mounted() {
    ApiGateWayService.getLoadBalancerProviders().then(res => {
      this.loadBalancerProviders = res.items
    })
  }

  @Watch('routeId', { immediate: true })
  private handleRouteIdChanged(routeId: number) {
    if (routeId && routeId > 0) {
      ApiGateWayService.getReRouteByRouteId(routeId).then(route => {
        this.apiGateWayRoute = route
      })
    } else {
      this.apiGateWayRoute = new ReRouteDto()
    }
  }

  private onSubmitEdit() {
    const routeEditForm = this.$refs.formRoute as any
    routeEditForm.validate(async(valid: boolean) => {
      if (valid) {
        if (this.routeId) {
          const updateRouteDto = new ReRouteUpdateDto()
          updateRouteDto.setBasicRoute(this.apiGateWayRoute)
          updateRouteDto.reRouteId = this.apiGateWayRoute.reRouteId
          updateRouteDto.concurrencyStamp = this.apiGateWayRoute.concurrencyStamp
          this.apiGateWayRoute = await ApiGateWayService.updateReRoute(updateRouteDto)
        } else {
          const createRouteDto = new ReRouteCreateDto()
          createRouteDto.setBasicRoute(this.apiGateWayRoute)
          createRouteDto.appId = this.apiGateWayRoute.appId
          this.apiGateWayRoute = await ApiGateWayService.createReRoute(createRouteDto)
        }
        this.$message.success(this.$t('successful').toString())
        routeEditForm.resetFields()
        this.activeTablePane = 'basicOptions'
        this.$emit('closed', true)
      }
    })
  }

  private onCancel() {
    const routeEditForm = this.$refs.formRoute as any
    routeEditForm.resetFields()
    this.activeTablePane = 'basicOptions'
    this.$emit('closed')
  }
}
</script>

<style lang="scss" scoped>
.route-select {
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
