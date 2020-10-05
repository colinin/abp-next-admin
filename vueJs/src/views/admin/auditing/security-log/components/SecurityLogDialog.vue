<template>
  <el-dialog
    :visible="showDialog"
    :title="$t('AbpAuditLogging.SecurityLog')"
    width="800px"
    custom-class="modal-form"
    :close-on-click-modal="true"
    @close="onFormClosed"
  >
    <security-log-profile :security-log-id="securityLogId" />
  </el-dialog>
</template>

<script lang="ts">
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

  @Prop({ default: '' })
  private securityLogId!: string

  private onFormClosed() {
    this.$emit('closed')
  }
}
</script>
