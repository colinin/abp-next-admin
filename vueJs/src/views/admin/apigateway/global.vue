<template>
  <div class="app-container">
    <div class="filter-container">
      <label
        class="radio-label"
        style="padding-left:10px;"
      >{{ $t('queryFilter') }}</label>
      <el-input
        v-model="dataFilter.filter"
        :placeholder="$t('filterString')"
        style="width: 250px;margin-left: 10px;"
        class="filter-item"
      />
      <el-button
        class="filter-item"
        style="margin-left: 10px; text-alignt"
        type="primary"
        @click="refreshPagedData"
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
      v-loading="dataLoading"
      row-key="itemId"
      :data="dataList"
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
            type="danger"
            @click="handleDeleteGlobalConfiguration(row.appId)"
          >
            {{ $t('apiGateWay.deleteGlobal') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <Pagination
      v-show="dataTotal>0"
      :total="dataTotal"
      :page.sync="currentPage"
      :limit.sync="pageSize"
      @pagination="refreshPagedData"
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
import DataListMiXin from '@/mixins/DataListMiXin'
import Component, { mixins } from 'vue-class-component'
import Pagination from '@/components/Pagination/index.vue'
import GlobalCreateOrEditForm from './components/GlobalCreateOrEditForm.vue'
import ApiGatewayService, { GlobalGetByPagedDto } from '@/api/apigateway'

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
export default class extends mixins(DataListMiXin) {
  private editGlobalConfigurationTitle = ''
  private showEditGlobalConfiguration = false
  private editGlobalConfigurationAppId = ''

  public dataFilter = new GlobalGetByPagedDto()

  mounted() {
    this.refreshPagedData()
  }

  protected getPagedList(filter: any) {
    return ApiGatewayService.getGlobalConfigurations(filter)
  }

  private handleCreateOrEditGlobalConfiguration(appId: string) {
    this.editGlobalConfigurationAppId = appId
    this.editGlobalConfigurationTitle = this.l('apiGateWay.createGlobal')
    if (appId) {
      this.editGlobalConfigurationTitle = this.l('apiGateWay.updateGlobalByApp', { name: appId })
    }
    this.showEditGlobalConfiguration = true
  }

  private handleGlobalConfigurationEditFormClosed(changed: boolean) {
    this.editGlobalConfigurationAppId = ''
    this.editGlobalConfigurationTitle = ''
    this.showEditGlobalConfiguration = false
    if (changed) {
      this.refreshPagedData()
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
          this.refreshPagedData()
        }
      }
    })
  }
}
</script>
