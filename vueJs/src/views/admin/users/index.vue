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
        {{ $t('users.searchList') }}
      </el-button>
      <el-button
        class="filter-item"
        type="primary"
        :disabled="!checkPermission(['AbpIdentity.Users.Create'])"
        @click="handleCreateUser"
      >
        {{ $t('users.createUser') }}
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
        :label="$t('users.userName')"
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
        :label="$t('users.name')"
        prop="name"
        width="110px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.name }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('users.email')"
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
        :label="$t('users.phoneNumber')"
        prop="phoneNumber"
        width="140px"
        align="center"
      >
        <template slot-scope="{row}">
          <span>{{ row.phoneNumber }}</span>
        </template>
      </el-table-column>
      <el-table-column
        :label="$t('users.lockoutEnd')"
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
        :label="$t('users.creationTime')"
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
        :label="$t('users.operaActions')"
        align="center"
        width="250px"
        min-width="250px"
      >
        <template slot-scope="{row}">
          <el-button
            :disabled="!checkPermission(['AbpIdentity.Users.Update'])"
            size="mini"
            type="primary"
            @click="handleShowEditUserForm(row)"
          >
            {{ $t('users.updateUser') }}
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
              {{ $t('users.otherOpera') }}<i class="el-icon-arrow-down el-icon--right" />
            </el-button>
            <el-dropdown-menu slot="dropdown">
              <el-dropdown-item
                :command="{key: 'claim', row}"
                :disabled="!checkPermission(['AbpIdentity.Users.ManageClaims'])"
              >
                管理声明
              </el-dropdown-item>
              <el-dropdown-item
                :command="{key: 'lock', row}"
                :disabled="!checkPermission(['AbpIdentity.Users.Update'])"
              >
                {{ $t('users.lockUser') }}
              </el-dropdown-item>
              <el-dropdown-item
                divided
                :command="{key: 'delete', row}"
                :disabled="!checkPermission(['AbpIdentity.Users.Delete'])"
              >
                {{ $t('users.deleteUser') }}
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
    />

    <el-dialog
      :visible.sync="showEditUserDialog"
      custom-class="profile"
      :title="$t('users.updateUserBy', {name: editUser.name})"
      :show-close="false"
    >
      <UserEditForm
        :user-id="editUser.id"
        @onClose="handleCloseUserProfile"
        @onUserProfileChanged="handleUserProfileChanged"
      />
    </el-dialog>

    <el-dialog
      :visible.sync="showCreateUserDialog"
      custom-class="profile"
      :title="$t('users.createUser')"
      :show-close="false"
    >
      <UserCreateForm
        @onClose="handleCloseUserProfile"
        @onUserProfileChanged="handleUserProfileChanged"
      />
    </el-dialog>

    <user-claim-create-or-update-form
      :show-dialog="showClaimDialog"
      :user-id="editUser.id"
      :title="$t('AbpIdentity.ClaimSubject', {0: editUser.name})"
      @closed="onClaimDialogClosed"
    />
  </div>
</template>

<script lang="ts">
import DataListMiXin from '@/mixins/DataListMiXin'
import Component, { mixins } from 'vue-class-component'
import Pagination from '@/components/Pagination/index.vue'
import { dateFormat, abpPagerFormat } from '@/utils'
import UserApiService, { UserDataDto, UsersGetPagedDto } from '@/api/users'
import UserCreateForm from './components/UserCreateForm.vue'
import UserEditForm from './components/UserEditForm.vue'
import UserClaimCreateOrUpdateForm from './components/UserClaimCreateOrUpdateForm.vue'
import { checkPermission } from '@/utils/permission'

@Component({
  name: 'UserList',
  components: {
    Pagination,
    UserEditForm,
    UserCreateForm,
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
export default class extends mixins(DataListMiXin) {
  /** 当前编辑用户 */
  private editUser = new UserDataDto()

  public dataFilter = new UsersGetPagedDto()

  private showCreateUserDialog = false
  private showEditUserDialog = false
  private showClaimDialog = false

  mounted() {
    this.refreshPagedData()
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

  private handleShowEditUserForm(row: UserDataDto) {
    this.editUser = row
    this.showEditUserDialog = true
  }

  private handleCloseUserProfile() {
    this.editUser = new UserDataDto()
    this.showCreateUserDialog = false
    this.showEditUserDialog = false
  }

  private handleUserProfileChanged() {
    this.refreshPagedData()
  }

  private handleCreateUser() {
    this.editUser = new UserDataDto()
    this.showCreateUserDialog = true
  }

  private handleShowCliamDialog(row: UserDataDto) {
    this.editUser = row
    this.showClaimDialog = true
  }

  private onClaimDialogClosed() {
    this.showClaimDialog = false
  }

  /** 响应更多操作命令 */
  private handleCommand(command: any) {
    switch (command.key) {
      case 'claim' :
        this.handleShowCliamDialog(command.row)
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
    this.$confirm(this.$t('users.delNotRecoverData').toString(),
      this.$t('users.whetherDeleteUser', { name: row.userName }).toString(), {
        callback: (action) => {
          if (action === 'confirm') {
            UserApiService.deleteUser(row.id).then(() => {
              this.$message.success(this.$t('users.userHasBeenDeleted', { name: row.userName }).toString())
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
