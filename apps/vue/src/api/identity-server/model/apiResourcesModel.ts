import {
  ExtensibleAuditedEntity,
  PagedAndSortedResultRequestDto,
  PagedResultDto,
  SecretBase,
} from '/@/api/model/baseModel';
import { UserClaim, Property } from './basicModel';

export type ApiResourceSecret = SecretBase;

export type ApiResourceClaim = UserClaim;

export type ApiResourceProperty = Property;

export interface ApiResourceScope {
  scope: string;
}

export enum HashType {
  Sha256,
  Sha512,
}

export interface ApiResourceSecretCreateOrUpdate extends SecretBase {
  hashType: HashType;
}

export interface ApiResource extends ExtensibleAuditedEntity<string> {
  name: string;
  displayName: string;
  description: string;
  enabled: boolean;
  allowedAccessTokenSigningAlgorithms: string;
  showInDiscoveryDocument: boolean;
  secrets: ApiResourceSecret[];
  scopes: ApiResourceScope[];
  userClaims: ApiResourceClaim[];
  properties: ApiResourceProperty[];
}

export interface ApiResourceCreateOrUpdate {
  displayName: string;
  description: string;
  enabled: boolean;
  allowedAccessTokenSigningAlgorithms: string;
  showInDiscoveryDocument: boolean;
  secrets: ApiResourceSecretCreateOrUpdate[];
  scopes: ApiResourceScope[];
  userClaims: ApiResourceClaim[];
  properties: ApiResourceProperty[];
}

export interface ApiResourceCreate extends ApiResourceCreateOrUpdate {
  name: string;
}

export type ApiResourceUpdate = ApiResourceCreateOrUpdate;

export class ApiResourcePagedResult extends PagedResultDto<ApiResource> {}

export class GetApiResourcePagedRequest extends PagedAndSortedResultRequestDto {
  filter = '';
}
