<template>
  <div class="app-container">
    <div class="filter-container">
      <label
        class="radio-label"
        style="padding-left:10px;"
      >{{ $t('apiGateWay.appId') }}</label>
      <el-select
        v-model="aggregateRouteGetPagedFilter.appId"
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
        v-model="aggregateRouteGetPagedFilter.filter"
        :placeholder="$t('filterString')"
        style="width: 250px;margin-left: 10px;"
        class="filter-item"
      />
      <el-button
        class="filter-item"
        style="margin-left: 10px; text-alignt"
        type="primary"
        @click="handleGetAggregateRoutes"
      >
        {{ $t('searchList') }}
      </el-button>
      <el-button
        class="filter-item"
        type="primary"
        :disabled="!checkPermission(['ApiGateway.AggregateRoute.Create'])"
        @click="handleCreateOrEditAggregateRoute()"
      >
        {{ $t('apiGateWay.createAggregateRoute') }}
      </el-button>
    </div>

    <el-table
      v-loading="aggregateRouteListLoading"
      row-key="reRouteId"
      :data="aggregateRouteList"
      border
      fit
      highlight-current-row
      style="width: 100%;"
      @sort-change="handleSortChange"
    >
      <el-table-column
        :label="$t('apiGateWay.aggregateRouteName')"
        prop="name"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.name }}</span>
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
        :label="$t('apiGateWay.upstreamHost')"
        prop="upstreamHost"
        width="250px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.upstreamHost }}</span>
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
        :label="$t('apiGateWay.reRouteKeys')"
        prop="reRouteKeys"
        width="140px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>
            <el-tag
              v-for="(routeKey) in row.reRouteKeys"
              :key="routeKey"
              style="margin-right: 4px;margin-top: 4px;"
            >
              {{ routeKey }}
            </el-tag>
          </span>
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
            :disabled="!checkPermission(['ApiGateway.AggregateRoute.Update'])"
            size="mini"
            type="primary"
            @click="handleCreateOrEditAggregateRoute(row.reRouteId, row.name)"
          >
            {{ $t('apiGateWay.updateAggregateRoute') }}
          </el-button>
          <el-dropdown
            class="options"
            @command="handleCommand"
          >
            <el-button
              v-permission="['ApiGateway.AggregateRoute']"
              size="mini"
              type="info"
            >
              {{ $t('global.otherOpera') }}<i class="el-icon-arrow-down el-icon--right" />
            </el-button>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                :command="{key: 'config', row}"
                :disabled="!checkPermission(['ApiGateway.AggregateRoute.ManageRouteConfig'])"
              >
                {{ $t('apiGateWay.routeKeysConfig') }}
              </el-dropdown-item>
              <el-dropdown-item
                divided
                :command="{key: 'delete', row}"
                :disabled="!checkPermission(['ApiGateway.AggregateRoute.Delete'])"
              >
                {{ $t('apiGateWay.deleteAggregateRoute') }}
              </el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
        </template>
      </el-table-column>
    </el-table>

    <Pagination
      v-show="routesCount>0"
      :total="routesCount"
      :page.sync="aggregateRouteGetPagedFilter.skipCount"
      :limit.sync="aggregateRouteGetPagedFilter.maxResultCount"
      @pagination="handleGetAggregateRoutes"
      @sort-change="handleSortChange"
    />

    <el-dialog
      v-el-draggable-dialog
      width="800px"
      :visible.sync="showEditAggregateRouteDialog"
      :title="editRouteTitle"
      custom-class="modal-form"
      :show-close="false"
      :close-on-click-modal="false"
      :close-on-press-escape="false"
    >
      <AggregateRouteCreateOrEditForm
        :aggregate-route-id="editAggregateRouteId"
        :app-id-options="routeGroupAppIdOptions"
        @closed="handleAggregateRouteEditFormClosed"
      />
    </el-dialog>

    <el-dialog
      v-el-draggable-dialog
      width="800px"
      :visible.sync="showEditAggregateRouteConfigDialog"
      :title="$t('apiGateWay.routeKeysConfig')"
      custom-class="modal-form"
      :show-close="false"
    >
      <AggregateRouteConfigEditForm
        :aggregate-route-id="editAggregateRouteId"
        @closed="handleAggregateRouteConfigFormClosed"
      />
    </el-dialog>
  </div>
</template>

