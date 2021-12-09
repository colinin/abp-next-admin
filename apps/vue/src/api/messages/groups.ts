import { defHttp } from '/@/utils/http/axios';
import { Group, GroupPagedResult, GroupSearchRequest } from './model/groupModel';
import { format } from '/@/utils/strings';

enum Api {
  Search = '/api/im/groups/search',
  GetById = '/api/im/groups/{groupId}',
}

export const search = (input: GroupSearchRequest) => {
  return defHttp.get<GroupPagedResult>({
    url: Api.Search,
    params: input,
  });
};

export const getById = (groupId: string) => {
  return defHttp.get<Group>({
    url: format(Api.GetById, { groupId: groupId }),
  });
};
