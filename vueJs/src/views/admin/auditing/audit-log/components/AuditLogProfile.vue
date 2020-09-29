<template>
  <div>
    <el-form label-width="100px">
      <el-tabs v-model="activedTabItem">
        <el-tab-pane
          label="应用信息"
          name="application"
        >
          <el-form-item label="应用名称">
            <el-input
              v-model="auditLog.applicationName"
              readonly
            />
          </el-form-item>
          <el-form-item label="租户标识">
            <el-input
              v-model="auditLog.tenantId"
              readonly
            />
          </el-form-item>
          <el-form-item label="租户名称">
            <el-input
              v-model="auditLog.tenantName"
              readonly
            />
          </el-form-item>
          <el-form-item
            v-if="auditLog.impersonatorTenantId !== null"
            label="模拟租户"
          >
            <el-input
              v-model="auditLog.impersonatorTenantId"
              readonly
            />
          </el-form-item>
        </el-tab-pane>
        <el-tab-pane
          label="用户信息"
          name="userInfo"
        >
          <el-form-item label="用户标识">
            <el-input
              v-model="auditLog.userId"
              readonly
            />
          </el-form-item>
          <el-form-item
            v-if="auditLog.impersonatorUserId !== null"
            label="模拟用户"
          >
            <el-input
              v-model="auditLog.impersonatorUserId"
              readonly
            />
          </el-form-item>
          <el-form-item label="用户名称">
            <el-input
              v-model="auditLog.userName"
              readonly
            />
          </el-form-item>
          <el-form-item label="客户端标识">
            <el-input
              v-model="auditLog.clientId"
              readonly
            />
          </el-form-item>
          <el-form-item label="客户端名称">
            <el-input
              v-model="auditLog.clientName"
              readonly
            />
          </el-form-item>
          <el-form-item label="客户端地址">
            <el-input
              v-model="auditLog.clientIpAddress"
              readonly
            />
          </el-form-item>
          <el-form-item label="浏览器信息">
            <el-input
              v-model="auditLog.browserInfo"
              type="textarea"
              readonly
            />
          </el-form-item>
        </el-tab-pane>
        <el-tab-pane
          label="操作信息"
          name="operation"
        >
          <el-form-item label="请求路径">
            <el-input
              v-model="auditLog.url"
              readonly
            />
          </el-form-item>
          <el-form-item label="操作方法">
            <el-input
              v-model="auditLog.httpMethod"
              readonly
            />
          </el-form-item>
          <el-form-item label="调用时间">
            <el-input
              :value="getFormatDateTime(auditLog.executionTime)"
              readonly
            />
          </el-form-item>
          <el-form-item label="响应时间">
            <el-input
              v-model="auditLog.executionDuration"
              readonly
            />
          </el-form-item>
          <el-form-item label="响应状态">
            <el-input
              v-model="auditLog.httpStatusCode"
              readonly
            />
          </el-form-item>
          <el-form-item label="调用链标识">
            <el-input
              v-model="auditLog.correlationId"
              readonly
            />
          </el-form-item>
        </el-tab-pane>
        <el-tab-pane
          label="调用方法"
          name="methodInvoke"
          style="height:300px;overflow-y:auto;overflow-x:hidden;"
        >
          <el-timeline>
            <el-timeline-item
              v-for="(action, index) in getActions"
              :key="index"
              :timestamp="getFormatDateTime(action.executionTime)"
              :type="auditLog.httpStatusCode | httpStatusCodeTimelineFilter"
              placement="top"
            >
              <el-card>
                <el-form-item label="调用服务">
                  <el-input
                    v-model="action.serviceName"
                    readonly
                  />
                </el-form-item>
                <el-form-item label="方法名称">
                  <el-input
                    v-model="action.methodName"
                    readonly
                  />
                </el-form-item>
                <el-form-item label="响应时间">
                  <el-input
                    v-model="action.executionDuration"
                    readonly
                  />
                </el-form-item>
                <el-form-item label="参数列表">
                  <json-editor
                    :value="getFormatJsonValue(action.parameters)"
                  />
                </el-form-item>
              </el-card>
            </el-timeline-item>
          </el-timeline>
        </el-tab-pane>
        <el-tab-pane
          v-if="getEntitiesChanges.length > 0"
          label="实体变更"
          name="entitiesChanged"
          style="height:300px;overflow-y:auto;overflow-x:hidden;"
        >
          <el-timeline>
            <el-timeline-item
              v-for="(entity, index) in getEntitiesChanges"
              :key="index"
              :timestamp="getFormatDateTime(entity.changeTime)"
              :type="entity.changeType | entityChangeTypeTimelineFilter"
              placement="top"
            >
              <el-card>
                <el-form-item label="变更类型">
                  <el-input
                    :value="entity.changeType | entityChangeTypeNameFilter"
                    readonly
                  />
                </el-form-item>
                <el-form-item label="实体类型">
                  <el-input
                    v-model="entity.entityTypeFullName"
                    readonly
                  />
                </el-form-item>
                <el-form-item label="实体标识">
                  <el-input
                    v-model="entity.entityId"
                    readonly
                  />
                </el-form-item>
                <el-form-item label="租户标识">
                  <el-input
                    v-model="entity.entityTenantId"
                    readonly
                  />
                </el-form-item>
                <el-form-item label="属性变更">
                  <el-table
                    row-key="id"
                    :data="entity.propertyChanges"
                    border
                    fit
                    highlight-current-row
                    style="width: 100%;"
                  >
                    <el-table-column
                      label="属性名称"
                      prop="propertyName"
                      sortable
                      width="200px"
                    >
                      <template slot-scope="{row}">
                        <span>{{ row.propertyName }}</span>
                      </template>
                    </el-table-column>
                    <el-table-column
                      label="当前值"
                      prop="newValue"
                      sortable
                      width="320px"
                    >
                      <template slot-scope="{row}">
                        <span>{{ row.newValue }}</span>
                      </template>
                    </el-table-column>
                    <el-table-column
                      label="原始值"
                      prop="originalValue"
                      sortable
                      width="320px"
                    >
                      <template slot-scope="{row}">
                        <span>{{ row.originalValue }}</span>
                      </template>
                    </el-table-column>
                    <el-table-column
                      label="属性类型"
                      prop="propertyTypeFullName"
                      sortable
                      width="500px"
                    >
                      <template slot-scope="{row}">
                        <span>{{ row.propertyTypeFullName }}</span>
                      </template>
                    </el-table-column>
                  </el-table>
                </el-form-item>
              </el-card>
            </el-timeline-item>
          </el-timeline>
        </el-tab-pane>
        <el-tab-pane
          v-if="hasException"
          label="异常信息"
          name="exception"
        >
          <el-form-item label="异常堆栈">
            <el-input
              v-model="auditLog.exceptions"
              type="textarea"
              :rows="15"
              readonly
            />
          </el-form-item>
        </el-tab-pane>
        <el-tab-pane
          v-if="hasExtraProperties"
          label="附加信息"
          name="extraProperties"
        >
          <el-form-item
            v-for="(key, index) in Object.keys(auditLog.extraProperties)"
            :key="index"
            :label="key"
          >
            <el-input
              v-model="auditLog.extraProperties[key]"
              readonly
            />
          </el-form-item>
        </el-tab-pane>
      </el-tabs>
    </el-form>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator'
