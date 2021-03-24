<template>
  <el-dialog
    v-el-draggable-dialog
    width="800px"
    :visible="showDialog"
    :title="$t('AbpIdentityServer.Client:Clone')"
    custom-class="modal-form"
    :show-close="false"
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    @close="onFormClosed(false)"
  >
    <div class="app-container">
      <el-form
        ref="formCloneClient"
        label-width="180px"
        :model="client"
      >
        <el-form-item
          prop="clientId"
          :label="$t('AbpIdentityServer.Client:Id')"
          :rules="{
            required: true,
            message: $t('pleaseInputBy', {key: $t('AbpIdentityServer.Client:Id')}),
            trigger: 'blur'
          }"
        >
          <el-input
            v-model="client.clientId"
            :placeholder="$t('pleaseInputBy', {key: $t('AbpIdentityServer.Client:Id')})"
          />
        </el-form-item>
        <el-form-item
          prop="clientName"
          :label="$t('AbpIdentityServer.Name')"
          :rules="{
            required: true,
            message: $t('pleaseInputBy', {key: $t('AbpIdentityServer.Name')}),
            trigger: 'blur'
          }"
        >
          <el-input
            v-model="client.clientName"
            :placeholder="$t('pleaseInputBy', {key: $t('AbpIdentityServer.Name')})"
          />
        </el-form-item>
        <el-form-item
          prop="description"
          :label="$t('AbpIdentityServer.Description')"
        >
          <el-input
            v-model="client.description"
          />
        </el-form-item>
        <el-form-item
          prop="copyAllowedGrantType"
          :label="$t('AbpIdentityServer.Clone:CopyAllowedGrantType')"
        >
          <el-switch
            v-model="client.copyAllowedGrantType"
          />
        </el-form-item>
        <el-form-item
          prop="copyRedirectUri"
          :label="$t('AbpIdentityServer.Clone:CopyRedirectUri')"
        >
          <el-switch
            v-model="client.copyRedirectUri"
          />
        </el-form-item>
        <el-form-item
          prop="copyAllowedScope"
          :label="$t('AbpIdentityServer.Clone:CopyAllowedScope')"
        >
          <el-switch
            v-model="client.copyAllowedScope"
          />
        </el-form-item>
        <el-form-item
          prop="copyClaim"
          :label="$t('AbpIdentityServer.Clone:CopyClaim')"
        >
          <el-switch
            v-model="client.copyClaim"
          />
        </el-form-item>
        <el-form-item
          prop="copySecret"
          :label="$t('AbpIdentityServer.Clone:CopySecret')"
        >
          <el-switch
            v-model="client.copySecret"
          />
        </el-form-item>
        <el-form-item
          prop="copyAllowedCorsOrigin"
          :label="$t('AbpIdentityServer.Clone:CopyAllowedCorsOrigin')"
        >
          <el-switch
            v-model="client.copyAllowedCorsOrigin"
          />
        </el-form-item>
        <el-form-item
          prop="copyPostLogoutRedirectUri"
          :label="$t('AbpIdentityServer.Clone:CopyPostLogoutRedirectUri')"
        >
          <el-switch
            v-model="client.copyPostLogoutRedirectUri"
          />
        </el-form-item>
        <el-form-item
          prop="copyPropertie"
          :label="$t('AbpIdentityServer.Clone:CopyProperties')"
        >
          <el-switch
            v-model="client.copyPropertie"
          />
        </el-form-item>
        <el-form-item
          prop="copyIdentityProviderRestriction"
          :label="$t('AbpIdentityServer.Clone:CopyIdentityProviderRestriction')"
        >
          <el-switch
            v-model="client.copyIdentityProviderRestriction"
          />
        </el-form-item>

        <el-form-item>
          <el-button
            class="cancel"
            style="width:100px"
            type="info"
            @click="onFormClosed(false)"
          >
            {{ $t('AbpIdentityServer.Cancel') }}
          </el-button>
          <el-button
            class="confirm"
            type="primary"
            style="width:100px"
            icon="el-icon-check"
            @click="onSave"
          >
            {{ $t('AbpIdentityServer.Save') }}
          </el-button>
        </el-form-item>
      </el-form>
    </div>
  </el-dialog>
</template>

<script lang="ts">
import ClientService, { ClientClone } from '@/api/clients'
import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'

@Component({
  name: 'ClientCloneForm'
})
export default class extends Mixins(LocalizationMiXin) {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: '' })
  private clientId!: string

  private client: ClientClone

  constructor() {
    super()
    this.client = ClientClone.empty()
  }

  @Watch('clientId', { immediate: true })
  private onClientIdChanged() {
    this.client.sourceClientId = this.clientId
  }

  private onSave() {
    const frmClient = this.$refs.formCloneClient as any
    frmClient.validate((valid: boolean) => {
      if (valid) {
        ClientService
          .clone(this.clientId, this.client)
          .then(() => {
            const successMessage = this.l('global.successful')
            this.$message.success(successMessage)
            this.onFormClosed(true)
          })
      }
    })
  }

  private onFormClosed(changed: boolean) {
    this.resetFields()
    this.$emit('closed', changed)
  }

  public resetFields() {
    const frmClient = this.$refs.formCloneClient as any
    frmClient.resetFields()
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
