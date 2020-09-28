<template>
  <div class="app-container">
    <div class="filter-container">
      <el-card>
        <el-collapse>
          <el-collapse-item>
            <template slot="title">
              <span class="data-filter-collapse-title">查询条件</span>
            </template>
            <el-form label-width="100px">
              <el-row>
                <el-col :span="8">
                  <el-form-item label="应用名称">
                    <el-input
                      v-model="dataFilter.applicationName"
                    />
                  </el-form-item>
                </el-col>
                <el-col :span="8">
                  <el-form-item label="用户名称">
                    <el-input
                      v-model="dataFilter.userName"
                    />
                  </el-form-item>
                </el-col>
                <el-col :span="8">
                  <el-form-item label="客户端标识">
                    <el-input
                      v-model="dataFilter.clientId"
                    />
                  </el-form-item>
                </el-col>
              </el-row>
              <el-row>
                <el-col :span="16">
                  <el-form-item label="主体名称">
                    <el-input
                      v-model="dataFilter.identity"
                    />
                  </el-form-item>
                </el-col>
                <el-col :span="8">
                  <el-form-item label="方法名称">
                    <el-input
                      v-model="dataFilter.actionName"
                    />
                  </el-form-item>
                </el-col>
              </el-row>
              <el-row>
                <el-col :span="12">
                  <el-form-item label="客户端标识">
                    <el-input
                      v-model="dataFilter.clientId"
                    />
                  </el-form-item>
                </el-col>
                <el-col :span="12">
                  <el-form-item label="调用链">
                    <el-input
                      v-model="dataFilter.correlationId"
                    />
                  </el-form-item>
                </el-col>
              </el-row>
              <el-row>
                <el-col :span="12">
                  <el-form-item label="开始时间">
                    <el-date-picker
                      v-model="dataFilter.startTime"
                      placeholder="选择日期时间"
                      type="datetime"
                      default-time="00:00:00"
                      style="width: 100%"
                      value-format="yyyy-MM-dd HH:mm:ss"
                    />
                  </el-form-item>
                </el-col>
                <el-col :span="12">
                  <el-form-item label="结束时间">
                    <el-date-picker
                      v-model="dataFilter.endTime"
                      placeholder="选择日期时间"
                      type="datetime"
                      default-time="23:59:59"
                      style="width: 100%"
                      value-format="yyyy-MM-dd HH:mm:ss"
                    />
                  </el-form-item>
                </el-col>
              </el-row>
              <el-button
                class="filter-item"
                style="display:block;margin:0 auto; width: 150px;"
                type="primary"
                @click="refreshPagedData"
              >
                <i class="el-icon-search" />
                查询日志
              </el-button>
            </el-form>
          </el-collapse-item>
        </el-collapse>
      </el-card>
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
      @row-dblclick="onRowDoubleClicked"
    >
      <el-table-column
        label="应用程序"
        prop="applicationName"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.applicationName }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="操作用户"
        prop="userName"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.userName }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="客户端标识"
        prop="clientId"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.clientId }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="客户端名称"
        prop="clientName"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.clientName }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="客户端地址"
        prop="clientIpAddress"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.clientIpAddress }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="认证服务器"
        prop="identity"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.identity }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="方法名称"
        prop="action"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.action }}</span>
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
            :disabled="!checkPermission(['AbpAuditing.SecurityLog'])"
            size="mini"
            type="primary"
            @click="handleShowSecurityLogDialog(row)"
          >
            查看日志
          </el-button>
          <el-button
            :disabled="!checkPermission(['AbpAuditing.SecurityLog.Delete'])"
            size="mini"
            type="danger"
            @click="handleDeleteSecurityLog(row.id)"
          >
            删除日志
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <pagination
      v-show="dataTotal>0"
      :total="dataTotal"
      :page.sync="dataFilter.skipCount"
      :limit.sync="dataFilter.maxResultCount"
      @pagination="refreshPagedData"
      @sort-change="handleSortChange"
    />

    <security-log-dialog
      :security-log="securityLog"
      :show-dialog="showSecurityLog"
      @closed="onSecurityLogDialogClosed"
    />
  </div>
</template>

<script lang="ts">
import { checkPermission } from '@/utils/permission'
import AuditingService, { SecurityLog, SecurityLogGetPaged } from '@/api/auditing'
import DataListMiXin from '@/mixins/DataListMiXin'
import Component, { mixins } from 'vue-class-component'
import Pagination from '@/components/Pagination/index.vue'
import SecurityLogDialog from './components/SecurityLogDialog.vue'

@Component({
  name: 'SecurityLog',
  components: {
    Pagination,
    SecurityLogDialog
  },
  methods: {
    checkPermission
  }
})
export default class extends mixins(DataListMiXin) {
  private securityLog = new SecurityLog()

  private showSecurityLog = false
  public dataFilter = new SecurityLogGetPaged()

  mounted() {
    this.refreshPagedData()
  }

  protected getPagedList(filter: any) {
    return AuditingService.getSecurityLogs(filter)
  }

  private handleShowSecurityLogDialog(securityLog: SecurityLog) {
    this.securityLog = securityLog
    this.showSecurityLog = true
  }

  private handleDeleteSecurityLog(id: string) {
    this.$confirm('是否要删除选定的安全日志记录?',
      '删除安全日志', {
        callback: (action) => {
          if (action === 'confirm') {
            AuditingService.deleteSecurityLog(id).then(() => {
              this.$message.success('删除成功!')
              this.refreshPagedData()
            })
          }
        }
      })
  }

  private onRowDoubleClicked(row: SecurityLog) {
    this.handleShowSecurityLogDialog(row)
  }

  private onSecurityLogDialogClosed() {
    this.showSecurityLog = false
  }
}
</script>

<style lang="scss" scoped>
.data-filter-collapse-title {
  font-size: 15px;
}
</style>
