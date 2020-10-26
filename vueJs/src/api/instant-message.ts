import ApiService from './serviceBase'
import { PagedResultDto, PagedAndSortedResultRequestDto, PagedResultRequestDto } from './types'

const serviceUrl = process.env.VUE_APP_BASE_API

export default class ImApiService {
  public static getMyFriends(payload: MyFriendGetByPaged) {
    let _url = '/api/im/my-friends'
    _url += '?filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&reverse=' + payload.reverse
    _url += '&skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<UserFriend>>(_url, serviceUrl)
  }

  public static getMyChatMessages(payload: UserMessageGetByPaged) {
    let _url = '/api/im/chat/messages/me'
    _url += '?receiveUserId=' + payload.receiveUserId
    _url += '&messageType=' + payload.messageType
    _url += '&filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&reverse=' + payload.reverse
    _url += '&skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<ChatMessage>>(_url, serviceUrl)
  }
}

export enum Sex
{
  Male,
  Female,
  Other
}

export class UserCard {
  tenantId?: string
  userId = ''
  userName = ''
  avatarUrl = ''
  nickName = ''
  age = 0
  sex = Sex.Male
  sign = ''
  description = ''
  birthday?: Date
}

export class UserFriend extends UserCard {
  friendId = ''
  remarkName = ''
  black = false
  specialFocus = false
  dontDisturb = false
}

export class MyFriendGetByPaged extends PagedAndSortedResultRequestDto {
  filter = ''
  sorting = 'UserName'
  skipCount = 0
  reverse = false
}

export class UserMessageGetByPaged extends PagedAndSortedResultRequestDto {
  filter = ''
  messageType = MessageType.Text
  sorting = 'CreationTime'
  reverse = true
  receiveUserId = ''
}

export enum MessageType {
  Text = 0,
  Image = 10,
  Link = 20,
  Video = 30,
  Voice = 40,
  File = 50
}

export class ChatMessage {
  tenantId?: string
  groupId = ''
  messageId = ''
  formUserId = ''
  formUserName = ''
  toUserId = ''
  content = ''
  sendTime = new Date()
  isAnonymous = false
  messageType = MessageType.Text

  public static getType(messageType: MessageType) {
    switch(messageType) {
      case MessageType.Text :
      case MessageType.Link :
        return 'text'
      case MessageType.Video :
        return 'video'
      case MessageType.Image :
        return 'image'
      case MessageType.Voice :
        return 'voice'
      case MessageType.File :
        return 'file'
      default :
      return 'text'
    }
  }
}
