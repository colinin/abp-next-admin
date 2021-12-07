import {
  ExtensibleEntity,
  PagedAndSortedResultRequestDto,
  PagedResultDto,
} from '/@/api/model/baseModel';

export interface PersistedGrant extends ExtensibleEntity<string> {
  key: string;
  type: string;
  subjectId: string;
  sessionId: string;
  description: string;
  consumedTime?: Date;
  clientId: string;
  creationTime: Date;
  expiration?: Date;
  data: string;
}

export class PersistedGrantPagedResult extends PagedResultDto<PersistedGrant> {}

export class GetPersistedGrantPagedRequest extends PagedAndSortedResultRequestDto {
  filter = '';
  subjectId = '';
}
