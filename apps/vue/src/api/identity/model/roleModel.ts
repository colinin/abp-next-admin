import {
  ListResultDto,
  PagedAndSortedResultRequestDto,
  PagedResultDto,
} from '../../model/baseModel';

export class RoleBase {
  name!: string;
  isDefault!: boolean;
  isPublic!: boolean;
}

export class Role extends RoleBase {
  id!: string;
  isStatic!: boolean;
  concurrencyStamp?: string;
}

export class RoleListResult extends ListResultDto<Role> {}

export class RolePagedResult extends PagedResultDto<Role> {}

export class GetRolePagedRequest extends PagedAndSortedResultRequestDto {
  filter = '';
}

export class CreateRole extends RoleBase {
  isDefault = false;
  isPublic = true;
  name = '';
}

export class UpdateRole extends RoleBase {
  concurrencyStamp?: string;
}

export class ChangeRoleOrganizationUnitDto {
  organizationUnitIds: string[] = [];
}

export class RoleClaimBase {
  claimType = '';
  claimValue = '';
}

export class DeleteRoleClaim {
  claimType = '';
  claimValue = '';
}

export class CreateRoleClaim extends RoleClaimBase {}

export class UpdateRoleClaim extends RoleClaimBase {
  newClaimValue!: string;
}

export class RoleClaim extends RoleClaimBase {
  id!: string;
}

export class RoleClaimListResult extends ListResultDto<RoleClaim> {}
