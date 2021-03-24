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
        ref="formApiScope"
        label-width="130px"
        :model="apiScope"
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
                v-model="apiScope.enabled"
              />
            </el-form-item>
            <el-form-item
              prop="showInDiscoveryDocument"
              :label="$t('AbpIdentityServer.ShowInDiscoveryDocument')"
            >
              <el-switch
                v-model="apiScope.showInDiscoveryDocument"
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
                v-model="apiScope.name"
                :readonly="isEdit"
                :placeholder="$t('pleaseInputBy', {key: $t('AbpIdentityServer.Name')})"
              />
            </el-form-item>
            <el-form-item
              prop="displayName"
              :label="$t('AbpIdentityServer.DisplayName')"
            >
              <el-input
                v-model="apiScope.displayName"
              />
            </el-form-item>
            <el-form-item
              prop="description"
              :label="$t('AbpIdentityServer.Description')"
            >
              <el-input
                v-model="apiScope.description"
              />
            </el-form-item>
          </el-tab-pane>
          <el-tab-pane
            name="userClaim"
            :label="$t('AbpIdentityServer.UserClaim')"
          >
            <user-claim-edit-form
              v-model="apiScope.userClaims"
            />
          </el-tab-pane>
          <el-tab-pane
            v-if="isEdit"
            name="properties"
            :label="$t('AbpIdentityServer.Propertites')"
          >
            <properties-edit-form
              v-model="apiScope.properties"
              :allowed-create-prop="checkPermission(['AbpIdentityServer.ApiScopes.ManageProperties'])"
              :allowed-delete-prop="checkPermission(['AbpIdentityServer.ApiScopes.ManageProperties'])"
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
import ApiScopeService, {
  ApiScope,
  ApiScopeCreate,
  ApiScopeUpdate,
  ApiScopeCreateOrUpdate
} from '@/api/api-scopes'
import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'
import { checkPermission } from '@/utils/permission'
import { dateFormat } from '@/utils/index'
import PropertiesEditForm from '../../components/PropertiesEditForm.vue'
import UserClaimEditForm from '../../components/UserClaimEditForm.vue'

@Component({
  name: 'ApiScopeCreateOrEditForm',
  components: {
    PropertiesEditForm,
    UserClaimEditForm
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
  private id!: string

  private activeTabPane = 'basics'
  private apiScope = new ApiScope()

  get isEdit() {
    if (this.id) {
      return true
    }
    return false
  }

  get title() {
    if (this.isEdit) {
      return '编辑 - ' + this.apiScope.displayName || this.apiScope.name
    }
    return '新增 ApiScope'
  }

  @Watch('showDialog', { immediate: true })
  private onShowDialogChanged() {
    this.handleGetApiResource()
  }

  private handleGetApiResource() {
    this.activeTabPane = 'basics'
    if (this.id && this.showDialog) {
      ApiScopeService
        .get(this.id)
        .then(res => {
          this.apiScope = res
        })
    } else {
      this.apiScope = new ApiScope()
    }
  }

  private onSave() {
    const formApiScope = this.$refs.formApiScope as any
    formApiScope.validate((valid: boolean) => {
      if (valid) {
        if (this.isEdit) {
          const input = new ApiScopeUpdate()
          this.updateByInput(input)
          ApiScopeService
            .update(this.id, input)
            .then(res => {
              this.apiScope = res
              const successMessage = this.l('global.successful')
              this.$message.success(successMessage)
              this.onFormClosed(true)
            })
        } else {
          const input = new ApiScopeCreate()
          this.updateByInput(input)
          input.name = this.apiScope.name
          ApiScopeService
            .create(input)
            .then(res => {
              this.apiScope = res
              const successMessage = this.l('global.successful')
              this.$message.success(successMessage)
              this.onFormClosed(true)
            })
        }
      }
    })
  }

  private updateByInput(input: ApiScopeCreateOrUpdate) {
    input.displayName = this.apiScope.displayName
    input.description = this.apiScope.description
    input.enabled = this.apiScope.enabled
    input.required = this.apiScope.required
    input.emphasize = this.apiScope.emphasize
    input.userClaims = this.apiScope.userClaims
    input.properties = this.apiScope.properties
    input.showInDiscoveryDocument = this.apiScope.showInDiscoveryDocument
  }

  private onFormClosed(changed: boolean) {
    this.resetFields()
    this.$emit('closed', changed)
  }

  private onCancel() {
    this.onFormClosed(false)
  }

  private onUserClaimChanged(value: any) {
    this.apiScope.userClaims = value
  }

  public resetFields() {
    const formApiScope = this.$refs.formApiScope as any
    formApiScope.resetFields()
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
