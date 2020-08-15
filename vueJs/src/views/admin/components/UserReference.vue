<template>
  <div class="user-reference-pane">
    <el-form>
      <el-form-item>
        <el-button
          class="confirm"
          type="primary"
          style="width:100px"
        >
          {{ $t('global.confirm') }}
        </el-button>
        <el-button
          class="cancel"
          style="width:100px"
        >
          {{ $t('global.cancel') }}
        </el-button>
      </el-form-item>
      <el-form-item>
        <el-table
          ref="userTable"
          v-loading="userLoading"
          row-key="id"
          :data="userList"
          border
          fit
          highlight-current-row
          max-height="250px"
          @row-click="onRowClick"
        >
          <el-table-column
            type="selection"
            width="50"
            align="center"
          />
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
        </el-table>

        <pagination
          v-show="userCount>0"
          :total="userCount"
          :page.sync="userQueryFilter.skipCount"
          :limit.sync="userQueryFilter.maxResultCount"
          @pagination="handleGetUserList"
        />
      </el-form-item>
    </el-form>
  </div>
</template>

<script lang="ts">
import { dateFormat } from '@/utils'
import { Component, Vue } from 'vue-property-decorator'
import Pagination from '@/components/Pagination/index.vue'
import UserAppService, { UsersGetPagedDto, UserDataDto } from '@/api/users'

@Component({
  name: 'UserReference',
  components: {
    Pagination
  },
  filters: {
    dateTimeFilter(datetime: string) {
      const date = new Date(datetime)
      return dateFormat(date, 'YYYY-mm-dd HH:MM')
    }
  }
})
export default class extends Vue {
  private userCount = 0
  private userLoading = false
  private userList = new Array<UserDataDto>()
  private userQueryFilter = new UsersGetPagedDto()

  mounted() {
    this.handleGetUserList()
    // 滚动延迟加载
    // const userTable = this.$refs.userTable as any
    // userTable.bodyWrapper.addEventListener('scroll', (res: any) => this.onTableScrollChanged(res), true)
  }

  private handleGetUserList() {
    this.userLoading = true
    UserAppService.getUsers(this.userQueryFilter).then(res => {
      this.userList = res.items
      this.userCount = res.totalCount
      this.userLoading = false
    })
  }

  private onRowClick(row: any) {
    const table = this.$refs.userTable as any
    table.toggleRowSelection(row)
  }

  private onTableScrollChanged(dom: any) {
    console.log(dom)
  }
}
</script>

<style lang="scss" scoped>
.user-reference-pane .user-table {
  width: 100%;
  cursor: pointer;
}
.confirm {
  position: relative;
}
.cancel {
  position: relative;
}
</style>
