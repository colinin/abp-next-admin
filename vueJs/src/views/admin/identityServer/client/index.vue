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
        :disabled="!checkPermission(['IdentityServer.Clients.Create'])"
        @click="handleShowCreateClientForm()"
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
            :disabled="!checkPermission(['IdentityServer.Clients.Update'])"
            size="mini"
            type="primary"
            @click="handleShowEditClientForm(row)"
          >
            {{ $t('AbpIdentityServer.Client:Edit') }}
          </el-button>
          <el-dropdown
            class="options"
            @command="handleCommand"
          >
            <el-button
              v-permission="['IdentityServer.Clients']"
              size="mini"
              type="info"
            >
              {{ $t('global.operaActions') }}<i class="el-icon-arrow-down el-icon--right" />
            </el-button>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                :command="{key: 'permissions', row}"
                :disabled="!checkPermission(['IdentityServer.Clients.ManagePermissions'])"
              >
                {{ $t('AbpIdentityServer.Permissions') }}
              </el-dropdown-item>
              <el-dropdown-item
                :command="{key: 'clone', row}"
                :disabled="!checkPermission(['IdentityServer.Clients.Clone'])"
              >
                {{ $t('AbpIdentityServer.Client:Clone') }}
              </el-dropdown-item>
              <el-dropdown-item
                divided
                :command="{key: 'delete', row}"
                :disabled="!checkPermission(['IdentityServer.Clients.Delete'])"
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

    <permission-form
      provider-name="C"
      :provider-key="editClientId"
      :show-dialog="showEditClientPermissionDialog"
      :readonly="!checkPermission(['IdentityServer.Clients.ManagePermissions'])"
      @closed="handleClientPermissionEditFormClosed"
    />
  </div>
</template>

<script lang="ts">
import { abpPagerFormat } from '@/utils/index'
import { checkPermission } from '@/utils/permission'
import DataListMiXin from '@/mixins/DataListMiXin'
import Component, { mixins } from 'vue-class-component'
import Pagination from '@/components/Pagination/index.vue'
import ClientService, { Client, ClientGetByPaged } from '@/api/clients'

import PermissionForm from '@/components/PermissionForm/index.vue'

@Component({
  name: 'IdentityServerClient',
  components: {
    Pagination,
    PermissionForm
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
export default class extends mixins(DataListMiXin) {
  private editClientId = ''
  private editClientTitle = ''

  private showEditClientDialog = false
  private showCloneClientDialog = false
  private showCreateClientDialog = false
  private showEditClientPermissionDialog = false

  public dataFilter = new ClientGetByPaged()

  mounted() {
    this.refreshPagedData()
  }

  protected processDataFilter() {
    this.dataFilter.skipCount = abpPagerFormat(this.currentPage, this.pageSize)
  }

  protected getPagedList(filter: any) {
    return ClientService.getClients(filter)
  }

  private handleShowCreateClientForm() {
    this.editClientTitle = this.l('AbpIdentityServer.Client:New')
    this.showCreateClientDialog = true
  }

  private handleShowEditClientForm(client: Client) {
    this.editClientId = client.clientId
    this.showEditClientDialog = true
  }

  private handleClientCreateFormClosed(changed: boolean) {
    this.showCreateClientDialog = false
    if (changed) {
      this.refreshPagedData()
    }
  }

  private handleClientCloneFormClosed(changed: boolean) {
    this.showCloneClientDialog = false
    if (changed) {
      this.refreshPagedData()
    }
  }

  private handleClientEditFormClosed(changed: boolean) {
    this.showEditClientDialog = false
    if (changed) {
      this.refreshPagedData()
    }
  }

  private handleClientPermissionEditFormClosed() {
    this.showEditClientPermissionDialog = false
  }

  private handleDeleteClient(id: string, clientId: string) {
    this.$confirm(this.l('AbpIdentityServer.Client:WillDelete', { 0: clientId }),
      this.l('AbpUi.AreYouSure'), {
        callback: (action) => {
          if (action === 'confirm') {
            ClientService.deleteClient(id).then(() => {
              this.$message.success(this.l('global.successful'))
              this.refreshPagedData()
            })
          }
        }
      })
  }

  private handleCommand(command: {key: string, row: Client}) {
    this.editClientId = command.row.clientId
    switch (command.key) {
      case 'clone' :
        this.showCloneClientDialog = true
        break
      case 'permissions':
        this.showEditClientPermissionDialog = true
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
