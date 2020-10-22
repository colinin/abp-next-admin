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
        label-width="100px"
        :model="apiResource"
      >
        <el-tabs
          v-model="activeTabPane"
          type="border-card"
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
          </el-tab-pane>
          <el-tab-pane
            name="userClaim"
            :label="$t('AbpIdentityServer.UserClaim')"
          >
            <el-transfer
              v-model="apiResource.userClaims"
              class="transfer-scope"
              :data="apiResourceClaims"
              :props="{
                key: 'type',
                label: 'value'
              }"
              :titles="[$t('AbpIdentityServer.NoClaim'), $t('AbpIdentityServer.ExistsClaim')]"
            />
          </el-tab-pane>
          <el-tab-pane
            v-if="isEdit"
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
                <el-dropdown-item command="api-resource-scope-edit-form">
                  {{ $t('AbpIdentityServer.Scope') }}
                </el-dropdown-item>
                <el-dropdown-item command="secret-edit-form">
                  {{ $t('AbpIdentityServer.Secret') }}
                </el-dropdown-item>
                <el-dropdown-item command="properties-edit-form">
                  {{ $t('AbpIdentityServer.Propertites') }}
                </el-dropdown-item>
              </el-dropdown-menu>
            </el-dropdown>
            <component
              :is="advancedComponent"
              :user-claims="apiResourceClaims"
              :api-resource-scopes="apiResource.scopes"
              :secrets="apiResource.secrets"
              :allowed-create-prop="checkPermission(['AbpIdentityServer.ApiResources.ManageProperties'])"
              :allowed-delete-prop="checkPermission(['AbpIdentityServer.ApiResources.ManageProperties'])"
              :allowed-create-secret="checkPermission(['AbpIdentityServer.ApiResources.ManageSecrets'])"
              :allowed-delete-secret="checkPermission(['AbpIdentityServer.ApiResources.ManageSecrets'])"
              :properties="apiResource.properties"
              @onScopeCreated="apiResourceScopeCreated"
              @onScopeDeleted="apiResourceScopeDeleted"
              @onSecretCreated="apiResourceSecretCreated"
              @onSecretDeleted="apiResourceSecretDeleted"
              @onCreated="onPropertyCreated"
              @onDeleted="onPropertyDeleted"
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
  ApiScope,
  ApiResource,
  ApiResourceCreate,
  ApiResourceUpdate,
  ApiSecretCreateOrUpdate,
  ApiResourceCreateOrUpdate
} from '@/api/api-resources'
import { Component, Prop, Vue, Watch } from 'vue-property-decorator'
import ClaimTypeApiService from '@/api/cliam-type'
import { checkPermission } from '@/utils/permission'
import { dateFormat } from '@/utils/index'
import { Claim } from '@/api/types'
import ApiResourceScopeEditForm from './ApiResourceScopeEditForm.vue'
import PropertiesEditForm from '../../components/PropertiesEditForm.vue'
import SecretEditForm from '../../components/SecretEditForm.vue'

