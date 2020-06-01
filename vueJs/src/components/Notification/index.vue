<template>
  <el-dropdown>
    <div>
      <svg-icon
        name="message"
        class="message-icon"
      />
    </div>
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
            <div
              style="overflow-x: hidden;overflow-y: auto;"
            >
              <List
                size="small"
              >
                <ListItem
                  v-for="(notify, index) in notifications"
                  :key="index"
                >
                  <ListItemMeta
                    :title="notify.Message"
                    :description="formatDateTime(notify.DateTime)"
                    avatar="ios-person"
                  />
                </ListItem>
              </List>
            </div>
          </el-tab-pane>
          <el-tab-pane label="消息">
            消息系统
          </el-tab-pane>
        </el-tabs>
      </div>
    </el-dropdown-menu>
  </el-dropdown>
</template>

<script lang="ts">
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr'
import { Component, Vue } from 'vue-property-decorator'
import { UserModule } from '@/store/modules/user'
import { dateFormat } from '@/utils/index'

enum Severity {
  success = 0,
  info = 10,
  warn = 20,
  error = 30,
  fatal = 40
}

class Notification {
  Title!: string
  Message!: string
  DateTime!: Date
  Severity!: Severity
}

@Component({
  name: 'Notification'
})
export default class extends Vue {
  private connection!: HubConnection
  private notifications = new Array<Notification>()

  mounted() {
    this.handleStartConnection()
  }

  destroyed() {
    this.handleStopConnection()
  }

  private renderIconType(item: any) {
    if (item.Severity !== Severity.success) {
      return ' el-icon-circle-close'
    }
    return ' el-icon-circle-check'
  }

  private renderIconStyle(item: any) {
    if (item.Severity !== Severity.success) {
      return 'backgroundColor: #f56a00'
    }
    return 'backgroundColor: #87d068'
  }

  private formatDateTime(datetime: string) {
    const date = new Date(datetime)
    return dateFormat(date, 'YYYY-mm-dd HH:MM:SS')
  }

  private handleStartConnection() {
    console.log('start signalr connection...')
    if (!this.connection) {
      const builder = new HubConnectionBuilder()
      console.log(builder)
      const userToken = UserModule.token.replace('Bearer ', '')
      console.log(userToken)
      this.connection = builder
        .withUrl('/signalr-hubs/signalr-hubs/notifications', { accessTokenFactory: () => userToken })
        .withAutomaticReconnect({ nextRetryDelayInMilliseconds: () => 60000 })
        .build()
      this.connection.on('getNotification', data => this.onNotificationReceived(data))
      this.connection.onclose(error => {
        console.log('signalr connection has closed, error:')
        console.log(error)
      })
    }
    if (this.connection.state !== HubConnectionState.Connected) {
      this.connection.start()
    }
  }

  private handleStopConnection() {
    console.log('stop signalr connection...')
    if (this.connection && this.connection.state === HubConnectionState.Connected) {
      this.connection.stop()
    }
  }

  private onNotificationReceived(data: any) {
    console.log('received signalr message...')
    console.log(data)
    this.notifications.push(data.data.properties)
    this.$notify.success(data.data.properties.message)
  }
}
</script>

<style scoped>
.el-scrollbar__wrap{
  overflow-x: hidden;
}
</style>
