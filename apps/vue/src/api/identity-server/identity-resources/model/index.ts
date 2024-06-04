import { UserClaim, Property } from '../../model';

export type IdentityResourceClaim = UserClaim;

export type IdentityResourceProperty = Property;

export interface IdentityResource extends ExtensibleAuditedEntityDto<string> {
  name: string;
  displayName: string;
  description: string;
  enabled: boolean;
  required: boolean;
  emphasize: boolean;
  showInDiscoveryDocument: boolean;
  userClaims: IdentityResourceClaim[];
  properties: IdentityResourceProperty[];
}

export interface IdentityResourceCreateOrUpdate {
  name: string;
  displayName: string;
  description: string;
  enabled: boolean;
  required: boolean;
  emphasize: boolean;
  showInDiscoveryDocument: boolean;
  userClaims: IdentityResourceClaim[];
  properties: IdentityResourceProperty[];
}

export type IdentityResourceCreate = IdentityResourceCreateOrUpdate;

export type IdentityResourceUpdate = IdentityResourceCreateOrUpdate;

export interface GetIdentityResourcePagedRequest extends PagedAndSortedResultRequestDto {
  filter?: string;
}
