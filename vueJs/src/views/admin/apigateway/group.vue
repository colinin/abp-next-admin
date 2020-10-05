<template>
  <div class="app-container">
    <div class="filter-container">
      <label
        class="radio-label"
        style="padding-left:0;"
      >{{ $t('apiGateWay.appId') }}</label>
      <el-input
        v-model="dataFilter.appId"
        :placeholder="$t('apiGateWay.appId')"
        style="width: 250px;margin-left: 10px;"
        class="filter-item"
      />
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
        :disabled="!checkPermission(['ApiGateway.RouteGroup.Create'])"
        @click="handleCreateOrEditRouteGroup()"
      >
        {{ $t('apiGateWay.createGroup') }}
      </el-button>
    </div>

    <el-table
      v-loading="dataLoading"
      row-key="id"
      :data="dataList"
      border
      fit
      highlight-current-row
      style="width: 100%;"
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
            type="danger"
            @click="handleDeleteRouteGroup(row.appId, row.appName)"
          >
            {{ $t('apiGateWay.deleteGroup') }}
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
import Pagination from '@/components/Pagination/index.vue'
import DataListMiXin from '@/mixins/DataListMiXin'
import Component, { mixins } from 'vue-class-component'
import RouteGroupCreateOrEditForm from './components/RouteGroupCreateOrEditForm.vue'
import ApiGatewayService, { GlobalGetByPagedDto } from '@/api/apigateway'

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
export default class extends mixins(DataListMiXin) {
  private editRouteGroupAppId = ''
  private editRouteGroupTitle = ''
  private showEditRouteGroupDialog = false

  public dataFilter = new GlobalGetByPagedDto()

  mounted() {
    this.refreshPagedData()
  }

  protected getPagedList(filter: any) {
    return ApiGatewayService.getRouteGroups(filter)
  }

  private handleCreateOrEditRouteGroup(appId: string, appName: string) {
    this.editRouteGroupAppId = appId
    this.editRouteGroupTitle = this.l('apiGateWay.createGroup')
    if (appName) {
      this.editRouteGroupTitle = this.l('apiGateWay.updateGroupByApp', { name: appName })
    }
    this.showEditRouteGroupDialog = true
  }

  private handleRouteGroupEditFormClosed(hasChanged: boolean) {
    this.editRouteGroupAppId = ''
    this.editRouteGroupTitle = ''
    this.showEditRouteGroupDialog = false
    if (hasChanged) {
      this.refreshPagedData()
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
          this.refreshPagedData()
        }
      }
    })
  }
}
</script>
