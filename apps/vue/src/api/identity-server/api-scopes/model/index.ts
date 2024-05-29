import { UserClaim, Property } from '../../model';

export type ApiScopeClaim = UserClaim;

export type ApiScopeProperty = Property;

export interface ApiScope extends ExtensibleAuditedEntityDto<string> {
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

export interface GetApiScopePagedRequest extends PagedAndSortedResultRequestDto {
  filter?: string;
}
