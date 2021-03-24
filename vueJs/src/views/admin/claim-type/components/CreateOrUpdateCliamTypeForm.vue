<template>
  <el-dialog
    v-el-draggable-dialog
    width="800px"
    :visible="showDialog"
    :title="title"
    custom-class="modal-form"
    :show-close="false"
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    @close="onFormClosed(false)"
  >
    <el-form
      ref="claimTypeForm"
      :model="claimType"
      label-width="120px"
      :rules="claimTypeRules"
    >
      <el-form-item
        prop="name"
        :label="$t('AbpIdentity.IdentityClaim:Name')"
      >
        <el-input
          v-model="claimType.name"
          :disabled="isEditClaimType"
        />
      </el-form-item>
      <el-form-item
        prop="description"
        :label="$t('AbpIdentity.IdentityClaim:Description')"
      >
        <el-input
          v-model="claimType.description"
        />
      </el-form-item>
      <el-form-item
        prop="regex"
        :label="$t('AbpIdentity.IdentityClaim:Regex')"
      >
        <el-input
          v-model="claimType.regex"
        />
      </el-form-item>
      <el-form-item
        prop="regexDescription"
        :label="$t('AbpIdentity.IdentityClaim:RegexDescription')"
      >
        <el-input
          v-model="claimType.regexDescription"
        />
      </el-form-item>
      <el-form-item
        prop="valueType"
        :label="$t('AbpIdentity.IdentityClaim:ValueType')"
      >
        <el-select
          v-model="claimType.valueType"
          style="width: 100%"
          :disabled="isEditClaimType"
        >
          <el-option
            v-for="valueType in claimValueTypes"
            :key="valueType.name"
            :label="valueType.name"
            :value="valueType.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item
        prop="required"
        :label="$t('AbpIdentity.IdentityClaim:Required')"
      >
        <el-switch
          v-model="claimType.required"
        />
      </el-form-item>
      <el-form-item
        prop="isStatic"
        :label="$t('AbpIdentity.IdentityClaim:IsStatic')"
      >
        <el-switch
          v-model="claimType.isStatic"
          :disabled="isEditClaimType"
        />
      </el-form-item>
      <el-form-item>
        <el-button
          class="cancel"
          style="width:100px"
          @click="onFormClosed(false)"
        >
          {{ $t('global.cancel') }}
        </el-button>
        <el-button
          class="confirm"
          type="primary"
          style="width:100px"
          @click="onSave"
        >
          {{ $t('global.confirm') }}
        </el-button>
      </el-form-item>
    </el-form>
  </el-dialog>
</template>

<script lang="ts">
import { Form } from 'element-ui'
import ClaimTypeApiService, {
  IdentityClaimType,
  IdentityClaimValueType,
  IdentityClaimTypeCreate,
  IdentityClaimTypeUpdate,
  IdentityClaimTypeCreateOrUpdateBase
} from '@/api/cliam-type'
import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'

@Component({
  name: 'CreateOrUpdateCliamTypeForm'
})
export default class CreateOrUpdateCliamTypeForm extends Mixins(LocalizationMiXin) {
  @Prop({ default: '' })
  private claimTypeId!: string

  @Prop({ default: '' })
  private title!: string

  @Prop({ default: false })
  private showDialog!: boolean

  private hasChanged = false
  private claimType = new IdentityClaimType()
  private claimValueTypes = [
    { name: 'Boolean', value: IdentityClaimValueType.Boolean },
    { name: 'DateTime', value: IdentityClaimValueType.DateTime },
    { name: 'Int', value: IdentityClaimValueType.Int },
    { name: 'String', value: IdentityClaimValueType.String }
  ]

  private claimTypeRules = {
    name: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('AbpIdentity.IdentityClaim:Name') }), trigger: 'blur' }
    ],
    valueType: [
      { required: true, message: this.l('pleaseSelectBy', { key: this.l('AbpIdentity.IdentityClaim:ValueType') }), trigger: 'blur' }
    ]
  }

  get isEditClaimType() {
    if (this.claimTypeId) {
      return true
    }
    return false
  }

  @Watch('showDialog', { immediate: true })
  private onShowDialogChanged() {
    this.handleGetClaimType()
  }

  private handleGetClaimType() {
    this.hasChanged = false
    if (this.showDialog && this.claimTypeId) {
      ClaimTypeApiService.getClaimType(this.claimTypeId).then(res => {
        this.claimType = res
      })
    }
  }

  private onSave() {
    const claimTypeForm = this.$refs.claimTypeForm as Form
    claimTypeForm.validate(valid => {
      if (valid) {
        if (this.isEditClaimType) {
          const updatePayload = new IdentityClaimTypeUpdate()
          this.updateClaimTypeByInput(updatePayload)
          ClaimTypeApiService.updateClaimType(this.claimTypeId, updatePayload).then(res => {
            this.claimType = res
            this.hasChanged = true
            this.$message.success(this.l('global.successful'))
          })
        } else {
          const createPayload = new IdentityClaimTypeCreate(this.claimType.name, this.claimType.isStatic, this.claimType.valueType)
          this.updateClaimTypeByInput(createPayload)
          ClaimTypeApiService.createClaimType(createPayload).then(() => {
            claimTypeForm.resetFields()
            this.hasChanged = true
            this.$message.success(this.l('global.successful'))
            this.onFormClosed(true)
          })
        }
      }
    })
  }

  private onFormClosed(changed: boolean) {
    const claimTypeForm = this.$refs.claimTypeForm as Form
    claimTypeForm.resetFields()
    this.hasChanged = this.hasChanged ? this.hasChanged : changed
    this.$emit('closed', this.hasChanged)
  }

  private updateClaimTypeByInput(claimType: IdentityClaimTypeCreateOrUpdateBase) {
    claimType.regex = this.claimType.regex
    claimType.regexDescription = this.claimType.regexDescription
    claimType.required = this.claimType.required
    claimType.description = this.claimType.description
  }
}
</script>

<style scoped>
.confirm {
  position: absolute;
  right: 10px;
}
.cancel {
  position: absolute;
  right: 120px;
}
</style>
