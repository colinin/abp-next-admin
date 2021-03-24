<template>
  <div class="app-container">
    <div class="filter-container">
      <el-form
        v-if="checkPermission(['ApiGateway.AggregateRoute.ManageRouteConfig'])"
        ref="formAggregateRouteConfig"
        label-width="100px"
        :model="routeConfig"
        :rules="routeConfigRules"
      >
        <el-form-item
          prop="reRouteKey"
          :label="$t('apiGateWay.aggregateRouteKey')"
        >
          <el-input
            v-model="routeConfig.reRouteKey"
            :placeholder="$t('pleaseInputBy', {key: $t('apiGateWay.aggregateRouteKey')})"
          />
        </el-form-item>
        <el-form-item
          prop="parameter"
          :label="$t('apiGateWay.aggregateParameter')"
        >
          <el-input
            v-model="routeConfig.parameter"
          />
        </el-form-item>
        <el-form-item
          prop="jsonPath"
          :label="$t('apiGateWay.aggregateJsonPath')"
        >
          <el-input
            v-model="routeConfig.jsonPath"
          />
        </el-form-item>

        <el-form-item
          style="text-align: center;"
          label-width="0px"
        >
          <el-button
            type="primary"
            style="width:180px"
            @click="onSaveAggregateRouteConfig"
          >
            {{ $t('apiGateWay.createAggregateRouteConfig') }}
          </el-button>
        </el-form-item>
        <el-divider />
      </el-form>
    </div>
    <el-table
      row-key="key"
      :data="aggregateRoute.reRouteKeysConfig"
      border
      fit
      highlight-current-row
      style="width: 100%;"
    >
      <el-table-column
        :label="$t('apiGateWay.aggregateRouteKey')"
        prop="aggregateRouteKey"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.aggregateRouteKey }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('apiGateWay.aggregateParameter')"
        prop="parameter"
        sortable
        min-width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.parameter }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('apiGateWay.aggregateJsonPath')"
        prop="jsonPath"
        sortable
        min-width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.jsonPath }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('global.operaActions')"
        align="center"
        width="150px"
        fixed="right"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!checkPermission(['ApiGateway.AggregateRoute.ManageRouteConfig'])"
            size="mini"
            type="primary"
            @click="handleDeleteAggregateRouteConfig(row.reRouteKey)"
          >
            {{ $t('apiGateWay.deleteAggregateRouteConfig') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script lang="ts">
import ApiGatewayService, { AggregateReRouteConfigCreate, AggregateReRoute } from '@/api/apigateway'
import { Component, Prop, Watch, Mixins } from 'vue-property-decorator'
import { checkPermission } from '@/utils/permission'

import LocalizationMiXin from '@/mixins/LocalizationMiXin'

@Component({
  name: 'AggregateRouteConfigEditForm',
  methods: {
    checkPermission
  }
})
export default class extends Mixins(LocalizationMiXin) {
  @Prop({ default: '' })
  private aggregateRouteId!: string

  private aggregateRoute!: AggregateReRoute
  private routeConfig: AggregateReRouteConfigCreate
  private routeConfigRules = {
    reRouteKey: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('apiGateWay.aggregateRouteKey') }), trigger: 'blur' }
    ]
  }

  constructor() {
    super()
    this.aggregateRoute = AggregateReRoute.empty()
    this.routeConfig = AggregateReRouteConfigCreate.empty()
  }

  @Watch('aggregateRouteId', { immediate: true })
  private onAggregateRouteIdChanged() {
    if (this.aggregateRouteId) {
      ApiGatewayService.getAggregateReRouteByRouteId(this.aggregateRouteId).then(route => {
        this.aggregateRoute = route
      })
    }
  }

  private handleDeleteAggregateRouteConfig(key: string) {
    this.$confirm(this.l('apiGateWay.deleteAggregateRouteConfigByKey', { key: key }),
      this.l('apiGateWay.deleteAggregateRouteConfig'), {
        callback: (action) => {
          if (action === 'confirm') {
            ApiGatewayService.deleteAggregateRouteConfig(this.aggregateRoute.reRouteId, key).then(() => {
              const deleteRouteConfigIndex = this.aggregateRoute.reRouteKeysConfig.findIndex(p => p.reRouteKey === key)
              this.aggregateRoute.reRouteKeysConfig.splice(deleteRouteConfigIndex, 1)
              this.$message.success(this.l('apiGateWay.deleteAggregateRouteConfigSuccess', { key: key }))
              this.$emit('closed', true)
            })
          }
        }
      })
  }

  private onSaveAggregateRouteConfig() {
    const frmAggregateRouteConfig = this.$refs.formAggregateRouteConfig as any
    frmAggregateRouteConfig.validate((valid: boolean) => {
      if (valid) {
        this.routeConfig.routeId = this.aggregateRoute.reRouteId
        ApiGatewayService.createAggregateRouteConfig(this.routeConfig).then(routeConfig => {
          this.aggregateRoute.reRouteKeysConfig.push(routeConfig)
          const successMessage = this.l('apiGateWay.createAggregateRouteConfigSuccess', { key: routeConfig.reRouteKey })
          this.$message.success(successMessage)
          this.resetFields()
          this.$emit('closed', true)
        })
      }
    })
  }

  public resetFields() {
    this.aggregateRouteId = ''
    this.routeConfig = AggregateReRouteConfigCreate.empty()
  }
}
</script>
