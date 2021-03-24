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
        ref="formIdentityResource"
        label-width="130px"
        :model="identityResource"
      >
        <el-tabs
          v-model="activeTable"
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
                v-model="identityResource.enabled"
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
                v-model="identityResource.name"
                :disabled="isEdit"
                :placeholder="$t('pleaseInputBy', {key: $t('AbpIdentityServer.Name')})"
              />
            </el-form-item>
            <el-form-item
              prop="displayName"
              :label="$t('AbpIdentityServer.DisplayName')"
            >
              <el-input
                v-model="identityResource.displayName"
              />
            </el-form-item>
            <el-form-item
              prop="description"
              :label="$t('AbpIdentityServer.Description')"
            >
              <el-input
                v-model="identityResource.description"
              />
            </el-form-item>
            <el-form-item
              prop="required"
              :label="$t('AbpIdentityServer.Required')"
            >
              <el-switch
                v-model="identityResource.required"
              />
            </el-form-item>
            <el-form-item
              prop="emphasize"
              :label="$t('AbpIdentityServer.Emphasize')"
            >
              <el-switch
                v-model="identityResource.emphasize"
              />
            </el-form-item>
            <el-form-item
              prop="showInDiscoveryDocument"
              :label="$t('AbpIdentityServer.ShowInDiscoveryDocument')"
            >
              <el-switch
                v-model="identityResource.showInDiscoveryDocument"
              />
            </el-form-item>
          </el-tab-pane>
          <el-tab-pane
            name="claims"
            :label="$t('AbpIdentityServer.UserClaim')"
          >
            <user-claim-edit-form
              v-model="identityResource.userClaims"
            />
          </el-tab-pane>
          <el-tab-pane
            v-if="isEdit"
            name="properties"
            :label="$t('AbpIdentityServer.Propertites')"
          >
            <properties-edit-form
              v-model="identityResource.properties"
              :allowed-create-prop="checkPermission(['AbpIdentityServer.IdentityResources.ManageProperties'])"
              :allowed-delete-prop="checkPermission(['AbpIdentityServer.IdentityResources.ManageProperties'])"
            />
          </el-tab-pane>
        </el-tabs>

        <el-form-item>
          <el-button
            class="cancel"
            type="info"
            @click="onFormClosed(false)"
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
import { checkPermission } from '@/utils/permission'

import { Form } from 'element-ui'
import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'

import PropertiesEditForm from '../../components/PropertiesEditForm.vue'
import UserClaimEditForm from '../../components/UserClaimEditForm.vue'
import IdentityResourceService, {
  IdentityResource,
  IdentityResourceCreateOrUpdate
} from '@/api/identity-resources'

@Component({
  name: 'IdentityResourceCreateOrEditForm',
  components: {
    UserClaimEditForm,
    PropertiesEditForm
  },
  methods: {
    checkPermission
  }
})
export default class extends Mixins(LocalizationMiXin) {
  @Prop({ default: false })
  private showDialog!: ConstrainBoolean

  @Prop({ default: '' })
  private id!: string

  private activeTable = 'basics'
  private identityResource = new IdentityResource()

  get isEdit() {
    if (this.id) {
      return true
    }
    return false
  }

  get title() {
    if (this.isEdit) {
      return '编辑资源 - ' + this.identityResource.displayName || this.identityResource.name
    }
    return '新增资源'
  }

  @Watch('showDialog', { immediate: true })
  private onShowDialogChanged() {
    this.handleGetIdentityResource()
  }

  private handleGetIdentityResource() {
    this.activeTable = 'basics'
    if (this.showDialog && this.id) {
      IdentityResourceService
        .get(this.id)
        .then(resource => {
          this.identityResource = resource
        })
    } else {
      this.identityResource = new IdentityResource()
    }
  }

  private onSave() {
    const frmIdentityResource = this.$refs.formIdentityResource as any
    frmIdentityResource.validate((valid: boolean) => {
      if (valid) {
        const input = new IdentityResourceCreateOrUpdate()
        this.updateByInput(input)
        if (this.isEdit) {
          IdentityResourceService
            .update(this.id, input)
            .then(resource => {
              this.identityResource = resource
              const successMessage = this.l('global.successful')
              this.$message.success(successMessage)
              this.onFormClosed(true)
            })
        } else {
          IdentityResourceService
            .create(input)
            .then(resource => {
              this.identityResource = resource
              const successMessage = this.l('global.successful')
              this.$message.success(successMessage)
              this.onFormClosed(true)
            })
        }
      }
    })
  }

  private updateByInput(input: IdentityResourceCreateOrUpdate) {
    input.name = this.identityResource.name
    input.displayName = this.identityResource.displayName
    input.description = this.identityResource.description
    input.enabled = this.identityResource.enabled
    input.required = this.identityResource.required
    input.emphasize = this.identityResource.emphasize
    input.showInDiscoveryDocument = this.identityResource.showInDiscoveryDocument
    input.userClaims = this.identityResource.userClaims
    input.properties = this.identityResource.properties
  }

  private onFormClosed(changed: boolean) {
    this.resetFields()
    this.$emit('closed', changed)
  }

  private resetFields() {
    const frmIdentityResource = this.$refs.formIdentityResource as Form
    frmIdentityResource.resetFields()
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
.transfer-scope ::v-deep .el-transfer-panel{
  width: 250px;
}
</style>
