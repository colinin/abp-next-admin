<template>
  <div id="InstantMessage">
    <div class="imui-center">
      <lemon-imui
        v-show="showDialog"
        ref="IMUI"
        :user="currentUser"
        @send="handleSendMessage"
        @pull-messages="onPullMessages"
        @change-menu="onChangeMenu"
        @change-contact="onChangeContract"
        @message-click="handleMessageClick"
        @menu-avatar-click="handleMenuAvatarClick"
      />
    </div>
  </div>
</template>

<script lang="ts">
import EventBusMiXin from '@/mixins/EventBusMiXin'
import Component, { mixins } from 'vue-class-component'
import AddFriend from './components/AddFriend.vue'

import { abpPagerFormat } from '@/utils'

import { UserModule } from '@/store/modules/user'

import ImApiService, {
  MyFrientGetAll,
  UserMessageGetByPaged,
  GetUserLastMessage,
  UserFriend,
  ChatMessage
} from '@/api/instant-message'

import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr'
import { User } from '@/api/users'

class MyContract {
  id = ''
  displayName = ''
  avatar = ''
  type = ''
  index = 'A'
  unread = 0
  lastSendTime = 1345209465000
  lastContent = ''
  readMsgPage = 1
}

class ChatMenu {
  name = ''
  title = ''
  unread = 0
  click?: Function
  render?: Function
  renderContainer?: Function
  isBottom = false

  constructor(
    name: string,
    title: string,
    unread = 0,
    isBottom = false
  ) {
    this.name = name
    this.title = title
    this.unread = unread
    this.isBottom = isBottom
  }
}

class FromUser {
  id = ''
  displayName = ''
  avatar = ''
}

class Message {
  id = ''
  status = ''
  type = 'text'
  sendTime = 0
  content = ''
  fileSize = 0
  fileName = ''
  toContactId = ''
  fromUser = new FromUser()

  public fromChatMessage(chatMessage: ChatMessage) {
    this.id = chatMessage.messageId
    this.content = chatMessage.content
    this.fromUser.id = chatMessage.formUserId
    this.fromUser.displayName = chatMessage.formUserName
    this.toContactId = chatMessage.toUserId
    this.type = ChatMessage.getType(chatMessage.messageType)
    this.sendTime = new Date(chatMessage.sendTime).getTime()
  }
}

@Component({
  name: 'InstantMessage',
  components: {
    AddFriend
  }
})
export default class InstantMessage extends mixins(EventBusMiXin) {
  private showDialog = false
  private myFriendCount = 0
  private myFriends = new Array<UserFriend>()

  private connection!: HubConnection
  private chatMenus = new Array<ChatMenu>()

  private getMyFriendFilter = new MyFrientGetAll()
  private getChatMessageFilter = new UserMessageGetByPaged()

  private showAddFriendDialog = false

  get currentUser() {
    return {
      id: UserModule.id,
      displayName: UserModule.userName,
      avatar: ''
    }
  }

  mounted() {
    this.unSubscribeAll()
    this.subscribe('onShowImDialog', this.onShowImDialog)
    this.subscribe('onUserFriendAdded', this.onUserFriendAdded)
    this.handleInitDefaultMenus()
    this.handleStartConnection()
  }

  destroyed() {
    this.unSubscribe('onShowImDialog')
  }

  private handleInitIMUI() {
    const imui = this.$refs.IMUI as any
    ImApiService
      .getMyAllFriends(this.getMyFriendFilter)
      .then(res => {
        this.myFriends = res.items
        this.myFriendCount = res.items.length
        this.handleInitContracts(imui)
      })
      .finally(() => {
        imui.initMenus(this.chatMenus)
        this.handleInitLastContractMessages(imui)
      })
  }

  private handleInitContracts(imui: any) {
    const myContracts = new Array<MyContract>()
    this.myFriends
      .forEach(friend => {
        const myContract = new MyContract()
        myContract.id = friend.friendId
        myContract.displayName = friend.remarkName ?? friend.userName
        myContract.unread = 0
        myContract.type = 'many'
        myContracts.push(myContract)
      })
    imui.initContacts(myContracts)
  }

  private handleInitLastContractMessages(imui: any) {
    const filter = new GetUserLastMessage()
    ImApiService
      .getMyLastMessages(filter)
      .then(res => {
        res.items
          .sort((now, next) => { // 如果一条消息是与同一个用户的发送与接收,会被分为两条消息返回,所以需要按照发送时间升序排序,历史消息才会准确
            return next.sendTime < now.sendTime ? 1 : -1
          })
          .forEach(msg => {
            const imuiMsg = new Message()
            imuiMsg.fromChatMessage(msg)
            let contractId = msg.formUserId
            if (msg.formUserId === this.currentUser.id) {
              contractId = msg.toUserId
            }
            imui.updateContact(contractId, {
              lastSendTime: imuiMsg.sendTime,
              lastContent: imui.lastContentRender(imuiMsg)
            })
          })
      })
  }

  private handleInitDefaultMenus() {
    const lastMsgMenu = new ChatMenu('lastMessages', '历史消息')
    const contractsMenu = new ChatMenu('contacts', '联系人')
    const addFriendMenu = new ChatMenu('addFriends', '添加朋友')
    addFriendMenu.isBottom = true
    addFriendMenu.render = () => {
      return this.$createElement('i', { class: 'lemon-icon-attah' })
    }
    addFriendMenu.renderContainer = () => {
      return this.$createElement(AddFriend, {
        on: {
          onUserFriendAdded: this.onUserFriendAdded
        }
      })
    }
    this.chatMenus.push(lastMsgMenu, contractsMenu, addFriendMenu)
  }

