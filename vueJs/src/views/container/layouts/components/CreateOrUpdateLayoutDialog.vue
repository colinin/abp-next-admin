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
      ref="formLayout"
      :model="layout"
      label-width="120px"
    >
      <el-form-item
        v-if="!isEdit"
        prop="framework"
        :label="$t('AppPlatform.DisplayName:UIFramework')"
        :rules="{
          required: true,
          message: $t('pleaseInputBy', {key: $t('AppPlatform.DisplayName:UIFramework')}),
          trigger: 'blur'
        }"
      >
        <el-select
          v-model="layout.framework"
          style="width: 100%;"
          class="filter-item"
          clearable
          :placeholder="$t('pleaseSelectBy', {name: $t('AppPlatform.DisplayName:UIFramework')})"
        >
          <el-option
            v-for="framework in uiFrameworks"
            :key="framework"
            :label="framework"
            :value="framework"
          />
        </el-select>
      </el-form-item>
      <el-form-item
        v-if="!isEdit"
        :label="$t('AppPlatform.DisplayName:DataDictionary')"
        :rules="{
          required: true
        }"
      >
        <el-select
          v-model="dataId"
          style="width: 100%;"
          class="filter-item"
          clearable
          :placeholder="$t('pleaseSelectBy', {name: $t('AppPlatform.DisplayName:DataDictionary')})"
        >
          <el-option
            v-for="data in datas"
            :key="data.name"
            :label="data.displayName"
            :value="data.id"
          />
        </el-select>
      </el-form-item>
      <el-form-item
        prop="name"
        :label="$t(('AppPlatform.DisplayName:Name'))"
        :rules="{
          required: true,
          message: $t('pleaseInputBy', {key: $t('AppPlatform.DisplayName:Name')}),
          trigger: 'blur'
        }"
      >
        <el-input
          v-model="layout.name"
        />
      </el-form-item>
      <el-form-item
        prop="displayName"
        :label="$t(('AppPlatform.DisplayName:DisplayName'))"
        :rules="{
          required: true,
          message: $t('pleaseInputBy', {key: $t('AppPlatform.DisplayName:DisplayName')}),
          trigger: 'blur'
        }"
      >
        <el-input
          v-model="layout.displayName"
        />
      </el-form-item>
      <el-form-item
        prop="path"
        :label="$t(('AppPlatform.DisplayName:Path'))"
        :rules="{
          required: true,
          message: $t('pleaseInputBy', {key: $t('AppPlatform.DisplayName:Path')}),
          trigger: 'blur'
        }"
      >
        <el-input
          v-model="layout.path"
        />
      </el-form-item>
      <el-form-item
        prop="redirect"
        :label="$t(('AppPlatform.DisplayName:Redirect'))"
      >
        <el-input
          v-model="layout.redirect"
        />
      </el-form-item>
      <el-form-item
        prop="description"
        :label="$t(('AppPlatform.DisplayName:Description'))"
      >
        <el-input
          v-model="layout.description"
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
          {{ $t('AbpUi.Save') }}
        </el-button>
      </el-form-item>
    </el-form>
  </el-dialog>
</template>

<script lang="ts">
import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'

import { Form } from 'element-ui'
import DataService, { Data } from '@/api/data-dictionary'
import LayoutService, {
  Layout,
  LayoutCreateOrUpdate,
  LayoutCreate,
  LayoutUpdate
} from '@/api/layout'

@Component({
  name: 'CreateOrUpdateLayoutDialog'
})
export default class CreateOrUpdateLayoutDialog extends Mixins(LocalizationMiXin) {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: null })
  private layoutId!: string

  @Prop({ default: [] })
  private uiFrameworks!: string[]

  private layout = new Layout()
  private datas = new Array<Data>()
  private dataId = ''

  get isEdit() {
    if (this.layoutId) {
      return true
    }
    return false
  }

  get title() {
    if (this.isEdit) {
      return this.l('AppPlatform.Layout:EditByName', { 0: this.layout.displayName })
    }
    return this.l('AppPlatform.Layout:AddNew')
  }

  @Watch('showDialog')
  private onShowDialogChanged() {
    this.handleGetLayout()
  }

  mounted() {
    this.handleGetDataDictionarys()
  }

  private handleGetDataDictionarys() {
    DataService
      .getAll()
      .then(res => {
        this.datas = res.items
      })
  }

  private handleGetLayout() {
    if (this.showDialog && this.layoutId) {
      LayoutService
        .get(this.layoutId)
        .then(res => {
          this.layout = res
        })
    } else {
      this.layout = new Layout()
    }
  }

  private onSave() {
    const formLayout = this.$refs.formLayout as Form
    formLayout
      .validate(valid => {
        if (valid) {
          if (this.isEdit) {
            const update = new LayoutUpdate()
            this.updateMenuByInput(update)
            LayoutService
              .update(this.layoutId, update)
              .then(res => {
                this.layout = res
                this.$message.success(this.l('successful'))
                this.onFormClosed(true)
              })
          } else {
            if (!this.dataId) {
              this.$message.warning(this.l('pleaseSelectBy', { key: this.l('AppPlatform.DisplayName:DataDictionary') }))
              return
            }
            const create = new LayoutCreate()
            this.updateMenuByInput(create)
            create.dataId = this.dataId
            create.framework = this.layout.framework
            LayoutService
              .create(create)
              .then(res => {
                this.layout = res
                this.$message.success(this.l('successful'))
                this.onFormClosed(true)
              })
          }
        }
      })
  }

  private onFormClosed(changed: boolean) {
    const formLayout = this.$refs.formLayout as Form
    formLayout.resetFields()
    this.$emit('closed', changed)
  }

  private updateMenuByInput(update: LayoutCreateOrUpdate) {
    update.name = this.layout.name
    update.path = this.layout.path
    update.displayName = this.layout.displayName
    update.description = this.layout.description
    update.redirect = this.layout.redirect
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
