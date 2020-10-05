<template>
  <div class="app-container">
    <div class="filter-container">
      <el-card>
        <el-collapse>
          <el-collapse-item>
            <template slot="title">
              <span class="data-filter-collapse-title">{{ $t('queryFilter') }}</span>
            </template>
            <el-form label-width="100px">
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
                  <el-form-item :label="$t('AbpAuditLogging.ClientId')">
                    <el-input
                      v-model="dataFilter.clientId"
                    />
                  </el-form-item>
                </el-col>
              </el-row>
              <el-row>
                <el-col :span="8">
                  <el-form-item :label="$t('AbpAuditLogging.Identity')">
                    <el-input
                      v-model="dataFilter.identity"
                    />
                  </el-form-item>
                </el-col>
                <el-col :span="8">
                  <el-form-item :label="$t('AbpAuditLogging.ActionName')">
                    <el-input
                      v-model="dataFilter.actionName"
                    />
                  </el-form-item>
                </el-col>
                <el-col :span="8">
                  <el-form-item :label="$t('AbpAuditLogging.CorrelationId')">
                    <el-input
                      v-model="dataFilter.correlationId"
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
        :label="$t('AbpAuditLogging.CreationTime')"
        prop="creationTime"
        sortable
        width="200px"
      >
        <template slot-scope="{row}">
          <span>{{ row.creationTime | dateTimeFormatFilter }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpAuditLogging.UserName')"
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
        :label="$t('AbpAuditLogging.ClientId')"
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
        :label="$t('AbpAuditLogging.ClientName')"
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
        :label="$t('AbpAuditLogging.ClientIpAddress')"
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
        :label="$t('AbpAuditLogging.Identity')"
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
        :label="$t('AbpAuditLogging.ActionName')"
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
            {{ $t('AbpAuditLogging.ShowLogDialog') }}
          </el-button>
          <el-button
            :disabled="!checkPermission(['AbpAuditing.SecurityLog.Delete'])"
            size="mini"
            type="danger"
            @click="handleDeleteSecurityLog(row.id)"
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

    <security-log-dialog
      :security-log-id="securityLogId"
      :show-dialog="showSecurityLog"
      @closed="onSecurityLogDialogClosed"
    />
  </div>
</template>

<script lang="ts">
import { dateFormat, abpPagerFormat } from '@/utils'
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
  filters: {
    dateTimeFormatFilter(dateTime: Date) {
      return dateFormat(new Date(dateTime), 'YYYY-mm-dd HH:MM:SS:NS')
    }
  },
  methods: {
    checkPermission
  }
})
export default class extends mixins(DataListMiXin) {
  private securityLogId = ''
  private showSecurityLog = false
  public dataFilter = new SecurityLogGetPaged()

  mounted() {
    this.refreshPagedData()
  }

  protected processDataFilter() {
    this.dataFilter.skipCount = abpPagerFormat(this.currentPage, this.pageSize)
  }

  protected getPagedList(filter: any) {
    return AuditingService.getSecurityLogs(filter)
  }

  private handleShowSecurityLogDialog(securityLog: SecurityLog) {
    this.securityLogId = securityLog.id
    this.showSecurityLog = true
  }

  private handleDeleteSecurityLog(id: string) {
    this.$confirm(this.l('questingDeleteByMessage', { message: id }),
      this.l('AbpAuditLogging.DeleteLog'), {
        callback: (action) => {
          if (action === 'confirm') {
            AuditingService.deleteSecurityLog(id).then(() => {
              this.$message.success(this.l('successful'))
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
