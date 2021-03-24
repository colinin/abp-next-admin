<template>
  <el-dialog
    v-el-draggable-dialog
    :visible="showDialog"
    :title="$t('tenant.connectionOptions')"
    width="800px"
    custom-class="modal-form"
    close-on-click-modal
    @close="onFormClosed"
  >
    <div
      class="app-container"
    >
      <el-form
        v-if="checkPermission(['AbpTenantManagement.Tenants.ManageConnectionStrings'])"
        ref="formTenantConnection"
        label-width="100px"
        :model="tenantConnection"
        :rules="tenantConnectionRules"
      >
        <el-form-item
          prop="name"
          :label="$t('tenant.connectionName')"
        >
          <el-input
            v-model="tenantConnection.name"
            :placeholder="$t('pleaseInputBy', {key: $t('tenant.connectionName')})"
          />
        </el-form-item>
        <el-form-item
          prop="value"
          :label="$t('tenant.connectionString')"
        >
          <el-input
            v-model="tenantConnection.value"
            :placeholder="$t('pleaseInputBy', {key: $t('tenant.connectionString')})"
          />
        </el-form-item>

        <el-form-item
          style="text-align: center;"
          label-width="0px"
        >
          <el-button
            type="primary"
            style="width:180px"
            @click="onSaveTenantConnection"
          >
            {{ $t('tenant.setTenantConnection') }}
          </el-button>
        </el-form-item>
        <el-divider />
      </el-form>
    </div>
    <el-table
      row-key="key"
      :data="tenantConnections"
      border
      fit
      highlight-current-row
      style="width: 100%;"
    >
      <el-table-column
        :label="$t('tenant.connectionName')"
        prop="name"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.name }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('tenant.connectionString')"
        prop="value"
        sortable
        min-width="400px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.value }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('operaActions')"
        align="center"
        width="150px"
        fixed="right"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!checkPermission(['AbpTenantManagement.Tenants.ManageConnectionStrings'])"
            size="mini"
            type="primary"
            @click="handleDeleteTenantConnection(row.name)"
          >
            {{ $t('tenant.deleteConnection') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>
  </el-dialog>
</template>

<script lang="ts">
import TenantService, { TenantConnectionString } from '@/api/tenant-management'
import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'
import { checkPermission } from '@/utils/permission'

@Component({
  name: 'TenantConnectionEditForm',
  methods: {
    checkPermission
  }
})
export default class extends Mixins(LocalizationMiXin) {
  @Prop({ default: '' })
  private tenantId!: string

  @Prop({ default: false })
  private showDialog!: boolean

  private tenantConnection: TenantConnectionString
  private tenantConnections: TenantConnectionString[]
  private tenantConnectionRules = {
    name: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('tenant.connectionName') }), trigger: 'blur' }
    ],
    value: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('tenant.connectionString') }), trigger: 'blur' }
    ]
  }

  constructor() {
    super()
    this.tenantConnection = TenantConnectionString.empty()
    this.tenantConnections = new Array<TenantConnectionString>()
  }

  @Watch('tenantId')
  private onTenantIdChanged() {
    this.handleGetTenantConnections()
  }

  mounted() {
    this.handleGetTenantConnections()
  }

  private handleGetTenantConnections() {
    if (this.showDialog && this.tenantId) {
      TenantService.getTenantConnections(this.tenantId).then(connections => {
        this.tenantConnections = connections.items
      })
    } else {
      this.tenantConnection = TenantConnectionString.empty()
      this.tenantConnections = new Array<TenantConnectionString>()
    }
  }

  private handleDeleteTenantConnection(name: string) {
    this.$confirm(this.l('tenant.deleteTenantConnectionName', { name: name }),
      this.l('tenant.deleteConnection'), {
        callback: (action) => {
          if (action === 'confirm') {
            TenantService.deleteTenantConnectionByName(this.tenantId, name).then(() => {
              const deleteTenantConnectionIndex = this.tenantConnections.findIndex(p => p.name === name)
              this.tenantConnections.splice(deleteTenantConnectionIndex, 1)
              this.$message.success(this.l('tenant.deleteTenantConnectionSuccess', { name: name }))
              this.onFormClosed()
            })
          }
        }
      })
  }

  private onSaveTenantConnection() {
    const frmTenantConnection = this.$refs.formTenantConnection as any
    frmTenantConnection.validate((valid: boolean) => {
      if (valid) {
        TenantService.setTenantConnection(this.tenantId, this.tenantConnection).then(connection => {
          const tenantConnection = this.tenantConnections.find(tc => tc.name === connection.name)
          if (tenantConnection) {
            tenantConnection.value = connection.value
          } else {
            this.tenantConnections.push(connection)
          }
          this.$message.success(this.l('tenant.setTenantConnectionSuccess', { name: name }))
          this.resetFields()
        })
      }
    })
  }

  private onFormClosed() {
    this.resetFields()
    this.$emit('closed')
  }

  public resetFields() {
    this.tenantConnection = TenantConnectionString.empty()
  }
}
</script>

<style lang="scss" scoped>
.full-select {
  width: 100%;
}
</style>
