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
      <el-tabs
        v-model="activeTabPane"
        :before-leave="onTabBeforeLeave"
      >
        <el-tab-pane
          name="basics"
          :label="$t('AbpIdentityServer.Basics')"
        >
          <el-form-item
            prop="enabled"
            label-width="0"
          >
            <el-checkbox
              v-model="client.enabled"
              class="label-title"
            >
              {{ $t('AbpIdentityServer.Client:Enabled') }}
            </el-checkbox>
          </el-form-item>
          <el-form-item
            prop="requireRequestObject"
            label-width="0"
          >
            <el-checkbox
              v-model="client.requireRequestObject"
              class="label-title"
            >
              {{ $t('AbpIdentityServer.Client:RequireRequestObject') }}
            </el-checkbox>
          </el-form-item>
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
              disabled
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
              type="textarea"
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
          <el-form-item
            prop="allowedIdentityTokenSigningAlgorithms"
            :label="$t('AbpIdentityServer.Client:AllowedIdentityTokenSigningAlgorithms')"
            label-width="180px"
          >
            <el-input
              v-model="client.allowedIdentityTokenSigningAlgorithms"
            />
          </el-form-item>
          <el-form-item
            prop="requirePkce"
            label-width="0"
          >
            <el-checkbox
              v-model="client.requirePkce"
              class="label-title"
            >
              {{ $t('AbpIdentityServer.Client:RequiredPkce') }}
            </el-checkbox>
          </el-form-item>
          <el-form-item
            prop="allowPlainTextPkce"
            label-width="0"
          >
            <el-checkbox
              v-model="client.allowPlainTextPkce"
              class="label-title"
            >
              {{ $t('AbpIdentityServer.Client:AllowedPlainTextPkce') }}
            </el-checkbox>
          </el-form-item>
        </el-tab-pane>
        <el-tab-pane
          name="applicationUris"
        >
          <el-dropdown
            slot="label"
            @command="onUrlsDropdownItemChanged"
          >
            <span class="el-dropdown-link">
              {{ $t('AbpIdentityServer.Client:ApplicationUrls') }}
              <i class="el-icon-arrow-down el-icon--right" />
            </span>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                :command="{
                  component: 'redirect-edit-form',
                  prop: 'redirectUris',
                  key: 'redirectUri',
                  title: $t('AbpIdentityServer.Client:CallbackUrl'),
                  suggestes: []
                }"
              >
                {{ $t('AbpIdentityServer.Client:CallbackUrl') }}
              </el-dropdown-item>
              <el-dropdown-item
                :command="{
                  component: 'signout-edit-form',
                  prop: 'postLogoutRedirectUris',
                  key: 'postLogoutRedirectUri',
                  title: $t('AbpIdentityServer.Client:PostLogoutRedirectUri'),
                  suggestes: []
                }"
              >
                {{ $t('AbpIdentityServer.Client:PostLogoutRedirectUri') }}
              </el-dropdown-item>
              <el-dropdown-item
                :command="{
                  component: 'cors-origin-edit-form',
                  prop: 'allowedCorsOrigins',
                  key: 'origin',
                  title: $t('AbpIdentityServer.Client:AllowedCorsOrigins'),
                  suggestes: distinctAllowedCorsOrigins
                }"
              >
                {{ $t('AbpIdentityServer.Client:AllowedCorsOrigins') }}
              </el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
          <UrlsEditForm
            v-model="client[clientUrlProp]"
            :url-key="clientUrlKey"
            :title="clientUrlTitle"
            :fetch-urls="clientUrlSuggestes"
          />
        </el-tab-pane>
        <el-tab-pane
          name="resources"
        >
          <el-dropdown
            slot="label"
            @command="onResourceDropdownItemChanged"
          >
            <span class="el-dropdown-link">
              {{ $t('AbpIdentityServer.Client:Resources') }}
              <i class="el-icon-arrow-down el-icon--right" />
            </span>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                :command="{
                  title: $t('AbpIdentityServer.Resource:Api'),
                  scopes: apiResources
                }"
              >
                {{ $t('AbpIdentityServer.Resource:Api') }}
              </el-dropdown-item>
              <el-dropdown-item
                :command="{
                  title: $t('AbpIdentityServer.Resource:Identity'),
                  scopes: identityResources
                }"
              >
                {{ $t('AbpIdentityServer.Resource:Identity') }}
              </el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
          <client-scope-edit-form
            v-model="client.allowedScopes"
            :title="clientScopeTitle"
            :scopes="clientResources"
          />
        </el-tab-pane>
        <el-tab-pane
          name="authentication"
          :label="$t('AbpIdentityServer.Authentication')"
        >
          <el-form-item
            prop="frontChannelLogoutSessionRequired"
            label-width="0"
          >
            <el-checkbox
              v-model="client.frontChannelLogoutSessionRequired"
              class="label-title"
            >
              {{ $t('AbpIdentityServer.Client:FrontChannelLogoutSessionRequired') }}
            </el-checkbox>
          </el-form-item>
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
          <el-form-item
            prop="backChannelLogoutSessionRequired"
            label-width="0"
          >
            <el-checkbox
              v-model="client.backChannelLogoutSessionRequired"
              class="label-title"
            >
              {{ $t('AbpIdentityServer.Client:BackChannelLogoutSessionRequired') }}
            </el-checkbox>
          </el-form-item>
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
            prop="userSsoLifetime"
            :label="$t('AbpIdentityServer.Client:UserSsoLifetime')"
            label-width="140px"
          >
            <el-input
              v-model="client.userSsoLifetime"
              type="number"
            />
          </el-form-item>
          <el-row>
            <el-col :span="8">
              <el-form-item
                prop="allowOfflineAccess"
                label-width="0"
              >
                <el-checkbox
                  v-model="client.allowOfflineAccess"
                  class="label-title"
                >
                  {{ $t('AbpIdentityServer.Client:AllowedOfflineAccess') }}
                </el-checkbox>
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item
                prop="allowAccessTokensViaBrowser"
                label-width="0"
              >
                <el-checkbox
                  v-model="client.allowAccessTokensViaBrowser"
                  class="label-title"
                >
                  {{ $t('AbpIdentityServer.Client:AllowedAccessTokensViaBrowser') }}
                </el-checkbox>
              </el-form-item>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="8">
              <el-form-item
                prop="updateAccessTokenClaimsOnRefresh"
                label-width="0"
              >
                <el-checkbox
                  v-model="client.updateAccessTokenClaimsOnRefresh"
                  class="label-title"
                >
                  {{ $t('AbpIdentityServer.Client:UpdateAccessTokenClaimsOnRefresh') }}
                </el-checkbox>
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item
                prop="includeJwtId"
                label-width="0"
              >
                <el-checkbox
                  v-model="client.includeJwtId"
                  class="label-title"
                >
                  {{ $t('AbpIdentityServer.Client:IncludeJwtId') }}
                </el-checkbox>
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
          <el-form-item
            prop="requireConsent"
            label-width="0"
          >
            <el-checkbox
              v-model="client.requireConsent"
              class="label-title"
            >
              {{ $t('AbpIdentityServer.Client:RequireConsent') }}
            </el-checkbox>
          </el-form-item>
          <el-form-item
            prop="allowRememberConsent"
            label-width="0"
          >
            <el-checkbox
              v-model="client.allowRememberConsent"
              class="label-title"
            >
              {{ $t('AbpIdentityServer.Client:AllowRememberConsent') }}
            </el-checkbox>
          </el-form-item>
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
          name="advanced"
        >
          <el-dropdown
            slot="label"
            @command="onAdvancedDropdownItemChanged"
          >
            <span class="el-dropdown-link">
              {{ $t('AbpIdentityServer.Advanced') }}<i class="el-icon-arrow-down el-icon--right" />
            </span>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                :command="{
                  component: 'client-secret-edit-form',
                  prop: 'clientSecrets',
                  title: $t('AbpIdentityServer.Secret')
                }"
              >
                {{ $t('AbpIdentityServer.Secret') }}
              </el-dropdown-item>
              <el-dropdown-item
                :command="{
                  component: 'client-claim-edit-form',
                  prop: 'claims',
                  title: $t('AbpIdentityServer.Claims')
                }"
              >
                {{ $t('AbpIdentityServer.Claims') }}
              </el-dropdown-item>
              <el-dropdown-item
                :command="{
                  component: 'properties-edit-form',
                  prop: 'properties',
                  key: 'key',
                  title: $t('AbpIdentityServer.Propertites')
                }"
              >
                {{ $t('AbpIdentityServer.Propertites') }}
              </el-dropdown-item>
              <el-dropdown-item
                :command="{
                  component: 'grant-type-edit-form',
                  prop: 'allowedGrantTypes',
                  key: 'grantType',
                  title: $t('AbpIdentityServer.Client:AllowedGrantTypes')
                }"
              >
                {{ $t('AbpIdentityServer.Client:AllowedGrantTypes') }}
              </el-dropdown-item>
              <el-dropdown-item
                :command="{
                  component: 'client-idp-edit-form',
                  prop: 'identityProviderRestrictions',
                  key: 'provider',
                  title: $t('AbpIdentityServer.Client:IdentityProviderRestrictions')
                }"
              >
                {{ $t('AbpIdentityServer.Client:IdentityProviderRestrictions') }}
              </el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
          <component
            :is="advancedComponent"
            v-model="client[clientAdvancedPropName]"
            :prop-name="clientAdvancedPropKey"
            :supported-grantypes="supportedGrantypes"
            :client="client"
            :allowed-create-prop="checkPermission(['AbpIdentityServer.Clients.ManageProperties'])"
            :allowed-delete-prop="checkPermission(['AbpIdentityServer.Clients.ManageProperties'])"
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
          :loading="changeClient"
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

