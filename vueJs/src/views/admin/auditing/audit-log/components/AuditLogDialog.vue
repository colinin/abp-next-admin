<template>
  <el-dialog
    :visible="showDialog"
    :title="$t('AbpAuditLogging.AuditLog')"
    width="800px"
    custom-class="modal-form"
    :close-on-click-modal="true"
    @close="onFormClosed"
  >
    <audit-log-profile :audit-log-id="auditLogId" />
  </el-dialog>
</template>

<script lang="ts">
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

  @Prop({ default: '' })
  private auditLogId!: string

  private onFormClosed() {
    this.$emit('closed')
  }
}
</script>
