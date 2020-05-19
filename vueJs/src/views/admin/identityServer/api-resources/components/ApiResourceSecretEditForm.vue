<template>
  <div class="app-container">
    <div class="filter-container">
      <el-form
        ref="formApiSecret"
        label-width="100px"
        :model="apiSecret"
        :rules="apiSecretRules"
      >
        <el-form-item
          prop="type"
          :label="$t('identityServer.secretType')"
        >
          <el-select
            v-model="apiSecret.type"
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
            v-model="apiSecret.hashType"
            v-popover:popHashType
            :disabled="apiSecret.type !== 'SharedSecret'"
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
            v-model="apiSecret.value"
            :placeholder="$t('pleaseInputBy', {key: $t('identityServer.secretValue')})"
          />
        </el-form-item>
        <el-form-item
          prop="description"
          :label="$t('identityServer.secretDescription')"
        >
          <el-input
            v-model="apiSecret.description"
          />
        </el-form-item>
        <el-form-item
          prop="expiration"
          :label="$t('identityServer.expiration')"
        >
          <el-date-picker
            v-model="apiSecret.expiration"
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
            :disabled="!checkPermission(['IdentityServer.ApiResources.Secrets.Create'])"
            @click="onSaveApiSecret"
          >
            {{ $t('identityServer.createApiSecret') }}
          </el-button>
        </el-form-item>
      </el-form>
    </div>

    <el-divider />

    <el-table
      row-key="value"
      :data="apiSecrets"
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
            :disabled="!checkPermission(['IdentityServer.ApiResources.Secrets.Delete'])"
            size="mini"
            type="primary"
            @click="handleDeleteApiSecret(row.type, row.value)"
          >
            {{ $t('identityServer.deleteApiSecret') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script lang="ts">
import ApiResourceService, { ApiSecret, ApiSecretCreate } from '@/api/apiresources'
import { Component, Vue, Prop, Watch } from 'vue-property-decorator'
import { dateFormat } from '@/utils/index'
import { checkPermission } from '@/utils/permission'

@Component({
  name: 'ApiSecretEditForm',
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
  private apiResourceId!: string

  @Prop({ default: () => new Array<ApiSecret>() })
  private apiSecrets!: ApiSecret[]

  private apiSecretChanged: boolean
  private apiSecret: ApiSecretCreate
  private apiSecretRules = {
    type: [
      { required: true, message: this.l('pleaseSelectBy', { key: this.l('identityServer.secretType') }), trigger: 'change' }
    ],
    value: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.secretValue') }), trigger: 'blur' }
    ]
  }

  constructor() {
    super()
    this.apiSecretChanged = false
    this.apiSecret = ApiSecretCreate.empty()
  }

  @Watch('apiResourceId', { immediate: true })
  private onApiResourceIdChanged() {
    this.apiSecret.apiResourceId = this.apiResourceId
  }

  private handleDeleteApiSecret(type: string, value: string) {
    this.$confirm(this.l('identityServer.deleteApiSecretByType', { type: value }),
      this.l('identityServer.deleteApiSecret'), {
        callback: (action) => {
          if (action === 'confirm') {
            ApiResourceService.deleteApiSecret(this.apiResourceId, type, value).then(() => {
              const deleteSecretIndex = this.apiSecrets.findIndex(secret => secret.type === type)
              this.apiSecrets.splice(deleteSecretIndex, 1)
              this.$message.success(this.l('identityServer.deleteApiSecretSuccess', { type: value }))
              this.$emit('apiSecretChanged')
            })
          }
        }
      })
  }

  private onSaveApiSecret() {
    const frmApiSecret = this.$refs.formApiSecret as any
    frmApiSecret.validate((valid: boolean) => {
      if (valid) {
        this.apiSecret.apiResourceId = this.apiResourceId
        ApiResourceService.addApiSecret(this.apiSecret).then(secret => {
          this.apiSecrets.push(secret)
          const successMessage = this.l('identityServer.createApiSecretSuccess', { type: this.apiSecret.type })
          this.$message.success(successMessage)
          frmApiSecret.resetFields()
          this.$emit('apiSecretChanged')
        })
      }
    })
  }

  public resetFields() {
    const frmApiSecret = this.$refs.formApiSecret as any
    frmApiSecret.resetFields()
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
