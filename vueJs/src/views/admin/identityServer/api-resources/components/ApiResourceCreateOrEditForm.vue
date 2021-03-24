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
    <div class="app-container">
      <el-form
        ref="formApiResource"
        label-width="130px"
        :model="apiResource"
      >
        <el-tabs
          v-model="activeTabPane"
          type="border-card"
          :before-leave="onTabBeforeLeave"
        >
          <el-tab-pane
            name="basics"
            :label="$t('AbpIdentityServer.Basics')"
          >
            <el-form-item
              prop="enabled"
              :label="$t('AbpIdentityServer.Resource:Enabled')"
            >
              <el-switch
                v-model="apiResource.enabled"
              />
            </el-form-item>
            <el-form-item
              prop="showInDiscoveryDocument"
              :label="$t('AbpIdentityServer.ShowInDiscoveryDocument')"
            >
              <el-switch
                v-model="apiResource.showInDiscoveryDocument"
              />
            </el-form-item>
            <el-form-item
              prop="name"
              :label="$t('AbpIdentityServer.Name')"
              :rules="{
                required: true,
                message: $t('pleaseInputBy', {key: $t('AbpIdentityServer.Name')}),
                trigger: 'blur'
              }"
            >
              <el-input
                v-model="apiResource.name"
                :readonly="isEdit"
                :placeholder="$t('pleaseInputBy', {key: $t('AbpIdentityServer.Name')})"
              />
            </el-form-item>
            <el-form-item
              prop="displayName"
              :label="$t('AbpIdentityServer.DisplayName')"
            >
              <el-input
                v-model="apiResource.displayName"
              />
            </el-form-item>
            <el-form-item
              prop="description"
              :label="$t('AbpIdentityServer.Description')"
            >
              <el-input
                v-model="apiResource.description"
              />
            </el-form-item>
            <el-form-item
              prop="allowedAccessTokenSigningAlgorithms"
              :label="$t('AbpIdentityServer.AllowedAccessTokenSigningAlgorithms')"
            >
              <el-input
                v-model="apiResource.allowedAccessTokenSigningAlgorithms"
              />
            </el-form-item>
          </el-tab-pane>
          <el-tab-pane :label="$t('AbpIdentityServer.Scope')">
            <scope-edit-form
              v-model="apiResource.scopes"
              :scopes="apiScopes"
            />
          </el-tab-pane>
          <el-tab-pane
            name="userClaim"
            :label="$t('AbpIdentityServer.UserClaim')"
          >
            <user-claim-edit-form
              v-model="apiResource.userClaims"
            />
          </el-tab-pane>
          <el-tab-pane
            v-if="isEdit"
            name="advanced"
          >
            <el-dropdown
              slot="label"
              @command="onDropdownMenuItemChanged"
            >
              <span class="el-dropdown-link">
                {{ $t('AbpIdentityServer.Advanced') }}<i class="el-icon-arrow-down el-icon--right" />
              </span>
              <el-dropdown-menu slot="dropdown">
                <el-dropdown-item
                  :command="{
                    component: 'secret-edit-form',
                    prop: 'secrets'
                  }"
                >
                  {{ $t('AbpIdentityServer.Secret') }}
                </el-dropdown-item>
                <el-dropdown-item
                  :command="{
                    component: 'properties-edit-form',
                    prop: 'properties'
                  }"
                >
                  {{ $t('AbpIdentityServer.Propertites') }}
                </el-dropdown-item>
              </el-dropdown-menu>
            </el-dropdown>
            <component
              :is="advancedComponent"
              v-model="apiResource[advancedPropName]"
              :allowed-create-prop="checkPermission(['AbpIdentityServer.ApiResources.ManageProperties'])"
              :allowed-delete-prop="checkPermission(['AbpIdentityServer.ApiResources.ManageProperties'])"
              :allowed-create-secret="checkPermission(['AbpIdentityServer.ApiResources.ManageSecrets'])"
              :allowed-delete-secret="checkPermission(['AbpIdentityServer.ApiResources.ManageSecrets'])"
            />
          </el-tab-pane>
        </el-tabs>

        <el-form-item>
          <el-button
            class="cancel"
            type="info"
            @click="onCancel"
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
    </div>
  </el-dialog>
</template>

<script lang="ts">
import ApiResourceService, {
  ApiResource,
  ApiResourceCreate,
  ApiResourceUpdate,
  ApiResourceCreateOrUpdate
} from '@/api/api-resources'
import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'
import ClaimTypeApiService from '@/api/cliam-type'
import IdentityServer4Service from '@/api/identity-server4'
import { checkPermission } from '@/utils/permission'
import { dateFormat } from '@/utils/index'
import { Claim } from '@/api/types'

