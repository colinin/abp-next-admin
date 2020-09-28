<template>
  <el-dialog
    :visible="showDialog"
    title="审计日志"
    width="800px"
    :close-on-click-modal="true"
    @close="onFormClosed"
  >
    <audit-log-profile :audit-log="auditLog" />
  </el-dialog>
</template>

<script lang="ts">
import { AuditLog } from '@/api/auditing'
import { Component, Vue, Prop } from 'vue-property-decorator'
import AuditLogProfile from './AuditLogProfile.vue'

@Component({
  name: 'AuditLogDialog',
  components: {
    AuditLogProfile
  }
})
export default class extends Vue {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({
    default: () => { return {} }
  })
  private auditLog!: AuditLog

  private onFormClosed() {
    console.log('关闭窗口')
    this.$emit('closed')
  }
}
</script>
