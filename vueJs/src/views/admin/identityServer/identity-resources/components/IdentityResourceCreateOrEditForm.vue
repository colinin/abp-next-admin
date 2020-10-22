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
            <el-transfer
              v-model="identityResource.userClaims"
              class="transfer-scope"
              :data="identityClaims"
              :props="{
                key: 'type',
                label: 'value'
              }"
              :titles="[$t('AbpIdentityServer.NoClaim'), $t('AbpIdentityServer.ExistsClaim')]"
            />
          </el-tab-pane>
          <el-tab-pane
            v-if="isEdit"
            name="properties"
            :label="$t('AbpIdentityServer.Propertites')"
          >
            <properties-edit-form
              :properties="identityResource.properties"
              :allowed-create="checkPermission(['AbpIdentityServer.IdentityResources.ManageProperties'])"
              :allowed-delete="checkPermission(['AbpIdentityServer.IdentityResources.ManageProperties'])"
              @onCreated="onPropertyCreated"
              @onDeleted="onPropertyDeleted"
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
import { Component, Prop, Vue, Watch } from 'vue-property-decorator'

import { Claim } from '@/api/types'
import ClaimTypeApiService from '@/api/cliam-type'

import PropertiesEditForm from '../../components/PropertiesEditForm.vue'
import IdentityResourceService, { IdentityResource, IdentityResourceCreateOrUpdate } from '@/api/identity-resources'

@Component({
  name: 'IdentityResourceCreateOrEditForm',
  components: {
    PropertiesEditForm
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
  private identityResourceId!: string

  private activeTable = 'basics'
  private identityClaims = new Array<Claim>()
  private identityResource = new IdentityResource()

  get isEdit() {
    if (this.identityResource.id) {
      return true
    }
    return false
  }

  @Watch('showDialog', { immediate: true })
  private onShowDialogChanged() {
    this.handleGetIdentityResource()
  }

  mounted() {
    this.handleGetClaimTypes()
  }

  private handleGetClaimTypes() {
    ClaimTypeApiService.getActivedClaimTypes().then(res => {
      res.items.map(claim => {
        const identityClaim = new Claim(claim.name, claim.name)
        this.identityClaims.push(identityClaim)
      })
    })
  }

  private handleGetIdentityResource() {
    this.activeTable = 'basics'
    if (this.showDialog && this.identityResourceId) {
      IdentityResourceService.getIdentityResourceById(this.identityResourceId).then(resource => {
        this.identityResource = resource
      })
    } else {
      this.identityResource = new IdentityResource()
    }
  }

  private onPropertyCreated(key: string, value: string) {
    this.$set(this.identityResource.properties, key, value)
  }

  private onPropertyDeleted(key: string) {
    this.$delete(this.identityResource.properties, key)
  }

  private onSave() {
    const frmIdentityResource = this.$refs.formIdentityResource as any
    frmIdentityResource.validate((valid: boolean) => {
      if (valid) {
        const editIdentityResource = new IdentityResourceCreateOrUpdate()
        this.updateIdentityResourceByInput(editIdentityResource)
        if (this.isEdit) {
          IdentityResourceService.updateIdentityResource(this.identityResourceId, editIdentityResource).then(resource => {
            this.identityResource = resource
            const successMessage = this.l('global.successful')
            this.$message.success(successMessage)
            this.onFormClosed(true)
          })
        } else {
          IdentityResourceService.createIdentityResource(editIdentityResource).then(resource => {
            this.identityResource = resource
            const successMessage = this.l('global.successful')
            this.$message.success(successMessage)
            this.onFormClosed(true)
          })
        }
      }
    })
  }

  private updateIdentityResourceByInput(identityResource: IdentityResourceCreateOrUpdate) {
    identityResource.name = this.identityResource.name
    identityResource.displayName = this.identityResource.displayName
    identityResource.description = this.identityResource.description
    identityResource.enabled = this.identityResource.enabled
    identityResource.required = this.identityResource.required
    identityResource.emphasize = this.identityResource.emphasize
    identityResource.showInDiscoveryDocument = this.identityResource.showInDiscoveryDocument
    identityResource.userClaims = this.identityResource.userClaims
    identityResource.properties = this.identityResource.properties
  }

  private onFormClosed(changed: boolean) {
    this.resetFields()
    this.$emit('closed', changed)
  }

  private resetFields() {
    const frmIdentityResource = this.$refs.formIdentityResource as Form
    frmIdentityResource.resetFields()
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
.transfer-scope ::v-deep .el-transfer-panel{
  width: 250px;
}
</style>
