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
      ref="formDataItem"
      :model="dataItem"
      label-width="120px"
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
          v-model="dataItem.name"
          :disabled="isEdit"
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
          v-model="dataItem.displayName"
        />
      </el-form-item>
      <el-form-item
        prop="valueType"
        :label="$t(('AppPlatform.DisplayName:ValueType'))"
        :rules="{
          required: true,
          message: $t('pleaseSelectBy', {key: $t('AppPlatform.DisplayName:ValueType')}),
          trigger: 'blur'
        }"
      >
        <el-select
          v-model="dataItem.valueType"
          class="full-select"
          :placeholder="$t('pleaseSelectBy', {name: $t('AppPlatform.DisplayName:ValueType')})"
        >
          <el-option
            key="String"
            label="String"
            :value="0"
          />
          <el-option
            key="Numeic"
            label="Numeic"
            :value="1"
          />
          <el-option
            key="Boolean"
            label="Boolean"
            :value="2"
          />
          <el-option
            key="3"
            label="Date"
            :value="3"
          />
          <el-option
            key="DateTime"
            label="DateTime"
            :value="4"
          />
          <el-option
            key="Array"
            label="Array"
            :value="5"
          />
          <el-option
            key="Object"
            label="Object"
            :value="6"
          />
        </el-select>
      </el-form-item>
      <el-form-item
        prop="defaultValue"
        :label="$t(('AppPlatform.DisplayName:DefaultValue'))"
        :rules="{
          required: !dataItem.allowBeNull,
          message: $t('pleaseInputBy', {key: $t('AppPlatform.DisplayName:DefaultValue')}),
          trigger: 'blur'
        }"
      >
        <el-input
          v-model="dataItem.defaultValue"
        />
      </el-form-item>
      <el-form-item
        prop="allowBeNull"
        :label="$t(('AppPlatform.DisplayName:AllowBeNull'))"
      >
        <el-switch
          v-model="dataItem.allowBeNull"
        />
      </el-form-item>
      <el-form-item
        prop="description"
        :label="$t(('AppPlatform.DisplayName:Description'))"
      >
        <el-input
          v-model="dataItem.description"
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
import { Component, Mixins, Prop } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'

import { Form } from 'element-ui'
import DataDictionaryService, {
  DataItem,
  DataItemCreate,
  DataItemUpdate
} from '@/api/data-dictionary'

@Component({
  name: 'CreateOrUpdateDataItemDialog'
})
export default class CreateOrUpdateDataItemDialog extends Mixins(LocalizationMiXin) {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: null })
  private dataId!: string

  @Prop({ default: () => { return new DataItem() } })
  private dataItem!: DataItem

  get isEdit() {
    if (this.dataItem.id) {
      return true
    }
    return false
  }

  get title() {
    if (this.isEdit) {
      return this.l('AppPlatform.Data:EditItem')
    }
    return this.l('AppPlatform.Data:AppendItem')
  }

  private onSave() {
    const formDataItem = this.$refs.formDataItem as Form
    formDataItem
      .validate(valid => {
        if (valid) {
          if (this.isEdit) {
            const update = new DataItemUpdate()
            update.displayName = this.dataItem.displayName
            update.description = this.dataItem.description
            update.defaultValue = this.dataItem.defaultValue
            update.valueType = this.dataItem.valueType
            update.allowBeNull = this.dataItem.allowBeNull
            DataDictionaryService
              .updateItem(this.dataId, this.dataItem.name, update)
              .then(() => {
                this.$message.success(this.l('successful'))
                this.onFormClosed(true)
              })
          } else {
            const create = new DataItemCreate()
            create.name = this.dataItem.name
            create.displayName = this.dataItem.displayName
            create.description = this.dataItem.description
            create.defaultValue = this.dataItem.defaultValue
            create.valueType = this.dataItem.valueType
            create.allowBeNull = this.dataItem.allowBeNull
            DataDictionaryService
              .appendItem(this.dataId, create)
              .then(() => {
                this.$message.success(this.l('successful'))
                this.onFormClosed(true)
              })
          }
        }
      })
  }

  private onFormClosed(changed: boolean) {
    const formDataItem = this.$refs.formDataItem as Form
    formDataItem.resetFields()
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
.full-select {
  width: 100%;
}
</style>
