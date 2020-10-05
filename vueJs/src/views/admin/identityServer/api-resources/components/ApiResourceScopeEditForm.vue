<template>
  <el-dialog
    v-el-draggable-dialog
    width="800px"
    :visible="showDialog"
    :title="$t('identityServer.apiResourceScope')"
    custom-class="modal-form"
    :show-close="false"
    @close="onFormClosed"
  >
    <div class="app-container">
      <el-form
        v-if="checkPermission(['IdentityServer.ApiResources.Secrets.Create'])"
        ref="formApiScope"
        label-width="120px"
        :model="apiScope"
        :rules="apiScopeRules"
      >
        <el-form-item
          prop="name"
          :label="$t('identityServer.apiScopeName')"
        >
          <el-input
            v-model="apiScope.name"
            :placeholder="$t('pleaseInputBy', {key: $t('identityServer.apiScopeName')})"
          />
        </el-form-item>
        <el-form-item
          prop="displayName"
          :label="$t('identityServer.apiScopeDisplayName')"
        >
          <el-input
            v-model="apiScope.displayName"
          />
        </el-form-item>
        <el-form-item
          prop="description"
          :label="$t('identityServer.apiScopeDescription')"
        >
          <el-input
            v-model="apiScope.description"
          />
        </el-form-item>
        <el-form-item
          prop="required"
          :label="$t('identityServer.apiScopeRequired')"
        >
          <el-switch
            v-model="apiScope.required"
          />
        </el-form-item>
        <el-form-item
          prop="emphasize"
          :label="$t('identityServer.apiScopeEmphasize')"
        >
          <el-switch
            v-model="apiScope.emphasize"
          />
        </el-form-item>
        <el-form-item
          prop="showInDiscoveryDocument"
          :label="$t('identityServer.apiScopeShowInDiscoveryDocument')"
        >
          <el-switch
            v-model="apiScope.showInDiscoveryDocument"
          />
        </el-form-item>
        <el-form-item
          prop="userClaims"
          :label="$t('identityServer.resourceUserClaims')"
        >
          <el-input-tag-ex
            v-model="apiScope.userClaims"
            label="type"
          />
        </el-form-item>

        <el-form-item
          style="text-align: center;"
          label-width="0px"
        >
          <el-button
            type="primary"
            style="width:180px"
            @click="onSaveApiScope"
          >
            {{ $t('identityServer.createApiScope') }}
          </el-button>
        </el-form-item>
        <el-divider />
      </el-form>
    </div>

    <el-table
      row-key="value"
      :data="apiScopes"
      border
      fit
      highlight-current-row
      style="width: 100%;"
    >
      <el-table-column
        :label="$t('identityServer.apiScopeName')"
        prop="name"
        sortable
        width="150px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.name }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('identityServer.apiScopeDisplayName')"
        prop="displayName"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.displayName }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('identityServer.apiScopeDescription')"
        prop="description"
        width="300px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.description }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('operaActions')"
        align="center"
        width="150px"
        fixed="right"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!checkPermission(['IdentityServer.ApiResources.Scope.Delete'])"
            size="mini"
            type="primary"
            @click="handleDeleteApiScope(row.name)"
          >
            {{ $t('identityServer.deleteApiScope') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>
  </el-dialog>
</template>

<script lang="ts">
import ApiResourceService, { ApiScope, ApiScopeCreate } from '@/api/apiresources'
import { Component, Vue, Prop, Watch } from 'vue-property-decorator'
import { dateFormat } from '@/utils/index'
import { checkPermission } from '@/utils/permission'
import ElInputTagEx from '@/components/InputTagEx/index.vue'

@Component({
  name: 'ApiScopeEditForm',
  components: {
    ElInputTagEx
  },
  filters: {
    dateTimeFilter(datetime: string) {
      if (datetime) {
        const date = new Date(datetime)
        return dateFormat(date, 'YYYY-mm-dd HH:MM')
      }
      return ''
    }
  },
  methods: {
    checkPermission
  }
})
export default class extends Vue {
  @Prop({ default: false })
  private showDialog!: boolean

  @Prop({ default: '' })
  private apiResourceId!: string

  @Prop({ default: () => new Array<ApiScope>() })
  private apiScopes!: ApiScope[]

  private apiScopeChanged: boolean
  private apiScope: ApiScopeCreate
  private apiScopeRules = {
    name: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('identityServer.apiScopeName') }), trigger: 'blur' }
    ]
  }

  constructor() {
    super()
    this.apiScopeChanged = false
    this.apiScope = ApiScopeCreate.empty()
  }

  @Watch('apiResourceId', { immediate: true })
  private onApiResourceIdChanged() {
    this.apiScope.apiResourceId = this.apiResourceId
  }

  private handleDeleteApiScope(name: string) {
    this.$confirm(this.l('identityServer.deleteApiScopeByName', { name: name }),
      this.l('identityServer.deleteApiScope'), {
        callback: (action) => {
          if (action === 'confirm') {
            ApiResourceService.deleteApiScope(this.apiResourceId, name).then(() => {
              const deleteScopeIndex = this.apiScopes.findIndex(scope => scope.name === name)
              this.apiScopes.splice(deleteScopeIndex, 1)
              this.$message.success(this.l('identityServer.deleteApiScopeSuccess', { name: name }))
              this.$emit('apiScopeChanged')
            })
          }
        }
      })
  }

  private onSaveApiScope() {
    const frmApiScope = this.$refs.formApiScope as any
    frmApiScope.validate((valid: boolean) => {
      if (valid) {
        this.apiScope.apiResourceId = this.apiResourceId
        ApiResourceService.addApiScope(this.apiScope).then(scope => {
          this.apiScopes.push(scope)
          const successMessage = this.l('identityServer.createApiScopeSuccess', { name: this.apiScope.name })
          this.$message.success(successMessage)
          this.$emit('apiScopeChanged')
          this.onFormClosed()
        })
      }
    })
  }

  private onFormClosed() {
    this.resetFields()
    this.$emit('closed')
  }

  public resetFields() {
    const frmApiScope = this.$refs.formApiScope as any
    frmApiScope.resetFields()
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
