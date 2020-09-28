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
                  <el-form-item label="请求方法">
                    <el-input
                      v-model="dataFilter.httpMethod"
                    />
                  </el-form-item>
                </el-col>
              </el-row>
              <el-row>
                <el-col :span="16">
                  <el-form-item label="请求路径">
                    <el-input
                      v-model="dataFilter.url"
                    />
                  </el-form-item>
                </el-col>
                <el-col :span="8">
                  <el-form-item label="响应状态">
                    <el-input
                      v-model="dataFilter.httpStatusCode"
                      type="number"
                    />
                  </el-form-item>
                </el-col>
              </el-row>
              <el-row>
                <el-col :span="6">
                  <el-form-item label="包含异常">
                    <el-switch
                      v-model="dataFilter.hasException"
                    />
                  </el-form-item>
                </el-col>
                <el-col :span="18">
                  <el-form-item label="调用链">
                    <el-input
                      v-model="dataFilter.correlationId"
                    />
                  </el-form-item>
                </el-col>
              </el-row>
              <el-row>
                <el-col :span="12">
                  <el-form-item label="最短响应时间">
                    <el-input
                      v-model="dataFilter.minExecutionDuration"
                      type="number"
                    />
                  </el-form-item>
                </el-col>
                <el-col :span="12">
                  <el-form-item label="最长响应时间">
                    <el-input
                      v-model="dataFilter.maxExecutionDuration"
                      type="number"
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
        label="请求路径"
        prop="url"
        sortable
        width="250px"
      >
        <template slot-scope="{row}">
          <el-tag>
            {{ row.url }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="状态码"
        prop="httpStatusCode"
        sortable
        width="100px"
        align="center"
      >
        <template slot-scope="{row}">
          <el-tag
            :type="row.httpStatusCode | httpStatusCodeFilter"
          >
            {{ row.httpStatusCode }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column
        label="操作用户"
        prop="userName"
        sortable
        width="150px"
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
        width="150px"
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
        width="150px"
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
        width="150px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.clientIpAddress }}</span>
        </template>
      </el-table-column>
      <el-table-column
        label="操作方法"
        prop="httpMethod"
        sortable
        width="150px"
        align="center"
      >
        <template slot-scope="{row}">
          <el-tag
            :type="row.httpMethod | httpMethodFilter"
          >
            {{ row.httpMethod }}
          </el-tag>
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
            :disabled="!checkPermission(['AbpAuditing.AuditLog'])"
            size="mini"
            type="primary"
            @click="handleShowAuditLogDialog(row)"
          >
            查看日志
          </el-button>
          <el-button
            :disabled="!checkPermission(['AbpAuditing.AuditLog.Delete'])"
            size="mini"
            type="danger"
            @click="handleDeleteAuditLog(row.id)"
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

    <audit-log-dialog
      :audit-log="auditLog"
      :show-dialog="showAuditLog"
      @closed="onAuditLogDialogClosed"
    />
  </div>
</template>

<script lang="ts">
import { checkPermission } from '@/utils/permission'
import AuditingService, { AuditLog, AuditLogGetPaged } from '@/api/auditing'
import DataListMiXin from '@/mixins/DataListMiXin'
import Component, { mixins } from 'vue-class-component'
import Pagination from '@/components/Pagination/index.vue'
import AuditLogDialog from './components/AuditLogDialog.vue'

const statusMap: { [key: string]: string } = {
  GET: '',
  POST: 'success',
  PUT: 'warning',
  PATCH: 'warning',
  DELETE: 'danger'
}

@Component({
  name: 'AuditLog',
  components: {
    Pagination,
    AuditLogDialog
  },
  filters: {
    httpMethodFilter(httpMethod: string) {
      return statusMap[httpMethod]
    },
    httpStatusCodeFilter(httpStatusCode: number) {
      if (httpStatusCode >= 200 && httpStatusCode < 300) {
        return 'success'
      }
      if (httpStatusCode >= 300 && httpStatusCode < 500) {
        return 'warning'
      }
      if (httpStatusCode >= 500) {
        return 'danger'
      }
      return ''
    }
  },
  methods: {
    checkPermission
  }
})
export default class extends mixins(DataListMiXin) {
  private auditLog = new AuditLog()

  private showAuditLog = false
  public dataFilter = new AuditLogGetPaged()

  mounted() {
    this.refreshPagedData()
  }

  protected getPagedList(filter: any) {
    return AuditingService.getAuditLogs(filter)
  }

  private handleShowAuditLogDialog(auditLog: AuditLog) {
    this.auditLog = auditLog
    this.showAuditLog = true
  }

  private handleDeleteAuditLog(id: string) {
    this.$confirm('是否要删除选定的审计日志记录?',
      '删除审计日志', {
        callback: (action) => {
          if (action === 'confirm') {
            AuditingService.deleteAuditLog(id).then(() => {
              this.$message.success('删除成功!')
              this.refreshPagedData()
            })
          }
        }
      })
  }

  private onRowDoubleClicked(row: AuditLog) {
    this.handleShowAuditLogDialog(row)
  }

  private onAuditLogDialogClosed() {
    this.showAuditLog = false
  }
}
</script>

<style lang="scss" scoped>
.data-filter-collapse-title {
  font-size: 15px;
}
</style>
