import type {
  AuditedEntityDto,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

import type { GetUserPagedListInput } from './users';

/** 组织机构 */
interface OrganizationUnitDto extends AuditedEntityDto<string> {
  /** 编号 */
  code: string;
  /** 显示名称 */
  displayName: string;
  /** 父节点标识 */
  parentId?: string;
}

/** 组织机构分页查询对象 */
interface GetOrganizationUnitPagedListInput
  extends PagedAndSortedResultRequestDto {
  /** 过滤字符 */
  filter?: string;
}

/** 组织机构创建对象 */
interface OrganizationUnitCreateDto {
  /** 显示名称 */
  displayName: string;
  /** 父节点标识 */
  parentId?: string;
}

/** 组织机构变更对象 */
interface OrganizationUnitUpdateDto {
  /** 显示名称 */
  displayName: string;
}

/** 组织机构增加部门对象 */
interface OrganizationUnitAddRoleInput {
  /** 角色标识列表 */
  roleIds: string[];
}

/** 组织机构增加用户对象 */
interface OrganizationUnitAddUserInput {
  /** 用户标识列表 */
  userIds: string[];
}

interface OrganizationUnitGetChildrenDto {
  /** 上级组织机构id */
  id: string;
  /** 递归查询子级 */
  recursive?: boolean;
}

interface GetIdentityUsersInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

interface GetIdentityRolesInput extends PagedAndSortedResultRequestDto {
  filter?: string;
}

interface GetUnaddedUserListInput extends GetUserPagedListInput {
  id: string;
}

interface GetUnaddedRoleListInput extends GetUserPagedListInput {
  id: string;
}

interface OrganizationUnitAddUserDto {
  userIds: string[];
}

interface OrganizationUnitAddRoleDto {
  roleIds: string[];
}

export type {
  GetIdentityRolesInput,
  GetIdentityUsersInput,
  GetOrganizationUnitPagedListInput,
  GetUnaddedRoleListInput,
  GetUnaddedUserListInput,
  OrganizationUnitAddRoleDto,
  OrganizationUnitAddRoleInput,
  OrganizationUnitAddUserDto,
  OrganizationUnitAddUserInput,
  OrganizationUnitCreateDto,
  OrganizationUnitDto,
  OrganizationUnitGetChildrenDto,
  OrganizationUnitUpdateDto,
};
