<template>
  <div>
    <el-form
      v-if="allowedCreateSecret"
      ref="secretEditForm"
      :model="secret"
      label-width="80px"
    >
      <el-form-item
        prop="type"
        :label="$t('AbpIdentityServer.Secret:Type')"
        :rules="{
          required: true,
          message: $t('pleaseSelectBy', {key: $t('AbpIdentityServer.Secret:Type')}),
          trigger: 'blur'
        }"
      >
        <el-select
          v-model="secret.type"
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
      <el-form-item
        prop="hashType"
        :label="$t('AbpIdentityServer.Secret:HashType')"
      >
        <el-popover
          ref="popHashType"
          placement="top-start"
          trigger="hover"
          :content="$t('AbpIdentityServer.Secret:HashTypeOnlySharedSecret')"
        />
        <el-select
          v-model="secret.hashType"
          v-popover:popHashType
          :disabled="secret.type !== 'SharedSecret'"
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
      <el-form-item
        prop="value"
        :label="$t('AbpIdentityServer.Secret:Value')"
        :rules="{
          required: true,
          message: $t('pleaseInputBy', {key: $t('AbpIdentityServer.Secret:Value')}),
          trigger: 'blur'
        }"
      >
        <el-input
          v-model="secret.value"
          :placeholder="$t('pleaseInputBy', {key: $t('AbpIdentityServer.Secret:Value')})"
        />
      </el-form-item>
      <el-form-item
        prop="description"
        :label="$t('AbpIdentityServer.Description')"
      >
        <el-input
          v-model="secret.description"
        />
      </el-form-item>
      <el-form-item
        prop="expiration"
        :label="$t('AbpIdentityServer.Expiration')"
      >
        <el-date-picker
          v-model="secret.expiration"
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
    </el-form>
    <el-table
      row-key="value"
      :data="secrets"
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
        :label="$t('AbpIdentityServer.Actions')"
        align="center"
        width="80px"
        fixed="right"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!allowedDeleteSecret"
            type="danger"
            icon="el-icon-delete"
            size="mini"
            @click="handleDeleteApiSecret(row.type, row.value)"
          />
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script lang="ts">
import { ISecret, HashType } from '@/api/types'
import { Component, Vue, Prop } from 'vue-property-decorator'
import { dateFormat } from '@/utils/index'
import { Form } from 'element-ui'

class Secret implements ISecret {
  type = ''
  value = ''
  hashType = HashType.Sha256
  description = ''
  expiration: Date | undefined = undefined
}

@Component({
  name: 'SecretEditForm',
  filters: {
    dateTimeFilter(datetime: string) {
      if (datetime) {
        const date = new Date(datetime)
        return dateFormat(date, 'YYYY-mm-dd HH:MM:SS')
      }
      return ''
    }
  }
})
export default class SecretEditForm extends Vue {
  @Prop({ default: () => { return new Array<ISecret>() } })
  private secrets!: ISecret[]

  @Prop({ default: false })
  private allowedCreateSecret!: boolean

  @Prop({ default: false })
  private allowedDeleteSecret!: boolean

  private secret = new Secret()

  private onSave() {
    const secretEditForm = this.$refs.secretEditForm as Form
    secretEditForm.validate(valid => {
      if (valid) {
        this.$emit('onSecretCreated', this.secret)
        secretEditForm.resetFields()
      }
    })
  }

  private handleDeleteApiSecret(type: string, value: string) {
    this.$emit('onSecretDeleted', type, value)
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
