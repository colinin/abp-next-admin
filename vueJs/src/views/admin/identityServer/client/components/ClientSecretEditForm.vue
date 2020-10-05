<template>
  <el-dialog
    v-el-draggable-dialog
    width="800px"
    :visible="showDialog"
    :title="$t('identityServer.clientSecret')"
    custom-class="modal-form"
    :show-close="false"
    @close="onFormClosed"
  >
    <div class="app-container">
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
          prop="hashType"
          :label="$t('identityServer.secretHashType')"
        >
          <el-popover
            ref="popHashType"
            placement="top-start"
            trigger="hover"
            :content="$t('identityServer.hashOnlySharedSecret')"
          />
          <el-select
            v-model="clientSecret.hashType"
            v-popover:popHashType
            :disabled="clientSecret.type !== 'SharedSecret'"
            class="full-select"
            :placeholder="$t('pleaseSelectBy', {key: $t('identityServer.secretHashType')})"
          >
            <el-option
              :key="0"
              label="Sha256"
              :value="0"
            />
            <el-option
              :key="1"
              label="Sha512"
              :value="1"
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
            :disabled="!checkPermission(['IdentityServer.Clients.Secrets.Create'])"
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
            :disabled="!checkPermission(['IdentityServer.Clients.Secrets.Delete'])"
            size="mini"
            type="primary"
            @click="handleDeleteClientSecret(row.type, row.value)"
          >
            {{ $t('identityServer.deleteSecret') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>
  </el-dialog>
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
  @Prop({ default: false })
  private showDialog!: boolean

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
          this.$emit('clientSecretChanged')
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
