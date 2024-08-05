import { defHttp } from '/@/utils/http/axios';
import { Group, GroupSearchRequest } from './model';

export const search = (input: GroupSearchRequest) => {
  return defHttp.get<PagedResultDto<Group>>({
    url: '/api/im/groups/search',
    params: input,
  });
};

export const getById = (groupId: string) => {
  return defHttp.get<Group>({
    url: `/api/im/groups/${groupId}`,
  });
};
