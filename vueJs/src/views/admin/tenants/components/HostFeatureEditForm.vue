<template>
  <el-dialog
    :visible="showDialog"
    :title="$t('AbpTenantManagement.ManageHostFeatures')"
    width="800px"
    custom-class="modal-form"
    :show-close="false"
    @close="onFormClosed"
  >
    <feature-management
      ref="featureManagement"
      provider-name="G"
      provider-key=""
      :load-feature="showDialog"
      @closed="onFormClosed"
    />
  </el-dialog>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import FeatureManagement from '../../components/FeatureManagement.vue'

@Component({
  name: 'HostFeatureEditForm',
  components: {
    FeatureManagement
  }
})
export default class extends Vue {
  @Prop({ default: false })
  private showDialog!: boolean

  private onFormClosed() {
    this.$emit('closed')
  }
}
</script>
