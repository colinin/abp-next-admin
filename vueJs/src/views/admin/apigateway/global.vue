<template>
  <div class="app-container">
    <div class="filter-container">
      <label
        class="radio-label"
        style="padding-left:10px;"
      >{{ $t('queryFilter') }}</label>
      <el-input
        v-model="globalConfigurationGetQuery.filter"
        :placeholder="$t('filterString')"
        style="width: 250px;margin-left: 10px;"
        class="filter-item"
      />
      <el-button
        class="filter-item"
        style="margin-left: 10px; text-alignt"
        type="primary"
        @click="handledGetGlobalConfigurations"
      >
        {{ $t('searchList') }}
      </el-button>
      <el-button
        class="filter-item"
        type="primary"
        :disabled="!checkPermission(['ApiGateway.Global.Create'])"
        @click="handleCreateOrEditGlobalConfiguration()"
      >
        {{ $t('apiGateWay.createGlobal') }}
      </el-button>
    </div>

    <el-table
      v-loading="globalConfigurationsLoading"
      row-key="itemId"
      :data="globalConfigurations"
      border
      fit
      highlight-current-row
      style="width: 100%;"
      @sort-change="handleSortChange"
    >
      <el-table-column
        :label="$t('apiGateWay.baseUrl')"
        prop="baseUrl"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.baseUrl }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('apiGateWay.requestIdKey')"
        prop="requestIdKey"
        width="110px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.requestIdKey }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('apiGateWay.downstreamScheme')"
        prop="downstreamScheme"
        sortable
        min-width="180"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.downstreamScheme }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('apiGateWay.downstreamHttpVersion')"
        prop="downstreamHttpVersion"
        width="140px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.downstreamHttpVersion }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('apiGateWay.discoverHost')"
        prop="serviceDiscoveryProvider.host"
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.serviceDiscoveryProvider.host }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('apiGateWay.discoverPort')"
        prop="serviceDiscoveryProvider.port"
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.serviceDiscoveryProvider.port }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('apiGateWay.namespace')"
        prop="serviceDiscoveryProvider.namespace"
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.serviceDiscoveryProvider.namespace }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('operaActions')"
        align="center"
        width="250px"
        fixed="right"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!checkPermission(['ApiGateway.Global.Update'])"
            size="mini"
            type="primary"
            @click="handleCreateOrEditGlobalConfiguration(row.appId)"
          >
            {{ $t('apiGateWay.updateGlobal') }}
          </el-button>
          <el-button
            :disabled="!checkPermission(['ApiGateway.Global.Delete'])"
            size="mini"
            type="warning"
            @click="handleDeleteGlobalConfiguration(row.appId)"
          >
            {{ $t('apiGateWay.deleteGlobal') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <Pagination
      v-show="globalConfigurationsCount>0"
      :total="globalConfigurationsCount"
      :page.sync="globalConfigurationGetQuery.skipCount"
      :limit.sync="globalConfigurationGetQuery.maxResultCount"
      @pagination="handledGetGlobalConfigurations"
    />

    <el-dialog
      width="700px"
      :visible.sync="showEditGlobalConfiguration"
      :title="editGlobalConfigurationTitle"
      custom-class="modal-form"
      :show-close="false"
    >
      <GlobalCreateOrEditForm
        :app-id="editGlobalConfigurationAppId"
        @closed="handleGlobalConfigurationEditFormClosed"
      />
    </el-dialog>
  </div>
</template>

<script lang="ts">
import { checkPermission } from '@/utils/permission'
import { Component, Vue } from 'vue-property-decorator'
import Pagination from '@/components/Pagination/index.vue'
import GlobalCreateOrEditForm from './components/GlobalCreateOrEditForm.vue'
import ApiGatewayService, { GlobalGetByPagedDto, GlobalConfigurationDto } from '@/api/apigateway'

@Component({
  name: 'GlobalRoute',
  components: {
    Pagination,
    GlobalCreateOrEditForm
  },
  methods: {
    checkPermission
  }
})
export default class extends Vue {
  private editGlobalConfigurationTitle!: any
  private globalConfigurationsCount!: number
  private globalConfigurationsLoading!: boolean
  private showEditGlobalConfiguration!: boolean
  private editGlobalConfigurationAppId!: string
  private globalConfigurations!: GlobalConfigurationDto[]
  private globalConfigurationGetQuery!: GlobalGetByPagedDto

  constructor() {
    super()
    this.globalConfigurationsCount = 0
    this.editGlobalConfigurationTitle = ''
    this.editGlobalConfigurationAppId = ''
    this.globalConfigurationsLoading = false
    this.showEditGlobalConfiguration = false
    this.globalConfigurationGetQuery = new GlobalGetByPagedDto()
    this.globalConfigurations = new Array<GlobalConfigurationDto>()
  }

  mounted() {
    this.handledGetGlobalConfigurations()
  }

  private handledGetGlobalConfigurations() {
    this.globalConfigurationsLoading = true
    ApiGatewayService.getGlobalConfigurations(this.globalConfigurationGetQuery).then(globals => {
      this.globalConfigurations = globals.items
      this.globalConfigurationsCount = globals.totalCount
      this.globalConfigurationsLoading = false
    })
  }

  private handleCreateOrEditGlobalConfiguration(appId: string) {
    this.editGlobalConfigurationAppId = appId
    this.editGlobalConfigurationTitle = this.$t('apiGateWay.createGlobal')
    if (appId) {
      this.editGlobalConfigurationTitle = this.$t('apiGateWay.updateGlobalByApp', { name: appId })
    }
    this.showEditGlobalConfiguration = true
  }

  private handleGlobalConfigurationEditFormClosed(changed: boolean) {
    this.editGlobalConfigurationAppId = ''
    this.editGlobalConfigurationTitle = ''
    this.showEditGlobalConfiguration = false
    if (changed) {
      this.handledGetGlobalConfigurations()
    }
  }

  private handleDeleteGlobalConfiguration(itemId: number, appId: string) {
    const title = this.$t('delNotRecoverData').toString()
    const message = this.$t('whetherDeleteData', { name: appId }).toString()
    this.$confirm(message, title, {
      callback: async action => {
        if (action === 'confirm') {
          await ApiGatewayService.deleteGlobalConfiguration(itemId)
          const successMessage = this.$t('dataHasBeenDeleted', { name: appId }).toString()
          this.$message.success(successMessage)
          this.handledGetGlobalConfigurations()
        }
      }
    })
  }

  /** 响应表格排序事件 */
  private handleSortChange(column: any) {
    this.globalConfigurationGetQuery.sorting = column.prop
  }
}
</script>
