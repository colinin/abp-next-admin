<template>
  <div class="app-container">
    <div class="filter-container">
      <label
        class="radio-label"
        style="padding-left:0;"
      >{{ $t('users.queryFilter') }}</label>
      <el-input
        :placeholder="$t('users.filterString')"
        style="width: 250px;margin-left: 10px;"
        class="filter-item"
      />
      <el-button
        class="filter-item"
        style="margin-left: 10px; text-alignt"
        type="primary"
        @click="handleGetUsers"
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
      v-loading="userListLoading"
      row-key="id"
      :data="userList"
      border
      fit
      highlight-current-row
      style="width: 100%;"
      :default-sort="sortRule"
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
        fixed="right"
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
      v-show="totalCount>0"
      :total="totalCount"
      :page.sync="getUserQuery.skipCount"
      :limit.sync="getUserQuery.maxResultCount"
      @pagination="handleGetUsers"
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
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import Pagination from '@/components/Pagination/index.vue'
import { dateFormat } from '@/utils'
import UserApiService, { UserDataDto, UsersGetPagedDto } from '@/api/users'
import UserCreateForm from './components/UserCreateForm.vue'
import UserEditForm from './components/UserEditForm.vue'
import { checkPermission } from '@/utils/permission'

@Component({
  name: 'UserList',
  components: {
    Pagination,
    UserEditForm,
    UserCreateForm
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
export default class extends Vue {
  /** 用户列表加载中 */
  private userListLoading: boolean
  /** 用户列表 */
  private userList: UserDataDto[]
  /** 最大用户数量 */
  private totalCount: number
  /** 当前编辑用户 */
  private editUser: UserDataDto
  /** 排序组别 */
  private sortRule: { prop: string, sort: string }
  /** 查询用户过滤参数 */
  private getUserQuery: UsersGetPagedDto

  private showCreateUserDialog: boolean
  private showEditUserDialog: boolean

  constructor() {
    super()
    this.totalCount = 0
    this.editUser = new UserDataDto()
    this.userListLoading = false
    this.sortRule = { prop: '', sort: '' }
    this.getUserQuery = new UsersGetPagedDto()
    this.userList = new Array<UserDataDto>()

    this.showEditUserDialog = false
    this.showCreateUserDialog = false
  }

  mounted() {
    this.handleGetUsers()
  }

  /** 查询用户列表 */
  private handleGetUsers() {
    this.userListLoading = true
    UserApiService.getUsers(this.getUserQuery).then(res => {
      this.totalCount = res.totalCount
      this.userList = res.items
      this.userListLoading = false
    })
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
    this.handleGetUsers()
  }

  private handleCreateUser() {
    this.editUser = new UserDataDto()
    this.showCreateUserDialog = true
  }

  /** 响应更多操作命令 */
  private handleCommand(command: any) {
    switch (command.key) {
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
              this.handleGetUsers()
            })
          }
        }
      })
  }

  /** 响应表格排序事件 */
  private handleSortChange(column: any) {
    this.getUserQuery.sorting = column.sort
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
