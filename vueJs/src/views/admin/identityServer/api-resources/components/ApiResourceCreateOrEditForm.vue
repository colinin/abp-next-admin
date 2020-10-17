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
        :rules="apiResourceRules"
      >
        <el-tabs
          v-model="activeTable"
          type="border-card"
        >
          <el-tab-pane
            name="infomation"
            :label="$t('AbpIdentityServer.Information')"
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
            name="scopes"
            :label="$t('AbpIdentityServer.Scope')"
          >
            <api-resource-scope-edit-form
              :user-claims="apiResourceClaims"
              :api-resource-scopes="apiResource.scopes"
              @apiResourceScopeCreated="apiResourceScopeCreated"
              @apiResourceScopeDeleted="apiResourceScopeDeleted"
            />
          </el-tab-pane>
          <el-tab-pane
            v-if="isEdit"
            name="secrets"
            :label="$t('AbpIdentityServer.Secret')"
          >
            <el-card>
              <api-resource-secret-edit-form
                :api-resource-secrets="apiResource.secrets"
                @apiResourceSecretCreated="apiResourceSecretCreated"
                @apiResourceSecretDeleted="apiResourceSecretDeleted"
              />
            </el-card>
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
            @click="onSaveApiResource"
          >
            {{ $t('table.confirm') }}
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
  ApiResourceCreateOrUpdate, HashType
} from '@/api/api-resources'
import { Component, Prop, Vue, Watch } from 'vue-property-decorator'
import ClaimTypeApiService from '@/api/cliam-type'
import { checkPermission } from '@/utils/permission'
import { dateFormat } from '@/utils/index'
import { Claim } from '@/api/types'
import ApiResourceSecretEditForm from './ApiResourceSecretEditForm.vue'
import ApiResourceScopeEditForm from './ApiResourceScopeEditForm.vue'

@Component({
  name: 'ApiResourceCreateOrEditForm',
  components: {
    ApiResourceScopeEditForm,
    ApiResourceSecretEditForm
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

  private activeTable = 'infomation'
  private apiResource = new ApiResource()
  private apiResourceClaims = new Array<Claim>()
  private newApiSecret = new ApiSecretCreateOrUpdate()
  private apiResourceRules = {
    name: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('AbpIdentityServer.Name') }), trigger: 'blur' }
    ]
  }

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
    this.activeTable = 'infomation'
    if (this.apiResourceId && this.showDialog) {
      ApiResourceService.getApiResourceById(this.apiResourceId).then(res => {
        this.apiResource = res
      })
    } else {
      this.apiResource = new ApiResource()
    }
  }

  private apiResourceSecretCreated(hashType: HashType, type: string, value: string, description: string, expiration: Date | undefined) {
    const apiSecret = new ApiSecretCreateOrUpdate()
    apiSecret.hashType = hashType
    apiSecret.type = type
    apiSecret.value = value
    apiSecret.description = description
    apiSecret.expiration = expiration
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
    apiScope.userClaims = userClaims
    this.apiResource.scopes.push(apiScope)
  }

  private apiResourceScopeDeleted(name: string) {
    const scopeIndex = this.apiResource.scopes.findIndex(scope => scope.name === name)
    this.apiResource.scopes.splice(scopeIndex, 1)
  }

  private onSaveApiResource() {
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
}
.cancel {
  position: absolute;
  right: 120px;
  top: 20px;
}
.full-select {
  width: 100%;
}
.transfer-scope ::v-deep .el-transfer-panel{
  width: 250px;
}
</style>
