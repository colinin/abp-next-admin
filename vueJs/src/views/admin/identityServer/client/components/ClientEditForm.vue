<template>
  <el-form
    ref="formClient"
    label-width="100px"
    :model="client"
    :rules="clientRules"
  >
    <el-tabs>
      <el-tab-pane :label="$t('identityServer.basicOptions')">
        <el-form-item
          prop="enabled"
          :label="$t('identityServer.enabled')"
        >
          <el-switch
            v-model="client.enabled"
          />
        </el-form-item>
        <el-row>
          <el-col :span="12">
            <el-form-item
              prop="clientId"
              :label="$t('identityServer.clientId')"
            >
              <el-input
                v-model="client.clientId"
                :placeholder="$t('pleaseInputBy', {key: $t('identityServer.clientId')})"
              />
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item
              prop="clientName"
              :label="$t('identityServer.clientName')"
            >
              <el-input
                v-model="client.clientName"
                :placeholder="$t('pleaseInputBy', {key: $t('identityServer.clientName')})"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item
          prop="description"
          :label="$t('identityServer.description')"
        >
          <el-input
            v-model="client.description"
          />
        </el-form-item>
        <el-form-item
          prop="protocolType"
          :label="$t('identityServer.protocolType')"
        >
          <el-select
            v-model="client.protocolType"
            class="full-select"
            :placeholder="$t('pleaseSelectBy', {name: $t('identityServer.protocolType')})"
          >
            <el-option
              key="oidc"
              label="OpenID Connect"
              value="oidc"
            />
          </el-select>
        </el-form-item>
        <el-row>
          <el-col :span="6">
            <el-form-item
              prop="requireClientSecret"
              :label="$t('identityServer.requireClientSecret')"
              label-width="120px"
            >
              <el-switch
                v-model="client.requireClientSecret"
              />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item
              prop="requirePkce"
              :label="$t('identityServer.requirePkce')"
            >
              <el-switch
                v-model="client.requirePkce"
              />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item
              prop="allowPlainTextPkce"
              :label="$t('identityServer.allowPlainTextPkce')"
              label-width="180px"
            >
              <el-switch
                v-model="client.allowPlainTextPkce"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="6">
            <el-form-item
              prop="allowOfflineAccess"
              :label="$t('identityServer.allowOfflineAccess')"
            >
              <el-switch
                v-model="client.allowOfflineAccess"
              />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item
              prop="allowAccessTokensViaBrowser"
              :label="$t('identityServer.allowAccessTokensViaBrowser')"
              label-width="180px"
            >
              <el-switch
                v-model="client.allowAccessTokensViaBrowser"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item
          prop="allowedScopes"
          :label="$t('identityServer.allowedScopes')"
        >
          <el-input-tag-ex
            v-model="client.allowedScopes"
            label="scope"
          />
        </el-form-item>
        <el-form-item
          prop="redirectUris"
          :label="$t('identityServer.redirectUris')"
        >
          <el-input-tag-ex
            v-model="client.redirectUris"
            label="redirectUri"
            validate="url"
          />
        </el-form-item>
        <el-form-item
          prop="allowedGrantTypes"
          :label="$t('identityServer.allowedGrantTypes')"
          label-width="120px"
        >
          <el-input-tag-ex
            v-model="client.allowedGrantTypes"
            label="grantType"
          />
        </el-form-item>
      </el-tab-pane>
      <el-tab-pane label="认证/注销">
        <el-form-item
          prop="enableLocalLogin"
          :label="$t('identityServer.enableLocalLogin')"
        >
          <el-switch
            v-model="client.enableLocalLogin"
          />
        </el-form-item>
        <el-row>
          <el-col :span="8">
            <el-form-item
              prop="frontChannelLogoutSessionRequired"
              :label="$t('identityServer.frontChannelLogoutSessionRequired')"
              label-width="160px"
            >
              <el-switch
                v-model="client.frontChannelLogoutSessionRequired"
              />
            </el-form-item>
          </el-col>
          <el-col :span="16">
            <el-form-item
              prop="frontChannelLogoutUri"
              :label="$t('identityServer.frontChannelLogoutUri')"
              label-width="140px"
            >
              <el-input
                v-model="client.frontChannelLogoutUri"
                :readonly="!client.frontChannelLogoutSessionRequired"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item
              prop="backChannelLogoutSessionRequired"
              :label="$t('identityServer.backChannelLogoutSessionRequired')"
              label-width="160px"
            >
              <el-switch
                v-model="client.backChannelLogoutSessionRequired"
              />
            </el-form-item>
          </el-col>
          <el-col :span="16">
            <el-form-item
              prop="backChannelLogoutUri"
              :label="$t('identityServer.backChannelLogoutUri')"
              label-width="140px"
            >
              <el-input
                v-model="client.backChannelLogoutUri"
                :readonly="!client.backChannelLogoutSessionRequired"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item
          prop="postLogoutRedirectUris"
          :label="$t('identityServer.postLogoutRedirectUris')"
          label-width="140px"
        >
          <el-input-tag-ex
            v-model="client.postLogoutRedirectUris"
            label="postLogoutRedirectUri"
            validate="url"
          />
        </el-form-item>
        <el-form-item
          prop="identityProviderRestrictions"
          :label="$t('identityServer.identityProviderRestrictions')"
          label-width="140px"
        >
          <el-input-tag-ex
            v-model="client.identityProviderRestrictions"
            label="provider"
          />
        </el-form-item>
        <el-form-item
          prop="userSsoLifetime"
          :label="$t('identityServer.userSsoLifetime')"
          label-width="140px"
        >
          <el-input
            v-model="client.userSsoLifetime"
            type="number"
          />
        </el-form-item>
      </el-tab-pane>
      <el-tab-pane label="令牌">
        <el-form-item
          prop="identityTokenLifetime"
          :label="$t('identityServer.identityTokenLifetime')"
          label-width="165px"
        >
          <el-input
            v-model="client.identityTokenLifetime"
            type="number"
          />
        </el-form-item>
        <el-form-item
          prop="accessTokenLifetime"
          :label="$t('identityServer.accessTokenLifetime')"
          label-width="165px"
        >
          <el-input
            v-model="client.accessTokenLifetime"
            type="number"
          />
        </el-form-item>
        <el-form-item
          prop="accessTokenType"
          :label="$t('identityServer.accessTokenType')"
          label-width="165px"
        >
          <el-select
            v-model="client.accessTokenType"
            class="full-select"
            :placeholder="$t('pleaseSelectBy', {name: $t('identityServer.accessTokenType')})"
          >
            <el-option
              :key="0"
              label="Jwt"
              :value="0"
            />
            <el-option
              :key="1"
              label="Reference"
              :value="1"
            />
          </el-select>
        </el-form-item>
        <el-form-item
          prop="authorizationCodeLifetime"
          :label="$t('identityServer.authorizationCodeLifetime')"
          label-width="165px"
        >
          <el-input
            v-model="client.authorizationCodeLifetime"
            type="number"
          />
        </el-form-item>
        <el-form-item
          prop="absoluteRefreshTokenLifetime"
          :label="$t('identityServer.absoluteRefreshTokenLifetime')"
          label-width="165px"
        >
          <el-input
            v-model="client.absoluteRefreshTokenLifetime"
            type="number"
          />
        </el-form-item>
        <el-form-item
          prop="slidingRefreshTokenLifetime"
          :label="$t('identityServer.slidingRefreshTokenLifetime')"
          label-width="165px"
        >
          <el-input
            v-model="client.slidingRefreshTokenLifetime"
            type="number"
          />
        </el-form-item>
        <el-form-item
          prop="refreshTokenUsage"
          :label="$t('identityServer.refreshTokenUsage')"
          label-width="165px"
        >
          <el-select
            v-model="client.refreshTokenUsage"
            class="full-select"
            :placeholder="$t('pleaseSelectBy', {name: $t('identityServer.refreshTokenUsage')})"
          >
            <el-option
              :key="0"
              label="ReUse"
              :value="0"
            />
            <el-option
              :key="1"
              label="OneTimeOnly"
              :value="1"
            />
          </el-select>
        </el-form-item>
        <el-form-item
          prop="refreshTokenExpiration"
          :label="$t('identityServer.refreshTokenExpiration')"
          label-width="165px"
        >
          <el-select
            v-model="client.refreshTokenExpiration"
            class="full-select"
            :placeholder="$t('pleaseSelectBy', {name: $t('identityServer.refreshTokenExpiration')})"
          >
            <el-option
              :key="0"
              label="Sliding"
              :value="0"
            />
            <el-option
              :key="1"
              label="Absolute"
              :value="1"
            />
          </el-select>
        </el-form-item>
        <el-form-item
          prop="allowedCorsOrigins"
          :label="$t('identityServer.allowedCorsOrigins')"
          label-width="165px"
        >
          <el-input-tag-ex
            v-model="client.allowedCorsOrigins"
            label="origin"
            validate="url"
          />
        </el-form-item>
        <el-row>
          <el-col :span="8">
            <el-form-item
              prop="updateAccessTokenClaimsOnRefresh"
              :label="$t('identityServer.updateAccessTokenClaimsOnRefresh')"
              label-width="170px"
            >
              <el-switch
                v-model="client.updateAccessTokenClaimsOnRefresh"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item
              prop="includeJwtId"
              :label="$t('identityServer.includeJwtId')"
            >
              <el-switch
                v-model="client.includeJwtId"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item
              prop="alwaysSendClientClaims"
              :label="$t('identityServer.alwaysSendClientClaims')"
              label-width="170px"
            >
              <el-switch
                v-model="client.alwaysSendClientClaims"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item
              prop="alwaysIncludeUserClaimsInIdToken"
              :label="$t('identityServer.alwaysIncludeUserClaimsInIdToken')"
              label-width="210px"
            >
              <el-switch
                v-model="client.alwaysIncludeUserClaimsInIdToken"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item
          prop="clientClaimsPrefix"
          :label="$t('identityServer.clientClaimsPrefix')"
          label-width="165px"
        >
          <el-input
            v-model="client.clientClaimsPrefix"
          />
        </el-form-item>
        <el-form-item
          prop="pairWiseSubjectSalt"
          :label="$t('identityServer.pairWiseSubjectSalt')"
          label-width="165px"
        >
          <el-input
            v-model="client.pairWiseSubjectSalt"
          />
        </el-form-item>
      </el-tab-pane>
      <el-tab-pane label="同意屏幕">
        <el-row>
          <el-col :span="6">
            <el-form-item
              prop="requireConsent"
              :label="$t('identityServer.requireConsent')"
            >
              <el-switch
                v-model="client.requireConsent"
              />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item
              prop="allowRememberConsent"
              :label="$t('identityServer.allowRememberConsent')"
            >
              <el-switch
                v-model="client.allowRememberConsent"
              />
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item
          prop="clientUri"
          :label="$t('identityServer.clientUri')"
        >
          <el-input
            v-model="client.clientUri"
          />
        </el-form-item>
        <el-form-item
          prop="logoUri"
          :label="$t('identityServer.logoUri')"
        >
          <el-input
            v-model="client.logoUri"
          />
        </el-form-item>
      </el-tab-pane>
      <el-tab-pane label="设备流程">
        <el-form-item
          prop="userCodeType"
          :label="$t('identityServer.userCodeType')"
          label-width="155px"
        >
          <el-input
            v-model="client.userCodeType"
          />
        </el-form-item>
        <el-form-item
          prop="deviceCodeLifetime"
          :label="$t('identityServer.deviceCodeLifetime')"
          label-width="155px"
        >
          <el-input
            v-model="client.deviceCodeLifetime"
            type="number"
          />
        </el-form-item>
      </el-tab-pane>
    </el-tabs>

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
        @click="onSavedEditClient"
      >
        {{ $t('table.confirm') }}
      </el-button>
    </el-form-item>
  </el-form>
