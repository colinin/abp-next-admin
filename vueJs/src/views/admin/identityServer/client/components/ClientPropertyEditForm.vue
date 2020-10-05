<template>
  <el-dialog
    v-el-draggable-dialog
    width="800px"
    :visible="showDialog"
    :title="$t('identityServer.clientProperty')"
    custom-class="modal-form"
    :show-close="false"
    @close="onFormClosed"
  >
    <div class="app-container">
      <el-form
        ref="formClientProperty"
        label-width="100px"
        :model="clientProperty"
        :rules="clientPropertyRules"
      >
        <el-form-item
          prop="key"
          :label="$t('identityServer.propertyKey')"
        >
          <el-input
            v-model="clientProperty.key"
            :placeholder="$t('pleaseInputBy', {key: $t('identityServer.propertyKey')})"
          />
        </el-form-item>
        <el-form-item
          prop="value"
          :label="$t('identityServer.propertyValue')"
        >
          <el-input
            v-model="clientProperty.value"
            :placeholder="$t('pleaseInputBy', {key: $t('identityServer.propertyValue')})"
          />
        </el-form-item>

        <el-form-item
          style="text-align: center;"
          label-width="0px"
        >
          <el-button
            type="primary"
            style="width:180px"
            :disabled="!checkPermission(['IdentityServer.Clients.Properties.Create'])"
            @click="onSaveClientProperty"
          >
            {{ $t('identityServer.createClientProperty') }}
          </el-button>
        </el-form-item>
      </el-form>
    </div>

    <el-divider />

    <el-table
      row-key="key"
      :data="clientProperties"
      border
      fit
      highlight-current-row
      style="width: 100%;"
    >
      <el-table-column
        :label="$t('identityServer.propertyKey')"
        prop="key"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.key }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('identityServer.propertyValue')"
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
            :disabled="!checkPermission(['IdentityServer.Clients.Properties.Delete'])"
            size="mini"
            type="primary"
            @click="handleDeleteClientProperty(row.key, row.value)"
          >
            {{ $t('identityServer.deleteProperty') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>
  </el-dialog>
</template>

<script lang="ts">
import ClientService, { ClientProperty, ClientPropertyCreate } from '@/api/clients'
import { Component, Vue, Prop, Watch } from 'vue-property-decorator'
import { checkPermission } from '@/utils/permission'

@Component({
  name: 'ClientPropertyEditForm',
  methods: {
    checkPermission
  }
})
export default class extends Vue {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: '' })
  private clientId!: string

  @Prop({ default: () => new Array<ClientProperty>() })
  private clientProperties!: ClientProperty[]

  private clientProperty: ClientPropertyCreate
  private clientPropertyRules = {
    key: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.propertyKey') }), trigger: 'change' }
    ],
    value: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.propertyValue') }), trigger: 'blur' }
    ]
  }

  constructor() {
    super()
    this.clientProperty = new ClientPropertyCreate()
  }

  @Watch('clientId', { immediate: true })
  private onClientIdChanged() {
    this.clientProperty.clientId = this.clientId
  }

  private handleDeleteClientProperty(key: string, value: string) {
    this.$confirm(this.l('identityServer.deletePropertyByType', { key: key }),
      this.l('identityServer.deleteProperty'), {
        callback: (action) => {
          if (action === 'confirm') {
            ClientService.deleteClientProperty(this.clientId, key, value).then(() => {
              const deletePropertyIndex = this.clientProperties.findIndex(p => p.key === key && p.value === value)
              this.clientProperties.splice(deletePropertyIndex, 1)
              this.$message.success(this.l('identityServer.deletePropertySuccess', { type: value }))
              this.$emit('clientPropertyChanged')
            })
          }
        }
      })
  }

  private onSaveClientProperty() {
    const frmClientProperty = this.$refs.formClientProperty as any
    frmClientProperty.validate((valid: boolean) => {
      if (valid) {
        this.clientProperty.clientId = this.clientId
        ClientService.addClientProperty(this.clientProperty).then(property => {
          this.clientProperties.push(property)
          const successMessage = this.l('identityServer.createPropertySuccess', { type: this.clientProperty.key })
          this.$message.success(successMessage)
          this.$emit('clientPropertyChanged')
          this.onFormClosed()
        })
      }
    })
  }

  private onFormClosed() {
    this.resetFields()
    this.$emit('closed')
  }

  public resetFields() {
    const frmClientProperty = this.$refs.formClientProperty as any
    frmClientProperty.resetFields()
  }

  private l(name: string, values?: any[] | { [key: string]: any }) {
    return this.$t(name, values).toString()
  }
}
</script>

<style lang="scss" scoped>
.full-select {
  width: 100%;
}
</style>
