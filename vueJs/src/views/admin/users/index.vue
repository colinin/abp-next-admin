<template>
  <div class="app-container">
    <div class="filter-container">
      <label
        class="radio-label"
        style="padding-left:0;"
      >{{ $t('users.queryFilter') }}</label>
      <el-input
        v-model="dataFilter.filter"
        :placeholder="$t('users.filterString')"
        style="width: 250px;margin-left: 10px;"
        class="filter-item"
      />
      <el-button
        class="filter-item"
        style="margin-left: 10px; text-alignt"
        type="primary"
        @click="refreshPagedData"
      >
        {{ $t('AbpIdentity.Search') }}
      </el-button>
      <el-button
        class="filter-item"
        type="primary"
        :disabled="!checkPermission(['AbpIdentity.Users.Create'])"
        @click="handleShowUserDialog('')"
      >
        {{ $t('AbpIdentity.NewUser') }}
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
        :label="$t('AbpIdentity.DisplayName:UserName')"
        prop="userName"
        sortable
        width="110px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.userName }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentity.DisplayName:Surname')"
        prop="surname"
        width="110px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.surname }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentity.DisplayName:Name')"
        prop="name"
        width="110px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.name }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentity.DisplayName:Email')"
        prop="email"
        sortable
        min-width="180"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.email }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentity.DisplayName:PhoneNumber')"
        prop="phoneNumber"
        width="140px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.phoneNumber }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentity.LockoutEnd')"
        prop="lockoutEnd"
        sortable
        width="140px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.lockoutEnd | dateTimeFilter }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentity.CreationTime')"
        prop="creationTime"
        sortable
        width="140px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.creationTime | dateTimeFilter }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('AbpIdentity.Actions')"
        align="center"
        width="250px"
        min-width="250px"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!checkPermission(['AbpIdentity.Users.Update'])"
            size="mini"
            type="primary"
            icon="el-icon-edit"
            @click="handleShowUserDialog(row.id)"
          >
            {{ $t('AbpIdentity.Edit') }}
          </el-button>
          <el-dropdown
            class="options"
            @command="handleCommand"
          >
            <el-button
              v-permission="['AbpIdentity.Users']"
              size="mini"
              type="info"
            >
              {{ $t('AbpIdentity.Actions') }}<i class="el-icon-arrow-down el-icon--right" />
            </el-button>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                :command="{key: 'permission', row}"
                :disabled="!checkPermission(['AbpIdentity.Users.ManagePermissions'])"
              >
                {{ $t('AbpIdentity.Permissions') }}
              </el-dropdown-item>
              <el-dropdown-item
                :command="{key: 'lock', row}"
                :disabled="!checkPermission(['AbpIdentity.Users.Update'])"
              >
                {{ $t('AbpIdentity.Lock') }}
              </el-dropdown-item>
              <el-dropdown-item
                :command="{key: 'claim', row}"
                :disabled="!checkPermission(['AbpIdentity.Users.ManageClaims'])"
              >
                {{ $t('AbpIdentity.ManageClaim') }}
              </el-dropdown-item>
              <el-dropdown-item
                :command="{key: 'menu', row}"
                :disabled="!checkPermission(['Platform.Menu.ManageUsers'])"
              >
                {{ $t('AppPlatform.Menu:Manage') }}
              </el-dropdown-item>
              <el-dropdown-item
                divided
                :command="{key: 'delete', row}"
                :disabled="!checkPermission(['AbpIdentity.Users.Delete'])"
              >
                {{ $t('AbpIdentity.Delete') }}
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

    <user-create-or-update-form
      :show-dialog="showUserDialog"
      :edit-user-id="editUserId"
      @closed="onUserDialogClosed"
    />

    <user-claim-create-or-update-form
      :show-dialog="showClaimDialog"
      :user-id="editUserId"
      @closed="onClaimDialogClosed"
    />

    <manage-user-menu-dialog
      :show-dialog="showManageUserMenuDialog"
      :user-id="editUserId"
      @closed="showManageUserMenuDialog=false"
    />

    <permission-form
      provider-name="U"
      :provider-key="editUserId"
      :readonly="!allowedEditPermission"
      :show-dialog="showPermissionDialog"
      @closed="onPermissionDialogClosed"
    />
  </div>
</template>

