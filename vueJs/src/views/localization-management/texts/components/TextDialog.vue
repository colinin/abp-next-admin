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
      ref="formText"
      :model="text"
      label-width="120px"
    >
      <el-form-item
        v-if="!isEdit"
        prop="resourceName"
        :label="$t(('LocalizationManagement.DisplayName:ResourceName'))"
        :rules="{
          required: true,
          message: $t('pleaseSelectBy', {key: $t('LocalizationManagement.DisplayName:ResourceName')}),
          trigger: 'blur'
        }"
      >
        <el-select
          v-model="text.resourceName"
          style="width: 100%;"
        >
          <el-option
            v-for="resource in resources"
            :key="resource.name"
            :label="resource.displayName"
            :value="resource.name"
          />
        </el-select>
      </el-form-item>
      <el-form-item
        prop="cultureName"
        :label="$t(('LocalizationManagement.DisplayName:CultureName'))"
        :rules="{
          required: true,
          message: $t('pleaseSelectBy', {key: $t('LocalizationManagement.DisplayName:CultureName')}),
          trigger: 'blur'
        }"
      >
        <el-select
          v-model="text.cultureName"
          style="width: 100%;"
          @change="onLanguageChanged"
        >
          <el-option
            v-for="language in languages"
            :key="language.cultureName"
            :label="language.displayName"
            :value="language.cultureName"
          />
        </el-select>
      </el-form-item>
      <el-form-item
        prop="key"
        :label="$t(('LocalizationManagement.DisplayName:Key'))"
        :rules="{
          required: true,
          message: $t('pleaseInputBy', {key: $t('LocalizationManagement.DisplayName:Key')}),
          trigger: 'blur'
        }"
      >
        <el-input
          v-model="text.key"
          :readonly="isEdit"
        />
      </el-form-item>
      <el-form-item
        prop="value"
        :label="$t(('LocalizationManagement.DisplayName:Value'))"
      >
        <el-input
          v-model="text.value"
          type="textarea"
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
          {{ saveButtonTitle }}
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
  Text,
  CreateTextInput,
  UpdateTextInput,
  CreateOrUpdateTextInput
} from '../types'
import { Language } from '../../languages/types'
import { Resource } from '../../resources/types'

import { Form } from 'element-ui'

@Component({
  name: 'TextDialog'
})
export default class TextDialog extends Mixins(LocalizationMiXin, HttpProxyMiXin) {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: null })
  private textId!: string

  @Prop({ default: () => { return new Array<Language>() } })
  private languages!: Language[]

  @Prop({ default: () => { return new Array<Resource>() } })
  private resources!: Resource[]

  get title() {
    if (this.isEdit) {
      return this.l('LocalizationManagement.EditByName', { 0: this.text.key })
    }
    return this.l('LocalizationManagement.Text:AddNew')
  }

  get saveButtonTitle() {
    if (this.isEdit) {
      return this.l('AbpUi.Save')
    }
    return this.l('LocalizationManagement.SaveAndNext')
  }

  private text = new Text()
  private isEdit = false

  @Watch('showDialog')
  private onShowDialogChanged() {
    this.handleGetText()
  }

  private handleGetText() {
    if (this.showDialog && this.textId) {
      this.isEdit = true
      this.request<Text>({
        service: service,
        controller: controller,
        action: 'GetAsync',
        params: {
          id: this.textId
        }
      }).then(res => {
        this.text = res
      })
    } else {
      this.isEdit = false
      this.text = new Text()
    }
  }

  private onSave() {
    const formText = this.$refs.formText as Form
    formText
      .validate(valid => {
        if (valid) {
          let action = 'CreateAsync'
          let params = {}
          let input: CreateOrUpdateTextInput = new CreateTextInput()
          if (this.isEdit) {
            action = 'UpdateAsync'
            input = new UpdateTextInput()
            params = { id: this.textId }
          } else {
            const create = new CreateTextInput()
            create.resourceName = this.text.resourceName
            create.cultureName = this.text.cultureName
            create.key = this.text.key
            input = create
          }
          this.updateByInput(input)
          this.request<Text>({
            service: service,
            controller: controller,
            action: action,
            data: input,
            params: params
          }).then(res => {
            if (!this.isEdit) {
              formText.resetFields()
            } else {
              this.text = res
              this.onFormClosed(true)
            }
            this.$message.success(this.l('successful'))
          })
        }
      })
  }

  private onFormClosed(changed: boolean) {
    const formText = this.$refs.formText as Form
    formText.resetFields()
    this.$emit('closed', changed)
  }

  private updateByInput(text: CreateOrUpdateTextInput) {
    text.value = this.text.value
  }

  private onLanguageChanged(cultureName: string) {
    if (this.isEdit) {
      const key = this.text.key
      const resourceName = this.text.resourceName
      this.request<Text>({
        service: service,
        controller: controller,
        action: 'GetByCultureKeyAsync',
        params: {
          input: {
            resourceName: resourceName,
            cultureName: cultureName,
            key: key
          }
        }
      }).then(res => {
        if (res) {
          this.text = res
        } else {
          this.isEdit = false
          this.text = new Text()
          this.text.resourceName = resourceName
          this.text.cultureName = cultureName
          this.text.key = key
        }
      })
    }
  }
}
</script>

<style scoped>
.confirm {
  position: absolute;
  right: 10px;
  width:140px;
}
.cancel {
  position: absolute;
  right: 160px;
  width:140px;
}
</style>
