<template>
  <div>
    <div>
      <lemon-imui
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
  ChatMessage,
  AddUserFriend,
  MessageType
} from '@/api/instant-message'

import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr'

class MyContract {
  id = ''
  displayName = ''
  avatar = 'http://upload.qqbodys.com/allimg/1710/1035512943-0.jpg'
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
    this.fromUser.avatar = 'http://upload.qqbodys.com/allimg/1710/1035512943-0.jpg'
    this.toContactId = chatMessage.toUserId
    this.type = ChatMessage.getType(chatMessage.messageType)
    this.sendTime = new Date(chatMessage.sendTime).getTime()
  }

  public static tryParseToChatMessage(message: Message) {
    const chatMessage = new ChatMessage()
    chatMessage.formUserId = message.fromUser.id
    chatMessage.formUserName = message.fromUser.displayName
    chatMessage.toUserId = message.toContactId
    chatMessage.content = message.content
    chatMessage.sendTime = new Date(message.sendTime)
    chatMessage.messageType = Message.getChatMessageType(message)
    return chatMessage
  }

  public static getChatMessageType(message: Message) {
    switch (message.type) {
      case 'text' :
        return MessageType.Text
      case 'video' :
        return MessageType.Video
      case 'image' :
        return MessageType.Image
      case 'voice' :
        return MessageType.Voice
      case 'file' :
        return MessageType.File
      default :
        return MessageType.Text
    }
  }
}