<script lang="ts">
import { checkPermission } from '@/utils/permission'
import { Component, Vue } from 'vue-property-decorator'
import Pagination from '@/components/Pagination/index.vue'
import AggregateRouteConfigEditForm from './components/AggregateRouteConfigEditForm.vue'
import AggregateRouteCreateOrEditForm from './components/AggregateRouteCreateOrEditForm.vue'
import ApiGatewayService, { RouteGroupAppIdDto, AggregateReRoute, AggregateReRouteGetByPaged } from '@/api/apigateway'

@Component({
  name: 'AggregateRoute',
  components: {
    Pagination,
    AggregateRouteConfigEditForm,
    AggregateRouteCreateOrEditForm
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
  private editAggregateRouteId: string
  private routesCount: number
  private editRouteTitle: any
  private aggregateRouteList: AggregateReRoute[]
  private aggregateRouteListLoading: boolean
  private aggregateRouteGetPagedFilter: AggregateReRouteGetByPaged
  private routeGroupAppIdOptions: RouteGroupAppIdDto[]

  private showEditAggregateRouteDialog: boolean
  private showEditAggregateRouteConfigDialog: boolean

  constructor() {
    super()
    this.editAggregateRouteId = ''
    this.routesCount = 0
    this.editRouteTitle = ''
    this.aggregateRouteListLoading = false
    this.aggregateRouteList = new Array<AggregateReRoute>()
    this.aggregateRouteGetPagedFilter = new AggregateReRouteGetByPaged()
    this.routeGroupAppIdOptions = new Array<RouteGroupAppIdDto>()

    this.showEditAggregateRouteDialog = false
    this.showEditAggregateRouteConfigDialog = false
  }

  mounted() {
    ApiGatewayService.getRouteGroupAppIds().then(appKeys => {
      this.routeGroupAppIdOptions = appKeys.items
    })
  }

  private handleGetAggregateRoutes() {
    if (this.aggregateRouteGetPagedFilter.appId) {
      this.aggregateRouteListLoading = true
      ApiGatewayService.getAggregateReRoutes(this.aggregateRouteGetPagedFilter).then(routes => {
        this.aggregateRouteList = routes.items
        this.routesCount = routes.totalCount
      }).finally(() => {
        this.aggregateRouteListLoading = false
      })
    } else {
      const errorMessage = this.$t('apiGateWay.appIdHasRequired').toString()
      this.$message.warning(errorMessage)
    }
  }

  private handleSortChange(column: any) {
    this.aggregateRouteGetPagedFilter.sorting = column.prop
  }

  private handleCommand(command: {key: string, row: AggregateReRoute }) {
    this.editAggregateRouteId = command.row.reRouteId
    switch (command.key) {
      case 'config':
        this.showEditAggregateRouteConfigDialog = true
        break
      case 'delete':
        this.handleDeleteAggregateRoute(this.editAggregateRouteId, command.row.name)
        break
    }
  }

  private handleDeleteAggregateRoute(reRouteId: string, name: string) {
    this.$confirm(this.l('apiGateWay.deleteAggregateRouteByName', { name: name }),
      this.l('apiGateWay.deleteAggregateRoute'), {
        callback: (action) => {
          if (action === 'confirm') {
            ApiGatewayService.deleteAggregateReRoute(reRouteId).then(() => {
              this.$message.success(this.l('apiGateWay.deleteAggregateRouteSuccess', { name: name }))
              this.handleGetAggregateRoutes()
            })
          }
        }
      })
  }

  private handleCreateOrEditAggregateRoute(reRouteId: string, name: string) {
    this.editAggregateRouteId = reRouteId
    this.editRouteTitle = this.$t('apiGateWay.createAggregateRoute')
    if (reRouteId) {
      this.editRouteTitle = this.$t('apiGateWay.updateAggregateRouteByName', { name: name })
    }
    this.showEditAggregateRouteDialog = true
  }

  private handleAggregateRouteEditFormClosed(changed: boolean) {
    this.editAggregateRouteId = ''
    this.editRouteTitle = ''
    this.showEditAggregateRouteDialog = false
    if (changed && this.aggregateRouteGetPagedFilter.appId) {
      this.handleGetAggregateRoutes()
    }
  }

  private handleAggregateRouteConfigFormClosed() {
    this.editAggregateRouteId = ''
    this.showEditAggregateRouteConfigDialog = false
  }

  private l(name: string, values?: any[] | { [key: string]: any }) {
    return this.$t(name, values).toString()
  }
}
</script>

<style scoped>
.options {
  vertical-align: top;
  margin-left: 20px;
}
</style>
