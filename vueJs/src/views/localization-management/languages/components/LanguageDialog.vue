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
      ref="formLanguage"
      :model="language"
      label-width="120px"
    >
      <el-form-item
        prop="enable"
        :label="$t(('LocalizationManagement.DisplayName:Enable'))"
      >
        <el-switch
          v-model="language.enable"
        />
      </el-form-item>
      <el-form-item
        prop="cultureName"
        :label="$t(('LocalizationManagement.DisplayName:CultureName'))"
        :rules="{
          required: true,
          message: $t('pleaseInputBy', {key: $t('LocalizationManagement.DisplayName:CultureName')}),
          trigger: 'blur'
        }"
      >
        <el-input
          v-model="language.cultureName"
        />
      </el-form-item>
      <el-form-item
        prop="uiCultureName"
        :label="$t(('LocalizationManagement.DisplayName:UiCultureName'))"
        :rules="{
          required: true,
          message: $t('pleaseInputBy', {key: $t('LocalizationManagement.DisplayName:UiCultureName')}),
          trigger: 'blur'
        }"
      >
        <el-input
          v-model="language.uiCultureName"
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
          v-model="language.displayName"
        />
      </el-form-item>
      <el-form-item
        prop="flagIcon"
        :label="$t(('LocalizationManagement.DisplayName:FlagIcon'))"
      >
        <el-input
          v-model="language.flagIcon"
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
  Language,
  CreateOrUpdateLanguageInput
} from '../types'

import { Form } from 'element-ui'

@Component({
  name: 'LanguageDialog'
})
export default class LanguageDialog extends Mixins(LocalizationMiXin, HttpProxyMiXin) {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: null })
  private languageId!: string

  get isEdit() {
    if (this.languageId) {
      return true
    }
    return false
  }

  get title() {
    if (this.isEdit) {
      return this.l('LocalizationManagement.EditByName', { 0: this.language.displayName })
    }
    return this.l('LocalizationManagement.Language:AddNew')
  }

  private language = new Language()

  @Watch('showDialog')
  private onShowDialogChanged() {
    this.handleGetLanguage()
  }

  private handleGetLanguage() {
    if (this.showDialog && this.languageId) {
      this.request<Language>({
        service: service,
        controller: controller,
        action: 'GetAsync',
        params: {
          id: this.languageId
        }
      }).then(res => {
        this.language = res
      })
    } else {
      this.language = new Language()
    }
  }

  private onSave() {
    const formLanguage = this.$refs.formLanguage as Form
    formLanguage
      .validate(valid => {
        if (valid) {
          const action = this.isEdit ? 'UpdateAsync' : 'CreateAsync'
          const params = this.isEdit ? { id: this.languageId } : {}
          const input = new CreateOrUpdateLanguageInput()
          this.updateByInput(input)
          this.request<Language>({
            service: service,
            controller: controller,
            action: action,
            data: input,
            params: params
          }).then(res => {
            this.language = res
            this.$message.success(this.l('successful'))
            this.onFormClosed(true)
          })
        }
      })
  }

  private onFormClosed(changed: boolean) {
    const formLanguage = this.$refs.formLanguage as Form
    formLanguage.resetFields()
    this.$emit('closed', changed)
  }

  private updateByInput(language: CreateOrUpdateLanguageInput) {
    language.enable = this.language.enable
    language.cultureName = this.language.cultureName
    language.uiCultureName = this.language.uiCultureName
    language.displayName = this.language.displayName
    language.flagIcon = this.language.flagIcon
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
