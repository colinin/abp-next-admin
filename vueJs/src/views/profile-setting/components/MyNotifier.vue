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
          />
        </el-form-item>
      </el-card>
    </el-form>
  </div>
</template>

<script lang="ts">
import NotificationApiService, { NotificationGroup, UserSubscreNotification } from '@/api/notification'
import { Component, Vue } from 'vue-property-decorator'
import PanThumb from '@/components/PanThumb/index.vue'

@Component({
  name: 'MyNotifier',
  components: {
    PanThumb
  }
})
export default class extends Vue {
  private notifierGroups = new Array<NotificationGroup>()
  private mySubscredNotifiers = new Array<UserSubscreNotification>()

  get isSubscred() {
    return (notifier: any) => {
      return this.mySubscredNotifiers.some(snf => snf.name === notifier.name)
    }
  }

  mounted() {
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
