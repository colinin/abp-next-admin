<template>
  <el-dialog
    :visible="showDialog"
    :title="$t('tenant.updateTenant')"
    width="800px"
    custom-class="modal-form"
    :show-close="false"
    @close="onFormClosed"
  >
    <div
      class="app-container"
    >
      <el-form
        ref="formTenant"
        label-width="120px"
        :model="tenant"
        :rules="tenantRules"
      >
        <el-form-item
          prop="name"
          :label="$t('tenant.name')"
        >
          <el-input
            v-model="tenant.name"
            :placeholder="$t('pleaseInputBy', {key: $t('tenant.name')})"
          />
        </el-form-item>
        <el-form-item
          v-if="!isEditTenant"
          prop="adminEmailAddress"
          :label="$t('tenant.adminEmailAddress')"
        >
          <el-input
            v-model="tenant.adminEmailAddress"
            :placeholder="$t('pleaseInputBy', {key: $t('tenant.adminEmailAddress')})"
          />
        </el-form-item>
        <el-form-item
          v-if="!isEditTenant"
          prop="adminPassword"
          :label="$t('tenant.adminPassword')"
        >
          <el-input
            v-model="tenant.adminPassword"
            :placeholder="$t('pleaseInputBy', {key: $t('tenant.adminPassword')})"
          />
        </el-form-item>

        <el-form-item>
          <el-button
            class="cancel"
            style="width:100px;right: 120px;position: absolute;"
            @click="onCancel"
          >
            {{ $t('global.cancel') }}
          </el-button>
          <el-button
            class="confirm"
            type="primary"
            style="width:100px;right: 10px;position: absolute;"
            @click="onSaveTenant"
          >
            {{ $t('global.confirm') }}
          </el-button>
        </el-form-item>
      </el-form>
    </div>
  </el-dialog>
</template>

<script lang="ts">
import TenantService, { TenantCreateOrEdit } from '@/api/tenant-management'
import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'

@Component({
  name: 'TenantCreateOrEditForm'
})
export default class extends Mixins(LocalizationMiXin) {
  @Prop({ default: '' })
  private tenantId!: string

  @Prop({ default: false })
  private showDialog!: boolean

  private tenant!: TenantCreateOrEdit

  get isEditTenant() {
    if (this.tenantId) {
      return true
    }
    return false
  }

  private tenantRules = {
    name: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('tenant.name') }), trigger: 'blur' }
    ],
    adminEmailAddress: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('tenant.adminEmailAddress') }), trigger: 'blur' },
      { type: 'email', message: this.l('pleaseInputBy', { key: this.l('global.correctEmailAddress') }), trigger: 'blur' }
    ],
    adminPassword: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('tenant.adminPassword') }), trigger: 'blur' }
    ]
  }

  constructor() {
    super()
    this.tenant = TenantCreateOrEdit.empty()
  }

  @Watch('tenantId')
  private onTenantIdChanged() {
    this.handleGetEditTenant()
  }

  mounted() {
    this.handleGetEditTenant()
  }

  private handleGetEditTenant() {
    if (this.showDialog && this.tenantId) {
      TenantService.getTenantById(this.tenantId).then(tenant => {
        this.tenant.name = tenant.name
      })
    } else {
      this.tenant = TenantCreateOrEdit.empty()
    }
  }

  private onSaveTenant() {
    const frmTenant = this.$refs.formTenant as any
    frmTenant.validate((valid: boolean) => {
      if (valid) {
        if (this.isEditTenant) {
          TenantService.changeTenantName(this.tenantId, this.tenant.name).then(tenant => {
            const message = this.l('tenant.tenantNameChanged', { name: tenant.name })
            this.$message.success(message)
            this.reset()
            this.$emit('closed', true)
          })
        } else {
          TenantService.createTenant(this.tenant).then(tenant => {
            const message = this.l('tenant.createTenantSuccess', { name: tenant.name })
            this.$message.success(message)
            this.reset()
            this.$emit('closed', true)
          })
        }
      }
    })
  }

  private onFormClosed() {
    this.$emit('closed', false)
  }

  private onCancel() {
    this.reset()
    this.$emit('closed', false)
  }

  private reset() {
    this.tenant = TenantCreateOrEdit.empty()
    const frmTenant = this.$refs.formTenant as any
    frmTenant.resetFields()
  }
}
</script>
