<template>
  <div class="app-container">
    <div class="filter-container">
      <label
        class="radio-label"
        style="padding-left:10px;"
      >{{ $t('queryFilter') }}</label>
      <el-input
        v-model="apiResourceGetPagedFilter.filter"
        :placeholder="$t('filterString')"
        style="width: 250px;margin-left: 10px;"
        class="filter-item"
      />
      <el-button
        class="filter-item"
        style="margin-left: 10px; text-alignt"
        type="primary"
        @click="handleGetApiResources"
      >
        {{ $t('searchList') }}
      </el-button>
      <el-button
        class="filter-item"
        type="primary"
        :disabled="!checkPermission(['IdentityServer.ApiResources.Create'])"
        @click="handleShowEditApiResourceForm()"
      >
        {{ $t('identityServer.createApiResource') }}
      </el-button>
    </div>

    <el-table
      v-loading="apiResourceListLoading"
      row-key="id"
      :data="apiResourceList"
      border
      fit
      highlight-current-row
      style="width: 100%;"
      @sort-change="handleSortChange"
    >
      <el-table-column
        :label="$t('identityServer.resourceName')"
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
        :label="$t('identityServer.resourceDisplayName')"
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
        :label="$t('identityServer.resourceStatus')"
        prop="enabled"
        sortable
        width="140px"
        align="center"
      >
        <template slot-scope="{row}">
          <el-tag :type="row.enabled | statusFilter">
            {{ formatStatusText(row.enabled) }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('identityServer.resourceDescription')"
        prop="description"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.description }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('global.creationTime')"
        prop="creationTime"
        width="170px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>
            <el-tag>
              {{ row.creationTime | datetimeFilter }}
            </el-tag>
          </span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('global.lastModificationTime')"
        prop="lastModificationTime"
        width="170px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>
            <el-tag type="warning">
              {{ row.lastModificationTime | datetimeFilter }}
            </el-tag>
          </span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('global.operaActions')"
        align="center"
        width="250px"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!checkPermission(['IdentityServer.ApiResources.Update'])"
            size="mini"
            type="primary"
            @click="handleShowEditApiResourceForm(row)"
          >
            {{ $t('identityServer.updateApiResource') }}
          </el-button>
          <el-dropdown
            class="options"
            @command="handleCommand"
          >
            <el-button
              v-permission="['IdentityServer.ApiResources']"
              size="mini"
              type="info"
            >
              {{ $t('identityServer.otherOpera') }}<i class="el-icon-arrow-down el-icon--right" />
            </el-button>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                :command="{key: 'secret', row}"
                :disabled="!checkPermission(['IdentityServer.ApiResources.Secrets'])"
              >
                {{ $t('identityServer.apiResourceSecret') }}
              </el-dropdown-item>
              <el-dropdown-item
                :command="{key: 'scope', row}"
                :disabled="!checkPermission(['IdentityServer.ApiResources.Scope'])"
              >
                {{ $t('identityServer.apiResourceScope') }}
              </el-dropdown-item>
              <el-dropdown-item
                divided
                :command="{key: 'delete', row}"
                :disabled="!checkPermission(['IdentityServer.ApiResources.Delete'])"
              >
                {{ $t('identityServer.deleteApiResource') }}
              </el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
        </template>
      </el-table-column>
    </el-table>

    <Pagination
      v-show="apiResourceListCount>0"
      :total="apiResourceListCount"
      :page.sync="apiResourceGetPagedFilter.skipCount"
      :limit.sync="apiResourceGetPagedFilter.maxResultCount"
      @pagination="handleGetApiResources"
      @sort-change="handleSortChange"
    />

    <el-dialog
      v-el-draggable-dialog
      width="800px"
      :visible.sync="showEditApiResourceDialog"
      :title="editApiResourceTitle"
      custom-class="modal-form"
      :show-close="false"
      :close-on-click-modal="false"
      :close-on-press-escape="false"
    >
      <ApiResourceCreateOrEditForm
        :api-resource-id="editApiResource.id"
        @closed="handleApiResourceEditFormClosed"
      />
    </el-dialog>

    <el-dialog
      v-el-draggable-dialog
      width="800px"
      :visible.sync="showEditApiSecretDialog"
      :title="$t('identityServer.apiResourceSecret')"
      custom-class="modal-form"
      :show-close="false"
      @closed="handleApiSecretEditFormClosed"
    >
      <ApiSecretEditForm
        ref="formApiSecret"
        :api-resource-id="editApiResource.id"
        :api-secrets="editApiResource.secrets"
        @apiSecretChanged="handleGetApiResources"
      />
    </el-dialog>

    <el-dialog
      v-el-draggable-dialog
      width="800px"
      :visible.sync="showEditApiScopeDialog"
      :title="$t('identityServer.apiResourceScope')"
      custom-class="modal-form"
      :show-close="false"
      @closed="handleApiScopeEditFormClosed"
    >
      <ApiScopeEditForm
        ref="formApiScope"
        :api-resource-id="editApiResource.id"
        :api-scopes="editApiResource.scopes"
        @apiSecretChanged="handleGetApiResources"
      />
    </el-dialog>
  </div>
</template>