import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'
import ClientApiService, {
  Client,
  ClientUpdate,
  ClientCreateOrUpdate
} from '@/api/clients'

import UrlsEditForm from '../../components/UrlsEditForm.vue'
import ClientClaimEditForm from './ClientClaimEditForm.vue'
import GrantTypeEditForm from './GrantTypeEditForm.vue'
import ClientIdpEditForm from './ClientIdpEditForm.vue'
import ClientScopeEditForm from './ClientScopeEditForm.vue'
import ClientSecretEditForm from './ClientSecretEditForm.vue'
import PropertiesEditForm from '../../components/PropertiesEditForm.vue'

@Component({
  name: 'ClientEditForm',
  components: {
    UrlsEditForm,
    ClientSecretEditForm,
    ClientIdpEditForm,
    GrantTypeEditForm,
    PropertiesEditForm,
    ClientClaimEditForm,
    ClientScopeEditForm
  },
  methods: {
    checkPermission
  }
})
export default class extends Mixins(LocalizationMiXin) {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: '' })
  private clientId!: string

  @Prop({ default: () => { return new Array<string>() } })
  private supportedGrantypes!: string[]

  private changeClient = false

  get title() {
    return this.l('AbpIdentityServer.Client:Name', { 0: this.client.clientName })
  }

  private activeTabPane = 'basics'
  private advancedComponent = 'client-secret-edit-form'
  private apiResources = new Array<string>()
  private identityResources = new Array<string>()
  private distinctAllowedCorsOrigins = new Array<string>()

  private client = new Client()
  // 客户端高级选项属性名称
  private clientAdvancedPropName = ''
  // 高级选项属性键名称
  private clientAdvancedPropKey = ''
  // 高级选项标题
  private clientAdvancedTitle = ''
  // Url选项属性    传递给子元素的属性名称  v-model
  private clientUrlProp = ''
  // Url选项键名称  用于在组件中遍历子元素
  private clientUrlKey = ''
  // Url选项标题    用于在编辑框上显示当前选项卡的名称
  private clientUrlTitle = ''
  // 客户端用于搜索匹配的Url列表
  private clientUrlSuggestes = []
  // 客户端资源标题
  private clientScopeTitle = ''
  // 客户端可用的资源列表
  private clientResources = []

  private blockSwitchTabPane = ['applicationUris', 'advanced', 'resources']

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
      ClientApiService
        .get(this.clientId)
        .then(client => {
          this.client = client
        })
    }
  }

  private onTabBeforeLeave(activeName: string) {
    return !this.blockSwitchTabPane.some(name => name === activeName)
  }

  private onResourceDropdownItemChanged(command: any) {
    this.clientScopeTitle = command.title
    this.clientResources = command.scopes
    this.switchTabPane('resources')
  }

  private onAdvancedDropdownItemChanged(command: any) {
    this.clientAdvancedPropName = command.prop
    this.clientAdvancedPropKey = command.key
    this.clientAdvancedTitle = command.title
    this.advancedComponent = command.component
    this.switchTabPane('advanced')
  }

  private onUrlsDropdownItemChanged(command: any) {
    this.clientUrlProp = command.prop
    this.clientUrlKey = command.key
    this.clientUrlTitle = command.title
    this.switchTabPane('applicationUris')
  }

  private switchTabPane(tabPaneName: string) {
    const tabPaneIndex = this.blockSwitchTabPane.findIndex(name => name === tabPaneName)
    if (tabPaneIndex >= 0) {
      this.blockSwitchTabPane.splice(tabPaneIndex, 1)
    }
    this.activeTabPane = tabPaneName
  }

  private onDropdownMenuItemChanged(component: any) {
    this.activeTabPane = 'avanced'
    this.advancedComponent = component
  }

  private onSave() {
    const clientEditForm = this.$refs.formClient as any
    clientEditForm.validate((valid: boolean) => {
      if (valid) {
        this.changeClient = true
        const updateClient = new ClientUpdate()
        this.updateClientByInput(updateClient)
        updateClient.updateByClient(this.client)
        ClientApiService
          .update(this.clientId, updateClient)
          .then(client => {
            this.client = client
            const successMessage = this.l('global.successful')
            this.$message.success(successMessage)
            this.onFormClosed()
          })
          .finally(() => {
            this.changeClient = false
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
    this.changeClient = false
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
.label-title {
  text-align: right;
  font-size: 14px;
  color: #1f2d3d;
  padding: 0 12px 0 0;
  box-sizing: border-box;
  font-weight: 700;
}
</style>