  private handleStartConnection() {
    if (!this.connection) {
      const builder = new HubConnectionBuilder()
      const userToken = UserModule.token.replace('Bearer ', '')
      this.connection = builder
        .withUrl('/signalr-hubs/signalr-hubs/messages', { accessTokenFactory: () => userToken })
        .withAutomaticReconnect({ nextRetryDelayInMilliseconds: () => 60000 })
        .build()
      this.connection.on('getChatMessage', this.handleReceiveMessage)
      this.connection.onreconnected(() => {
        this.handleInitIMUI()
      })
      this.connection.onclose(error => {
        console.log('signalr connection has closed, error:')
        console.log(error)
      })
    }
    if (this.connection.state !== HubConnectionState.Connected) {
      this.connection
        .start()
        .then(() => {
          this.handleInitIMUI()
        })
    }
  }

  private onShowImDialog() {
    this.showDialog = !this.showDialog
  }

  private onChangeMenu() {
    console.log('Event:change-menu')
  }

  private onPullMessages(contact: any, next: any) {
    console.log(contact)
    const imui = this.$refs.IMUI as any
    if (this.getChatMessageFilter.receiveUserId !== contact.id) {
      this.getChatMessageFilter.receiveUserId = contact.id
    } else {
      contact.readMsgPage += 1
    }
    this.getChatMessageFilter.skipCount = abpPagerFormat(contact.readMsgPage, this.getChatMessageFilter.maxResultCount)
    ImApiService
      .getMyChatMessages(this.getChatMessageFilter)
      .then(res => {
        const messages = res.items
          .sort((last, next) => {
            return next.sendTime < last.sendTime ? 1 : -1
          })
          .map(msg => {
            const message = new Message()
            message.fromChatMessage(msg)
            return message
          })
        let isEnd = true
        if (imui.getMessages(contact.id).length < res.totalCount) {
          isEnd = false
        }
        next(messages, isEnd)
      })
      .catch(() => {
        next(new Array<Message>(), true)
        imui.messageViewToBottom()
      })
  }

  private handleMenuAvatarClick() {
    console.log('Event:menu-avatar-click')
  }

  private handleMessageClick(e: any, key: any, message: any) {
    console.log(e)
    console.log(key)
    console.log(message)
  }

  private handleReceiveMessage(chatMessage: ChatMessage) {
    const message = new Message()
    message.fromChatMessage(chatMessage)
    const imui = this.$refs.IMUI as any
    imui.appendMessage(message, chatMessage.formUserId)
    const currentContact = imui.currentContact
    if (currentContact && currentContact.id === chatMessage.formUserId) {
      currentContact.lastContent = chatMessage.content
      currentContact.lastSendTime = new Date(chatMessage.sendTime).getTime()
    } else {
      imui.updateContact(chatMessage.formUserId, {
        unread: '+1',
        lastSendTime: new Date(chatMessage.sendTime).getTime(),
        lastContent: chatMessage.content
      })
    }
  }

  private handleSendMessage(message: Message, next: any, file: any) {
    console.log(message, next, file)
    const imui = this.$refs.IMUI as any
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
      .catch(() => {
        imui
          .updateMessage(message.id, message.toContactId, {
            status: 'failed'
          })
      })
  }

  private onChangeContract(contract: any) {
    const imui = this.$refs.IMUI as any
    imui.updateContact(contract.id, {
      unread: 0
    })
    imui.closeDrawer()
  }

  private onUserFriendAdded(user: User) {
    if (UserModule.id && UserModule.userName) {
      const chatMessage = new ChatMessage()
      chatMessage.formUserId = UserModule.id
      chatMessage.formUserName = UserModule.userName
      chatMessage.toUserId = user.id
      chatMessage.content = '我已经添加你为好友,让我们一起聊天吧!'
      console.log(chatMessage)
      this.connection.invoke('SendMessage', chatMessage)
    }
  }
}
</script>

<style lang="scss" scoped>
.body {
  background: #3d495c !important
}
.link {
  padding: 15px 0
}
.link >>> .a {
  display: inline-block;
  font-size: 16px;
  color: #ccd3dc;
  text-decoration: none;
  border-radius: 5px;
  margin-right: 15px;
  &:hover {
    color: #85acda;
  }
}
.action {
  margin-top: 30px;
  .button {
    margin-right: 10px
  }
}
.imui-center {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%,-50%);
}
.drawer-content {
  padding: 15px
}
.more {
  font-size: 32px;
  line-height: 18px;
  height: 32px;
  position: absolute;
  top: 6px;
  right: 14px;
  cursor: pointer;
  user-select: none;
  color: #999;
  &:active {
    color: #000;
  }
}
.bar {
  text-align: center;
  line-height: 30px;
  background: #fff;
  margin: 15px;
  color: #666;
  user-select: none;
  font-size: 12px;
}
.cover {
  text-align: center;
  user-select: none;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%,-50%);
  i {
    font-size: 84px;
    color: #e6e6e6;
  }
  p {
    font-size: 18px;
    color: #ddd;
    line-height: 50px;
  }
}
.article-item {
  line-height: 34px;
  cursor: pointer;
  &:hover {
    text-decoration: underline;
    color: #318efd;
  }
}
</style>
