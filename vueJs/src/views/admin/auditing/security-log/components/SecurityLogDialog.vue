<template>
  <el-dialog
    :visible="showDialog"
    title="安全日志"
    width="800px"
    :close-on-click-modal="true"
    @close="onFormClosed"
  >
    <security-log-profile :security-log="securityLog" />
  </el-dialog>
</template>

<script lang="ts">
import { SecurityLog } from '@/api/auditing'
import { Component, Vue, Prop } from 'vue-property-decorator'
import SecurityLogProfile from './SecurityLogProfile.vue'

@Component({
  name: 'SecurityLogDialog',
  components: {
    SecurityLogProfile
  }
})
export default class extends Vue {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({
    default: () => { return {} }
  })
  private securityLog!: SecurityLog

  private onFormClosed() {
    this.$emit('closed')
  }
}
</script>
