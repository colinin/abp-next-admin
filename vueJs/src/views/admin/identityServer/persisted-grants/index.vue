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
        {{ $t('AbpIdentityServer.Search') }}
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
        :label="$t('AbpIdentityServer.Client:Id')"
        prop="clientId"
        sortable
        width="180px"
      >
        <template slot-scope="{row}">
          <span>{{ row.clientId }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Grants:Key')"
        prop="key"
        sortable
        width="300px"
      >
        <template slot-scope="{row}">
          <span>{{ row.key }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Grants:Type')"
        prop="type"
        sortable
        width="150px"
      >
        <template slot-scope="{row}">
          <span>{{ row.type }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Grants:SessionId')"
        prop="sessionId"
        sortable
        width="180px"
      >
        <template slot-scope="{row}">
          <span>{{ row.sessionId }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Description')"
        prop="description"
        sortable
        width="200px"
      >
        <template slot-scope="{row}">
          <span>{{ row.description }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('global.operaActions')"
        align="center"
        width="150px"
      >
        <template slot-scope="{row}">
          <el-button
            size="mini"
            type="success"
            icon="el-icon-tickets"
            @click="onShowProfile(row.id)"
          />
          <el-button
            :disabled="!checkPermission(['AbpIdentityServer.Grants.Delete'])"
            size="mini"
            type="danger"
            icon="el-icon-delete"
            @click="onDeleted(row.id, row.key)"
          />
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

    <PersistedGrantProfile
      :id="selectId"
      :show-dialog="showProfileDialog"
      @closed="showProfileDialog=false"
    />
  </div>
</template>

<script lang="ts">
import { dateFormat, abpPagerFormat } from '@/utils/index'
import { checkPermission } from '@/utils/permission'
import DataListMiXin from '@/mixins/DataListMiXin'
import HttpProxyMiXin from '@/mixins/HttpProxyMiXin'
import Component, { mixins } from 'vue-class-component'
import Pagination from '@/components/Pagination/index.vue'
import PersistedGrantProfile from './components/PersistedGrantProfile.vue'

import { GetGrantByPaged, PersistedGrant } from '@/api/grants'

@Component({
  name: 'IdentityServerGrant',
  components: {
    Pagination,
    PersistedGrantProfile
  },
  methods: {
    checkPermission,
    local(name: string) {
      return this.$t(name)
    }
  },
  filters: {
    statusFilter(status: boolean) {
      if (status) {
        return 'success'
      }
      return 'warning'
    },
    datetimeFilter(val: string) {
      const date = new Date(val)
      return dateFormat(date, 'YYYY-mm-dd HH:MM')
    }
  }
})
export default class extends mixins(DataListMiXin, HttpProxyMiXin) {
  public dataFilter = new GetGrantByPaged()
  private selectId = ''
  private showProfileDialog = false

  mounted() {
    this.refreshPagedData()
  }

  protected processDataFilter() {
    this.dataFilter.skipCount = abpPagerFormat(this.currentPage, this.pageSize)
  }

  protected getPagedList(filter: any) {
    return this.pagedRequest<PersistedGrant>({
      service: 'IdentityServer',
      controller: 'PersistedGrant',
      action: 'GetListAsync',
      params: {
        input: filter
      }
    })
  }

  private onShowProfile(id: string) {
    this.selectId = id
    this.showProfileDialog = true
  }

  private onDeleted(id: string, key: string) {
    this.$confirm(this.l('AbpIdentityServer.Grants:DeleteByKey', { Key: key }),
      this.l('AbpUi.AreYouSure'), {
        callback: (action) => {
          if (action === 'confirm') {
            this.request<void>({
              service: 'IdentityServer',
              controller: 'PersistedGrant',
              action: 'DeleteAsync',
              params: {
                id: id
              }
            }).then(() => {
              this.$message.success(this.l('global.successful'))
              this.refreshPagedData()
            })
          }
        }
      })
  }
}
</script>

<style lang="scss" scoped>
.roleItem {
  width: 40px;
}
.options {
  vertical-align: top;
  margin-left: 20px;
}
.el-dropdown + .el-dropdown {
  margin-left: 15px;
}
.el-icon-arrow-down {
  font-size: 12px;
}
</style>
