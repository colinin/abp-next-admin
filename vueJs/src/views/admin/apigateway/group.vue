<template>
  <div class="app-container">
    <div class="filter-container">
      <label
        class="radio-label"
        style="padding-left:0;"
      >{{ $t('apiGateWay.appId') }}</label>
      <el-input
        v-model="routeGroupQuery.appId"
        :placeholder="$t('apiGateWay.appId')"
        style="width: 250px;margin-left: 10px;"
        class="filter-item"
      />
      <label
        class="radio-label"
        style="padding-left:10px;"
      >{{ $t('queryFilter') }}</label>
      <el-input
        v-model="routeGroupQuery.filter"
        :placeholder="$t('filterString')"
        style="width: 250px;margin-left: 10px;"
        class="filter-item"
      />
      <el-button
        class="filter-item"
        style="margin-left: 10px; text-alignt"
        type="primary"
        @click="handleGetRouteGroups"
      >
        {{ $t('searchList') }}
      </el-button>
      <el-button
        class="filter-item"
        type="primary"
        :disabled="!checkPermission(['ApiGateway.RouteGroup.Create'])"
        @click="handleCreateOrEditRouteGroup()"
      >
        {{ $t('apiGateWay.createGroup') }}
      </el-button>
    </div>

    <el-table
      v-loading="routeGroupListLoading"
      row-key="id"
      :data="routeGroupList"
      border
      fit
      highlight-current-row
      style="width: 100%;"
      :default-sort="sortRule"
      @sort-change="handleSortChange"
    >
      <el-table-column
        :label="$t('apiGateWay.groupName')"
        prop="name"
        sortable
        width="110px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.name }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('apiGateWay.appId')"
        prop="appId"
        width="110px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.appId }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('apiGateWay.appName')"
        prop="appName"
        sortable
        min-width="180"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.appName }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('apiGateWay.appIpAddress')"
        prop="appIpAddress"
        width="140px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.appIpAddress }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('apiGateWay.isActive')"
        prop="isActive"
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <el-tag
            :type="row.isActive ? 'success' : 'warning'"
          >
            {{ row.isActive ? $t('apiGateWay.active') : $t('apiGateWay.dnActive') }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('apiGateWay.description')"
        prop="description"
        sortable
        width="140px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.description }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('creationTime')"
        prop="creationTime"
        sortable
        width="140px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.creationTime | dateTimeFilter }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('operaActions')"
        align="center"
        width="250px"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!checkPermission(['ApiGateway.RouteGroup.Update'])"
            size="mini"
            type="primary"
            @click="handleCreateOrEditRouteGroup(row.appId, row.appName)"
          >
            {{ $t('apiGateWay.updateGroup') }}
          </el-button>
          <el-button
            :disabled="!checkPermission(['ApiGateway.RouteGroup.Delete'])"
            size="mini"
            type="warning"
            @click="handleDeleteRouteGroup(row.appId, row.appName)"
          >
            {{ $t('apiGateWay.deleteGroup') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <Pagination
      v-show="routeGroupCount>0"
      :total="routeGroupCount"
      :page.sync="routeGroupQuery.skipCount"
      :limit.sync="routeGroupQuery.maxResultCount"
      @pagination="handleGetRouteGroups"
    />

    <el-dialog
      :visible.sync="showEditRouteGroupDialog"
      :title="editRouteGroupTitle"
      custom-class="modal-form"
      :show-close="false"
    >
      <RouteGroupCreateOrEditForm
        :app-id="editRouteGroupAppId"
        @closed="handleRouteGroupEditFormClosed"
      />
    </el-dialog>
  </div>
</template>

<script lang="ts">
import { dateFormat } from '@/utils'
import { checkPermission } from '@/utils/permission'
import { Component, Vue } from 'vue-property-decorator'
import Pagination from '@/components/Pagination/index.vue'
import RouteGroupCreateOrEditForm from './components/RouteGroupCreateOrEditForm.vue'
import ApiGatewayService, { RouteGroupDto, RouteGroupGetByPagedDto } from '@/api/apigateway'

@Component({
  name: 'RouteGroup',
  components: {
    Pagination,
    RouteGroupCreateOrEditForm
  },
  filters: {
    dateTimeFilter(datetime: string) {
      const date = new Date(datetime)
      return dateFormat(date, 'YYYY-mm-dd HH:MM')
    }
  },
  methods: {
    checkPermission
  }
})
export default class extends Vue {
  private editRouteGroupAppId!: string
  private editRouteGroupTitle!: any
  private showEditRouteGroupDialog!: boolean
  private routeGroupListLoading!: boolean
  private routeGroupList?: RouteGroupDto[]
  private routeGroupQuery!: RouteGroupGetByPagedDto
  private routeGroupCount!: number
  /** 排序组别 */
  private sortRule!: { prop: string, sort: string }

  constructor() {
    super()
    this.routeGroupCount = 0
    this.editRouteGroupAppId = ''
    this.editRouteGroupTitle = ''
    this.routeGroupListLoading = false
    this.showEditRouteGroupDialog = false
    this.sortRule = { prop: '', sort: '' }
    this.routeGroupList = new Array<RouteGroupDto>()
    this.routeGroupQuery = new RouteGroupGetByPagedDto()
  }

  mounted() {
    this.handleGetRouteGroups()
  }

  private handleGetRouteGroups() {
    this.routeGroupListLoading = true
    ApiGatewayService.getRouteGroups(this.routeGroupQuery).then(groupData => {
      this.routeGroupList = groupData.items
      this.routeGroupCount = groupData.totalCount
      this.routeGroupListLoading = false
    })
  }

  private handleCreateOrEditRouteGroup(appId: string, appName: string) {
    this.editRouteGroupAppId = appId
    this.editRouteGroupTitle = this.$t('apiGateWay.createGroup')
    if (appName) {
      this.editRouteGroupTitle = this.$t('apiGateWay.updateGroupByApp', { name: appName })
    }
    this.showEditRouteGroupDialog = true
  }

  private handleRouteGroupEditFormClosed(hasChanged: boolean) {
    this.editRouteGroupAppId = ''
    this.editRouteGroupTitle = ''
    this.showEditRouteGroupDialog = false
    if (hasChanged) {
      this.handleGetRouteGroups()
    }
  }

  private handleDeleteRouteGroup(appId: string, appName: string) {
    const title = this.$t('delNotRecoverData').toString()
    const message = this.$t('whetherDeleteData', { name: appName }).toString()
    this.$confirm(message, title, {
      callback: async action => {
        if (action === 'confirm') {
          await ApiGatewayService.deleteRouteGroup(appId)
          const successMessage = this.$t('dataHasBeenDeleted', { name: appName }).toString()
          this.$message.success(successMessage)
          this.handleGetRouteGroups()
        }
      }
    })
  }

  /** 响应表格排序事件 */
  private handleSortChange(column: any) {
    this.sortRule.prop = column.prop
    this.sortRule.sort = column.sort
  }
}
</script>
