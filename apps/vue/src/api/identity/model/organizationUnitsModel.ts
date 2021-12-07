import {
  AuditedEntityDto,
  PagedAndSortedResultRequestDto,
  ListResultDto,
  PagedResultDto,
} from '../../model/baseModel';

/** 组织机构 */
export class OrganizationUnit extends AuditedEntityDto {
  /** 标识 */
  id!: string;
  /** 父节点标识 */
  parentId?: string;
  /** 编号 */
  code!: string;
  /** 显示名称 */
  displayName!: string;
}

/** 组织机构分页查询对象 */
export class GetOrganizationUnitPagedRequest extends PagedAndSortedResultRequestDto {
  /** 过滤字符 */
  filter!: string;
}

/** 组织结构分页返回结果 */
export class OrganizationUnitPagedResult extends PagedResultDto<OrganizationUnit> {}

/** 组织结构列表返回结果 */
export class OrganizationUnitListResult extends ListResultDto<OrganizationUnit> {}

/** 组织机构创建对象 */
export class CreateOrganizationUnit {
  /** 显示名称 */
  displayName!: string;
  /** 父节点标识 */
  parentId?: string;
}

/** 组织机构变更对象 */
export class UpdateOrganizationUnit {
  /** 显示名称 */
  displayName!: string;
}

/** 组织机构增加部门对象 */
export class OrganizationUnitAddRole {
  /** 角色标识列表 */
  roleIds: string[] = [];
}

/** 组织机构增加用户对象 */
export class OrganizationUnitAddUser {
  /** 用户标识列表 */
  userIds: string[] = [];
}
