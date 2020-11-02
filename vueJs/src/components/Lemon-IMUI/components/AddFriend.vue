<template>
  <div
    class="friend-search"
  >
    <el-card style="top: 10px; width: 100%; height: 100%;">
      <div
        slot="header"
        class="clearfix"
      >
        <span>添加朋友</span>
      </div>
      <div>
        <el-form class="fs-form">
          <el-input
            v-model="filterName"
            placeholder="输入用户名称搜索"
            class="fs-form-input"
          >
            <el-button
              slot="append"
              icon="el-icon-search"
              @click="onSearchFriendClick"
            />
          </el-input>
        </el-form>
        <el-divider />
        <div
          v-infinite-scroll="onSearchScrollChanged"
          style="height: 400px;overflow-y: auto;"
        >
          <el-row
            v-for="(users, index) in userList"
            :key="index"
          >
            <el-col
              v-for="user in users"
              :key="user.id"
              :span="8"
            >
              <el-card>
                <el-row>
                  <el-col :span="12">
                    <el-avatar
                      :size="100"
                      shape="square"
                      src="https://fuss10.elemecdn.com/e/5d/4a731a90594a4af544c0c25941171jpeg.jpeg"
                    />
                  </el-col>
                  <el-col :span="12">
                    <div style="padding: 14px;">
                      <el-row>
                        <span>{{ user.userName | ellipsis(7) }}</span>
                      </el-row>
                      <el-row>
                        <span>{{ user.userName | ellipsis(7) }}</span>
                      </el-row>
                      <el-row>
                        <el-button
                          type="primary"
                          size="small"
                          @click="onAddFriendClick(user)"
                        >
                          添加好友
                        </el-button>
                      </el-row>
                    </div>
                  </el-col>
                </el-row>
              </el-card>
            </el-col>
          </el-row>
        </div>
      </div>
    </el-card>
  </div>
</template>

<script lang="ts">
import EventBusMiXin from '@/mixins/EventBusMiXin'
import Component, { mixins } from 'vue-class-component'

import { User } from '@/api/users'
import ImApiService, { RequestUserFriend } from '@/api/instant-message'
import UserLookupApiService from '@/api/user-lookup'

import { abpPagerFormat } from '@/utils'

@Component({
  name: 'AddFriend',
  filters: {
    ellipsis(value: string, limit: number) {
      if (!value) return ''
      if (value.length > limit) {
        return value.slice(0, limit) + '...'
      }
      return value
    }
  }
})
export default class extends mixins(EventBusMiXin) {
  private filterName = ''
  private currentPage = 1
  private maxRsultCount = 10

  private isEnd = false
  private searchUserList = new Array<User>()

  get userList() {
    let index = 0
    const newArray = []
    while (index < this.searchUserList.length) {
      newArray.push(this.searchUserList.slice(index, index += 3))
    }
    return newArray
  }

  private onFormClosed() {
    this.$emit('closed')
  }

  private onSearchFriendClick() {
    this.isEnd = false
    this.searchUserList.length = 0
    this.currentPage = 1
    this.handleSearchUsers()
  }

  private onSearchScrollChanged() {
    if (!this.isEnd) {
      this.currentPage += 1
      this.handleSearchUsers()
    }
  }

  private handleSearchUsers() {
    UserLookupApiService
      .searchUsers(
        this.filterName, 'UserName',
        abpPagerFormat(this.currentPage, this.maxRsultCount),
        this.maxRsultCount
      )
      .then(res => {
        if (res.items.length === 0) {
          this.isEnd = true
        }
        if (this.currentPage === 1) {
          this.searchUserList = res.items
        } else {
          this.searchUserList.push(...res.items)
        }
        this.$forceUpdate()
      })
  }

  private onAddFriendClick(user: User) {
    const requestFriend = new RequestUserFriend(user.id, user.userName)
    ImApiService
      .addRequest(requestFriend)
      .then(() => {
        this.$message.success('已发送用户好友申请')
      })
  }
}
</script>

<style lang="scss" scoped>
.friend-search {
  position: absolute;
  width: 100%;
  height: 100%;
}
.fs-form {
  width: 80%;
  left: 18%;
}

.fs-form-input {
  width: 80%;
  left: 20%;
}
</style>
