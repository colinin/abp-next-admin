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
      ref="formResource"
      :model="resource"
      label-width="120px"
    >
      <el-form-item
        prop="enable"
        :label="$t(('LocalizationManagement.DisplayName:Enable'))"
      >
        <el-switch
          v-model="resource.enable"
        />
      </el-form-item>
      <el-form-item
        prop="name"
        :label="$t(('LocalizationManagement.DisplayName:Name'))"
        :rules="{
          required: true,
          message: $t('pleaseInputBy', {key: $t('LocalizationManagement.DisplayName:Name')}),
          trigger: 'blur'
        }"
      >
        <el-input
          v-model="resource.name"
        />
      </el-form-item>
      <el-form-item
        prop="displayName"
        :label="$t(('LocalizationManagement.DisplayName:DisplayName'))"
        :rules="{
          required: true,
          message: $t('pleaseInputBy', {key: $t('LocalizationManagement.DisplayName:DisplayName')}),
          trigger: 'blur'
        }"
      >
        <el-input
          v-model="resource.displayName"
        />
      </el-form-item>
      <el-form-item
        prop="description"
        :label="$t(('LocalizationManagement.DisplayName:Description'))"
      >
        <el-input
          v-model="resource.description"
        />
      </el-form-item>

      <el-form-item>
        <el-button
          class="cancel"
          type="info"
          @click="onFormClosed(false)"
        >
          {{ $t('AbpUi.Cancel') }}
        </el-button>
        <el-button
          class="confirm"
          type="primary"
          icon="el-icon-check"
          @click="onSave"
        >
          {{ $t('AbpUi.Save') }}
        </el-button>
      </el-form-item>
    </el-form>
  </el-dialog>
</template>

<script lang="ts">
import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'
import HttpProxyMiXin from '@/mixins/HttpProxyMiXin'

import {
  service,
  controller,
  Resource,
  CreateOrUpdateResourceInput
} from '../types'

import { Form } from 'element-ui'

@Component({
  name: 'ResourceDialog'
})
export default class ResourceDialog extends Mixins(LocalizationMiXin, HttpProxyMiXin) {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: null })
  private resourceId!: string

  get isEdit() {
    if (this.resourceId) {
      return true
    }
    return false
  }

  get title() {
    if (this.isEdit) {
      return this.l('LocalizationManagement.EditByName', { 0: this.resource.displayName })
    }
    return this.l('LocalizationManagement.Resource:AddNew')
  }

  private resource = new Resource()

  @Watch('showDialog')
  private onShowDialogChanged() {
    this.handleGetResource()
  }

  private handleGetResource() {
    if (this.showDialog && this.resourceId) {
      this.request<Resource>({
        service: service,
        controller: controller,
        action: 'GetAsync',
        params: {
          id: this.resourceId
        }
      }).then(res => {
        this.resource = res
      })
    } else {
      this.resource = new Resource()
    }
  }

  private onSave() {
    const formResource = this.$refs.formResource as Form
    formResource
      .validate(valid => {
        if (valid) {
          const action = this.isEdit ? 'UpdateAsync' : 'CreateAsync'
          const params = this.isEdit ? { id: this.resourceId } : {}
          const input = new CreateOrUpdateResourceInput()
          this.updateByInput(input)
          this.request<Resource>({
            service: service,
            controller: controller,
            action: action,
            data: input,
            params: params
          }).then(res => {
            this.resource = res
            this.$message.success(this.l('successful'))
            this.onFormClosed(true)
          })
        }
      })
  }

  private onFormClosed(changed: boolean) {
    const formResource = this.$refs.formResource as Form
    formResource.resetFields()
    this.$emit('closed', changed)
  }

  private updateByInput(resource: CreateOrUpdateResourceInput) {
    resource.enable = this.resource.enable
    resource.name = this.resource.name
    resource.displayName = this.resource.displayName
    resource.description = this.resource.description
  }
}
</script>

<style scoped>
.confirm {
  position: absolute;
  right: 10px;
  width:100px;
}
.cancel {
  position: absolute;
  right: 120px;
  width:100px;
}
</style>
