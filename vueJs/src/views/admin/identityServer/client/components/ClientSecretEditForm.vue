<template>
  <div class="app-container">
    <div class="filter-container">
      <el-form
        ref="formClientSecret"
        label-width="100px"
        :model="clientSecret"
        :rules="clientSecretRules"
      >
        <el-form-item
          prop="type"
          :label="$t('identityServer.secretType')"
        >
          <el-select
            v-model="clientSecret.type"
            class="full-select"
            :placeholder="$t('pleaseSelectBy', {key: $t('identityServer.secretType')})"
          >
            <el-option
              key="JWK"
              label="JsonWebKey"
              value="JWK"
            />
            <el-option
              key="SharedSecret"
              label="SharedSecret"
              value="SharedSecret"
            />
            <el-option
              key="X509Name"
              label="X509CertificateName"
              value="X509Name"
            />
            <el-option
              key="X509CertificateBase64"
              label="X509CertificateBase64"
              value="X509CertificateBase64"
            />
            <el-option
              key="X509Thumbprint"
              label="X509CertificateThumbprint"
              value="X509Thumbprint"
            />
          </el-select>
        </el-form-item>
        <el-form-item
          prop="value"
          :label="$t('identityServer.secretValue')"
        >
          <el-input
            v-model="clientSecret.value"
            :placeholder="$t('pleaseInputBy', {key: $t('identityServer.secretValue')})"
          />
        </el-form-item>
        <el-form-item
          prop="description"
          :label="$t('identityServer.secretDescription')"
        >
          <el-input
            v-model="clientSecret.description"
          />
        </el-form-item>
        <el-form-item
          prop="expiration"
          :label="$t('identityServer.expiration')"
        >
          <el-date-picker
            v-model="clientSecret.expiration"
            class="full-select"
            type="datetime"
          />
        </el-form-item>

        <el-form-item
          style="text-align: center;"
          label-width="0px"
        >
          <el-button
            type="primary"
            style="width:180px"
            @click="onSaveClientSecret"
          >
            {{ $t('identityServer.createSecret') }}
          </el-button>
        </el-form-item>
      </el-form>
    </div>

    <el-divider />

    <el-table
      row-key="clientId"
      :data="clientSecrets"
      border
      fit
      highlight-current-row
      style="width: 100%;"
    >
      <el-table-column
        :label="$t('identityServer.secretType')"
        prop="type"
        sortable
        width="150px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.type }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('identityServer.secretValue')"
        prop="value"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.value }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('identityServer.secretDescription')"
        prop="description"
        width="170px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.description }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('identityServer.expiration')"
        prop="expiration"
        width="170px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.expiration | dateTimeFilter }}</span>
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
            :disabled="!checkPermission(['IdentityServer.Clients.Update'])"
            size="mini"
            type="primary"
            @click="handleDeleteClientSecret(row.type, row.value)"
          >
            {{ $t('identityServer.deleteSecret') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script lang="ts">
import ClientService, { ClientSecret, ClientSecretCreate } from '@/api/clients'
import { Component, Vue, Prop, Watch } from 'vue-property-decorator'
import { dateFormat } from '@/utils/index'
import { checkPermission } from '@/utils/permission'

@Component({
  name: 'ClientSecretEditForm',
  filters: {
    dateTimeFilter(datetime: string) {
      if (datetime) {
        const date = new Date(datetime)
        return dateFormat(date, 'YYYY-mm-dd HH:MM')
      }
      return ''
    }
  },
  methods: {
    checkPermission
  }
})
export default class extends Vue {
  @Prop({ default: '' })
  private clientId!: string

  @Prop({ default: () => new Array<ClientSecret>() })
  private clientSecrets!: ClientSecret[]

  private clientSecretChanged: boolean
  private clientSecret: ClientSecretCreate
  private clientSecretRules = {
    type: [
      { required: true, message: this.l('pleaseSelectBy', { key: this.l('identityServer.secretType') }), trigger: 'change' }
    ],
    value: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.secretValue') }), trigger: 'blur' }
    ]
  }

  constructor() {
    super()
    this.clientSecretChanged = false
    this.clientSecret = new ClientSecretCreate()
  }

  @Watch('clientId', { immediate: true })
  private onClientIdChanged() {
    this.clientSecret.clientId = this.clientId
  }

  private handleDeleteClientSecret(type: string, value: string) {
    this.$confirm(this.l('identityServer.deleteSecretByType', { type: value }),
      this.l('identityServer.deleteSecret'), {
        callback: (action) => {
          if (action === 'confirm') {
            ClientService.deleteClientSecret(this.clientId, type, value).then(() => {
              const deleteSecretIndex = this.clientSecrets.findIndex(secret => secret.type === type)
              this.clientSecrets.splice(deleteSecretIndex, 1)
              this.$message.success(this.l('identityServer.deleteSecretSuccess', { type: value }))
              this.$emit('clientSecretChanged')
            })
          }
        }
      })
  }

  private onSaveClientSecret() {
    const frmClientSecret = this.$refs.formClientSecret as any
    frmClientSecret.validate((valid: boolean) => {
      if (valid) {
        this.clientSecret.clientId = this.clientId
        ClientService.addClientSecret(this.clientSecret).then(secret => {
          this.clientSecrets.push(secret)
          const successMessage = this.l('identityServer.createSecretSuccess', { type: this.clientSecret.type })
          this.$message.success(successMessage)
          frmClientSecret.resetFields()
          this.$emit('clientSecretChanged')
        })
      }
    })
  }

  public resetFields() {
    const frmClientSecret = this.$refs.formClientSecret as any
    frmClientSecret.resetFields()
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
