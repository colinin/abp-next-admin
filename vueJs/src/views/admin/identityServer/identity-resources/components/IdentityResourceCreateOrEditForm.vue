<template>
  <div class="app-container">
    <div class="filter-container">
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
            :readonly="isEdit"
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
          <el-input-tag-ex
            v-model="identityResource.userClaims"
            label="type"
          />
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
  </div>
</template>

<script lang="ts">
import IdentityResourceService, { IdentityResourceCreate, IdentityResourceUpdate, IdentityResource } from '@/api/identityresources'
import { Component, Prop, Vue, Watch } from 'vue-property-decorator'
import ElInputTagEx from '@/components/InputTagEx/index.vue'

@Component({
  name: 'IdentityResourceCreateOrEditForm',
  components: {
    ElInputTagEx
  }
})
export default class extends Vue {
  @Prop({ default: '' })
  private identityResourceId!: string

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
            frmIdentityResource.resetFields()
            this.$emit('closed', true)
          })
        } else {
          const createIdentityResource = IdentityResourceCreate.create(this.identityResource)
          IdentityResourceService.createIdentityResource(createIdentityResource).then(resource => {
            this.identityResource = resource
            const successMessage = this.l('identityServer.createIdentityResourceSuccess', { name: resource.name })
            this.$message.success(successMessage)
            this.resetFields()
            this.$emit('closed', true)
          })
        }
      }
    })
  }

  private onCancel() {
    this.resetFields()
    this.$emit('closed', false)
  }

  public resetFields() {
    this.identityResource = IdentityResource.empty()
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
