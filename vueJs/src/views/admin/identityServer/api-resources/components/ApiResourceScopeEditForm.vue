<template>
  <div>
    <el-form
      ref="apiResourceScopeEditForm"
      label-width="80px"
      :model="apiResourceScope"
      :rules="apiResourceScopeRules"
    >
      <el-tabs
        type="border-card"
        style="width: 100%;"
      >
        <el-tab-pane :label="$t('AbpIdentityServer.Information')">
          <el-row>
            <el-col :span="12">
              <el-form-item
                prop="name"
                :label="$t('AbpIdentityServer.Name')"
              >
                <el-input v-model="apiResourceScope.name" />
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <el-form-item
                prop="displayName"
                :label="$t('AbpIdentityServer.DisplayName')"
              >
                <el-input v-model="apiResourceScope.displayName" />
              </el-form-item>
            </el-col>
          </el-row>
          <el-form-item
            prop="description"
            :label="$t('AbpIdentityServer.Description')"
          >
            <el-input v-model="apiResourceScope.description" />
          </el-form-item>
          <el-row>
            <el-col :span="6">
              <el-form-item
                prop="required"
                :label="$t('AbpIdentityServer.Required')"
              >
                <el-switch v-model="apiResourceScope.required" />
              </el-form-item>
            </el-col>
            <el-col :span="6">
              <el-form-item
                prop="emphasize"
                :label="$t('AbpIdentityServer.Emphasize')"
              >
                <el-switch v-model="apiResourceScope.emphasize" />
              </el-form-item>
            </el-col>
            <el-col :span="12">
              <el-form-item
                prop="showInDiscoveryDocument"
                :label="$t('AbpIdentityServer.ShowInDiscoveryDocument')"
                label-width="150px"
              >
                <el-switch v-model="apiResourceScope.showInDiscoveryDocument" />
              </el-form-item>
            </el-col>
          </el-row>
        </el-tab-pane>
        <el-tab-pane :label="$t('AbpIdentityServer.UserClaim')">
          <el-transfer
            v-model="apiResourceScope.userClaims"
            class="transfer-scope-new"
            :data="userClaims"
            :props="{
              key: 'type',
              label: 'value'
            }"
            :titles="[$t('AbpIdentityServer.NoClaim'), $t('AbpIdentityServer.ExistsClaim')]"
          />
        </el-tab-pane>
      </el-tabs>
      <el-form-item
        style="text-align: center;"
        label-width="0px"
      >
        <el-button
          type="primary"
          style="width:180px; margin-top: 20px;"
          @click="onSave"
        >
          {{ $t('AbpIdentityServer.Scope:New') }}
        </el-button>
      </el-form-item>
    </el-form>
    <el-collapse accordion>
      <el-collapse-item
        v-for="(scope, index) in apiResourceScopes"
        :key="index"
      >
        <h3 slot="title">
          {{ scope.name }}
        </h3>
        <el-card>
          <el-form
            :model="scope"
            label-width="80px"
          >
            <el-button
              :disabled="!checkPermission(['IdentityServer.ApiResources.Scope.Delete'])"
              type="danger"
              icon="el-icon-delete"
              size="mini"
              style="margin-bottom: 10px;"
              @click="handleDeleteApiScope(scope.name)"
            >
              {{ $t('AbpIdentityServer.Scope:Delete') }}
            </el-button>
            <el-tabs type="border-card">
              <el-tab-pane :label="$t('AbpIdentityServer.Information')">
                <el-row>
                  <el-col :span="12">
                    <el-form-item :label="$t('AbpIdentityServer.Name')">
                      <el-input v-model="scope.name" />
                    </el-form-item>
                  </el-col>
                  <el-col :span="12">
                    <el-form-item :label="$t('AbpIdentityServer.DisplayName')">
                      <el-input v-model="scope.displayName" />
                    </el-form-item>
                  </el-col>
                </el-row>
                <el-form-item :label="$t('AbpIdentityServer.Description')">
                  <el-input v-model="scope.description" />
                </el-form-item>
                <el-row>
                  <el-col :span="6">
                    <el-form-item :label="$t('AbpIdentityServer.Required')">
                      <el-switch v-model="scope.required" />
                    </el-form-item>
                  </el-col>
                  <el-col :span="6">
                    <el-form-item :label="$t('AbpIdentityServer.Emphasize')">
                      <el-switch v-model="scope.emphasize" />
                    </el-form-item>
                  </el-col>
                  <el-col :span="12">
                    <el-form-item
                      :label="$t('AbpIdentityServer.ShowInDiscoveryDocument')"
                      label-width="150px"
                    >
                      <el-switch v-model="scope.showInDiscoveryDocument" />
                    </el-form-item>
                  </el-col>
                </el-row>
              </el-tab-pane>
              <el-tab-pane :label="$t('AbpIdentityServer.UserClaim')">
                <el-transfer
                  v-model="scope.userClaims"
                  class="transfer-scope-edit"
                  :data="userClaims"
                  :props="{
                    key: 'type',
                    label: 'value'
                  }"
                  :titles="[$t('AbpIdentityServer.NoClaim'), $t('AbpIdentityServer.ExistsClaim')]"
                />
              </el-tab-pane>
            </el-tabs>
          </el-form>
        </el-card>
      </el-collapse-item>
    </el-collapse>
  </div>
</template>

<script lang="ts">
import { ApiScope } from '@/api/api-resources'
import { Component, Vue, Prop } from 'vue-property-decorator'
import { dateFormat } from '@/utils/index'
import { checkPermission } from '@/utils/permission'
import ElInputTagEx from '@/components/InputTagEx/index.vue'
import { Claim } from '@/api/types'
import { Form } from 'element-ui'

@Component({
  name: 'ApiResourceScopeEditForm',
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
export default class ApiResourceScopeEditForm extends Vue {
  @Prop({ default: () => { return new Array<Claim>() } })
  private userClaims!: Claim[]

  @Prop({ default: () => { return new Array<ApiScope>() } })
  private apiResourceScopes!: ApiScope[]

  private apiResourceScope = new ApiScope()
  private apiResourceScopeRules = {
    name: [
      { required: true, message: this.l('pleaseInputBy', { key: this.l('AbpIdentityServer.Name') }), trigger: 'blur' }
    ]
  }

  private handleDeleteApiScope(name: string) {
    this.$emit('apiResourceScopeDeleted', name)
  }

  private onSave() {
    const apiResourceScopeEditForm = this.$refs.apiResourceScopeEditForm as Form
    apiResourceScopeEditForm.validate(valid => {
      if (valid) {
        this.$emit('apiResourceScopeCreated',
          this.apiResourceScope.name, this.apiResourceScope.required,
          this.apiResourceScope.emphasize, this.apiResourceScope.showInDiscoveryDocument,
          this.apiResourceScope.userClaims, this.apiResourceScope.displayName, this.apiResourceScope.description)
        apiResourceScopeEditForm.resetFields()
        this.apiResourceScope.userClaims.length = 0
      }
    })
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
.transfer-scope-new ::v-deep .el-transfer-panel{
  width: 236px;
}
.transfer-scope-edit ::v-deep .el-transfer-panel{
  width: 216px;
}
</style>
