<template>
  <el-form
    ref="formAggregateRoute"
    label-width="120px"
    :model="aggregateRoute"
    :rules="aggregateRouteRules"
  >
    <el-row>
      <el-col :span="24">
        <el-form-item
          v-if="!isEditRoute"
          prop="appId"
          :label="$t('apiGateWay.appId')"
        >
          <el-select
            v-model="aggregateRoute.appId"
            style="width: 100%"
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
    <el-form-item
      prop="name"
      :label="$t('apiGateWay.aggregateRouteName')"
    >
      <el-input
        v-model="aggregateRoute.name"
        :placeholder="$t('pleaseInputBy', {key: $t('apiGateWay.aggregateRouteName')})"
      />
    </el-form-item>
    <el-row>
      <el-col :span="12">
        <el-form-item
          prop="aggregator"
          :label="$t('apiGateWay.definedAggregatorProviders')"
        >
          <el-select
            v-model="aggregateRoute.aggregator"
            clearable
            style="width: 100%"
            :placeholder="$t('pleaseSelectBy', {name: $t('apiGateWay.definedAggregatorProviders')})"
          >
            <el-option
              v-for="(provider, index) in definedAggregatorProviders"
              :key="index"
              :label="provider"
              :value="provider"
            />
          </el-select>
        </el-form-item>
      </el-col>
      <el-col :span="12">
        <el-form-item
          prop="priority"
          :label="$t('apiGateWay.priority')"
        >
          <el-input
            v-model="aggregateRoute.priority"
            type="number"
          />
        </el-form-item>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="16">
        <el-form-item
          prop="upstreamHost"
          :label="$t('apiGateWay.upstreamHost')"
        >
          <el-input
            v-model="aggregateRoute.upstreamHost"
          />
        </el-form-item>
      </el-col>
      <el-col :span="8">
        <el-form-item
          prop="reRouteIsCaseSensitive"
          :label="$t('apiGateWay.reRouteIsCaseSensitive')"
        >
          <el-switch
            v-model="aggregateRoute.reRouteIsCaseSensitive"
            :placeholder="$t('pleaseInputBy', {key: $t('apiGateWay.reRouteIsCaseSensitive')})"
          />
        </el-form-item>
      </el-col>
    </el-row>
    <el-form-item
      prop="upstreamPathTemplate"
      :label="$t('apiGateWay.upstreamPathTemplate')"
    >
      <el-input
        v-model="aggregateRoute.upstreamPathTemplate"
        :placeholder="$t('pleaseInputBy', {key: $t('apiGateWay.upstreamPathTemplate')})"
      />
    </el-form-item>
    <el-form-item
      prop="upstreamHttpMethod"
      :label="$t('apiGateWay.upstreamHttpMethod')"
    >
      <el-input-tag
        v-model="aggregateRoute.upstreamHttpMethod"
      />
    </el-form-item>
    <el-form-item
      prop="reRouteKeys"
      :label="$t('apiGateWay.reRouteKeys')"
    >
      <el-input-tag
        v-model="aggregateRoute.reRouteKeys"
      />
    </el-form-item>

    <el-form-item>
      <el-button
        class="cancel"
        style="width:100px;right: 120px;position: absolute;"
        @click="onCancel"
      >
        {{ $t('global.cancel') }}
      </el-button>
      <el-button
        class="confirm"
        type="primary"
        style="width:100px;right: 10px;position: absolute;"
        @click="onSaveAggregateRoute"
      >
        {{ $t('global.confirm') }}
      </el-button>
    </el-form-item>
  </el-form>
</template>

<script lang="ts">
import ElInputTag from '@/components/InputTag/index.vue'
import { Component, Prop, Mixins, Watch } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'
import ApiGatewayService, { RouteGroupAppIdDto, AggregateReRoute, AggregateReRouteCreate, AggregateReRouteUpdate } from '@/api/apigateway'

@Component({
  name: 'AggregateRouteCreateOrEditForm',
  components: {
    ElInputTag
  }
})
export default class extends Mixins(LocalizationMiXin) {
  @Prop({ default: '' })
  private aggregateRouteId!: string

  @Prop({ default: () => new Array<RouteGroupAppIdDto>() })
  private appIdOptions!: RouteGroupAppIdDto[]

  private aggregateRoute: AggregateReRoute
  private definedAggregatorProviders = new Array<string>()

  get isEditRoute() {
    if (this.aggregateRouteId) {
      return true
    }
    return false
  }

  private validateReRouteKeys = (rule: any, value: string[], callback: any) => {
    if (!value || value.length === 0) {
      callback(new Error(this.l('pleaseInputBy', { key: this.l('apiGateWay.reRouteKeys') })))
    } else {
      callback()
    }
  }

  private aggregateRouteRules = {
    appId: [
      { required: true, message: this.l('pleaseSelectBy', { key: this.l('apiGateWay.appId') }), trigger: 'blur' }
    ],
    name: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('apiGateWay.aggregateRouteName') }), trigger: 'blur' }
    ],
    upstreamPathTemplate: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('apiGateWay.upstreamPathTemplate') }), trigger: 'blur' }
    ],
    reRouteKeys: [
      { required: true, validator: this.validateReRouteKeys, trigger: 'blur' }
    ]
  }

  constructor() {
    super()
    this.aggregateRoute = AggregateReRoute.empty()
  }

  mounted() {
    ApiGatewayService.getDefinedAggregatorProviders().then(res => {
      this.definedAggregatorProviders = res.items
    })
  }

  @Watch('aggregateRouteId', { immediate: true })
  private onAggregateRouteIdChanged() {
    if (this.aggregateRouteId) {
      ApiGatewayService.getAggregateReRouteByRouteId(this.aggregateRouteId).then(aggregateRoute => {
        this.aggregateRoute = aggregateRoute
      })
    } else {
      this.aggregateRoute = AggregateReRoute.empty()
    }
  }

  private onSaveAggregateRoute() {
    const frmAggregateRoute = this.$refs.formAggregateRoute as any
    frmAggregateRoute.validate((valid: boolean) => {
      if (valid) {
        if (this.isEditRoute) {
          const updateAggregateRoute = AggregateReRouteUpdate.create(this.aggregateRoute)
          ApiGatewayService.updateAggregateReRoute(updateAggregateRoute).then(route => {
            this.aggregateRoute = route
          })
        } else {
          const createAggregateRoute = AggregateReRouteCreate.create(this.aggregateRoute)
          ApiGatewayService.createAggregateReRoute(createAggregateRoute).then(route => {
            this.aggregateRoute = route
          })
        }
        this.reset()
        this.$emit('closed', true)
      }
    })
  }

  private onCancel() {
    this.reset()
    this.$emit('closed', false)
  }

  private reset() {
    this.aggregateRoute = AggregateReRoute.empty()
    const formAggregateRoute = this.$refs.formAggregateRoute as any
    formAggregateRoute.resetFields()
  }
}
</script>
