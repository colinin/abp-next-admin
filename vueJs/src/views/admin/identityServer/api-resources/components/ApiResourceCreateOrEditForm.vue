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
        <el-form-item
          prop="enabled"
          :label="$t('identityServer.enabledResource')"
        >
          <el-switch
            v-model="apiResource.enabled"
          />
        </el-form-item>
        <el-form-item
          prop="name"
          :label="$t('identityServer.resourceName')"
        >
          <el-input
            v-model="apiResource.name"
            :readonly="isEdit"
            :placeholder="$t('pleaseInputBy', {key: $t('identityServer.resourceName')})"
          />
        </el-form-item>
        <el-form-item
          prop="displayName"
          :label="$t('identityServer.resourceDisplayName')"
        >
          <el-input
            v-model="apiResource.displayName"
          />
        </el-form-item>
        <el-form-item
          prop="description"
          :label="$t('identityServer.resourceDescription')"
        >
          <el-input
            v-model="apiResource.description"
          />
        </el-form-item>
        <el-form-item
          prop="userClaims"
          :label="$t('identityServer.resourceUserClaims')"
        >
          <el-input-tag-ex
            v-model="apiResource.userClaims"
            label="type"
          />
        </el-form-item>

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
import ApiResourceService, { ApiResourceCreate, ApiResourceUpdate, ApiResource } from '@/api/apiresources'
import { Component, Prop, Vue, Watch } from 'vue-property-decorator'
import ElInputTagEx from '@/components/InputTagEx/index.vue'

@Component({
  name: 'ApiResourceCreateOrEditForm',
  components: {
    ElInputTagEx
  }
})
export default class extends Vue {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: '' })
  private title!: string

  @Prop({ default: '' })
  private apiResourceId!: string

  private apiResource: ApiResource
  private apiResourceRules = {
    name: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.resourceName') }), trigger: 'blur' }
    ]
  }

  get isEdit() {
    if (this.apiResource.id) {
      return true
    }
    return false
  }

  constructor() {
    super()
    this.apiResource = ApiResource.empty()
  }

  @Watch('apiResourceId', { immediate: true })
  private onApiResourceIdChanged() {
    if (this.apiResourceId) {
      ApiResourceService.getApiResourceById(this.apiResourceId).then(resource => {
        this.apiResource = resource
      })
    } else {
      this.apiResource = ApiResource.empty()
    }
  }

  private onSaveApiResource() {
    const frmApiResource = this.$refs.formApiResource as any
    frmApiResource.validate((valid: boolean) => {
      if (valid) {
        if (this.isEdit) {
          const updateApiResource = ApiResourceUpdate.create(this.apiResource)
          ApiResourceService.updateApiResource(updateApiResource).then(resource => {
            this.apiResource = resource
            const successMessage = this.l('identityServer.updateApiResourceSuccess', { name: resource.name })
            this.$message.success(successMessage)
            this.onFormClosed(true)
          })
        } else {
          const createApiResource = ApiResourceCreate.create(this.apiResource)
          ApiResourceService.createApiResource(createApiResource).then(resource => {
            this.apiResource = resource
            const successMessage = this.l('identityServer.createApiResourceSuccess', { name: resource.name })
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
}
.cancel {
  position: absolute;
  right: 120px;
}
</style>
