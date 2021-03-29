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
        {{ $t('AbpIdentityServer.Search') }}
      </el-button>
      <el-button
        class="filter-item"
        type="primary"
        :disabled="!checkPermission(['AbpIdentityServer.Clients.Create'])"
        @click="handleShowCreateClientDialog()"
      >
        {{ $t('AbpIdentityServer.Client:New') }}
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
        :label="$t('AbpIdentityServer.Client:Id')"
        prop="clientId"
        sortable
        width="150px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.clientId }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Name')"
        prop="clientName"
        sortable
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.clientName }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Description')"
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
        :label="$t('AbpIdentityServer.Client:Enabled')"
        prop="enabled"
        sortable
        width="140px"
        align="center"
      >
        <template slot-scope="{row}">
          <el-switch
            v-model="row.enabled"
            disabled
          />
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Client:ProtocolType')"
        prop="protocolType"
        sortable
        width="120px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.protocolType }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Client:IdentityTokenLifetime')"
        prop="identityTokenLifetime"
        width="170px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.identityTokenLifetime }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Client:AccessTokenLifetime')"
        prop="accessTokenLifetime"
        width="170px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.accessTokenLifetime }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Client:AuthorizationCodeLifetime')"
        prop="authorizationCodeLifetime"
        width="170px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.authorizationCodeLifetime }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Client:DeviceCodeLifetime')"
        prop="deviceCodeLifetime"
        width="170px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.deviceCodeLifetime }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Client:AbsoluteRefreshTokenLifetime')"
        prop="absoluteRefreshTokenLifetime"
        width="180px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.absoluteRefreshTokenLifetime }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Client:SlidingRefreshTokenLifetime')"
        prop="slidingRefreshTokenLifetime"
        width="180px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.slidingRefreshTokenLifetime }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentityServer.Client:ClientClaimsPrefix')"
        prop="clientClaimsPrefix"
        width="120px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.clientClaimsPrefix }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('operaActions')"
        align="center"
        width="250px"
        fixed="right"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!checkPermission(['AbpIdentityServer.Clients.Update'])"
            size="mini"
            type="primary"
            @click="handleShowEditClientDialog(row)"
          >
            {{ $t('AbpIdentityServer.Client:Edit') }}
          </el-button>
          <el-dropdown
            class="options"
            @command="handleCommand"
          >
            <el-button
              v-permission="['AbpIdentityServer.Clients']"
              size="mini"
              type="info"
            >
              {{ $t('global.operaActions') }}<i class="el-icon-arrow-down el-icon--right" />
            </el-button>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                :command="{key: 'permissions', row}"
                :disabled="!checkPermission(['AbpIdentityServer.Clients.ManagePermissions'])"
              >
                {{ $t('AbpIdentityServer.Permissions') }}
              </el-dropdown-item>
              <el-dropdown-item
                v-permission="['FeatureManagement.ManageHostFeatures']"
                :command="{key: 'features', row}"
              >
                {{ $t('AbpFeatureManagement.ManageFeatures') }}
              </el-dropdown-item>
              <el-dropdown-item
                :command="{key: 'clone', row}"
                :disabled="!checkPermission(['AbpIdentityServer.Clients.Clone'])"
              >
                {{ $t('AbpIdentityServer.Client:Clone') }}
              </el-dropdown-item>
              <el-dropdown-item
                divided
                :command="{key: 'delete', row}"
                :disabled="!checkPermission(['AbpIdentityServer.Clients.Delete'])"
              >
                {{ $t('AbpIdentityServer.Client:Delete') }}
              </el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
        </template>
      </el-table-column>
    </el-table>

    <pagination
      v-show="dataTotal>0"
      :total="dataTotal"
      :page.sync="currentPage"
      :limit.sync="pageSize"
      @pagination="refreshPagedData"
      @sort-change="handleSortChange"
    />

    <client-clone-form
      :client-id="editClient.id"
      :show-dialog="showCloneClientDialog"
      @closed="onCloneClientDialogClosed"
    />

    <permission-form
      provider-name="C"
      :provider-key="editClient.clientId"
      :show-dialog="showEditClientPermissionDialog"
      :readonly="!checkPermission(['AbpIdentityServer.Clients.ManagePermissions'])"
      @closed="onPermissionDialogClosed"
    />

    <client-create-form
      :supported-grantypes="supportedGrantypes"
      :show-dialog="showCreateClientDialog"
      @closed="onCreateClientDialogClosed"
    />

    <client-edit-form
      :supported-grantypes="supportedGrantypes"
      :client-id="editClient.id"
      :title="editClientTitle"
      :show-dialog="showEditClientDialog"
      @closed="onEditClientDialogClosed"
    />

    <client-feature-edit-form
      :show-dialog="showManageClientFeatureDialog"
      :client-id="editClient.clientId"
      @closed="showManageClientFeatureDialog=false"
    />
  </div>
