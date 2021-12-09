import { defHttp } from '/@/utils/http/axios';
import {
  UserFriend,
  UserFriendListResult,
  UserFriendPagedResult,
  FriendCreateRequest,
  FriendAddRequest,
  GetMyFriendsRequest,
  GetMyFriendsPagedRequest,
} from './model/friendsModel';
import { format } from '/@/utils/strings';

enum Api {
  Create = 'api/im/my-friends',
  AddFriend = '/api/im/my-friends/add-request',
  GetList = '/api/im/my-friends',
  GetAll = '/api/im/my-friends/all',
  GetByFriendId = '/api/im/my-friends/{friendId}',
}

export const create = (input: FriendCreateRequest) => {
  return defHttp.post<void>({
    url: Api.Create,
    data: input,
  });
};

export const addFriend = (input: FriendAddRequest) => {
  return defHttp.post<void>({
    url: Api.AddFriend,
    data: input,
  });
};

export const getByFriendId = (friendId: string) => {
  return defHttp.get<UserFriend>({
    url: format(Api.GetByFriendId, { friendId: friendId }),
  });
};

export const getList = (input: GetMyFriendsPagedRequest) => {
  return defHttp.get<UserFriendPagedResult>({
    url: Api.GetList,
    params: input,
  });
};

export const getAll = (input: GetMyFriendsRequest) => {
  return defHttp.get<UserFriendListResult>({
    url: Api.GetAll,
    params: input,
  });
};
