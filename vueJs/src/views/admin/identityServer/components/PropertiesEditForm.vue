<template>
  <div>
    <el-form
      v-if="allowedCreateProp"
      ref="propertyEditForm"
      label-width="100px"
      :model="property"
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
          v-model="property.key"
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
          v-model="property.value"
          :placeholder="$t('pleaseInputBy', {key: $t('AbpIdentityServer.Propertites:Value')})"
        />
      </el-form-item>

      <el-form-item
        style="text-align: center;"
        label-width="0px"
      >
        <el-button
          type="primary"
          style="width:180px"
          @click="onSave"
        >
          {{ $t('AbpIdentityServer.Propertites:New') }}
        </el-button>
      </el-form-item>
    </el-form>

    <el-table
      row-key="key"
      :data="editProperties"
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
        width="80px"
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
import { Component, Vue, Prop, Watch } from 'vue-property-decorator'
import { Form } from 'element-ui'

class Property {
  key = ''
  value = ''

  constructor(
    key: string,
    value: string
  ) {
    this.key = key
    this.value = value
  }
}

@Component({
  name: 'PropertiesEditForm'
})
export default class PropertiesEditForm extends Vue {
  @Prop({ default: () => { return {} } })
  private properties!: {[key: string]: string}

  @Prop({ default: false })
  private allowedCreateProp!: boolean

  @Prop({ default: false })
  private allowedDeleteProp!: boolean

  private editProperties = new Array<Property>()
  private property = new Property('', '')

  @Watch('properties', { immediate: true, deep: true })
  private onPropertiesChanged() {
    this.editProperties = new Array<Property>()
    Object.keys(this.properties)
      .forEach(key => {
        this.editProperties.push(new Property(key, this.properties[key]))
      })
  }

  private validateInput() {
    return !this.editProperties.some(prop => prop.key === this.property.key)
  }

  private onSave() {
    const propertyEditForm = this.$refs.propertyEditForm as Form
    propertyEditForm.validate(valid => {
      if (valid) {
        if (this.validateInput()) {
          this.$emit('onCreated', this.property.key, this.property.value)
          propertyEditForm.resetFields()
        } else {
          this.$message.warning(this.l('AbpIdentityServer.Propertites:DuplicateKey'))
        }
      }
    })
  }

  private onDelete(key: string) {
    this.$emit('onDeleted', key)
  }

  private l(name: string, values?: any[] | { [key: string]: any }) {
    return this.$t(name, values).toString()
  }
}
</script>
