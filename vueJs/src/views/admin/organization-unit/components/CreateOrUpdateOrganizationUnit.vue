<template>
  <el-dialog
    v-el-draggable-dialog
    width="400px"
    :visible="showDialog"
    :title="title"
    custom-class="modal-form"
    :show-close="false"
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    @close="onFormClosed"
  >
    <el-form
      ref="formOrganizationUnit"
      :model="organizationUnit"
    >
      <el-form-item
        prop="displayName"
        :label="$t(('AbpIdentity.OrganizationUnit:DisplayName'))"
        :rules="{
          required: true,
          message: $t('pleaseInputBy', {key: $t('AbpIdentity.OrganizationUnit:DisplayName')}),
          trigger: 'blur'
        }"
      >
        <el-input
          v-model="organizationUnit.displayName"
        />
      </el-form-item>

      <el-form-item>
        <el-button
          class="cancel"
          type="info"
          @click="onFormClosed"
        >
          {{ $t('AbpIdentityServer.Cancel') }}
        </el-button>
        <el-button
          class="confirm"
          type="primary"
          icon="el-icon-check"
          @click="onSave"
        >
          {{ $t('AbpIdentityServer.Save') }}
        </el-button>
      </el-form-item>
    </el-form>
  </el-dialog>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator'

import { Form } from 'element-ui'
import OrganizationUnitService, {
  OrganizationUnit,
  OrganizationUnitCreate,
  OrganizationUnitUpdate
} from '@/api/organizationunit'

@Component({
  name: 'CreateOrUpdateOrganizationUnit'
})
export default class CreateOrUpdateOrganizationUnit extends Vue {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: '' })
  private title!: string

  @Prop({ default: false })
  private isEdit!: boolean

  @Prop({ default: null })
  private organizationUnitId?: string

  @Prop({ default: () => { console.log('onOrganizationUnitChanged') } })
  private onOrganizationUnitChanged!: Function

  private organizationUnit = new OrganizationUnit()

  @Watch('showDialog')
  private onShowDialogChanged() {
    this.handleGetOrganizationUnit()
  }

  private handleGetOrganizationUnit() {
    if (this.showDialog && this.isEdit && this.organizationUnitId) {
      OrganizationUnitService
        .getOrganizationUnit(this.organizationUnitId)
        .then(ou => {
          this.organizationUnit = ou
        })
    }
  }

  private onSave() {
    const formOrganizationUnit = this.$refs.formOrganizationUnit as Form
    formOrganizationUnit
      .validate(valid => {
        if (valid) {
          if (this.isEdit) {
            const updateOu = new OrganizationUnitUpdate()
            updateOu.displayName = this.organizationUnit.displayName
            OrganizationUnitService
              .updateOrganizationUnit(this.organizationUnit.id, updateOu)
              .then(ou => {
                this.onOrganizationUnitChanged(ou)
                this.onFormClosed()
              })
          } else {
            const createOu = new OrganizationUnitCreate()
            createOu.displayName = this.organizationUnit.displayName
            createOu.parentId = this.organizationUnitId
            OrganizationUnitService
              .createOrganizationUnit(createOu)
              .then(ou => {
                this.onOrganizationUnitChanged(ou)
                this.onFormClosed()
              })
          }
        }
      })
  }

  private onFormClosed() {
    const formOrganizationUnit = this.$refs.formOrganizationUnit as Form
    formOrganizationUnit.resetFields()
    this.$emit('closed')
  }
}
</script>

<style scoped>
.confirm {
  position: absolute;
  right: 10px;
  width:100px;
}
.cancel {
  position: absolute;
  right: 120px;
  width:100px;
}
</style>
