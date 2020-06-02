<template>
  <div class="app-container">
    <div class="filter-container">
      <label
        class="radio-label"
        style="padding-left:10px;"
      >{{ $t('global.queryFilter') }}</label>
      <el-input
        v-model="tenantGetPagedFilter.filter"
        :placeholder="$t('filterString')"
        style="width: 250px;margin-left: 10px;"
        class="filter-item"
      />
      <el-button
        class="filter-item"
        style="margin-left: 10px; text-alignt"
        type="primary"
        @click="handleGetTenants"
      >
        {{ $t('global.searchList') }}
      </el-button>
      <el-button
        class="filter-item"
        type="primary"
        :disabled="!checkPermission(['AbpTenantManagement.Tenants.Create'])"
        @click="handleShowCreateOrEditTenantDialog('')"
      >
        {{ $t('tenant.createTenant') }}
      </el-button>
    </div>

    <el-table
      v-loading="tenantListLoading"
      row-key="id"
      :data="tenantList"
      border
      fit
      highlight-current-row
      style="width: 100%;"
      @sort-change="handleSortChange"
    >
      <el-table-column
        :label="$t('tenant.id')"
        prop="id"
        sortable
        width="310px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.id }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('tenant.name')"
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
        width="230px"
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
        min-width="250px"
        fixed="right"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!checkPermission(['AbpTenantManagement.Tenants.Update'])"
            size="mini"
            type="primary"
            @click="handleShowCreateOrEditTenantDialog(row.id)"
          >
            {{ $t('tenant.updateTenant') }}
          </el-button>
          <el-dropdown
            class="options"
            @command="handleCommand"
          >
            <el-button
              v-permission="['AbpTenantManagement.Tenants']"
              size="mini"
              type="info"
            >
              {{ $t('global.otherOpera') }}<i class="el-icon-arrow-down el-icon--right" />
            </el-button>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                :command="{key: 'connection', row}"
                :disabled="!checkPermission(['AbpTenantManagement.Tenants.ManageConnectionStrings'])"
              >
                {{ $t('tenant.connectionOptions') }}
              </el-dropdown-item>
              <el-dropdown-item
                divided
                :command="{key: 'delete', row}"
                :disabled="!checkPermission(['AbpTenantManagement.Tenants.Delete'])"
              >
                {{ $t('tenant.deleteTenant') }}
              </el-dropdown-item>
            </el-dropdown-menu>
          </el-dropdown>
        </template>
      </el-table-column>
    </el-table>

    <Pagination
      v-show="tenantListCount>0"
      :total="tenantListCount"
      :page.sync="tenantGetPagedFilter.skipCount"
      :limit.sync="tenantGetPagedFilter.maxResultCount"
      @pagination="handleGetTenants"
      @sort-change="handleSortChange"
    />

    <el-dialog
      v-el-draggable-dialog
      width="800px"
      :visible.sync="showCreateOrEditTenantDialog"
      :title="$t('tenant.updateTenant')"
      custom-class="modal-form"
      :show-close="false"
      @closed="handleCreateOrEditTenantFormClosed"
    >
      <TenantCreateOrEditForm
        ref="formCreateOrEditTenant"
        :tenant-id="editTenantId"
        @closed="handleCreateOrEditTenantFormClosed"
      />
    </el-dialog>

    <el-dialog
      v-el-draggable-dialog
      width="800px"
      :visible.sync="showEditTenantConnectionDialog"
      :title="$t('tenant.connectionOptions')"
      custom-class="modal-form"
      :show-close="false"
      @closed="handleTenantConnectionEditFormClosed"
    >
      <TenantEditConnectionForm
        ref="formEditTenantConnection"
        :tenant-id="editTenantId"
        @closed="handleTenantConnectionEditFormClosed"
      />
    </el-dialog>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import TenantService, { TenantDto, TenantGetByPaged } from '@/api/tenant'
import { dateFormat } from '@/utils/index'
import { checkPermission } from '@/utils/permission'
import Pagination from '@/components/Pagination/index.vue'
import TenantCreateOrEditForm from './components/TenantCreateOrEditForm.vue'
import TenantEditConnectionForm from './components/TenantEditConnectionForm.vue'

@Component({
  name: 'RoleList',
  components: {
    Pagination,
    TenantCreateOrEditForm,
    TenantEditConnectionForm
  },
  methods: {
    checkPermission
  },
  filters: {
    datetimeFilter(val: string) {
      const date = new Date(val)
      return dateFormat(date, 'YYYY-mm-dd HH:MM')
    }
  }
})
export default class extends Vue {
  private editTenantId: string
  private tenantList: TenantDto[]
  private tenantListCount: number
  private tenantListLoading: boolean
  private tenantGetPagedFilter: TenantGetByPaged

  private showEditTenantConnectionDialog: boolean
  private showCreateOrEditTenantDialog: boolean

  constructor() {
    super()
    this.editTenantId = ''
    this.tenantListCount = 0
    this.tenantListLoading = false
    this.tenantList = new Array<TenantDto>()
    this.tenantGetPagedFilter = new TenantGetByPaged()

    this.showCreateOrEditTenantDialog = false
    this.showEditTenantConnectionDialog = false
  }

  mounted() {
    this.handleGetTenants()
  }

  private handleGetTenants() {
    this.tenantListLoading = true
    TenantService.getTenants(this.tenantGetPagedFilter).then(tenants => {
      this.tenantList = tenants.items
      this.tenantListCount = tenants.totalCount
      this.tenantListLoading = false
    })
  }

  private handleCommand(command: {key: string, row: TenantDto}) {
    switch (command.key) {
      case 'connection' :
        this.editTenantId = command.row.id
        this.showEditTenantConnectionDialog = true
        break
      case 'delete' :
        this.handleDeleteTenant(command.row.id, command.row.name)
        break
      default: break
    }
  }

  private handleShowCreateOrEditTenantDialog(id: string) {
    this.editTenantId = id
    this.showCreateOrEditTenantDialog = true
  }

  private handleDeleteTenant(id: string, name: string) {
    this.$confirm(this.l('tenant.deleteTenantByName', { name: name }),
      this.l('tenant.deleteTenant'), {
        callback: (action) => {
          if (action === 'confirm') {
            TenantService.deleteTenant(id).then(() => {
              this.$message.success(this.l('tenant.deleteTenantSuccess', { name: name }))
              this.handleGetTenants()
            })
          }
        }
      })
  }

  private handleTenantConnectionEditFormClosed(changed: boolean) {
    this.showEditTenantConnectionDialog = false
    this.editTenantId = ''
    if (changed) {
      this.handleGetTenants()
    }
  }

  private handleCreateOrEditTenantFormClosed(changed: boolean) {
    this.showCreateOrEditTenantDialog = false
    this.editTenantId = ''
    if (changed) {
      this.handleGetTenants()
    }
  }

  private handleSortChange(column: any) {
    this.tenantGetPagedFilter.sorting = column.prop
  }

  private l(name: string, values?: any[] | { [key: string]: any }) {
    return this.$t(name, values).toString()
  }
}
</script>

<style lang="scss" scoped>
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
