<template>
  <el-dialog
    :visible="showDialog"
    :title="$t('AbpTenantManagement.Permission:ManageFeatures')"
    width="800px"
    custom-class="modal-form"
    :show-close="false"
    @close="onFormClosed"
  >
    <feature-management
      ref="featureManagement"
      provider-name="T"
      :provider-key="tenantId"
      :load-feature="showDialog"
      @closed="onFormClosed"
    />
  </el-dialog>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import FeatureManagement from '../../components/FeatureManagement.vue'

@Component({
  name: 'TenantFeatureEditForm',
  components: {
    FeatureManagement
  }
})
export default class extends Vue {
  @Prop({ default: '' })
  private tenantId!: string

  @Prop({ default: false })
  private showDialog!: boolean

  private onFormClosed() {
    this.$emit('closed')
  }
}
</script>
