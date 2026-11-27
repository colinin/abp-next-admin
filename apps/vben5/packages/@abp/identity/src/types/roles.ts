import type {
  IHasConcurrencyStamp,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

interface RoleDtoBase {
  isDefault: boolean;
  isPublic: boolean;
  name: string;
}

interface IdentityRoleDto extends IHasConcurrencyStamp, RoleDtoBase {
  [key: string]: any;
  id: string;
  isStatic: boolean;
}

interface GetRolePagedListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

type IdentityRoleCreateDto = RoleDtoBase;

interface IdentityRoleUpdateDto extends IHasConcurrencyStamp, RoleDtoBase {}

interface ChangeRoleOrganizationUnitDto {
  organizationUnitIds: string[];
}

export type {
  ChangeRoleOrganizationUnitDto,
  GetRolePagedListInput,
  IdentityRoleCreateDto,
  IdentityRoleDto,
  IdentityRoleUpdateDto,
};