import ScopeEditForm from '../../components/ScopeEditForm.vue'
import PropertiesEditForm from '../../components/PropertiesEditForm.vue'
import SecretEditForm from '../../components/SecretEditForm.vue'
import UserClaimEditForm from '../../components/UserClaimEditForm.vue'

@Component({
  name: 'ApiResourceCreateOrEditForm',
  components: {
    ScopeEditForm,
    SecretEditForm,
    UserClaimEditForm,
    PropertiesEditForm
  },
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
export default class extends Mixins(LocalizationMiXin) {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: '' })
  private apiResourceId!: string

  private apiResource = new ApiResource()
  private apiResourceClaims = new Array<Claim>()
  private apiScopes = new Array<string>()

  private activeTabPane = 'basics'
  private advancedPropName = ''
  private advancedComponent = 'secret-edit-form'
  private blockSwitchTabPane = ['advanced']

  get isEdit() {
    if (this.apiResourceId) {
      return true
    }
    return false
  }

  get title() {
    if (this.isEdit) {
      return this.$t('AbpIdentityServer.Resource:Name', { 0: this.apiResource.displayName || this.apiResource.name })
    }
    return this.$t('AbpIdentityServer.Resource:New')
  }

  @Watch('showDialog', { immediate: true })
  private onShowDialogChanged() {
    this.handleGetApiResource()
  }

  mounted() {
    this.handleGetClaimTypes()
  }

  private handleGetClaimTypes() {
    ClaimTypeApiService
      .getActivedClaimTypes()
      .then(res => {
        res.items.map(claim => {
          this.apiResourceClaims.push(new Claim(claim.name, claim.name))
        })
      })
    IdentityServer4Service
      .getOpenIdConfiguration()
      .then(res => {
        this.apiScopes = res.scopes_supported
      })
  }

  private handleGetApiResource() {
    this.activeTabPane = 'basics'
    if (this.apiResourceId && this.showDialog) {
      ApiResourceService
        .get(this.apiResourceId)
        .then(res => {
          this.apiResource = res
        })
    } else {
      this.apiResource = new ApiResource()
    }
  }

  private onTabBeforeLeave(activeName: string) {
    return !this.blockSwitchTabPane.some(name => name === activeName)
  }

  private switchTabPane(tabPaneName: string) {
    const tabPaneIndex = this.blockSwitchTabPane.findIndex(name => name === tabPaneName)
    if (tabPaneIndex >= 0) {
      this.blockSwitchTabPane.splice(tabPaneIndex, 1)
    }
    this.activeTabPane = tabPaneName
  }

  private onDropdownMenuItemChanged(command: any) {
    this.advancedPropName = command.prop
    this.advancedComponent = command.component
    this.switchTabPane('advanced')
  }

  private onSave() {
    const frmApiResource = this.$refs.formApiResource as any
    frmApiResource.validate((valid: boolean) => {
      if (valid) {
        if (this.isEdit) {
          const input = new ApiResourceUpdate()
          this.updateByInput(input)
          ApiResourceService
            .update(this.apiResourceId, input)
            .then(resource => {
              this.apiResource = resource
              const successMessage = this.l('global.successful')
              this.$message.success(successMessage)
              this.onFormClosed(true)
            })
        } else {
          const input = new ApiResourceCreate()
          this.updateByInput(input)
          input.name = this.apiResource.name
          ApiResourceService
            .create(input)
            .then(resource => {
              this.apiResource = resource
              const successMessage = this.l('global.successful')
              this.$message.success(successMessage)
              this.onFormClosed(true)
            })
        }
      }
    })
  }

  private updateByInput(input: ApiResourceCreateOrUpdate) {
    input.displayName = this.apiResource.displayName
    input.description = this.apiResource.description
    input.enabled = this.apiResource.enabled
    input.userClaims = this.apiResource.userClaims
    input.scopes = this.apiResource.scopes
    input.secrets = this.apiResource.secrets
    input.properties = this.apiResource.properties
    input.showInDiscoveryDocument = this.apiResource.showInDiscoveryDocument
    input.allowedAccessTokenSigningAlgorithms = this.apiResource.allowedAccessTokenSigningAlgorithms
  }

  private onFormClosed(changed: boolean) {
    this.resetFields()
    this.$emit('closed', changed)
  }

  private onCancel() {
    this.onFormClosed(false)
  }

  public resetFields() {
    const frmApiResource = this.$refs.formApiResource as any
    frmApiResource.resetFields()
  }
}
</script>

<style lang="scss" scoped>
.confirm {
  position: absolute;
  right: 10px;
  top: 20px;
  width:100px;
}
.cancel {
  position: absolute;
  right: 120px;
  top: 20px;
  width:100px;
}
.full-select {
  width: 100%;
}
.transfer-scope ::v-deep .el-transfer-panel{
  width: 250px;
}
</style>
