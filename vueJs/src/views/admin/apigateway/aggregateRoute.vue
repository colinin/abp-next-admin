<template>
  <div class="app-container">
    <div class="filter-container">
      <label
        class="radio-label"
        style="padding-left:10px;"
      >{{ $t('apiGateWay.appId') }}</label>
      <el-select
        v-model="dataFilter.appId"
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
        :disabled="!checkPermission(['ApiGateway.AggregateRoute.Create'])"
        @click="handleCreateOrEditAggregateRoute()"
      >
        {{ $t('apiGateWay.createAggregateRoute') }}
      </el-button>
    </div>

    <el-table
      v-loading="dataLoading"
      row-key="reRouteId"
      :data="dataList"
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
      v-show="dataTotal>0"
      :total="dataTotal"
      :page.sync="currentPage"
      :limit.sync="pageSize"
      @pagination="refreshPagedData"
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
import DataListMiXin from '@/mixins/DataListMiXin'
import Component, { mixins } from 'vue-class-component'
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
export default class extends mixins(DataListMiXin) {
  private editAggregateRouteId = ''
  private editRouteTitle = ''
  private routeGroupAppIdOptions = new Array<RouteGroupAppIdDto>()

  private showEditAggregateRouteDialog = false
  private showEditAggregateRouteConfigDialog = false

  public dataFilter = new AggregateReRouteGetByPaged()

  mounted() {
    ApiGatewayService.getRouteGroupAppIds().then(appKeys => {
      this.routeGroupAppIdOptions = appKeys.items
    })
  }

  protected getPagedList(filter: any) {
    if (filter.appId) {
      return ApiGatewayService.getAggregateReRoutes(filter)
    } else {
      const errorMessage = this.$t('apiGateWay.appIdHasRequired').toString()
      this.$message.warning(errorMessage)
    }
    return this.getEmptyPagedList()
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
              this.refreshPagedData()
            })
          }
        }
      })
  }

  private handleCreateOrEditAggregateRoute(reRouteId: string, name: string) {
    this.editAggregateRouteId = reRouteId
    this.editRouteTitle = this.l('apiGateWay.createAggregateRoute')
    if (reRouteId) {
      this.editRouteTitle = this.l('apiGateWay.updateAggregateRouteByName', { name: name })
    }
    this.showEditAggregateRouteDialog = true
  }

  private handleAggregateRouteEditFormClosed(changed: boolean) {
    this.editAggregateRouteId = ''
    this.editRouteTitle = ''
    this.showEditAggregateRouteDialog = false
    if (changed && this.dataFilter.appId) {
      this.refreshPagedData()
    }
  }

  private handleAggregateRouteConfigFormClosed() {
    this.editAggregateRouteId = ''
    this.showEditAggregateRouteConfigDialog = false
  }
}
</script>

<style scoped>
.options {
  vertical-align: top;
  margin-left: 20px;
}
</style>
