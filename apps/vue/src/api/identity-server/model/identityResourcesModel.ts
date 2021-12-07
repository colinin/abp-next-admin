import {
  ExtensibleAuditedEntity,
  PagedAndSortedResultRequestDto,
  PagedResultDto,
} from '/@/api/model/baseModel';
import { UserClaim, Property } from './basicModel';

export type IdentityResourceClaim = UserClaim;

export type IdentityResourceProperty = Property;

export interface IdentityResource extends ExtensibleAuditedEntity<string> {
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

export class IdentityResourcePagedResult extends PagedResultDto<IdentityResource> {}

export class GetIdentityResourcePagedRequest extends PagedAndSortedResultRequestDto {
  filter = '';
}