</template>

<script lang="ts">
import ElInputTagEx from '@/components/InputTagEx/index.vue'
import { Component, Vue, Prop, Watch } from 'vue-property-decorator'
import ClientService, { Client, ClientUpdate } from '@/api/clients'

@Component({
  name: 'ClientEditForm',
  components: {
    ElInputTagEx
  }
})
export default class extends Vue {
  @Prop({ default: '' })
  private clientId!: string

  private client: Client
  private clientRules = {
    clientId: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.clientId') }), trigger: 'blur' },
      { min: 3, max: 200, message: this.l('fieldLengthMustBeRange', { min: 3, max: 200 }), trigger: 'blur' }
    ],
    clientName: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.clientName') }), trigger: 'blur' }
    ],
    protocolType: [
      { required: true, message: this.l('pleaseSelectBy', { key: this.l('identityServer.protocolType') }), trigger: 'blur' }
    ],
    identityTokenLifetime: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.identityTokenLifetime') }), trigger: 'blur' }
    ],
    accessTokenLifetime: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.accessTokenLifetime') }), trigger: 'blur' }
    ],
    deviceCodeLifetime: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.deviceCodeLifetime') }), trigger: 'blur' }
    ],
    authorizationCodeLifetime: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.authorizationCodeLifetime') }), trigger: 'blur' }
    ],
    absoluteRefreshTokenLifetime: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.absoluteRefreshTokenLifetime') }), trigger: 'blur' }
    ],
    slidingRefreshTokenLifetime: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.slidingRefreshTokenLifetime') }), trigger: 'blur' }
    ],
    refreshTokenUsage: [
      { required: true, message: this.l('pleaseSelectBy', { key: this.l('identityServer.refreshTokenUsage') }), trigger: 'blur' }
    ],
    refreshTokenExpiration: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.refreshTokenExpiration') }), trigger: 'blur' }
    ],
    accessTokenType: [
      { required: true, message: this.l('pleaseSelectBy', { key: this.l('identityServer.accessTokenType') }), trigger: 'blur' }
    ]
  }

  constructor() {
    super()
    this.client = new Client()
  }

  @Watch('clientId', { immediate: true })
  private handleClientIdChanged(id: string) {
    if (id) {
      ClientService.getClientById(id).then(client => {
        this.client = client
      })
    }
  }

  private onSavedEditClient() {
    const clientEditForm = this.$refs.formClient as any
    clientEditForm.validate((valid: boolean) => {
      if (valid) {
        const updateClient = new ClientUpdate()
        updateClient.id = this.clientId
        updateClient.client.setClient(this.client)
        ClientService.updateClient(updateClient).then(clientDto => {
          this.client = clientDto
          const successMessage = this.l('identityServer.updateClientSuccess', { id: this.client.clientId })
          this.$message.success(successMessage)
          clientEditForm.resetFields()
          this.$emit('closed', true)
        })
      }
    })
  }

  private onCancel() {
    const clientEditForm = this.$refs.formClient as any
    clientEditForm.resetFields()
    this.$emit('closed', false)
  }

  private l(name: string, values?: any[] | { [key: string]: any }) {
    return this.$t(name, values).toString()
  }

  private test(v: any) {
    console.log(v)
    console.log(this.client.redirectUris)
  }
}
</script>

<style lang="scss" scoped>
.full-select {
  width: 100%;
}
.confirm {
  position: absolute;
  right: 10px;
}
.cancel {
  position: absolute;
  right: 120px;
}
</style>
