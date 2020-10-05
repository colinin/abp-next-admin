<template>
  <div class="app-container">
    <div class="filter-container">
      <el-card>
        <el-collapse>
          <el-collapse-item>
            <template slot="title">
              <span class="data-filter-collapse-title">{{ $t('queryFilter') }}</span>
            </template>
            <el-form label-width="130px">
              <el-row>
                <el-col :span="8">
                  <el-form-item :label="$t('AbpAuditLogging.ApplicationName')">
                    <el-input
                      v-model="dataFilter.applicationName"
                    />
                  </el-form-item>
                </el-col>
                <el-col :span="8">
                  <el-form-item :label="$t('AbpAuditLogging.UserName')">
                    <el-input
                      v-model="dataFilter.userName"
                    />
                  </el-form-item>
                </el-col>
                <el-col :span="8">
                  <el-form-item :label="$t('AbpAuditLogging.HttpMethod')">
                    <el-input
                      v-model="dataFilter.httpMethod"
                    />
                  </el-form-item>
                </el-col>
              </el-row>
              <el-row>
                <el-col :span="16">
                  <el-form-item :label="$t('AbpAuditLogging.RequestUrl')">
                    <el-input
                      v-model="dataFilter.url"
                    />
                  </el-form-item>
                </el-col>
                <el-col :span="8">
                  <el-form-item :label="$t('AbpAuditLogging.HttpStatusCode')">
                    <el-input
                      v-model="dataFilter.httpStatusCode"
                      type="number"
                    />
                  </el-form-item>
                </el-col>
              </el-row>
              <el-row>
                <el-col :span="6">
                  <el-form-item :label="$t('AbpAuditLogging.HasException')">
                    <el-switch
                      v-model="dataFilter.hasException"
                    />
                  </el-form-item>
                </el-col>
                <el-col :span="18">
                  <el-form-item :label="$t('AbpAuditLogging.CorrelationId')">
                    <el-input
                      v-model="dataFilter.correlationId"
                    />
                  </el-form-item>
                </el-col>
              </el-row>
              <el-row>
                <el-col :span="12">
                  <el-form-item :label="$t('AbpAuditLogging.MinExecutionDuration')">
                    <el-input
                      v-model="dataFilter.minExecutionDuration"
                      type="number"
                    />
                  </el-form-item>
                </el-col>
                <el-col :span="12">
                  <el-form-item :label="$t('AbpAuditLogging.MaxExecutionDuration')">
                    <el-input
                      v-model="dataFilter.maxExecutionDuration"
                      type="number"
                    />
                  </el-form-item>
                </el-col>
              </el-row>
              <el-row>
                <el-col :span="12">
                  <el-form-item :label="$t('AbpAuditLogging.StartTime')">
                    <el-date-picker
                      v-model="dataFilter.startTime"
                      :placeholder="$t('AbpAuditLogging.SelectDateTime')"
                      type="datetime"
                      default-time="00:00:00"
                      style="width: 100%"
                      value-format="yyyy-MM-dd HH:mm:ss"
                    />
                  </el-form-item>
                </el-col>
                <el-col :span="12">
                  <el-form-item :label="$t('AbpAuditLogging.EndTime')">
                    <el-date-picker
                      v-model="dataFilter.endTime"
                      :placeholder="$t('AbpAuditLogging.SelectDateTime')"
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
                @click="resetPagedList"
              >
                <i class="el-icon-search" />
                {{ $t('AbpAuditLogging.SecrchLog') }}
              </el-button>
            </el-form>
          </el-collapse-item>
        </el-collapse>
      </el-card>
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
      @row-dblclick="onRowDoubleClicked"
    >
      <el-table-column
        :label="$t('AbpAuditLogging.ApplicationName')"
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
        :label="$t('AbpAuditLogging.RequestUrl')"
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
        :label="$t('AbpAuditLogging.HttpMethod')"
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
        :label="$t('AbpAuditLogging.ExecutionTime')"
        prop="executionTime"
        sortable
        width="200px"
      >
        <template slot-scope="{row}">
          <span>{{ row.executionTime | dateTimeFormatFilter }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpAuditLogging.ExecutionDuration')"
        prop="executionDuration"
        sortable
        width="160px"
        align="center"
      >
        <template slot-scope="{row}">
          <el-tag
            :type="row.executionDuration | executionDurationFilter"
          >
            {{ row.executionDuration }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpAuditLogging.HttpStatusCode')"
        prop="httpStatusCode"
        sortable
        width="130px"
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
        :label="$t('AbpAuditLogging.UserName')"
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
        :label="$t('AbpAuditLogging.ClientId')"
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
        :label="$t('AbpAuditLogging.ClientName')"
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
        :label="$t('AbpAuditLogging.ClientIpAddress')"
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
            {{ $t('AbpAuditLogging.ShowLogDialog') }}
          </el-button>
          <el-button
            :disabled="!checkPermission(['AbpAuditing.AuditLog.Delete'])"
            size="mini"
            type="danger"
            @click="handleDeleteAuditLog(row.id)"
          >
            {{ $t('AbpAuditLogging.DeleteLog') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <pagination
      v-show="dataTotal>0"
      :total="dataTotal"
      :page.sync="currentPage"
      :limit.sync="pageSize"
      @pagination="refreshPagedData"
      @sort-change="handleSortChange"
    />

    <audit-log-dialog
      :audit-log-id="auditLogId"
      :show-dialog="showAuditLog"
      @closed="onAuditLogDialogClosed"
    />
  </div>
</template>

<script lang="ts">
import { dateFormat, abpPagerFormat } from '@/utils'
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
    dateTimeFormatFilter(dateTime: Date) {
      return dateFormat(new Date(dateTime), 'YYYY-mm-dd HH:MM:SS:NS')
    },
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
    },
    executionDurationFilter(executionDuration: number) {
      if (executionDuration < 100) {
        return 'success'
      }
      if (executionDuration < 500) {
        return 'primary'
      }
      if (executionDuration < 1000) {
        return 'warning'
      }
      if (executionDuration > 1000) {
        return 'danger'
      }
    }
  },
  methods: {
    checkPermission
  }
})
export default class extends mixins(DataListMiXin) {
  private auditLogId = ''

  private showAuditLog = false
  public dataFilter = new AuditLogGetPaged()

  mounted() {
    this.refreshPagedData()
  }

  protected processDataFilter() {
    this.dataFilter.skipCount = abpPagerFormat(this.currentPage, this.pageSize)
  }

  protected getPagedList(filter: any) {
    return AuditingService.getAuditLogs(filter)
  }

  private handleShowAuditLogDialog(auditLog: AuditLog) {
    this.auditLogId = auditLog.id
    this.showAuditLog = true
  }

  private handleDeleteAuditLog(id: string) {
    this.$confirm(this.l('questingDeleteByMessage', { message: id }),
      this.l('AbpAuditLogging.DeleteLog'), {
        callback: (action) => {
          if (action === 'confirm') {
            AuditingService.deleteAuditLog(id).then(() => {
              this.$message.success(this.l('successful'))
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
