<template>
  <el-dialog
    v-el-draggable-dialog
    width="800px"
    :visible="showDialog"
    :title="title"
    custom-class="modal-form"
    :show-close="false"
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    @close="onFormClosed(false)"
  >
    <el-form
      ref="formClient"
      label-width="100px"
      :model="client"
    >
      <el-tabs v-model="activeTabPane">
        <el-tab-pane
          name="basics"
          :label="$t('AbpIdentityServer.Basics')"
        >
          <el-form-item
            prop="enabled"
            :label="$t('AbpIdentityServer.Client:Enabled')"
          >
            <el-switch
              v-model="client.enabled"
            />
          </el-form-item>
          <el-row>
            <el-col :span="12">
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
            </el-col>
            <el-col :span="12">
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
            </el-col>
          </el-row>
          <el-form-item
            prop="description"
            :label="$t('AbpIdentityServer.Description')"
          >
            <el-input
              v-model="client.description"
            />
          </el-form-item>
          <el-form-item
            prop="protocolType"
            :label="$t('AbpIdentityServer.Client:ProtocolType')"
            :rules="{
              required: true,
              message: $t('pleaseSelectBy', {key: $t('AbpIdentityServer.Client:ProtocolType')}),
              trigger: 'blur'
            }"
          >
            <el-select
              v-model="client.protocolType"
              class="full-select"
              :placeholder="$t('pleaseSelectBy', {name: $t('AbpIdentityServer.Client:ProtocolType')})"
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
                :label="$t('AbpIdentityServer.Client:RequiredClientSecret')"
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
                :label="$t('AbpIdentityServer.Client:RequiredPkce')"
              >
                <el-switch
                  v-model="client.requirePkce"
                />
              </el-form-item>
            </el-col>
            <el-col :span="6">
              <el-form-item
                prop="allowPlainTextPkce"
                :label="$t('AbpIdentityServer.Client:AllowedPlainTextPkce')"
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
                :label="$t('AbpIdentityServer.Client:AllowedOfflineAccess')"
              >
                <el-switch
                  v-model="client.allowOfflineAccess"
                />
              </el-form-item>
            </el-col>
            <el-col :span="6">
              <el-form-item
                prop="allowAccessTokensViaBrowser"
                :label="$t('AbpIdentityServer.Client:AllowedAccessTokensViaBrowser')"
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
            :label="$t('AbpIdentityServer.Client:AllowedScopes')"
          >
            <el-select
              v-model="client.allowedScopes"
              multiple
              filterable
              allow-create
              clearable
              style="width: 100%;"
            >
              <el-option-group :label="$t('AbpIdentityServer.Resource:Api')">
                <el-option
                  v-for="resource in apiResources"
                  :key="resource"
                  :label="resource"
                  :value="resource"
                />
              </el-option-group>
              <el-option-group :label="$t('AbpIdentityServer.Resource:Identity')">
                <el-option
                  v-for="resource in identityResources"
                  :key="resource"
                  :label="resource"
                  :value="resource"
                />
              </el-option-group>
            </el-select>
          </el-form-item>
          <el-form-item
            prop="redirectUris"
            :label="$t('AbpIdentityServer.Client:RedirectUris')"
          >
            <el-select
              v-model="client.redirectUris"
              multiple
              filterable
              allow-create
              clearable
              style="width: 100%;"
            />
          </el-form-item>
          <el-form-item
            prop="allowedGrantTypes"
            :label="$t('AbpIdentityServer.Client:AllowedGrantTypes')"
            label-width="120px"
          >
            <el-select
              v-model="client.allowedGrantTypes"
              multiple
              filterable
              allow-create
              clearable
              class="full-select"
            >
              <el-option
                v-for="(grantType, index) in supportedGrantypes"
                :key="index"
                :label="grantType"
                :value="grantType"
              />
            </el-select>
          </el-form-item>
        </el-tab-pane>
        <el-tab-pane
          name="authentication"
          :label="$t('AbpIdentityServer.Authentication')"
        >
          <el-form-item
            prop="enableLocalLogin"
            :label="$t('AbpIdentityServer.Client:EnableLocalLogin')"
          >
            <el-switch
              v-model="client.enableLocalLogin"
            />
          </el-form-item>
          <el-row>
            <el-col :span="8">
              <el-form-item
                prop="frontChannelLogoutSessionRequired"
                :label="$t('AbpIdentityServer.Client:FrontChannelLogoutSessionRequired')"
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
                :label="$t('AbpIdentityServer.Client:FrontChannelLogoutUri')"
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
                :label="$t('AbpIdentityServer.Client:BackChannelLogoutSessionRequired')"
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
                :label="$t('AbpIdentityServer.Client:BackChannelLogoutUri')"
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
            :label="$t('AbpIdentityServer.Client:PostLogoutRedirectUris')"
            label-width="140px"
          >
            <el-select
              v-model="client.postLogoutRedirectUris"
              multiple
              filterable
              allow-create
              clearable
              style="width: 100%;"
            />
          </el-form-item>
          <el-form-item
            prop="identityProviderRestrictions"
            :label="$t('AbpIdentityServer.Client:IdentityProviderRestrictions')"
            label-width="140px"
          >
            <el-select
              v-model="client.identityProviderRestrictions"
              multiple
              filterable
              allow-create
              clearable
              style="width: 100%;"
            />
          </el-form-item>
          <el-form-item
            prop="userSsoLifetime"
            :label="$t('AbpIdentityServer.Client:UserSsoLifetime')"
            label-width="140px"
          >
            <el-input
              v-model="client.userSsoLifetime"
              type="number"
            />
          </el-form-item>
        </el-tab-pane>
        <el-tab-pane
          name="token"
          :label="$t('AbpIdentityServer.Token')"
        >
          <el-form-item
            prop="identityTokenLifetime"
            :label="$t('AbpIdentityServer.Client:IdentityTokenLifetime')"
            label-width="165px"
            :rules="{
              required: true,
              message: $t('pleaseInputBy', {key: $t('AbpIdentityServer.Client:IdentityTokenLifetime')}),
              trigger: 'blur'
            }"
          >
            <el-input
              v-model="client.identityTokenLifetime"
              type="number"
            />
          </el-form-item>
          <el-form-item
            prop="accessTokenLifetime"
            :label="$t('AbpIdentityServer.Client:AccessTokenLifetime')"
            label-width="165px"
            :rules="{
              required: true,
              message: $t('pleaseInputBy', {key: $t('AbpIdentityServer.Client:AccessTokenLifetime')}),
              trigger: 'blur'
            }"
          >
            <el-input
              v-model="client.accessTokenLifetime"
              type="number"
            />
          </el-form-item>
          <el-form-item
            prop="accessTokenType"
            :label="$t('AbpIdentityServer.Client:AccessTokenType')"
            label-width="165px"
            :rules="{
              required: true,
              message: $t('pleaseSelectBy', {key: $t('AbpIdentityServer.Client:AccessTokenType')}),
              trigger: 'blur'
            }"
          >
            <el-select
              v-model="client.accessTokenType"
              class="full-select"
              :placeholder="$t('pleaseSelectBy', {name: $t('AbpIdentityServer.Client:AccessTokenType')})"
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
            :label="$t('AbpIdentityServer.Client:AuthorizationCodeLifetime')"
            label-width="165px"
            :rules="{
              required: true,
              message: $t('pleaseInputBy', {key: $t('AbpIdentityServer.Client:AuthorizationCodeLifetime')}),
              trigger: 'blur'
            }"
          >
            <el-input
              v-model="client.authorizationCodeLifetime"
              type="number"
            />
          </el-form-item>
          <el-form-item
            prop="absoluteRefreshTokenLifetime"
            :label="$t('AbpIdentityServer.Client:AbsoluteRefreshTokenLifetime')"
            label-width="165px"
            :rules="{
              required: true,
              message: $t('pleaseInputBy', {key: $t('AbpIdentityServer.Client:AbsoluteRefreshTokenLifetime')}),
              trigger: 'blur'
            }"
          >
            <el-input
              v-model="client.absoluteRefreshTokenLifetime"
              type="number"
            />
          </el-form-item>
          <el-form-item
            prop="slidingRefreshTokenLifetime"
            :label="$t('AbpIdentityServer.Client:SlidingRefreshTokenLifetime')"
            label-width="165px"
            :rules="{
              required: true,
              message: $t('pleaseInputBy', {key: $t('AbpIdentityServer.Client:SlidingRefreshTokenLifetime')}),
              trigger: 'blur'
            }"
          >
            <el-input
              v-model="client.slidingRefreshTokenLifetime"
              type="number"
            />
          </el-form-item>
          <el-form-item
            prop="refreshTokenUsage"
            :label="$t('AbpIdentityServer.Client:RefreshTokenUsage')"
            label-width="165px"
            :rules="{
              required: true,
              message: $t('pleaseSelectBy', {key: $t('AbpIdentityServer.Client:RefreshTokenUsage')}),
              trigger: 'blur'
            }"
          >
            <el-select
              v-model="client.refreshTokenUsage"
              class="full-select"
              :placeholder="$t('pleaseSelectBy', {name: $t('AbpIdentityServer.Client:RefreshTokenUsage')})"
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
            :label="$t('AbpIdentityServer.Client:RefreshTokenExpiration')"
            label-width="165px"
            :rules="{
              required: true,
              message: $t('pleaseInputBy', {key: $t('AbpIdentityServer.Client:RefreshTokenExpiration')}),
              trigger: 'blur'
            }"
          >
            <el-select
              v-model="client.refreshTokenExpiration"
              class="full-select"
              :placeholder="$t('pleaseSelectBy', {name: $t('AbpIdentityServer.Client:RefreshTokenExpiration')})"
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
            :label="$t('AbpIdentityServer.Client:AllowedCorsOrigins')"
            label-width="165px"
          >
            <el-select
              v-model="client.allowedCorsOrigins"
              multiple
              filterable
              allow-create
              clearable
              style="width: 100%;"
            >
              <el-option
                v-for="(corsOrigin, index) in distinctAllowedCorsOrigins"
                :key="index"
                :label="corsOrigin"
                :value="corsOrigin"
              />
            </el-select>
          </el-form-item>
          <el-row>
            <el-col :span="8">
              <el-form-item
                prop="updateAccessTokenClaimsOnRefresh"
                :label="$t('AbpIdentityServer.Client:UpdateAccessTokenClaimsOnRefresh')"
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
                :label="$t('AbpIdentityServer.Client:IncludeJwtId')"
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
                :label="$t('AbpIdentityServer.Client:AlwaysSendClientClaims')"
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
                :label="$t('AbpIdentityServer.Client:AlwaysIncludeUserClaimsInIdToken')"
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
            :label="$t('AbpIdentityServer.Client:ClientClaimsPrefix')"
            label-width="165px"
          >
            <el-input
              v-model="client.clientClaimsPrefix"
            />
          </el-form-item>
          <el-form-item
            prop="pairWiseSubjectSalt"
            :label="$t('AbpIdentityServer.Client:PairWiseSubjectSalt')"
            label-width="165px"
          >
            <el-input
              v-model="client.pairWiseSubjectSalt"
            />
          </el-form-item>
        </el-tab-pane>
        <el-tab-pane
          name="consent"
          :label="$t('AbpIdentityServer.Consent')"
        >
          <el-row>
            <el-col :span="6">
              <el-form-item
                prop="requireConsent"
                :label="$t('AbpIdentityServer.Client:RequireConsent')"
              >
                <el-switch
                  v-model="client.requireConsent"
                />
              </el-form-item>
            </el-col>
            <el-col :span="6">
              <el-form-item
                prop="allowRememberConsent"
                :label="$t('AbpIdentityServer.Client:AllowRememberConsent')"
              >
                <el-switch
                  v-model="client.allowRememberConsent"
                />
              </el-form-item>
            </el-col>
          </el-row>
          <el-form-item
            prop="clientUri"
            :label="$t('AbpIdentityServer.Client:ClientUri')"
          >
            <el-input
              v-model="client.clientUri"
            />
          </el-form-item>
          <el-form-item
            prop="logoUri"
            :label="$t('AbpIdentityServer.Client:LogoUri')"
          >
            <el-input
              v-model="client.logoUri"
            />
          </el-form-item>
        </el-tab-pane>
        <el-tab-pane
          name="deviceFlow"
          :label="$t('AbpIdentityServer.DeviceFlow')"
        >
          <el-form-item
            prop="userCodeType"
            :label="$t('AbpIdentityServer.Client:UserCodeType')"
            label-width="155px"
          >
            <el-input
              v-model="client.userCodeType"
            />
          </el-form-item>
          <el-form-item
            prop="deviceCodeLifetime"
            :label="$t('AbpIdentityServer.Client:DeviceCodeLifetime')"
            label-width="155px"
            :rules="{
              required: true,
              message: $t('pleaseInputBy', {key: $t('AbpIdentityServer.Client:DeviceCodeLifetime')}),
              trigger: 'blur'
            }"
          >
            <el-input
              v-model="client.deviceCodeLifetime"
              type="number"
            />
          </el-form-item>
        </el-tab-pane>
        <el-tab-pane
          name="avanced"
        >
          <el-dropdown
            slot="label"
            @command="onDropdownMenuItemChanged"
          >
            <span class="el-dropdown-link">
              {{ $t('AbpIdentityServer.Advanced') }}<i class="el-icon-arrow-down el-icon--right" />
            </span>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item command="secret-edit-form">
                {{ $t('AbpIdentityServer.Secret') }}
              </el-dropdown-item>
              <el-dropdown-item command="client-claim-edit-form">
                {{ $t('AbpIdentityServer.Claims') }}
              </el-dropdown-item>
              <el-dropdown-item command="properties-edit-form">
                {{ $t('AbpIdentityServer.Propertites') }}
              </el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
          <component
            :is="advancedComponent"
            :secrets="client.clientSecrets"
            :client-claims="client.claims"
            :properties="client.properties"
            :allowed-create-prop="checkPermission(['AbpIdentityServer.Clients.ManageProperties'])"
            :allowed-delete-prop="checkPermission(['AbpIdentityServer.Clients.ManageProperties'])"
            :allowed-create-secret="checkPermission(['AbpIdentityServer.Clients.ManageClaims'])"
            :allowed-delete-secret="checkPermission(['AbpIdentityServer.Clients.ManageClaims'])"
            @onSecretCreated="onClientSecretCreated"
            @onSecretDeleted="onClientSecretDeleted"
            @onClientClaimCreated="onClientClaimCreated"
            @onClientClaimDeleted="onClientClaimDeleted"
            @onCreated="onClientPropertyCreated"
            @onDeleted="onClientPropertyDeleted"
          />
        </el-tab-pane>
      </el-tabs>

      <el-form-item>
        <el-button
          class="cancel"
          type="info"
          @click="onFormClosed"
        >
          {{ $t('AbpIdentityServer.Cancel') }}
        </el-button>
        <el-button
          class="confirm"
          type="primary"
          icon="el-icon-check"
          @click="onSave"
        >
          {{ $t('AbpIdentityServer.Save') }}
        </el-button>
      </el-form-item>
    </el-form>
  </el-dialog>
