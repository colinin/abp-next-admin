<template>
  <div>
    <lemon-imui
      v-show="showDialog"
      ref="IMUI"
      :user="currentUser"
      @send="handleSendMessage"
    />
  </div>
</template>

<script lang="ts">
import EventBusMiXin from '@/mixins/EventBusMiXin'
import Component, { mixins } from 'vue-class-component'

import { UserModule } from '@/store/modules/user'

import ImApiService, { MyFriendGetByPaged, UserFriend, ChatMessage } from '@/api/instant-message'

import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr'

class MyContract {
  id = ''
  displayName = ''
  avatar = ''
  type = ''
  index = 'A'
  unread = 0
  lastSendTime = 0
  lastContent = '0'
}

@Component({
  name: 'InstantMessage'
})
export default class InstantMessage extends mixins(EventBusMiXin) {
  private showDialog = false
  private dataFilter = new MyFriendGetByPaged()
  private myFriendCount = 0
  private myFriends = new Array<UserFriend>()

  private connection!: HubConnection

  get currentUser() {
    return {
      id: UserModule.id,
      displayName: UserModule.userName,
      avatar: ''
    }
  }

  mounted() {
    this.subscribe('onShowImDialog', this.onShowImDialog)
    this.handleStartConnection()
  }

  destroyed() {
    this.unSubscribe('onShowImDialog')
  }

  private handleStartConnection() {
    if (!this.connection) {
      const builder = new HubConnectionBuilder()
      const userToken = UserModule.token.replace('Bearer ', '')
      this.connection = builder
        .withUrl('/signalr-hubs/signalr-hubs/messages', { accessTokenFactory: () => userToken })
        .withAutomaticReconnect({ nextRetryDelayInMilliseconds: () => 60000 })
        .build()
      this.connection.onclose(error => {
        console.log('signalr connection has closed, error:')
        console.log(error)
      })
    }
    if (this.connection.state !== HubConnectionState.Connected) {
      this.connection
        .start()
        .then(() => {
          ImApiService
            .getMyFriends(this.dataFilter)
            .then(res => {
              this.myFriends = res.items
              this.myFriendCount = res.totalCount
              this.handleInitContracts()
            })
        })
    }
  }

  private handleInitContracts() {
    const myContracts = new Array<MyContract>()
    this.myFriends
      .forEach(friend => {
        const myContract = new MyContract()
        myContract.id = friend.friendId
        myContract.displayName = friend.remarkName ?? friend.userName
        myContracts.push(myContract)
      })
    const imui = this.$refs.IMUI as any
    imui.initContacts(myContracts)
  }

  private onShowImDialog() {
    this.showDialog = !this.showDialog
  }

  private handleSendMessage(message: any, next: any, file: any) {
    console.log(message, next, file)
    const chatMessage = new ChatMessage()
    chatMessage.formUserId = message.fromUser.id
    chatMessage.formUserName = message.fromUser.displayName
    chatMessage.toUserId = message.toContactId
    chatMessage.content = message.content
    this.connection
      .invoke('SendMessage', chatMessage)
      .then(() => {
        setTimeout(() => {
          next()
        }, 1000)
      })
  }
}
</script>
