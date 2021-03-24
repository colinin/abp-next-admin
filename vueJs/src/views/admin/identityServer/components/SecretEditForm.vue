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

      <el-form-item>
        <el-button
          type="success"
          class="add-button"
          @click="onSave"
        >
          <i class="ivu-icon ivu-icon-md-add" />
          {{ $t('AbpIdentityServer.AddNew') }}
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
            @click="onDeleted(row.type, row.value)"
          />
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script lang="ts">
import { Secret } from '@/api/identity-server4'
import { Component, Vue, Prop } from 'vue-property-decorator'
import { dateFormat } from '@/utils/index'
import { Form } from 'element-ui'

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
  },
  model: {
    prop: 'secrets',
    event: 'change'
  }
})
export default class SecretEditForm extends Vue {
  @Prop({ default: () => { return new Array<Secret>() } })
  private secrets!: Secret[]

  @Prop({ default: false })
  private allowedCreateSecret!: boolean

  @Prop({ default: false })
  private allowedDeleteSecret!: boolean

  private secret = new Secret()

  private onSave() {
    const secretEditForm = this.$refs.secretEditForm as Form
    secretEditForm.validate(valid => {
      if (valid) {
        this.$emit('change', this.secrets.concat({
          type: this.secret.type,
          value: this.secret.value,
          description: this.secret.description,
          expiration: this.secret.expiration
        }))
        secretEditForm.resetFields()
      }
    })
  }

  private onDeleted(type: string, value: string) {
    console.log(type, value)
    this.$emit('change', this.secrets.filter(secret => secret.value !== value || secret.type !== type))
  }
}
</script>

<style lang="scss" scoped>
.full-select {
  width: 100%;
}
.add-button {
  width: 150px;
  float: right;
}
</style>
