<template>
  <el-dropdown trigger="click">
    <el-badge
      :value="notificationCount"
      class="item"
      :hidden="notificationCount<=0"
    >
      <svg-icon
        name="message"
        class="message-icon"
      />
    </el-badge>
    <el-dropdown-menu slot="dropdown">
      <div
        class="app-container"
        style="width: 400px;max-height: 300px;"
      >
        <el-tabs
          stretch
        >
          <el-tab-pane
            label="通知"
          >
            <user-nofitications />
          </el-tab-pane>
          <el-tab-pane
            label="消息"
          >
            {{ $t('messages.noMessages') }}
          </el-tab-pane>
        </el-tabs>
      </div>
    </el-dropdown-menu>
  </el-dropdown>
</template>

<script lang="ts">
import Component, { mixins } from 'vue-class-component'
import EventBusMiXin from '@/mixins/EventBusMiXin'
import UserNofitications from './components/UserNofitications.vue'

@Component({
  name: 'Notification',
  components: {
    UserNofitications
  }
})
export default class extends mixins(EventBusMiXin) {
  private notificationCount = 0

  created() {
    this.subscribe('onNotificationReceived', () => {
      this.notificationCount += 1
    })
    this.subscribe('onNotificationReadChanged', () => {
      this.notificationCount -= 1
    })
    this.subscribe('onReceivedChatMessage', () => {
      this.notificationCount += 1
    })
  }
}
</script>

<style lang="scss">
.item {
  margin-top: 0px;
  margin-right: 10px;
}
.item.el-dropdown-selfdefine > .el-badge__content.el-badge__content--undefined.is-fixed {
  top: 10px;
}
</style>
