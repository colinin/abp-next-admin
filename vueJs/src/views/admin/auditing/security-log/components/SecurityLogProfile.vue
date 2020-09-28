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
              v-model="securityLog.applicationName"
              readonly
            />
          </el-form-item>
          <el-form-item label="租户名称">
            <el-input
              v-model="securityLog.tenantName"
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
              v-model="securityLog.userId"
              readonly
            />
          </el-form-item>
          <el-form-item label="用户名称">
            <el-input
              v-model="securityLog.userName"
              readonly
            />
          </el-form-item>
          <el-form-item label="客户端标识">
            <el-input
              v-model="securityLog.clientId"
              readonly
            />
          </el-form-item>
          <el-form-item label="客户端名称">
            <el-input
              v-model="securityLog.clientName"
              readonly
            />
          </el-form-item>
          <el-form-item label="客户端地址">
            <el-input
              v-model="securityLog.clientIpAddress"
              readonly
            />
          </el-form-item>
          <el-form-item label="浏览器信息">
            <el-input
              v-model="securityLog.browserInfo"
              type="textarea"
              readonly
            />
          </el-form-item>
        </el-tab-pane>
        <el-tab-pane
          label="操作信息"
          name="operation"
        >
          <el-form-item label="主体名称">
            <el-input
              v-model="securityLog.identity"
              readonly
            />
          </el-form-item>
          <el-form-item label="方法名称">
            <el-input
              v-model="securityLog.action"
              readonly
            />
          </el-form-item>
          <el-form-item label="创建时间">
            <el-input
              :value="getFormatDateTime(securityLog.creationTime)"
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
            v-for="(key, index) in Object.keys(securityLog.extraProperties)"
            :key="index"
            :label="key"
          >
            <el-input
              v-model="securityLog.extraProperties[key]"
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
import { SecurityLog } from '@/api/auditing'
import { dateFormat } from '@/utils'

@Component({
  name: 'SecurityLogProfile',
  computed: {
    getFormatDateTime() {
      return (dateTime: any) => {
        return dateFormat(new Date(dateTime), 'YYYY-mm-dd HH:MM:SS:NS')
      }
    }
  }
})
export default class extends Vue {
  @Prop({
    default: () => { return {} }
  })
  private securityLog!: SecurityLog

  private activedTabItem = 'application'

  get hasExtraProperties() {
    if (this.securityLog.extraProperties) {
      return Object.keys(this.securityLog.extraProperties).length > 0
    }
    return false
  }

  @Watch('securityLog')
  private onSecurityLogChanged() {
    this.activedTabItem = 'application'
  }
}
</script>

<style lang="scss" scoped>
</style>
