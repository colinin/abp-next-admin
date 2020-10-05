<template>
  <el-dialog
    v-el-draggable-dialog
    width="800px"
    :visible="showDialog"
    :title="$t('identityServer.identityResourceProperties')"
    custom-class="modal-form"
    :show-close="false"
    @close="onFormClosed(false)"
  >
    <div class="app-container">
      <el-form
        v-if="checkPermission(['IdentityServer.IdentityResources.Properties.Create'])"
        ref="formIdentityProperty"
        label-width="100px"
        :model="identityProperty"
        :rules="identityPropertyRules"
      >
        <el-form-item
          prop="key"
          :label="$t('identityServer.propertyKey')"
        >
          <el-input
            v-model="identityProperty.key"
            :placeholder="$t('pleaseInputBy', {key: $t('identityServer.propertyKey')})"
          />
        </el-form-item>
        <el-form-item
          prop="value"
          :label="$t('identityServer.propertyValue')"
        >
          <el-input
            v-model="identityProperty.value"
            :placeholder="$t('pleaseInputBy', {key: $t('identityServer.propertyValue')})"
          />
        </el-form-item>

        <el-form-item
          style="text-align: center;"
          label-width="0px"
        >
          <el-button
            type="primary"
            style="width:180px"
            @click="onSaveIdentityProperty"
          >
            {{ $t('identityServer.createIdentityProperty') }}
          </el-button>
        </el-form-item>
        <el-divider />
      </el-form>
    </div>
    <el-table
      row-key="key"
      :data="identityResource.properties"
      border
      fit
      highlight-current-row
      style="width: 100%;"
    >
      <el-table-column
        :label="$t('identityServer.propertyKey')"
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
        :label="$t('identityServer.propertyValue')"
        prop="value"
        sortable
        min-width="400px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.value }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('operaActions')"
        align="center"
        width="150px"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!checkPermission(['IdentityServer.Clients.Properties.Delete'])"
            size="mini"
            type="primary"
            @click="handleDeleteIdentityProperty(row.key, row.value)"
          >
            {{ $t('identityServer.deleteProperty') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>
  </el-dialog>
</template>

<script lang="ts">
import IdentityResourceService, { IdentityResource, IdentityPropertyCreate } from '@/api/identityresources'
import { Component, Vue, Prop } from 'vue-property-decorator'
import { checkPermission } from '@/utils/permission'
import { ElForm } from 'element-ui/types/form'

@Component({
  name: 'IdentityPropertyEditForm',
  methods: {
    checkPermission
  }
})
export default class extends Vue {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: () => IdentityResource.empty() })
  private identityResource!: IdentityResource

  private identityProperty: IdentityPropertyCreate
  private identityPropertyRules = {
    key: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.propertyKey') }), trigger: 'blur' }
    ],
    value: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.propertyValue') }), trigger: 'blur' }
    ]
  }

  constructor() {
    super()
    this.identityProperty = IdentityPropertyCreate.empty()
  }

  private handleDeleteIdentityProperty(key: string) {
    this.$confirm(this.l('identityServer.deleteIdentityPropertyByKey', { key: key }),
      this.l('identityServer.deleteProperty'), {
        callback: (action) => {
          if (action === 'confirm') {
            IdentityResourceService.deleteProperty(this.identityResource.id, key).then(() => {
              const deletePropertyIndex = this.identityResource.properties.findIndex(p => p.key === key && p.value === key)
              this.identityResource.properties.splice(deletePropertyIndex, 1)
              this.$message.success(this.l('identityServer.deleteIdentityPropertySuccess', { key: key }))
              this.onFormClosed(true)
            })
          }
        }
      })
  }

  private onSaveIdentityProperty() {
    const frmIdentityProperty = this.$refs.formIdentityProperty as any
    frmIdentityProperty.validate((valid: boolean) => {
      if (valid) {
        this.identityProperty.identityResourceId = this.identityResource.id
        this.identityProperty.concurrencyStamp = this.identityResource.concurrencyStamp
        IdentityResourceService.createProperty(this.identityProperty).then(property => {
          this.identityResource.properties.push(property)
          const successMessage = this.l('identityServer.createIdentityPropertySuccess', { key: this.identityProperty.key })
          this.$message.success(successMessage)
          this.onFormClosed(true)
        })
      }
    })
  }

  private onFormClosed(changed: boolean) {
    this.resetFields()
    this.$emit('closed', changed)
  }

  private resetFields() {
    const frmIdentityProperty = this.$refs.formIdentityProperty as ElForm
    frmIdentityProperty.resetFields()
  }

  private l(name: string, values?: any[] | { [key: string]: any }) {
    return this.$t(name, values).toString()
  }
}
</script>

<style lang="scss" scoped>
.full-select {
  width: 100%;
}
</style>
