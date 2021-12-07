import { PagedAndSortedResultRequestDto, PagedResultDto } from '../../model/baseModel';

export interface Group {
  id: string;
  name: string;
  avatarUrl: string;
  allowAnonymous: boolean;
  allowSendMessage: boolean;
  maxUserLength: number;
  groupUserCount: number;
}

export interface GroupSearchRequest extends PagedAndSortedResultRequestDto {
  filter: string;
}

export interface GroupPagedResult extends PagedResultDto<Group> {}
