import {
  ExtensibleAuditedEntity,
  PagedAndSortedResultRequestDto,
  PagedResultDto,
} from '/@/api/model/baseModel';
import { UserClaim, Property } from './basicModel';

export type ApiScopeClaim = UserClaim;

export type ApiScopeProperty = Property;

export interface ApiScope extends ExtensibleAuditedEntity<string> {
  enabled: boolean;
  name: string;
  displayName: string;
  description: string;
  required: boolean;
  emphasize: boolean;
  showInDiscoveryDocument: boolean;
  userClaims: ApiScopeClaim[];
  properties: ApiScopeProperty[];
}

export interface ApiScopeCreateOrUpdate {
  enabled: boolean;
  displayName: string;
  description: string;
  required: boolean;
  emphasize: boolean;
  showInDiscoveryDocument: boolean;
  userClaims: ApiScopeClaim[];
  properties: ApiScopeProperty[];
}

export interface ApiScopeCreate extends ApiScopeCreateOrUpdate {
  name: string;
}

export type ApiScopeUpdate = ApiScopeCreateOrUpdate;

export class ApiScopePagedResult extends PagedResultDto<ApiScope> {}

export class GetApiScopePagedRequest extends PagedAndSortedResultRequestDto {
  filter = '';
}
