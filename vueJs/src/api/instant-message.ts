import ApiService from './serviceBase'
import { PagedResultDto, PagedAndSortedResultRequestDto, ListResultDto } from './types'

const serviceUrl = process.env.VUE_APP_BASE_API

export default class ImApiService {
  public static addFriend(payload: AddUserFriend) {
    const _url = '/api/im/my-friends'
    return ApiService.Post<void>(_url, payload, serviceUrl)
  }

  public static addRequest(payload: RequestUserFriend) {
    const _url = '/api/im/my-friends/add-request'
    return ApiService.Post<void>(_url, payload, serviceUrl)
  }

  public static getMyFriends(payload: MyFriendGetByPaged) {
    let _url = '/api/im/my-friends'
    _url += '?filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&reverse=' + payload.reverse
    _url += '&skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<UserFriend>>(_url, serviceUrl)
  }

  public static getMyAllFriends(payload: MyFrientGetAll) {
    let _url = '/api/im/my-friends/all'
    _url += '?sorting=' + payload.sorting
    _url += '&reverse=' + payload.reverse
    return ApiService.Get<ListResultDto<UserFriend>>(_url, serviceUrl)
  }

  public static getMyLastMessages(payload: GetUserLastMessage) {
    let _url = '/api/im/chat/my-last-messages'
    _url += '?sorting=' + payload.sorting
    _url += '&reverse=' + payload.reverse
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<ListResultDto<ChatMessage>>(_url, serviceUrl)
  }

  public static getMyChatMessages(payload: UserMessageGetByPaged) {
    let _url = '/api/im/chat/my-messages'
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

export class AddUserFriend {
  friendId!: string

  constructor(
    friendId: string
  ) {
    this.friendId = friendId
  }
}

export class RequestUserFriend {
  friendId!: string
  remarkName!: string

  constructor(
    friendId: string,
    remarkName: string
  ) {
    this.friendId = friendId
    this.remarkName = remarkName
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

export class MyFrientGetAll {
  sorting = 'UserName'
  reverse = false
}

export class GetUserLastMessage {
  sorting = 'SendTime'
  reverse = false
  maxResultCount = 10
}

export enum MessageType {
  Text = 0,
  Image = 10,
  Link = 20,
  Video = 30,
  Voice = 40,
  File = 50
}

export class UserMessageGetByPaged extends PagedAndSortedResultRequestDto {
  filter = ''
  messageType = MessageType.Text
  sorting = 'CreationTime'
  reverse = true
  receiveUserId = ''
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
    switch (messageType) {
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
