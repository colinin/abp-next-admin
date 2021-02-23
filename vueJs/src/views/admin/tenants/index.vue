<template>
  <div class="app-container">
    <div class="filter-container">
      <label
        class="radio-label"
        style="padding-left:10px;"
      >{{ $t('global.queryFilter') }}</label>
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
      <el-button
        v-if="isHost"
        class="filter-item"
        type="primary"
        :disabled="!checkPermission(['FeatureManagement.ManageHostFeatures'])"
        @click="handleManageHostFeatures"
      >
        {{ $t('AbpTenantManagement.ManageHostFeatures') }}
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
                :command="{key: 'feature', row}"
                :disabled="!checkPermission(['AbpTenantManagement.Tenants.ManageFeatures'])"
              >
                管理功能
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
      v-show="dataTotal>0"
      :total="dataTotal"
      :page.sync="currentPage"
      :limit.sync="pageSize"
      @pagination="refreshPagedData"
      @sort-change="handleSortChange"
    />

    <tenant-create-or-edit-form
      :show-dialog="showCreateOrEditTenantDialog"
      :tenant-id="editTenantId"
      @closed="handleCreateOrEditTenantFormClosed"
    />

    <tenant-connection-edit-form
      :show-dialog="showEditTenantConnectionDialog"
      :tenant-id="editTenantId"
      @closed="handleTenantConnectionEditFormClosed"
    />

    <!-- <tenant-feature-edit-form
      :show-dialog="showFeatureEditFormDialog"
      :tenant-id="editTenantId"
      @closed="onFeatureEditFormClosed"
    />

    <host-feature-edit-form
      v-if="isHost"
      :show-dialog="showHostFeatureDialog"
      @closed="showHostFeatureDialog=false"
    /> -->

    <el-dialog
      :visible="showFeatureDialog"
      :title="featureManagementTitle"
      width="800px"
      custom-class="modal-form"
      :show-close="false"
      @close="showFeatureDialog=false"
    >
      <feature-management
        :provider-name="featureProviderName"
        :provider-key="featureProviderKey"
        :load-feature="showFeatureDialog"
        @closed="showFeatureDialog=false"
      />
    </el-dialog>
  </div>
</template>

<script lang="ts">
import { AbpModule } from '@/store/modules/abp'
import DataListMiXin from '@/mixins/DataListMiXin'
import Component, { mixins } from 'vue-class-component'
import TenantService, { TenantDto, TenantGetByPaged } from '@/api/tenant-management'
import { dateFormat, abpPagerFormat } from '@/utils/index'
import { checkPermission } from '@/utils/permission'
import Pagination from '@/components/Pagination/index.vue'
import FeatureManagement from '../components/FeatureManagement.vue'
import TenantFeatureEditForm from './components/TenantFeatureEditForm.vue'
import TenantCreateOrEditForm from './components/TenantCreateOrEditForm.vue'
import TenantConnectionEditForm from './components/TenantConnectionEditForm.vue'

@Component({
  name: 'RoleList',
  components: {
    Pagination,
    FeatureManagement,
    TenantFeatureEditForm,
    TenantCreateOrEditForm,
    TenantConnectionEditForm
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
export default class extends mixins(DataListMiXin) {
  private editTenantId = ''
  private showEditTenantConnectionDialog = false
  private showCreateOrEditTenantDialog = false
  private showFeatureEditFormDialog = false

  private showFeatureDialog = false
  private featureManagementTitle = ''
  private featureProviderName = ''
  private featureProviderKey = ''

  public dataFilter = new TenantGetByPaged()

  get isHost() {
    if (!AbpModule.configuration) {
      return true
    }
    return !AbpModule.configuration.currentTenant.isAvailable
  }

  mounted() {
    this.refreshPagedData()
  }

  protected processDataFilter() {
    this.dataFilter.skipCount = abpPagerFormat(this.currentPage, this.pageSize)
  }

  protected getPagedList(filter: any) {
    return TenantService.getTenants(filter)
  }

  private handleCommand(command: {key: string, row: TenantDto}) {
    switch (command.key) {
      case 'connection' :
        this.editTenantId = command.row.id
        this.showEditTenantConnectionDialog = true
        break
      case 'feature' :
        this.featureProviderName = 'T'
        this.featureProviderKey = command.row.id
        this.featureManagementTitle = this.l('AbpTenantManagement.Permission:ManageFeatures')
        this.showFeatureDialog = true
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
              this.refreshPagedData()
            })
          }
        }
      })
  }

  private handleTenantConnectionEditFormClosed(changed: boolean) {
    this.showEditTenantConnectionDialog = false
    this.editTenantId = ''
    if (changed) {
      this.refreshPagedData()
    }
  }

  private handleCreateOrEditTenantFormClosed(changed: boolean) {
    this.showCreateOrEditTenantDialog = false
    this.editTenantId = ''
    if (changed) {
      this.refreshPagedData()
    }
  }

  private handleManageHostFeatures() {
    this.featureProviderName = 'T'
    this.featureProviderKey = ''
    this.featureManagementTitle = this.l('AbpTenantManagement.ManageHostFeatures')
    this.showFeatureDialog = true
  }

  private onFeatureEditFormClosed() {
    this.showFeatureDialog = false
    this.editTenantId = ''
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
