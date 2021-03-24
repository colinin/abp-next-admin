<template>
  <div class="app-container">
    <div class="filter-container">
      <el-button
        class="filter-item"
        style="margin-left: 10px; text-alignt"
        type="primary"
        @click="refreshPagedData"
      >
        {{ $t('roles.refreshList') }}
      </el-button>
      <el-button
        class="filter-item"
        type="primary"
        :disabled="!checkPermission(['AbpIdentity.Roles.Create'])"
        @click="handleCreateRole"
      >
        {{ $t('roles.createRole') }}
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
      size="small"
    >
      <el-table-column
        :label="$t('roles.id')"
        prop="id"
        sortable
        width="300px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.id }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('roles.name')"
        prop="name"
        sortable
        width="350px"
        min-width="350px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.name }}</span>

          <el-tag
            v-if="row.isDefault"
            type="success"
          >
            {{ $t('roles.isDefault') }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('roles.isPublic')"
        prop="isPublic"
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <el-tag
            :type="row.isPublic ? 'success' : 'warning'"
          >
            {{ row.isPublic ? $t('roles.isPublic') : $t('roles.isPrivate') }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('roles.type')"
        prop="isStatic"
        width="200px"
        align="center"
      >
        <template slot-scope="{row}">
          <el-tag
            :type="row.isStatic ? 'info' : 'success'"
          >
            {{ row.isStatic ? $t('roles.system') : $t('roles.custom') }}
          </el-tag>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('roles.operaActions')"
        align="center"
        width="250px"
        min-width="250px"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!checkPermission(['AbpIdentity.Roles.Update'])"
            size="mini"
            type="primary"
            @click="handleEditRole(row)"
          >
            {{ $t('roles.updateRole') }}
          </el-button>
          <el-dropdown
            class="options"
            @command="handleCommand"
          >
            <el-button
              v-permission="['AbpIdentity.Roles']"
              size="mini"
              type="info"
            >
              {{ $t('roles.otherOpera') }}<i class="el-icon-arrow-down el-icon--right" />
            </el-button>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                :command="{key: 'claim', row}"
                :disabled="!checkPermission(['AbpIdentity.Roles.ManageClaims'])"
              >
                {{ $t('AbpIdentity.ManageClaim') }}
              </el-dropdown-item>
              <el-dropdown-item
                :command="{key: 'menu', row}"
                :disabled="!checkPermission(['Platform.Menu.ManageRoles'])"
              >
                {{ $t('AppPlatform.Menu:Manage') }}
              </el-dropdown-item>
              <el-dropdown-item
                :command="{key: 'permission', row}"
                :disabled="!checkPermission(['AbpIdentity.Roles.ManagePermissions'])"
              >
                {{ $t('AbpIdentity.Permissions') }}
              </el-dropdown-item>
              <el-dropdown-item
                :command="row.isDefault ? {key: 'unDefault', row} : {key: 'default', row}"
                :disabled="row.isStatic || !checkPermission(['AbpIdentity.Roles.Update'])"
              >
                {{ row.isDefault ? $t('roles.unSetDefault') : $t('roles.setDefault') }}
              </el-dropdown-item>
              <el-dropdown-item
                divided
                :command="{key: 'delete', row}"
                :disabled="row.isStatic || !checkPermission(['AbpIdentity.Roles.Delete'])"
              >
                {{ $t('roles.deleteRole') }}
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
    />

    <role-edit-form
      :show-dialog="showEditDialog"
      :role-id="editRoleId"
      @closed="onEditRoleFormClosed"
    />

    <role-create-form
      :show-dialog="showCreateDialog"
      @closed="onCreateDialogClosed"
    />

    <role-claim-create-or-update-form
      :show-dialog="showClaimDialog"
      :role-id="editRoleId"
      @closed="onClaimDialogClosed"
    />

    <manage-role-menu-dialog
      :show-dialog="showManageRoleMenuDialog"
      :role-name="editRoleName"
      @closed="showManageRoleMenuDialog=false"
    />

    <permission-form
      provider-name="R"
      :provider-key="editRoleName"
      :readonly="!checkPermission(['AbpIdentity.Roles.ManagePermissions'])"
      :show-dialog="showPermissionDialog"
      @closed="onPermissionDialogClosed"
    />
  </div>
</template>

<script lang="ts">
import { abpPagerFormat } from '@/utils'
import DataListMiXin from '@/mixins/DataListMiXin'
import Component, { mixins } from 'vue-class-component'
import RoleService, { RoleDto, UpdateRoleDto, RoleGetPagedDto } from '@/api/roles'
import { checkPermission } from '@/utils/permission'
import Pagination from '@/components/Pagination/index.vue'
import PermissionForm from '@/components/PermissionForm/index.vue'
import RoleEditForm from './components/RoleEditForm.vue'
import RoleCreateForm from './components/RoleCreateForm.vue'
import ManageRoleMenuDialog from './components/ManageRoleMenuDialog.vue'
import RoleClaimCreateOrUpdateForm from './components/RoleClaimCreateOrUpdateForm.vue'

@Component({
  name: 'RoleList',
  components: {
    PermissionForm,
    Pagination,
    RoleEditForm,
    RoleCreateForm,
    ManageRoleMenuDialog,
    RoleClaimCreateOrUpdateForm
  },
  methods: {
    checkPermission
  }
})
export default class extends mixins(DataListMiXin) {
  private showEditDialog = false
  private showManageRoleMenuDialog = false
  private editRoleId = ''
  private showClaimDialog = false
  private showCreateDialog = false
  private editRoleName = ''
  private showPermissionDialog = false

  public dataFilter = new RoleGetPagedDto()

  mounted() {
    this.refreshPagedData()
  }

  protected processDataFilter() {
    this.dataFilter.skipCount = abpPagerFormat(this.currentPage, this.pageSize)
  }

  /** 获取角色权限列表 */
  protected getPagedList(filter: any) {
    return RoleService.getRoles(filter)
  }

  /** 响应角色行操作事件 */
  private handleCommand(command: {key: string, row: RoleDto}) {
    switch (command.key) {
      case 'claim' :
        this.handleShowCliamDialog(command.row)
        break
      case 'permission' :
        this.handleShowPermissionDialog(command.row)
        break
      case 'default' :
        this.handleSetDefaultRole(command.row, true)
        break
      case 'unDefault' :
        this.handleSetDefaultRole(command.row, false)
        break
      case 'delete' :
        this.handleDeleteRole(command.row)
        break
      case 'menu' :
        this.editRoleName = command.row.name
        this.showManageRoleMenuDialog = true
        break
      default: break
    }
  }

  /** 新建角色 */
  private handleCreateRole() {
    this.showCreateDialog = true
  }

  private handleEditRole(role: RoleDto) {
    this.editRoleId = role.id
    this.showEditDialog = true
  }

  private handleShowCliamDialog(row: RoleDto) {
    this.editRoleId = row.id
    this.showClaimDialog = true
  }

  private handleShowPermissionDialog(row: RoleDto) {
    this.editRoleName = row.name
    this.showPermissionDialog = true
  }

  private onPermissionDialogClosed() {
    this.showPermissionDialog = false
  }

  private onClaimDialogClosed() {
    this.showClaimDialog = false
  }

  private onCreateDialogClosed(changed: boolean) {
    this.showCreateDialog = false
    if (changed) {
      this.refreshPagedData()
    }
  }

  /** 设置默认角色 */
  private handleSetDefaultRole(role: RoleDto, setDefault: boolean) {
    // console.log('handleSetDefaultRole:' + role.id)
    const setDefaultRoleDto = new UpdateRoleDto()
    setDefaultRoleDto.name = role.name
    setDefaultRoleDto.isDefault = setDefault
    setDefaultRoleDto.concurrencyStamp = role.concurrencyStamp
    RoleService.updateRole(role.id, setDefaultRoleDto).then(role => {
      this.$message.success(this.l('roles.roleHasBeenSetDefault', { name: role.name }))
      this.refreshPagedData()
    })
  }

  /** 删除角色 */
  private handleDeleteRole(role: RoleDto) {
    this.$confirm(this.l('roles.delNotRecoverData'),
      this.l('roles.whetherDeleteRole', { name: role.name }), {
        callback: (action) => {
          if (action === 'confirm') {
            RoleService.deleteRole(role.id).then(() => {
              this.$message.success(this.l('roles.roleHasBeenDeleted', { name: role.name }))
              this.refreshPagedData()
            })
          }
        }
      })
  }

  private onEditRoleFormClosed(changed: boolean) {
    this.showEditDialog = false
    if (changed) {
      this.refreshPagedData()
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
