<template>
  <div class="app-container">
    <div class="filter-container">
      <label
        class="radio-label"
        style="padding-left:10px;"
      >{{ $t('queryFilter') }}</label>
      <el-input
        v-model="dataFilter.filter"
        :placeholder="$t('filterString')"
        style="width: 250px;margin-left: 10px;"
        class="filter-item"
      />
      <el-button
        class="filter-item"
        style="margin-left: 10px; text-alignt"
        type="primary"
        @click="refreshPagedData"
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
      v-loading="dataLoading"
      row-key="id"
      :data="dataList"
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
      v-show="dataTotal>0"
      :total="dataTotal"
      :page.sync="currentPage"
      :limit.sync="pageSize"
      @pagination="refreshPagedData"
      @sort-change="handleSortChange"
    />

    <api-resource-create-or-edit-form
      :show-dialog="showEditApiResourceDialog"
      :title="editApiResourceTitle"
      :api-resource-id="editApiResource.id"
      @closed="handleApiResourceEditFormClosed"
    />

    <api-secret-edit-form
      :show-dialog="showEditApiSecretDialog"
      :api-resource-id="editApiResource.id"
      :api-secrets="editApiResource.secrets"
      @apiSecretChanged="refreshPagedData"
      @closed="handleApiSecretEditFormClosed"
    />

    <api-scope-edit-form
      :show-dialog="showEditApiScopeDialog"
      :api-resource-id="editApiResource.id"
      :api-scopes="editApiResource.scopes"
      @apiSecretChanged="refreshPagedData"
      @closed="handleApiScopeEditFormClosed"
    />
  </div>
</template>

<script lang="ts">
import { dateFormat, abpPagerFormat } from '@/utils/index'
import { checkPermission } from '@/utils/permission'
import DataListMiXin from '@/mixins/DataListMiXin'
import Component, { mixins } from 'vue-class-component'
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
export default class extends mixins(DataListMiXin) {
  private editApiResource = new ApiResource()
  private editApiResourceTitle = ''

  private showEditApiScopeDialog = false
  private showEditApiSecretDialog = false
  private showEditApiResourceDialog = false

  public dataFilter = new ApiResourceGetByPaged()

  mounted() {
    this.refreshPagedData()
  }

  protected processDataFilter() {
    this.dataFilter.skipCount = abpPagerFormat(this.currentPage, this.pageSize)
  }

  protected getPagedList(filter: any) {
    return ApiResourceService.getApiResources(filter)
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
    this.showEditApiResourceDialog = false
    if (changed) {
      this.refreshPagedData()
    }
  }

  private handleApiSecretEditFormClosed() {
    this.showEditApiSecretDialog = false
  }

  private handleApiScopeEditFormClosed() {
    this.showEditApiScopeDialog = false
  }

  private handleDeleteApiResource(id: string, name: string) {
    this.$confirm(this.l('identityServer.deleteApiResourceByName', { name: name }),
      this.l('identityServer.deleteApiResource'), {
        callback: (action) => {
          if (action === 'confirm') {
            ApiResourceService.deleteApiResource(id).then(() => {
              this.$message.success(this.l('identityServer.deleteApiResourceSuccess', { name: name }))
              this.refreshPagedData()
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
