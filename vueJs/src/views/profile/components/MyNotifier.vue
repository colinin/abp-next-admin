<template>
  <div class="app-container">
    <h4>消息通知</h4>
    <el-divider />
    <el-form
      label-width="180px"
    >
      <el-card
        v-for="group in notifierGroups"
        :key="group.name"
        class="box-card"
      >
        <div
          slot="header"
          class="clearfix"
        >
          <span>{{ group.displayName }}</span>
        </div>
        <el-form-item
          v-for="notifier in group.notifications"
          :key="notifier.name"
          :label="notifier.displayName "
        >
          <el-switch
            :value="isSubscred(notifier)"
            @change="onUserSubscredChanged(notifier)"
          />
        </el-form-item>
      </el-card>
    </el-form>
  </div>
</template>

<script lang="ts">
import NotificationApiService, { NotificationGroup, UserSubscreNotification } from '@/api/notification'
import { Component } from 'vue-property-decorator'
import { mixins } from 'vue-class-component'
import PanThumb from '@/components/PanThumb/index.vue'
import EventBusMiXin from '@/mixins/EventBusMiXin'

@Component({
  name: 'MyNotifier',
  components: {
    PanThumb
  }
})
export default class extends mixins(EventBusMiXin) {
  private notifierGroups = new Array<NotificationGroup>()
  private mySubscredNotifiers = new Array<UserSubscreNotification>()

  get isSubscred() {
    return (notifier: any) => {
      return this.mySubscredNotifiers.some(snf => snf.name === notifier.name)
    }
  }

  mounted() {
    this.subscribe('onSubscribed', this.onSubscribed)
    this.subscribe('onUnSubscribed', this.onUnSubscribed)
    this.handleGetMyNotifications()
  }

  destroyed() {
    this.unSubscribe('onSubscribed')
    this.unSubscribe('onUnSubscribed')
  }

  private handleGetMyNotifications() {
    NotificationApiService
      .getAssignableNotifiers()
      .then(res => {
        this.notifierGroups = res.items
      })
    NotificationApiService
      .getMySubscribedNotifiers()
      .then(res => {
        this.mySubscredNotifiers = res.items
      })
  }

  private onUserSubscredChanged(notifier: any) {
    const index = this.mySubscredNotifiers.findIndex(x => x.name === notifier.name)
    if (index >= 0) {
      this.mySubscredNotifiers.splice(index, 1)
      this.trigger('onUnSubscribed', notifier.name)
    } else {
      const userSubscre = new UserSubscreNotification()
      userSubscre.name = notifier.name
      this.mySubscredNotifiers.push(userSubscre)
      this.trigger('onSubscribed', userSubscre)
    }
  }

  private onSubscribed(userSubscre: UserSubscreNotification) {
    NotificationApiService
      .subscribeNotifier(userSubscre)
  }

  private onUnSubscribed(name: string) {
    NotificationApiService
      .unSubscribeNotifier(name)
  }
}
</script>

<style lang="scss" scoped>
.box-center {
  margin: 0 auto;
  display: table;
}
.user-avatar {
  .box-center {
    padding-top: 10px;
  }
}
</style>
