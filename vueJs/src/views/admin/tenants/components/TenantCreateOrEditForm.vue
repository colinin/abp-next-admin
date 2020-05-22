<template>
  <div class="app-container">
    <div class="filter-container">
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
  </div>
</template>

<script lang="ts">
import TenantService, { TenantCreateOrEdit } from '@/api/tenant'
import { Component, Prop, Vue, Watch } from 'vue-property-decorator'

@Component({
  name: 'TenantCreateOrEditForm'
})
export default class extends Vue {
  @Prop({ default: '' })
  private tenantId!: string

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

  @Watch('tenantId', { immediate: true })
  private onTenantIdChanged() {
    if (this.tenantId) {
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

  private onCancel() {
    this.reset()
    this.$emit('closed', false)
  }

  private reset() {
    this.tenant = TenantCreateOrEdit.empty()
    const frmTenant = this.$refs.formTenant as any
    frmTenant.resetFields()
  }

  private l(name: string, values?: any[] | { [key: string]: any }) {
    return this.$t(name, values).toString()
  }
}
</script>
