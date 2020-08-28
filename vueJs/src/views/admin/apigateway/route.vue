<template>
  <div class="app-container">
    <div class="filter-container">
      <label
        class="radio-label"
        style="padding-left:10px;"
      >{{ $t('apiGateWay.appId') }}</label>
      <el-select
        v-model="routeGetPagedFilter.appId"
        style="width: 250px;margin-left: 10px;"
        class="filter-item"
        :placeholder="$t('pleaseSelectBy', {name: $t('apiGateWay.appId')})"
      >
        <el-option
          v-for="item in routeGroupAppIdOptions"
          :key="item.appId"
          :label="item.appName"
          :value="item.appId"
        />
      </el-select>
      <label
        class="radio-label"
        style="padding-left:10px;"
      >{{ $t('queryFilter') }}</label>
      <el-input
        v-model="routeGetPagedFilter.filter"
        :placeholder="$t('filterString')"
        style="width: 250px;margin-left: 10px;"
        class="filter-item"
      />
      <el-button
        class="filter-item"
        style="margin-left: 10px; text-alignt"
        type="primary"
        @click="handleGetRoutes"
      >
        {{ $t('searchList') }}
      </el-button>
      <el-button
        class="filter-item"
        type="primary"
        :disabled="!checkPermission(['ApiGateway.Route.Create'])"
        @click="handleCreateOrEditRoute()"
      >
        {{ $t('apiGateWay.createRoute') }}
      </el-button>
    </div>

    <el-table
      v-loading="routeListLoading"
      row-key="itemId"
      :data="routeList"
      border
      fit
      highlight-current-row
      style="width: 100%;"
      @sort-change="handleSortChange"
    >
      <el-table-column
        :label="$t('apiGateWay.reRouteName')"
        prop="reRouteName"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.reRouteName }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('apiGateWay.upstreamPathTemplate')"
        prop="upstreamPathTemplate"
        width="250px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.upstreamPathTemplate }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('apiGateWay.downstreamPathTemplate')"
        prop="downstreamPathTemplate"
        width="250px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.downstreamPathTemplate }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('apiGateWay.upstreamHttpMethod')"
        prop="upstreamHttpMethod"
        sortable
        min-width="180"
        align="center"
      >
        <template slot-scope="{row}">
          <span>
            <el-tag
              v-for="(method, index) in row.upstreamHttpMethod"
              :key="index"
              :type="method | httpMethodsFilter"
              style="margin-right: 4px;margin-top: 4px;"
            >
              {{ method }}
            </el-tag>
          </span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('apiGateWay.downstreamHostAndPorts')"
        prop="downstreamHostAndPorts"
        width="140px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>
            <el-tag
              v-for="(address) in row.downstreamHostAndPorts"
              :key="address.host"
              style="margin-right: 4px;margin-top: 4px;"
            >
              {{ address.host + ':' + address.port }}
            </el-tag>
          </span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('apiGateWay.downstreamScheme')"
        prop="downstreamScheme"
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>
            <el-tag type="info">
              {{ row.downstreamScheme }}
            </el-tag>
          </span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('apiGateWay.timeoutValue')"
        prop="timeout"
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.timeout }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('apiGateWay.serviceName')"
        prop="serviceName"
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.serviceName }}</span>
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
            :disabled="!checkPermission(['ApiGateway.Route.Update'])"
            size="mini"
            type="primary"
            @click="handleCreateOrEditRoute(row.reRouteId, row.reRouteName)"
          >
            {{ $t('apiGateWay.updateRoute') }}
          </el-button>
          <el-button
            :disabled="!checkPermission(['ApiGateway.Route.Delete'])"
            size="mini"
            type="danger"
            @click="handleDeleteRoute(row.reRouteId, row.reRouteName)"
          >
            {{ $t('apiGateWay.deleteRoute') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <Pagination
      v-show="routesCount>0"
      :total="routesCount"
      :page.sync="routeGetPagedFilter.skipCount"
      :limit.sync="routeGetPagedFilter.maxResultCount"
      @pagination="handleGetRoutes"
      @sort-change="handleSortChange"
    />

    <el-dialog
      v-el-draggable-dialog
      width="800px"
      :visible.sync="showEditRouteDialog"
      :title="editRouteTitle"
      custom-class="modal-form"
      :show-close="false"
      :close-on-click-modal="false"
      :close-on-press-escape="false"
    >
      <RouteCreateOrEditForm
        :route-id="editRouteId"
        :app-id-options="routeGroupAppIdOptions"
        @closed="handleRouteEditFormClosed"
      />
    </el-dialog>
  </div>
</template>

<script lang="ts">
import { checkPermission } from '@/utils/permission'
import { Component, Vue } from 'vue-property-decorator'
import Pagination from '@/components/Pagination/index.vue'
import RouteCreateOrEditForm from './components/RouteCreateOrEditForm.vue'
import ApiGatewayService, { RouteGroupAppIdDto, ReRouteGetByPagedDto, ReRouteDto } from '@/api/apigateway'

@Component({
  name: 'Route',
  components: {
    Pagination,
    RouteCreateOrEditForm
  },
  methods: {
    checkPermission
  },
  filters: {
    httpMethodsFilter(httpMethod: string) {
      const statusMap: { [key: string]: string } = {
        GET: '',
        POST: 'success',
        PUT: 'warning',
        PATCH: 'warning',
        DELETE: 'danger'
      }
      return statusMap[httpMethod.toUpperCase()]
    }
  }
})
export default class extends Vue {
  private editRouteId: number
  private routesCount: number
  private editRouteTitle: any
  private routeList: ReRouteDto[]
  private routeListLoading: boolean
  private showEditRouteDialog: boolean
  private routeGetPagedFilter: ReRouteGetByPagedDto
  private routeGroupAppIdOptions: RouteGroupAppIdDto[]

  constructor() {
    super()
    this.editRouteId = 0
    this.routesCount = 0
    this.editRouteTitle = ''
    this.routeListLoading = false
    this.showEditRouteDialog = false
    this.routeList = new Array<ReRouteDto>()
    this.routeGetPagedFilter = new ReRouteGetByPagedDto()
    this.routeGroupAppIdOptions = new Array<RouteGroupAppIdDto>()
  }

  mounted() {
    ApiGatewayService.getRouteGroupAppIds().then(appKeys => {
      this.routeGroupAppIdOptions = appKeys.items
    })
  }

  private handleGetRoutes() {
    if (this.routeGetPagedFilter.appId) {
      this.routeListLoading = true
      ApiGatewayService.getReRoutes(this.routeGetPagedFilter).then(routes => {
        this.routeList = routes.items
        this.routesCount = routes.totalCount
      }).finally(() => {
        this.routeListLoading = false
      })
    } else {
      const errorMessage = this.$t('apiGateWay.appIdHasRequired').toString()
      this.$message.warning(errorMessage)
    }
  }

  private handleSortChange(column: any) {
    this.routeGetPagedFilter.sorting = column.prop
  }

  private handleDeleteRoute(reRouteId: number, reRouteName: string) {
    this.$confirm(this.l('apiGateWay.deleteRouteByName', { name: reRouteName }),
      this.l('apiGateWay.deleteRoute'), {
        callback: (action) => {
          if (action === 'confirm') {
            ApiGatewayService.deleteReRoute(reRouteId).then(() => {
              this.$message.success(this.l('apiGateWay.deleteRouteSuccess', { name: reRouteName }))
              this.handleGetRoutes()
            })
          }
        }
      })
  }

  private handleCreateOrEditRoute(reRouteId: number, reRouteName: string) {
    this.editRouteId = reRouteId
    this.editRouteTitle = this.$t('apiGateWay.createRoute')
    if (reRouteId) {
      this.editRouteTitle = this.$t('apiGateWay.updateRouteByApp', { name: reRouteName })
    }
    this.showEditRouteDialog = true
  }

  private handleRouteEditFormClosed(changed: boolean) {
    this.editRouteId = -1
    this.editRouteTitle = ''
    this.showEditRouteDialog = false
    if (changed && this.routeGetPagedFilter.appId) {
      this.handleGetRoutes()
    }
  }

  private l(name: string, values?: any[] | { [key: string]: any }) {
    return this.$t(name, values).toString()
  }
}
</script>
