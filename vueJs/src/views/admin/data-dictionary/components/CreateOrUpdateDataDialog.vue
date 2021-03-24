<template>
  <el-dialog
    v-el-draggable-dialog
    width="400px"
    :visible="showDialog"
    :title="title"
    custom-class="modal-form"
    :show-close="false"
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    @close="onFormClosed(false)"
  >
    <el-form
      ref="formData"
      :model="data"
      label-width="100px"
    >
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
          v-model="data.name"
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
          v-model="data.displayName"
        />
      </el-form-item>
      <el-form-item
        prop="description"
        :label="$t(('AppPlatform.DisplayName:Description'))"
      >
        <el-input
          v-model="data.description"
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
import DataDictionaryService, {
  Data,
  DataCreate,
  DataUpdate
} from '@/api/data-dictionary'

@Component({
  name: 'CreateOrUpdateDataDialog'
})
export default class CreateOrUpdateDataDialog extends Mixins(LocalizationMiXin) {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: '' })
  private title!: string

  @Prop({ default: false })
  private isEdit!: boolean

  @Prop({ default: null })
  private dataId?: string

  private data = new Data()

  @Watch('showDialog')
  private onShowDialogChanged() {
    this.handleGetData()
  }

  private handleGetData() {
    if (this.showDialog && this.isEdit && this.dataId) {
      DataDictionaryService
        .get(this.dataId)
        .then(res => {
          this.data = res
        })
    } else {
      this.data = new Data()
    }
  }

  private onSave() {
    const formData = this.$refs.formData as Form
    formData
      .validate(valid => {
        if (valid) {
          if (this.isEdit) {
            const update = new DataUpdate()
            update.name = this.data.name
            update.displayName = this.data.displayName
            update.description = this.data.description
            DataDictionaryService
              .update(this.data.id, update)
              .then(() => {
                this.$message.success(this.l('successful'))
                this.onFormClosed(true)
              })
          } else {
            const create = new DataCreate()
            create.name = this.data.name
            create.displayName = this.data.displayName
            create.description = this.data.description
            create.parentId = this.dataId
            DataDictionaryService
              .create(create)
              .then(() => {
                this.$message.success(this.l('successful'))
                this.onFormClosed(true)
              })
          }
        }
      })
  }

  private onFormClosed(changed: boolean) {
    const formData = this.$refs.formData as Form
    formData.resetFields()
    this.$emit('closed', changed)
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
