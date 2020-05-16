<template>
  <div class="app-container">
    <div class="filter-container">
      <label
        class="radio-label"
        style="padding-left:10px;"
      >{{ $t('queryFilter') }}</label>
      <el-input
        v-model="clientGetPagedFilter.filter"
        :placeholder="$t('filterString')"
        style="width: 250px;margin-left: 10px;"
        class="filter-item"
      />
      <el-button
        class="filter-item"
        style="margin-left: 10px; text-alignt"
        type="primary"
        @click="handleGetClients"
      >
        {{ $t('searchList') }}
      </el-button>
      <el-button
        class="filter-item"
        type="primary"
        :disabled="!checkPermission(['IdentityServer.Clients.Create'])"
        @click="handleShowEditClientForm()"
      >
        {{ $t('identityServer.createClient') }}
      </el-button>
    </div>

    <el-table
      v-loading="clientListLoading"
      row-key="itemId"
      :data="clientList"
      border
      fit
      highlight-current-row
      style="width: 100%;"
      @sort-change="handleSortChange"
    >
      <el-table-column
        :label="$t('identityServer.clientId')"
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
        :label="$t('identityServer.clientName')"
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
        :label="$t('identityServer.identityTokenLifetime')"
        prop="identityTokenLifetime"
        width="170px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.identityTokenLifetime }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('identityServer.accessTokenLifetime')"
        prop="accessTokenLifetime"
        width="170px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.accessTokenLifetime }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('identityServer.authorizationCodeLifetime')"
        prop="authorizationCodeLifetime"
        width="170px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.authorizationCodeLifetime }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('identityServer.deviceCodeLifetime')"
        prop="deviceCodeLifetime"
        width="170px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.deviceCodeLifetime }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('identityServer.absoluteRefreshTokenLifetime')"
        prop="absoluteRefreshTokenLifetime"
        width="180px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.absoluteRefreshTokenLifetime }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('identityServer.slidingRefreshTokenLifetime')"
        prop="slidingRefreshTokenLifetime"
        width="180px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.slidingRefreshTokenLifetime }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('identityServer.clientClaimsPrefix')"
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
            @click="handleShowEditClientForm(row.id, row.clientName)"
          >
            {{ $t('identityServer.updateClient') }}
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
              {{ $t('identityServer.otherOpera') }}<i class="el-icon-arrow-down el-icon--right" />
            </el-button>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                :command="row.enabled ? {key: 'disbled', row} : {key: 'enabled', row}"
                :disabled="!checkPermission(['IdentityServer.Clients.Enabled'])"
              >
                {{ row.enabled ? $t('identityServer.disbled') : $t('roles.enabled') }}
              </el-dropdown-item>
              <el-dropdown-item
                divided
                :command="{key: 'delete', row}"
                :disabled="row.enabled || !checkPermission(['IdentityServer.Clients.Delete'])"
              >
                {{ $t('identityServer.deleteClient') }}
              </el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
        </template>
      </el-table-column>
    </el-table>

    <Pagination
      v-show="clientListCount>0"
      :total="clientListCount"
      :page.sync="clientGetPagedFilter.skipCount"
      :limit.sync="clientGetPagedFilter.maxResultCount"
      @pagination="handleGetClients"
      @sort-change="handleSortChange"
    />

    <el-dialog
      v-el-draggable-dialog
      width="800px"
      :visible.sync="showEditClientDialog"
      :title="editClientTitle"
      custom-class="modal-form"
      :show-close="false"
    >
      <ClientEditForm
        :client-id="editClientId"
        @closed="handleClientEditFormClosed"
      />
    </el-dialog>
  </div>
</template>

<script lang="ts">
import { checkPermission } from '@/utils/permission'
import { Component, Vue } from 'vue-property-decorator'
import Pagination from '@/components/Pagination/index.vue'
import ClientEditForm from './components/ClientEditForm.vue'
import ClientService, { Client, ClientGetByPaged } from '@/api/clients'

@Component({
  name: 'IdentityServerClient',
  components: {
    Pagination,
    ClientEditForm
  },
  methods: {
    checkPermission
  }
})
export default class extends Vue {
  private editClientId: string
  private clientListCount: number
  private editClientTitle: any
  private clientList: Client[]
  private clientListLoading: boolean
  private showEditClientDialog: boolean
  private clientGetPagedFilter: ClientGetByPaged

  constructor() {
    super()
    this.editClientId = ''
    this.clientListCount = 0
    this.editClientTitle = ''
    this.clientListLoading = false
    this.showEditClientDialog = false
    this.clientList = new Array<Client>()
    this.clientGetPagedFilter = new ClientGetByPaged()
  }

  mounted() {
    this.handleGetClients()
  }

  private handleGetClients() {
    this.clientListLoading = true
    ClientService.getClients(this.clientGetPagedFilter).then(routes => {
      this.clientList = routes.items
      this.clientListCount = routes.totalCount
    }).finally(() => {
      this.clientListLoading = false
    })
  }

  private handleSortChange(column: any) {
    this.clientGetPagedFilter.sorting = column.prop
  }

  private handleShowEditClientForm(id: string, clientName: string) {
    this.editClientId = id
    this.editClientTitle = this.$t('identityServer.createClient')
    if (id) {
      this.editClientTitle = this.$t('identityServer.updateClientByName', { name: clientName })
    }
    this.showEditClientDialog = true
  }

  private handleClientEditFormClosed(changed: boolean) {
    this.editClientId = ''
    this.editClientTitle = ''
    this.showEditClientDialog = false
    if (changed) {
      this.handleGetClients()
    }
  }

  private handleDeleteClient(id: string) {
    console.log('handleDeleteClient:' + id)
  }

  private handleCommand(command: {key: string, row: Client}) {
    switch (command.key) {
      case 'disbled' :
        console.log('disbled')
        break
      case 'enabled' :
        console.log('enabled')
        break

      case 'delete' :
        this.handleDeleteClient(command.row.id)
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