<script lang="ts">
import { UserModule } from '@/store/modules/user'
import DataListMiXin from '@/mixins/DataListMiXin'
import EventBusMiXin from '@/mixins/EventBusMiXin'
import Component, { mixins } from 'vue-class-component'
import UserApiService, { User, UsersGetPagedDto } from '@/api/users'
import Pagination from '@/components/Pagination/index.vue'
import PermissionForm from '@/components/PermissionForm/index.vue'
import ManageUserMenuDialog from './components/ManageUserMenuDialog.vue'
import UserCreateOrUpdateForm from './components/UserCreateOrUpdateForm.vue'
import UserClaimCreateOrUpdateForm from './components/UserClaimCreateOrUpdateForm.vue'
import { dateFormat, abpPagerFormat } from '@/utils'
import { checkPermission } from '@/utils/permission'

@Component({
  name: 'UserList',
  components: {
    Pagination,
    PermissionForm,
    ManageUserMenuDialog,
    UserCreateOrUpdateForm,
    UserClaimCreateOrUpdateForm
  },
  filters: {
    dateTimeFilter(datetime: string) {
      const date = new Date(datetime)
      return dateFormat(date, 'YYYY-mm-dd HH:MM')
    }
  },
  methods: {
    checkPermission
  }
})
export default class extends mixins(DataListMiXin, EventBusMiXin) {
  private editUserId = ''

  public dataFilter = new UsersGetPagedDto()

  private showUserDialog = false
  private showClaimDialog = false
  private showPermissionDialog = false
  private showManageUserMenuDialog = false

  get allowedEditPermission() {
    return this.editUserId !== UserModule.id && checkPermission(['AbpIdentity.Users.ManagePermissions'])
  }

  mounted() {
    this.refreshPagedData()
    this.subscribe('userChanged', () => this.refreshPagedData())
  }

  destroyed() {
    this.unSubscribe('userChanged')
  }

  protected processDataFilter() {
    this.dataFilter.skipCount = abpPagerFormat(this.currentPage, this.pageSize)
  }

  /** 查询用户列表 */
  protected getPagedList(filter: any) {
    return UserApiService.getUsers(filter)
  }

  /** 锁定用户
   * @param row 操作行数据,可以转换为 UserDataDto 对象
   */
  private handleLockUser(row: any) {
    console.log('handleLockUser' + row.id)
  }

  private handleUserProfileChanged() {
    this.refreshPagedData()
  }

  private handleShowCliamDialog(row: User) {
    this.editUserId = row.id
    this.showClaimDialog = true
  }

  private onClaimDialogClosed() {
    this.showClaimDialog = false
  }

  private handleShowPermissionDialog(id: string) {
    this.editUserId = id
    this.showPermissionDialog = true
  }

  private onPermissionDialogClosed() {
    this.showPermissionDialog = false
  }

  private handleShowUserDialog(id: string) {
    this.editUserId = id
    this.showUserDialog = true
  }

  private onUserDialogClosed() {
    this.showUserDialog = false
  }

  /** 响应更多操作命令 */
  private handleCommand(command: any) {
    switch (command.key) {
      case 'claim' :
        this.handleShowCliamDialog(command.row)
        break
      case 'menu' :
        this.editUserId = command.row.id
        this.showManageUserMenuDialog = true
        break
      case 'permission' :
        this.handleShowPermissionDialog(command.row.id)
        break
      case 'lock' :
        this.handleLockUser(command.row)
        break
      case 'delete' :
        this.handleDeleteUser(command.row)
        break
      default: break
    }
  }

  /** 响应删除用户事件 */
  private handleDeleteUser(row: any) {
    this.$confirm(this.l('AbpIdentity.UserDeletionConfirmationMessage', { 0: row.userName }),
      this.l('AbpIdentity.AreYouSure'), {
        callback: (action) => {
          if (action === 'confirm') {
            UserApiService.deleteUser(row.id).then(() => {
              this.$message.success(this.l('global.successful'))
              this.refreshPagedData()
            })
          }
        }
      })
  }
}
</script>

<style lang="scss">
.profile {
  .el-dialog__header {
    background-color: dodgerblue;
    }
}
.options {
  vertical-align: top;
  margin-left: 20px;
}
.el-icon-arrow-down {
  font-size: 12px;
}
</style>