@Component({
  name: 'LemonIMUI',
  components: {
    AddFriend
  }
})
export default class extends mixins(EventBusMiXin) {
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
    this.subscribe('onReceivedChatMessage', this.onReceivedChatMessage)
    this.subscribe('LINGYUN.Abp.Messages.IM.FriendValidation', this.onNewFriendValidation)
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
        this.handleInitEmoji(imui)
        this.handleInitLastContractMessages(imui)
      })
  }

  private handleInitEmoji(imui: any) {
    imui.initEmoji([
      {
        label: '表情',
        children: [
          {
            name: '1f600',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f600.png'
          },
          {
            name: '1f62c',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f62c.png'
          },
          {
            name: '1f601',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f601.png'
          },
          {
            name: '1f602',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f602.png'
          },
          {
            name: '1f923',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f923.png'
          },
          {
            name: '1f973',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f973.png'
          },
          {
            name: '1f603',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f603.png'
          },
          {
            name: '1f604',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f604.png'
          },
          {
            name: '1f605',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f605.png'
          },
          {
            name: '1f606',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f606.png'
          },
          {
            name: '1f607',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f607.png'
          },
          {
            name: '1f609',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f609.png'
          },
          {
            name: '1f60a',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f60a.png'
          },
          {
            name: '1f642',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f642.png'
          },
          {
            name: '1f643',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f643.png'
          },
          {
            name: '1263a',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/263a.png'
          },
          {
            name: '1f60b',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f60b.png'
          },
          {
            name: '1f60c',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f60c.png'
          },
          {
            name: '1f60d',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f60d.png'
          },
          {
            name: '1f970',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f970.png'
          },
          {
            name: '1f618',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f618.png'
          },
          {
            name: '1f617',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f617.png'
          },
          {
            name: '1f619',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f619.png'
          },
          {
            name: '1f61a',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f61a.png'
          },
          {
            name: '1f61c',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f61c.png'
          },
          {
            name: '1f92a',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f92a.png'
          },
          {
            name: '1f928',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f928.png'
          },
          {
            name: '1f9d0',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f9d0.png'
          },
          {
            name: '1f61d',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f61d.png'
          },
          {
            name: '1f61b',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f61b.png'
          },
          {
            name: '1f911',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f911.png'
          },
          {
            name: '1f913',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f913.png'
          },
          {
            name: '1f60e',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f60e.png'
          },
          {
            name: '1f929',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f929.png'
          },
          {
            name: '1f921',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f921.png'
          },
          {
            name: '1f920',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f920.png'
          },
          {
            name: '1f917',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f917.png'
          },
          {
            name: '1f60f',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f60f.png'
          },
          {
            name: '1f636',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f636.png'
          },
          {
            name: '1f610',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f610.png'
          },
          {
            name: '1f611',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f611.png'
          },
          {
            name: '1f612',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f612.png'
          },
          {
            name: '1f644',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f644.png'
          },
          {
            name: '1f914',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f914.png'
          },
          {
            name: '1f925',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f925.png'
          },
          {
            name: '1f92d',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f92d.png'
          },
          {
            name: '1f92b',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f92b.png'
          },
          {
            name: '1f92c',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f92c.png'
          },
          {
            name: '1f92f',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f92f.png'
          },
          {
            name: '1f633',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f633.png'
          },
          {
            name: '1f61e',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f61e.png'
          },
          {
            name: '1f61f',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f61f.png'
          },
          {
            name: '1f620',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f620.png'
          },
          {
            name: '1f621',
            title: '微笑',
            src: 'https://twemoji.maxcdn.com/2/72x72/1f621.png'
          }
        ]
      }
    ])
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
      return this.$createElement(AddFriend)
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
        const isEnd = res.items.length === 0
        setTimeout(() => {
          next(messages, isEnd)
        }, 1000)
      })
      .catch(() => {
        setTimeout(() => {
          next([], true)
        }, 1000)
      })
  }

  private handleMenuAvatarClick() {
    console.log('Event:menu-avatar-click')
  }

  private handleMessageClick(e: any, key: any, message: Message) {
    const imui = this.$refs.IMUI as any
    if (key === 'status') {
      imui.updateMessage(message.id, message.toContactId, {
        status: 'going'
      })
      const chatMessage = Message.tryParseToChatMessage(message)
      this.connection
        .invoke('SendMessage', chatMessage)
        .then(() => {
          imui
            .updateMessage(message.id, message.toContactId, {
              status: 'succeed'
            })
        })
        .catch(() => {
          imui
            .updateMessage(message.id, message.toContactId, {
              status: 'failed'
            })
        })
    }
  }

  private handleReceiveMessage(chatMessage: ChatMessage) {
    this.trigger('onReceivedChatMessage', chatMessage)
  }

  private handleSendMessage(message: Message, next: any, file: any) {
    const imui = this.$refs.IMUI as any
    const chatMessage = Message.tryParseToChatMessage(message)
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

  private onReceivedChatMessage(chatMessage: ChatMessage) {
    const message = new Message()
    message.fromChatMessage(chatMessage)
    const imui = this.$refs.IMUI as any
    imui.appendMessage(message, chatMessage.formUserId)
    const currentContact = imui.currentContact
    if (currentContact && currentContact.id === chatMessage.formUserId) {
      currentContact.lastContent = imui.lastContentRender(message)
      currentContact.lastSendTime = new Date(chatMessage.sendTime).getTime()
    } else {
      imui.updateContact(chatMessage.formUserId, {
        unread: '+1',
        lastSendTime: new Date(chatMessage.sendTime).getTime(),
        lastContent: imui.lastContentRender(message)
      })
    }
  }

  private onNewFriendValidation(notify: any) {
    this.$confirm(
      notify.data.properties.message,
      this.$t('AbpIdentity.AreYouSure').toString(),
      {
        callback: action => {
          if (action === 'confirm') {
            const addUserFriend = new AddUserFriend(notify.data.properties.userId)
            ImApiService
              .addFriend(addUserFriend)
              .then(() => {
                this.$message.success('已添加对方为好友!')
              })
          }
        }
      }
    )
  }

  private onChangeContract(contract: any) {
    const imui = this.$refs.IMUI as any
    imui.updateContact(contract.id, {
      unread: 0
    })
    imui.closeDrawer()
    imui.forceUpdateMessage()
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
