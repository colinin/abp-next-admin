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
        label-width="120px"
        :model="identityResource"
        :rules="identityResourceRules"
      >
        <el-form-item
          prop="enabled"
          :label="$t('identityServer.enabledResource')"
        >
          <el-switch
            v-model="identityResource.enabled"
          />
        </el-form-item>
        <el-form-item
          prop="name"
          :label="$t('identityServer.resourceName')"
        >
          <el-input
            v-model="identityResource.name"
            :disabled="isEdit"
            :placeholder="$t('pleaseInputBy', {key: $t('identityServer.resourceName')})"
          />
        </el-form-item>
        <el-form-item
          prop="displayName"
          :label="$t('identityServer.resourceDisplayName')"
        >
          <el-input
            v-model="identityResource.displayName"
          />
        </el-form-item>
        <el-form-item
          prop="description"
          :label="$t('identityServer.resourceDescription')"
        >
          <el-input
            v-model="identityResource.description"
          />
        </el-form-item>
        <el-form-item
          prop="required"
          :label="$t('identityServer.identityResourceRequired')"
        >
          <el-switch
            v-model="identityResource.required"
          />
        </el-form-item>
        <el-form-item
          prop="emphasize"
          :label="$t('identityServer.identityResourceEmphasize')"
        >
          <el-switch
            v-model="identityResource.emphasize"
          />
        </el-form-item>
        <el-form-item
          prop="showInDiscoveryDocument"
          :label="$t('identityServer.identityResourceShowInDiscoveryDocument')"
        >
          <el-switch
            v-model="identityResource.showInDiscoveryDocument"
          />
        </el-form-item>
        <el-form-item
          prop="userClaims"
          :label="$t('identityServer.resourceUserClaims')"
        >
          <el-select
            v-model="identityResource.userClaims"
            multiple
            style="width: 100%;"
            value-key="type"
          >
            <el-option
              v-for="claim in identityClaims"
              :key="claim.type"
              :label="claim.type"
              :value="claim"
            />
          </el-select>
        </el-form-item>

        <el-form-item>
          <el-button
            class="cancel"
            style="width:100px"
            @click="onCancel"
          >
            {{ $t('global.cancel') }}
          </el-button>
          <el-button
            class="confirm"
            type="primary"
            style="width:100px"
            @click="onSaveIdentityResource"
          >
            {{ $t('global.confirm') }}
          </el-button>
        </el-form-item>
      </el-form>
    </div>
  </el-dialog>
</template>

<script lang="ts">
import IdentityResourceService, { IdentityResourceCreate, IdentityResourceUpdate, IdentityResource, IdentityClaim } from '@/api/identityresources'
import ClaimTypeApiService from '@/api/cliam-type'
import { Component, Prop, Vue, Watch } from 'vue-property-decorator'
import { Form } from 'element-ui'

@Component({
  name: 'IdentityResourceCreateOrEditForm'
})
export default class extends Vue {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: '' })
  private title!: string

  @Prop({ default: '' })
  private identityResourceId!: string

  private identityClaims = new Array<IdentityClaim>()
  private identityResource: IdentityResource
  private identityResourceRules = {
    name: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.resourceName') }), trigger: 'blur' }
    ]
  }

  get isEdit() {
    if (this.identityResource.id) {
      return true
    }
    return false
  }

  constructor() {
    super()
    this.identityResource = IdentityResource.empty()
  }

  @Watch('identityResourceId', { immediate: true })
  private onIdentityResourceIdChanged() {
    if (this.identityResourceId) {
      IdentityResourceService.getIdentityResourceById(this.identityResourceId).then(resource => {
        this.identityResource = resource
      })
    } else {
      this.identityResource = IdentityResource.empty()
    }
  }

  mounted() {
    this.handleGetClaimTypes()
  }

  private handleGetClaimTypes() {
    ClaimTypeApiService.getActivedClaimTypes().then(res => {
      res.items.map(claim => {
        const identityClaim = new IdentityClaim()
        identityClaim.type = claim.name
        this.identityClaims.push(identityClaim)
      })
    })
  }

  private onSaveIdentityResource() {
    const frmIdentityResource = this.$refs.formIdentityResource as any
    frmIdentityResource.validate((valid: boolean) => {
      if (valid) {
        if (this.isEdit) {
          const updateIdentityResource = IdentityResourceUpdate.create(this.identityResource)
          IdentityResourceService.updateIdentityResource(updateIdentityResource).then(resource => {
            this.identityResource = resource
            const successMessage = this.l('identityServer.updateIdentityResourceSuccess', { name: resource.name })
            this.$message.success(successMessage)
            this.onFormClosed(true)
          })
        } else {
          const createIdentityResource = IdentityResourceCreate.create(this.identityResource)
          IdentityResourceService.createIdentityResource(createIdentityResource).then(resource => {
            this.identityResource = resource
            const successMessage = this.l('identityServer.createIdentityResourceSuccess', { name: resource.name })
            this.$message.success(successMessage)
            this.onFormClosed(true)
          })
        }
      }
    })
  }

  private onFormClosed(changed: boolean) {
    this.resetFields()
    this.$emit('closed', changed)
  }

  private onCancel() {
    this.onFormClosed(false)
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
}
.cancel {
  position: absolute;
  right: 120px;
}
</style>
