<template>
  <div>
    <el-form
      v-if="allowedCreateProp"
      ref="propertyEditForm"
      label-width="100px"
      :model="newProp"
    >
      <el-form-item
        prop="key"
        :label="$t('AbpIdentityServer.Propertites:Key')"
        :rules="{
          required: true,
          message: $t('pleaseInputBy', {key: $t('AbpIdentityServer.Propertites:Key')}),
          trigger: 'blur'
        }"
      >
        <el-input
          v-model="newProp.key"
          :placeholder="$t('pleaseInputBy', {key: $t('AbpIdentityServer.Propertites:Key')})"
        />
      </el-form-item>
      <el-form-item
        prop="value"
        :label="$t('AbpIdentityServer.Propertites:Value')"
        :rules="{
          required: true,
          message: $t('pleaseInputBy', {key: $t('AbpIdentityServer.Propertites:Value')}),
          trigger: 'blur'
        }"
      >
        <el-input
          v-model="newProp.value"
          :placeholder="$t('pleaseInputBy', {key: $t('AbpIdentityServer.Propertites:Value')})"
        />
      </el-form-item>

      <el-form-item>
        <el-button
          type="success"
          class="add-button"
          @click="onSave"
        >
          <i class="ivu-icon ivu-icon-md-add" />
          {{ $t('AbpIdentityServer.AddNew') }}
        </el-button>
      </el-form-item>
    </el-form>

    <el-table
      row-key="key"
      :data="properties"
      border
      fit
      highlight-current-row
      style="width: 100%;"
    >
      <el-table-column
        :label="$t('AbpIdentityServer.Propertites:Key')"
        prop="key"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.key }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Propertites:Value')"
        prop="value"
        sortable
        min-width="300px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.value }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Actions')"
        align="center"
        min-width="80px"
      >
        <template slot-scope="{row}">
          <el-button
            size="mini"
            type="danger"
            :disabled="!allowedDeleteProp"
            icon="el-icon-delete"
            @click="onDelete(row.key)"
          />
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script lang="ts">
import { Component, Mixins, Prop } from 'vue-property-decorator'
import LocalizationMiXin from '@/mixins/LocalizationMiXin'
import { Property } from '@/api/identity-server4'
import { Form } from 'element-ui'

@Component({
  name: 'PropertiesEditForm',
  model: {
    prop: 'properties',
    event: 'change'
  }
})
export default class PropertiesEditForm extends Mixins(LocalizationMiXin) {
  @Prop({ default: () => { return new Array<Property>() } })
  private properties!: Property[]

  @Prop({ default: false })
  private allowedCreateProp!: boolean

  @Prop({ default: false })
  private allowedDeleteProp!: boolean

  private newProp = new Property()
  private showEditDialog = false

  private validateInput() {
    return !this.properties.some(prop => prop.key === this.newProp.key)
  }

  private onSave() {
    const propertyEditForm = this.$refs.propertyEditForm as Form
    propertyEditForm.validate(valid => {
      if (valid) {
        if (this.validateInput()) {
          const prop: Property = {
            key: this.newProp.key,
            value: this.newProp.value
          }
          this.$emit('change', this.properties.concat(prop))
          propertyEditForm.resetFields()
        } else {
          this.$message.warning(this.l('AbpIdentityServer.Propertites:DuplicateKey'))
        }
      }
    })
  }

  private onDelete(key: string) {
    this.$emit('change', this.properties.filter(prop => prop.key !== key))
  }
}
</script>

<style lang="scss" scoped>
.add-button {
  width: 150px;
  float: right;
}
</style>