</template>

<script lang="ts">
import { checkPermission } from '@/utils/permission'

import { Component, Vue, Prop, Watch } from 'vue-property-decorator'
import ClientApiService, { Client, ClientUpdate, ClientCreateOrUpdate, ClientClaim, SecretCreateOrUpdate } from '@/api/clients'

import ClientClaimEditForm from './ClientClaimEditForm.vue'
import SecretEditForm from '../../components/SecretEditForm.vue'
import PropertiesEditForm from '../../components/PropertiesEditForm.vue'

@Component({
  name: 'ClientEditForm',
  components: {
    SecretEditForm,
    PropertiesEditForm,
    ClientClaimEditForm
  },
  methods: {
    checkPermission
  }
})
export default class extends Vue {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: '' })
  private title!: string

  @Prop({ default: '' })
  private clientId!: string

  @Prop({ default: () => { return new Array<string>() } })
  private supportedGrantypes!: string[]

  private activeTabPane = 'basics'
  private advancedComponent = 'secret-edit-form'
  private apiResources = new Array<string>()
  private identityResources = new Array<string>()
  private distinctAllowedCorsOrigins = new Array<string>()

  private client = new Client()

  @Watch('showDialog', { immediate: true })
  private onShowDialogChanged() {
    this.handleGetClient()
  }

  mounted() {
    this.handleGetResources()
  }

  private handleGetResources() {
    ClientApiService.getAssignableApiResources()
      .then(res => {
        this.apiResources = res.items
      })
    ClientApiService.getAssignableIdentityResources()
      .then(res => {
        this.identityResources = res.items
      })
    ClientApiService.getAllDistinctAllowedCorsOrigins()
      .then(res => {
        this.distinctAllowedCorsOrigins = res.items
      })
  }

  private handleGetClient() {
    if (this.showDialog && this.clientId) {
      ClientApiService.getClientById(this.clientId)
        .then(client => {
          this.client = client
        })
    }
  }

  private onDropdownMenuItemChanged(component: any) {
    this.activeTabPane = 'avanced'
    this.advancedComponent = component
  }

  private onClientSecretCreated(secret: any) {
    const clientSecret = new SecretCreateOrUpdate()
    clientSecret.hashType = secret.hashType
    clientSecret.type = secret.type
    clientSecret.value = secret.value
    clientSecret.description = secret.description
    clientSecret.expiration = secret.expiration
    this.client.clientSecrets.push(clientSecret)
  }

  private onClientSecretDeleted(type: string, value: string) {
    const secretIndex = this.client.clientSecrets.findIndex(secret => secret.type === type && secret.value === value)
    this.client.clientSecrets.splice(secretIndex, 1)
  }

  private onClientClaimCreated(type: string, value: string) {
    this.client.claims.push(new ClientClaim(type, value))
  }

  private onClientClaimDeleted(type: string, value: string) {
    const claimIndex = this.client.claims.findIndex(c => c.type === type && c.value === value)
    this.client.claims.splice(claimIndex, 1)
  }

  private onClientPropertyCreated(key: string, value: string) {
    this.$set(this.client.properties, key, value)
  }

  private onClientPropertyDeleted(key: string) {
    this.$delete(this.client.properties, key)
  }

  private onSave() {
    const clientEditForm = this.$refs.formClient as any
    clientEditForm.validate((valid: boolean) => {
      if (valid) {
        const updateClient = new ClientUpdate()
        this.updateClientByInput(updateClient)
        updateClient.updateByClient(this.client)
        ClientApiService.updateClient(this.clientId, updateClient)
          .then(client => {
            this.client = client
            const successMessage = this.l('global.successful')
            this.$message.success(successMessage)
          })
      }
    })
  }

  private updateClientByInput(client: ClientCreateOrUpdate) {
    client.clientId = this.client.clientId
    client.clientName = this.client.clientName
    client.description = this.client.description
    client.allowedGrantTypes = this.client.allowedGrantTypes
  }

  private onFormClosed() {
    this.resetFormFields()
    this.$emit('closed')
  }

  private resetFormFields() {
    const clientEditForm = this.$refs.formClient as any
    clientEditForm.resetFields()
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
.confirm {
  position: absolute;
  right: 10px;
  width:100px;
  top: 10px;
}
.cancel {
  position: absolute;
  right: 120px;
  width:100px;
  top: 10px;
}
</style>
