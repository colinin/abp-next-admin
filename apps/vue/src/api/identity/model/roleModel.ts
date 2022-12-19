import { IdentityClaim } from './claimModel';

export interface RoleBase {
  name: string;
  isDefault: boolean;
  isPublic: boolean;
}

export interface Role extends RoleBase {
  id: string;
  isStatic: boolean;
  concurrencyStamp?: string;
}

export interface RoleListResult extends ListResultDto<Role> {}

export interface RolePagedResult extends PagedResultDto<Role> {}

export interface GetRolePagedRequest extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface CreateRole extends RoleBase {
}

export interface UpdateRole extends RoleBase {
  concurrencyStamp?: string;
}

export interface ChangeRoleOrganizationUnitDto {
  organizationUnitIds: string[];
}

export interface RoleClaim extends IdentityClaim {
  id: string;
}

export interface RoleClaimListResult extends ListResultDto<RoleClaim> {}