import AuditingService, { Action, AuditLog, EntityChange, ChangeType } from '@/api/auditing'
import { dateFormat } from '@/utils'
import JsonEditor from '@/components/JsonEditor/index.vue'

const entityChangeTypeNameMap: { [key: number]: string } = {
  [ChangeType.Created]: '新增',
  [ChangeType.Updated]: '修改',
  [ChangeType.Deleted]: '删除'
}
const entityChangeTypeTimelineMap: { [key: number]: string } = {
  [ChangeType.Created]: 'success',
  [ChangeType.Updated]: 'warning',
  [ChangeType.Deleted]: 'danger'
}

@Component({
  name: 'AuditLogProfile',
  components: {
    JsonEditor
  },
  filters: {
    httpStatusCodeTimelineFilter(httpStatusCode: number) {
      if (httpStatusCode >= 200 && httpStatusCode < 300) {
        return 'success'
      }
      if (httpStatusCode >= 300 && httpStatusCode < 500) {
        return 'warning'
      }
      if (httpStatusCode >= 500) {
        return 'danger'
      }
      return 'primary'
    },
    entityChangeTypeNameFilter(type: ChangeType) {
      return entityChangeTypeNameMap[type]
    },
    entityChangeTypeTimelineFilter(type: ChangeType) {
      return entityChangeTypeTimelineMap[type]
    }
  },
  computed: {
    getFormatDateTime() {
      return (dateTime: any) => {
        return dateFormat(new Date(dateTime), 'YYYY-mm-dd HH:MM:SS:NS')
      }
    },
    getFormatJsonValue() {
      return (jsonString: string) => {
        if (jsonString !== '') {
          return JSON.parse(jsonString)
        }
        return null
      }
    }
  }
})
export default class extends Vue {
  @Prop({ default: '' })
  private auditLogId!: string

  private auditLog = new AuditLog()
  private activedTabItem = 'application'

  @Watch('auditLogId', { immediate: true })
  private onAuditLogIdChanged() {
    if (this.auditLogId) {
      AuditingService.getAuditLogById(this.auditLogId).then(res => {
        this.auditLog = res
        this.activedTabItem = 'application'
      })
    }
  }

  get hasExtraProperties() {
    if (this.auditLog.extraProperties) {
      return Object.keys(this.auditLog.extraProperties).length > 0
    }
    return false
  }

  get hasException() {
    if (this.auditLog.exceptions) {
      return true
    }
    return false
  }

  get getActions() {
    if (this.auditLog.actions && this.auditLog.actions.length > 0) {
      return this.sortByDateTime(this.auditLog.actions, 'executionTime')
    }
    return new Array<Action>()
  }

  get getEntitiesChanges() {
    if (this.auditLog.entityChanges && this.auditLog.entityChanges.length > 0) {
      return this.sortByDateTime(this.auditLog.entityChanges, 'changeTime')
    }
    return new Array<EntityChange>()
  }

  private sortByDateTime(array: Array<any>, sortField: string) {
    return array.sort((obj1, obj2) => {
      if (obj1[sortField] && obj2[sortField]) {
        const time1 = new Date(obj1[sortField])
        const time2 = new Date(obj2[sortField])
        return time1.getTime() - time2.getTime()
      }
      return 0
    })
  }
}
</script>

<style lang="scss" scoped>
</style>