<script lang="ts">
import { checkPermission } from '@/utils/permission'
import { Component, Vue } from 'vue-property-decorator'
import { dateFormat } from '@/utils/index'
import Pagination from '@/components/Pagination/index.vue'
import ApiScopeEditForm from './components/ApiResourceScopeEditForm.vue'
import ApiSecretEditForm from './components/ApiResourceSecretEditForm.vue'
import ApiResourceCreateOrEditForm from './components/ApiResourceCreateOrEditForm.vue'
import ApiResourceService, { ApiResource, ApiResourceGetByPaged } from '@/api/apiresources'

@Component({
  name: 'IdentityServerApiResource',
  components: {
    Pagination,
    ApiScopeEditForm,
    ApiSecretEditForm,
    ApiResourceCreateOrEditForm
  },
  methods: {
    checkPermission,
    local(name: string) {
      return this.$t(name)
    }
  },
  filters: {
    statusFilter(status: boolean) {
      if (status) {
        return 'success'
      }
      return 'warning'
    },
    datetimeFilter(val: string) {
      const date = new Date(val)
      return dateFormat(date, 'YYYY-mm-dd HH:MM')
    }
  }
})
export default class extends Vue {
  private editApiResource: ApiResource
  private apiResourceListCount: number
  private editApiResourceTitle: any
  private apiResourceList: ApiResource[]
  private apiResourceListLoading: boolean
  private apiResourceGetPagedFilter: ApiResourceGetByPaged

  private showEditApiScopeDialog: boolean
  private showEditApiSecretDialog: boolean
  private showEditApiResourceDialog: boolean

  constructor() {
    super()
    this.apiResourceListCount = 0
    this.editApiResourceTitle = ''
    this.apiResourceListLoading = false
    this.editApiResource = new ApiResource()
    this.apiResourceList = new Array<ApiResource>()
    this.apiResourceGetPagedFilter = new ApiResourceGetByPaged()

    this.showEditApiScopeDialog = false
    this.showEditApiSecretDialog = false
    this.showEditApiResourceDialog = false
  }

  mounted() {
    this.handleGetApiResources()
  }

  private handleGetApiResources() {
    this.apiResourceListLoading = true
    ApiResourceService.getApiResources(this.apiResourceGetPagedFilter).then(resources => {
      this.apiResourceList = resources.items
      this.apiResourceListCount = resources.totalCount
    }).finally(() => {
      this.apiResourceListLoading = false
    })
  }

  private handleSortChange(column: any) {
    this.apiResourceGetPagedFilter.sorting = column.prop
  }

  private handleShowEditApiResourceForm(resource: ApiResource) {
    if (resource) {
      this.editApiResource = resource
      this.editApiResourceTitle = this.l('identityServer.updateApiResourceByName', { name: this.editApiResource.name })
    } else {
      this.editApiResource = ApiResource.empty()
      this.editApiResourceTitle = this.l('identityServer.createApiResource')
    }
    this.showEditApiResourceDialog = true
  }

  private handleApiResourceEditFormClosed(changed: boolean) {
    this.editApiResourceTitle = ''
    this.editApiResource = ApiResource.empty()
    this.showEditApiResourceDialog = false
    if (changed) {
      this.handleGetApiResources()
    }
  }

  private handleApiSecretEditFormClosed() {
    this.showEditApiSecretDialog = false
    const frmApiSecret = this.$refs.formApiSecret as ApiSecretEditForm
    frmApiSecret.resetFields()
  }

  private handleApiScopeEditFormClosed() {
    this.showEditApiScopeDialog = false
    const frmApiScope = this.$refs.formApiScope as ApiScopeEditForm
    frmApiScope.resetFields()
  }

  private handleDeleteApiResource(id: string, name: string) {
    this.$confirm(this.l('identityServer.deleteApiResourceByName', { name: name }),
      this.l('identityServer.deleteApiResource'), {
        callback: (action) => {
          if (action === 'confirm') {
            ApiResourceService.deleteApiResource(id).then(() => {
              this.$message.success(this.l('identityServer.deleteApiResourceSuccess', { name: name }))
              this.handleGetApiResources()
            })
          }
        }
      })
  }

  private handleCommand(command: {key: string, row: ApiResource}) {
    this.editApiResource = command.row
    switch (command.key) {
      case 'secret' :
        this.showEditApiSecretDialog = true
        break
      case 'scope' :
        this.showEditApiScopeDialog = true
        break
      case 'delete' :
        this.handleDeleteApiResource(command.row.id, command.row.name)
        break
      default: break
    }
  }

  private l(name: string, values?: any[] | { [key: string]: any }) {
    return this.$t(name, values).toString()
  }

  private formatStatusText(status: boolean) {
    let statusText = ''
    if (status) {
      statusText = this.l('enabled')
    } else {
      statusText = this.l('disbled')
    }
    return statusText
  }
}
</script>

<style lang="scss" scoped>
.roleItem {
  width: 40px;
}
.options {
  vertical-align: top;
  margin-left: 20px;
}
.el-dropdown + .el-dropdown {
  margin-left: 15px;
}
.el-icon-arrow-down {
  font-size: 12px;
}
</style>
