import {
  ListResultDto,
  PagedAndSortedResultRequestDto,
  PagedResultDto,
} from '../../model/baseModel';
import { UserCard } from './baseModel';

export interface UserFriend extends UserCard {
  friendId: string;
  black: boolean;
  specialFocus: boolean;
  dontDisturb: boolean;
  remarkName: string;
  /** 是否在线,仅ui项目使用 */
  onLined: boolean;
}

export interface FriendCreateRequest {
  friendId: string;
}

export interface FriendAddRequest extends FriendCreateRequest {
  remarkName: string;
}

export interface GetMyFriendsRequest {
  sorting?: string;
}

export interface GetMyFriendsPagedRequest extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface UserFriendListResult extends ListResultDto<UserFriend> {}

export interface UserFriendPagedResult extends PagedResultDto<UserFriend> {}