@Component({
  name: 'ApiResourceCreateOrEditForm',
  components: {
    SecretEditForm,
    PropertiesEditForm,
    ApiResourceScopeEditForm
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
export default class extends Vue {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: '' })
  private title!: string

  @Prop({ default: '' })
  private apiResourceId!: string

  private activeTabPane = 'basics'
  private advancedComponent = 'api-resource-scope-edit-form'
  private apiResource = new ApiResource()
  private apiResourceClaims = new Array<Claim>()
  private newApiSecret = new ApiSecretCreateOrUpdate()

  get isEdit() {
    if (this.apiResourceId) {
      return true
    }
    return false
  }

  @Watch('showDialog', { immediate: true })
  private onShowDialogChanged() {
    this.handleGetApiResource()
  }

  mounted() {
    this.handleGetClaimTypes()
  }

  private handleGetClaimTypes() {
    ClaimTypeApiService.getActivedClaimTypes().then(res => {
      res.items.map(claim => {
        this.apiResourceClaims.push(new Claim(claim.name, claim.name))
      })
    })
  }

  private handleGetApiResource() {
    this.activeTabPane = 'basics'
    if (this.apiResourceId && this.showDialog) {
      ApiResourceService.getApiResourceById(this.apiResourceId).then(res => {
        this.apiResource = res
      })
    } else {
      this.apiResource = new ApiResource()
    }
  }

  private apiResourceSecretCreated(secret: any) {
    const apiSecret = new ApiSecretCreateOrUpdate()
    apiSecret.hashType = secret.hashType
    apiSecret.type = secret.type
    apiSecret.value = secret.value
    apiSecret.description = secret.description
    apiSecret.expiration = secret.expiration
    this.apiResource.secrets.push(apiSecret)
  }

  private apiResourceSecretDeleted(type: string, value: string) {
    const secretIndex = this.apiResource.secrets.findIndex(secret => secret.type === type && secret.value === value)
    this.apiResource.secrets.splice(secretIndex, 1)
  }

  private apiResourceScopeCreated(name: string, required: boolean, emphasize: boolean, showInDiscoveryDocument: boolean,
    userClaims: string[], displayName?: string, description?: string) {
    const apiScope = new ApiScope()
    apiScope.name = name
    apiScope.displayName = displayName
    apiScope.description = description
    apiScope.required = required
    apiScope.emphasize = emphasize
    apiScope.showInDiscoveryDocument = showInDiscoveryDocument
    apiScope.userClaims.push(...userClaims)
    this.apiResource.scopes.push(apiScope)
  }

  private apiResourceScopeDeleted(name: string) {
    const scopeIndex = this.apiResource.scopes.findIndex(scope => scope.name === name)
    this.apiResource.scopes.splice(scopeIndex, 1)
  }

  private onDropdownMenuItemChanged(component: any) {
    this.activeTabPane = 'avanced'
    this.advancedComponent = component
  }

  private onSave() {
    const frmApiResource = this.$refs.formApiResource as any
    frmApiResource.validate((valid: boolean) => {
      if (valid) {
        if (this.isEdit) {
          const updateApiResource = new ApiResourceUpdate()
          this.updateApiResourceByInput(updateApiResource)
          ApiResourceService.updateApiResource(this.apiResourceId, updateApiResource).then(resource => {
            this.apiResource = resource
            const successMessage = this.l('global.successful')
            this.$message.success(successMessage)
            this.onFormClosed(true)
          })
        } else {
          const createApiResource = new ApiResourceCreate()
          this.updateApiResourceByInput(createApiResource)
          createApiResource.name = this.apiResource.name
          ApiResourceService.createApiResource(createApiResource).then(resource => {
            this.apiResource = resource
            const successMessage = this.l('global.successful')
            this.$message.success(successMessage)
            this.onFormClosed(true)
          })
        }
      }
    })
  }

  private updateApiResourceByInput(apiResource: ApiResourceCreateOrUpdate) {
    apiResource.displayName = this.apiResource.displayName
    apiResource.description = this.apiResource.description
    apiResource.enabled = this.apiResource.enabled
    apiResource.userClaims = this.apiResource.userClaims
    apiResource.scopes = this.apiResource.scopes
    apiResource.secrets = this.apiResource.secrets
    apiResource.properties = this.apiResource.properties
  }

  private onFormClosed(changed: boolean) {
    this.resetFields()
    this.$emit('closed', changed)
  }

  private onCancel() {
    this.onFormClosed(false)
  }

  private onPropertyCreated(key: string, value: string) {
    this.$set(this.apiResource.properties, key, value)
  }

  private onPropertyDeleted(key: string) {
    this.$delete(this.apiResource.properties, key)
  }

  public resetFields() {
    const frmApiResource = this.$refs.formApiResource as any
    frmApiResource.resetFields()
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
