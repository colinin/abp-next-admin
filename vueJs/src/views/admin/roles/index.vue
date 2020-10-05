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
                :command="row.isDefault ? {key: 'unDefault', row} : {key: 'default', row}"
                :disabled="!checkPermission(['AbpIdentity.Roles.Update'])"
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

    <el-dialog
      :visible="showEditDialog"
      custom-class="profile"
      title="编辑角色"
      :show-close="false"
    >
      <role-edit-form
        :role-id="editRoleId"
        @onClosed="onEditRoleFormClosed"
      />
    </el-dialog>
  </div>
</template>

<script lang="ts">
import { abpPagerFormat } from '@/utils'
import DataListMiXin from '@/mixins/DataListMiXin'
import Component, { mixins } from 'vue-class-component'
import RoleService, { CreateRoleDto, RoleDto, UpdateRoleDto, RoleGetPagedDto } from '@/api/roles'
import { checkPermission } from '@/utils/permission'
import Pagination from '@/components/Pagination/index.vue'
import PermissionTree from '@/components/PermissionTree/index.vue'
import RoleEditForm from './components/RoleEditForm.vue'

@Component({
  name: 'RoleList',
  components: {
    PermissionTree,
    Pagination,
    RoleEditForm
  },
  methods: {
    checkPermission
  }
})
export default class extends mixins(DataListMiXin) {
  private showEditDialog = false
  private editRoleId = ''

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
      case 'default' :
        this.handleSetDefaultRole(command.row, true)
        break
      case 'unDefault' :
        this.handleSetDefaultRole(command.row, false)
        break

      case 'delete' :
        this.handleDeleteRole(command.row)
        break
      default: break
    }
  }

  /** 新建角色 */
  private handleCreateRole() {
    this.$prompt(this.$t('roles.pleaseInputRoleName').toString(),
      this.$t('roles.createRole').toString(), {
        showInput: true,
        inputValidator: (val) => {
          return !(!val || val.length === 0)
        },
        inputErrorMessage: this.$t('roles.roleNameIsRequired').toString(),
        inputPlaceholder: this.$t('roles.pleaseInputRoleName').toString()
      }).then((val: any) => {
      const createRoleDto = new CreateRoleDto()
      createRoleDto.name = val.value
      createRoleDto.isDefault = false
      createRoleDto.isPublic = true
      RoleService.createRole(createRoleDto).then(role => {
        const message = this.$t('roles.createRoleSuccess', { name: role.name }).toString()
        this.$message.success(message)
        this.refreshPagedData()
      })
    })
  }

  private handleEditRole(role: RoleDto) {
    this.editRoleId = role.id
    this.showEditDialog = true
  }

  /** 设置默认角色 */
  private handleSetDefaultRole(role: RoleDto, setDefault: boolean) {
    // console.log('handleSetDefaultRole:' + role.id)
    const setDefaultRoleDto = new UpdateRoleDto()
    setDefaultRoleDto.name = role.name
    setDefaultRoleDto.isDefault = setDefault
    setDefaultRoleDto.concurrencyStamp = role.concurrencyStamp
    RoleService.updateRole(role.id, setDefaultRoleDto).then(role => {
      this.$message.success(this.$t('roles.roleHasBeenSetDefault', { name: role.name }).toString())
      this.refreshPagedData()
    })
  }

  /** 删除角色 */
  private handleDeleteRole(role: RoleDto) {
    this.$confirm(this.$t('roles.delNotRecoverData').toString(),
      this.$t('roles.whetherDeleteRole', { name: role.name }).toString(), {
        callback: (action) => {
          if (action === 'confirm') {
            RoleService.deleteRole(role.id).then(() => {
              this.$message.success(this.$t('roles.roleHasBeenDeleted', { name: role.name }).toString())
              this.refreshPagedData()
            })
          }
        }
      })
  }

  private onEditRoleFormClosed() {
    this.editRoleId = ''
    this.showEditDialog = false
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