</template>

<script lang="ts">
import { abpPagerFormat } from '@/utils/index'
import { checkPermission } from '@/utils/permission'

import DataListMiXin from '@/mixins/DataListMiXin'
import HttpProxyMiXin from '@/mixins/HttpProxyMiXin'
import Component, { mixins } from 'vue-class-component'
import Pagination from '@/components/Pagination/index.vue'
import ClientCloneForm from './components/ClientCloneForm.vue'
import PermissionForm from '@/components/PermissionForm/index.vue'
import ClientCreateForm from './components/ClientCreateForm.vue'
import ClientEditForm from './components/ClientEditForm.vue'
import ClientFeatureEditForm from './components/ClientFeatureEditForm.vue'

import IdentityServer4Service from '@/api/identity-server4'
import ClientService, { Client, ClientGetByPaged } from '@/api/clients'

@Component({
  name: 'IdentityServerClient',
  components: {
    Pagination,
    PermissionForm,
    ClientEditForm,
    ClientCloneForm,
    ClientCreateForm,
    ClientFeatureEditForm
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
    }
  }
})
export default class extends mixins(DataListMiXin, HttpProxyMiXin) {
  private editClient = new Client()
  private editClientTitle = ''

  private showEditClientDialog = false
  private showCreateClientDialog = false
  private showCloneClientDialog = false
  private showEditClientPermissionDialog = false
  private showManageClientFeatureDialog = false

  public dataFilter = new ClientGetByPaged()
  private supportedGrantypes = new Array<string>()

  mounted() {
    this.refreshPagedData()
    this.handleGetOpenIdConfiguration()
  }

  protected processDataFilter() {
    this.dataFilter.skipCount = abpPagerFormat(this.currentPage, this.pageSize)
  }

  protected getPagedList(filter: any) {
    return this.pagedRequest<Client>({
      service: 'IdentityServer',
      controller: 'Client',
      action: 'GetListAsync',
      params: {
        input: filter
      }
    })
    // return ClientService.getList(filter)
  }

  private handleGetOpenIdConfiguration() {
    IdentityServer4Service.getOpenIdConfiguration()
      .then(res => {
        this.supportedGrantypes = res.grant_types_supported
      })
  }

  private handleShowCreateClientDialog() {
    this.showCreateClientDialog = true
  }

  private handleShowEditClientDialog(client: Client) {
    this.editClient = client
    this.editClientTitle = this.l('AbpIdentityServer.Client:Name', { 0: this.editClient.clientName })
    this.showEditClientDialog = true
  }

  private onEditClientDialogClosed() {
    this.showEditClientDialog = false
  }

  private onCloneClientDialogClosed(changed: boolean) {
    this.showCloneClientDialog = false
    if (changed) {
      this.refreshPagedData()
    }
  }

  private onCreateClientDialogClosed(changed: boolean) {
    this.showCreateClientDialog = false
    if (changed) {
      this.refreshPagedData()
    }
  }

  private onPermissionDialogClosed() {
    this.showEditClientPermissionDialog = false
  }

  private handleDeleteClient(id: string, clientId: string) {
    this.$confirm(this.l('AbpIdentityServer.Client:WillDelete', { 0: clientId }),
      this.l('AbpUi.AreYouSure'), {
        callback: (action) => {
          if (action === 'confirm') {
            ClientService
              .delete(id)
              .then(() => {
                this.$message.success(this.l('global.successful'))
                this.refreshPagedData()
              })
          }
        }
      })
  }

  private handleCommand(command: {key: string, row: Client}) {
    this.editClient = command.row
    switch (command.key) {
      case 'clone' :
        this.showCloneClientDialog = true
        break
      case 'permissions':
        this.showEditClientPermissionDialog = true
        break
      case 'features':
        this.showManageClientFeatureDialog = true
        break
      case 'delete' :
        this.handleDeleteClient(command.row.id, command.row.clientId)
        break
      default: break
    }
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
