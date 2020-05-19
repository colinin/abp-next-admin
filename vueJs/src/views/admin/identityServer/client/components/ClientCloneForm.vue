<template>
  <div class="app-container">
    <div class="filter-container">
      <el-form
        ref="formCloneClient"
        label-width="175px"
        :model="client"
        :rules="clientRules"
      >
        <el-form-item
          prop="clientId"
          :label="$t('identityServer.clientId')"
        >
          <el-input
            v-model="client.clientId"
            :placeholder="$t('pleaseInputBy', {key: $t('identityServer.clientId')})"
          />
        </el-form-item>
        <el-form-item
          prop="clientName"
          :label="$t('identityServer.clientName')"
        >
          <el-input
            v-model="client.clientName"
            :placeholder="$t('pleaseInputBy', {key: $t('identityServer.clientName')})"
          />
        </el-form-item>
        <el-form-item
          prop="description"
          :label="$t('identityServer.description')"
        >
          <el-input
            v-model="client.description"
          />
        </el-form-item>
        <el-form-item
          prop="copyAllowedGrantType"
          :label="$t('identityServer.copyAllowedGrantType')"
        >
          <el-switch
            v-model="client.copyAllowedGrantType"
          />
        </el-form-item>
        <el-form-item
          prop="copyRedirectUri"
          :label="$t('identityServer.copyRedirectUri')"
        >
          <el-switch
            v-model="client.copyRedirectUri"
          />
        </el-form-item>
        <el-form-item
          prop="copyAllowedScope"
          :label="$t('identityServer.copyAllowedScope')"
        >
          <el-switch
            v-model="client.copyAllowedScope"
          />
        </el-form-item>
        <el-form-item
          prop="copyClaim"
          :label="$t('identityServer.copyClaim')"
        >
          <el-switch
            v-model="client.copyClaim"
          />
        </el-form-item>
        <el-form-item
          prop="copyAllowedCorsOrigin"
          :label="$t('identityServer.copyAllowedCorsOrigin')"
        >
          <el-switch
            v-model="client.copyAllowedCorsOrigin"
          />
        </el-form-item>
        <el-form-item
          prop="copyPostLogoutRedirectUri"
          :label="$t('identityServer.copyPostLogoutRedirectUri')"
        >
          <el-switch
            v-model="client.copyPostLogoutRedirectUri"
          />
        </el-form-item>
        <el-form-item
          prop="copyPropertie"
          :label="$t('identityServer.copyPropertie')"
        >
          <el-switch
            v-model="client.copyPropertie"
          />
        </el-form-item>
        <el-form-item
          prop="copyIdentityProviderRestriction"
          :label="$t('identityServer.copyIdentityProviderRestriction')"
        >
          <el-switch
            v-model="client.copyIdentityProviderRestriction"
          />
        </el-form-item>

        <el-form-item>
          <el-button
            class="cancel"
            style="width:100px"
            @click="onCancel"
          >
            {{ $t('table.cancel') }}
          </el-button>
          <el-button
            class="confirm"
            type="primary"
            style="width:100px"
            @click="onCloneClient"
          >
            {{ $t('table.confirm') }}
          </el-button>
        </el-form-item>
      </el-form>
    </div>
  </div>
</template>

<script lang="ts">
import ClientService, { ClientClone } from '@/api/clients'
import { Component, Vue, Prop, Watch } from 'vue-property-decorator'

@Component({
  name: 'ClientCloneForm'
})
export default class extends Vue {
  @Prop({ default: '' })
  private clientId!: string

  private client: ClientClone
  private clientRules = {
    clientId: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.clientId') }), trigger: 'blur' }
    ],
    clientName: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.clientName') }), trigger: 'blur' }
    ]
  }

  constructor() {
    super()
    this.client = ClientClone.empty()
  }

  @Watch('clientId', { immediate: true })
  private onClientIdChanged() {
    this.client.sourceClientId = this.clientId
  }

  private onCloneClient() {
    const frmClient = this.$refs.formCloneClient as any
    frmClient.validate((valid: boolean) => {
      if (valid) {
        ClientService.cloneClient(this.client).then(client => {
          const successMessage = this.l('identityServer.createClientSuccess', { id: client.clientId })
          this.$message.success(successMessage)
          frmClient.resetFields()
          this.$emit('closed', true)
        })
      }
    })
  }

  private onCancel() {
    this.resetFields()
    this.$emit('closed')
  }

  public resetFields() {
    const frmClient = this.$refs.formCloneClient as any
    frmClient.resetFields()
  }

  private l(name: string, values?: any[] | { [key: string]: any }) {
    return this.$t(name, values).toString()
  }
}
</script>

<style lang="scss" scoped>
.confirm {
  position: absolute;
  right: 10px;
}
.cancel {
  position: absolute;
  right: 120px;
}
</style>
