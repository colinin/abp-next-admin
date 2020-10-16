<template>
  <el-form
    ref="apiResourceSecretEditForm"
    :model="apiResourceSecret"
    label-width="80px"
    :rules="apiResourceSecretRules"
  >
    <el-row>
      <el-col :span="12">
        <el-form-item
          prop="type"
          :label="$t('AbpIdentityServer.Secret:Type')"
        >
          <el-select
            v-model="apiResourceSecret.type"
            class="full-select"
            :placeholder="$t('pleaseSelectBy', {key: $t('AbpIdentityServer.Secret:Type')})"
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
      </el-col>
      <el-col :span="12">
        <el-form-item
          prop="hashType"
          :label="$t('AbpIdentityServer.Secret:HashType')"
        >
          <el-popover
            ref="popHashType"
            placement="top-start"
            trigger="hover"
            :content="$t('identityServer.hashOnlySharedSecret')"
          />
          <el-select
            v-model="apiResourceSecret.hashType"
            v-popover:popHashType
            :disabled="apiResourceSecret.type !== 'SharedSecret'"
            class="full-select"
            :placeholder="$t('pleaseSelectBy', {key: $t('AbpIdentityServer.Secret:HashType')})"
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
      </el-col>
    </el-row>
    <el-form-item
      prop="value"
      :label="$t('AbpIdentityServer.Secret:Value')"
    >
      <el-input
        v-model="apiResourceSecret.value"
        :placeholder="$t('pleaseInputBy', {key: $t('AbpIdentityServer.Secret:Value')})"
      />
    </el-form-item>
    <el-form-item
      prop="description"
      :label="$t('AbpIdentityServer.Description')"
    >
      <el-input
        v-model="apiResourceSecret.description"
      />
    </el-form-item>
    <el-form-item
      prop="expiration"
      :label="$t('AbpIdentityServer.Expiration')"
    >
      <el-date-picker
        v-model="apiResourceSecret.expiration"
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
        @click="onSave"
      >
        {{ $t('AbpIdentityServer.Secret:New') }}
      </el-button>
    </el-form-item>
    <el-table
      row-key="value"
      :data="apiResourceSecrets"
      border
      fit
      highlight-current-row
      style="width: 100%;"
    >
      <el-table-column
        :label="$t('AbpIdentityServer.Secret:Type')"
        prop="type"
        sortable
        width="170px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.type }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Secret:Value')"
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
        :label="$t('AbpIdentityServer.Description')"
        prop="description"
        width="120px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.description }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Expiration')"
        prop="expiration"
        width="170px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.expiration | dateTimeFilter }}</span>
        </template>
      </el-table-column>
      <el-table-column
        align="center"
        width="80px"
        fixed="right"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!checkPermission(['IdentityServer.ApiResources.Secrets.Delete'])"
            type="danger"
            icon="el-icon-delete"
            size="mini"
            @click="handleDeleteApiSecret(row.type, row.value)"
          />
        </template>
      </el-table-column>
    </el-table>
  </el-form>
</template>

<script lang="ts">
import { ApiSecretCreateOrUpdate, ApiSecret } from '@/api/api-resources'
import { Component, Vue, Prop } from 'vue-property-decorator'
import { dateFormat } from '@/utils/index'
import { checkPermission } from '@/utils/permission'
import { Form } from 'element-ui'

@Component({
  name: 'ApiResourceSecretEditForm',
  filters: {
    dateTimeFilter(datetime: string) {
      if (datetime) {
        const date = new Date(datetime)
        return dateFormat(date, 'YYYY-mm-dd HH:MM:SS')
      }
      return ''
    }
  },
  methods: {
    checkPermission
  }
})
export default class ApiResourceSecretEditForm extends Vue {
  @Prop({ default: () => { return new Array<ApiSecret>() } })
  private apiResourceSecrets!: ApiSecret[]

  private apiResourceSecret = new ApiSecretCreateOrUpdate()
  private apiResourceSecretRules = {
    type: [
      { required: true, message: this.l('pleaseSelectBy', { key: this.l('AbpIdentityServer.Secret:Type') }), trigger: 'blur' }
    ],
    value: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('AbpIdentityServer.Secret:Value') }), trigger: 'blur' }
    ]
  }

  private onSave() {
    const apiResourceSecretEditForm = this.$refs.apiResourceSecretEditForm as Form
    apiResourceSecretEditForm.validate(valid => {
      if (valid) {
        this.$emit('apiResourceSecretCreated',
          this.apiResourceSecret.hashType, this.apiResourceSecret.type, this.apiResourceSecret.value,
          this.apiResourceSecret.description, this.apiResourceSecret.expiration)
        apiResourceSecretEditForm.resetFields()
      }
    })
  }

  private handleDeleteApiSecret(type: string, value: string) {
    this.$emit('apiResourceSecretDeleted', type, value)
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
