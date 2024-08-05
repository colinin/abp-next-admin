import { defHttp } from '/@/utils/http/axios';
import {
  UserFriend,
  FriendCreateRequest,
  FriendAddRequest,
  GetMyFriendsRequest,
  GetMyFriendsPagedRequest,
} from './model';

export const create = (input: FriendCreateRequest) => {
  return defHttp.post<void>({
    url: '/api/im/my-friends',
    data: input,
  });
};

export const addFriend = (input: FriendAddRequest) => {
  return defHttp.post<void>({
    url: '/api/im/my-friends/add-request',
    data: input,
  });
};

export const getByFriendId = (friendId: string) => {
  return defHttp.get<UserFriend>({
    url: `/api/im/my-friends/${friendId}`,
  });
};

export const getList = (input: GetMyFriendsPagedRequest) => {
  return defHttp.get<PagedResultDto<UserFriend>>({
    url: '/api/im/my-friends',
    params: input,
  });
};

export const getAll = (input: GetMyFriendsRequest) => {
  return defHttp.get<ListResultDto<UserFriend>>({
    url: '/api/im/my-friends/all',
    params: input,
  });
};
