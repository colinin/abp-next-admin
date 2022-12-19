/** 组织机构 */
export interface OrganizationUnit extends AuditedEntityDto<string> {
  /** 父节点标识 */
  parentId?: string;
  /** 编号 */
  code: string;
  /** 显示名称 */
  displayName: string;
}

/** 组织机构分页查询对象 */
export interface GetOrganizationUnitPagedRequest extends PagedAndSortedResultRequestDto {
  /** 过滤字符 */
  filter?: string;
}

/** 组织结构分页返回结果 */
export interface OrganizationUnitPagedResult extends PagedResultDto<OrganizationUnit> {}

/** 组织结构列表返回结果 */
export interface OrganizationUnitListResult extends ListResultDto<OrganizationUnit> {}

/** 组织机构创建对象 */
export interface CreateOrganizationUnit {
  /** 显示名称 */
  displayName: string;
  /** 父节点标识 */
  parentId?: string;
}

/** 组织机构变更对象 */
export interface UpdateOrganizationUnit {
  /** 显示名称 */
  displayName: string;
}

/** 组织机构增加部门对象 */
export interface OrganizationUnitAddRole {
  /** 角色标识列表 */
  roleIds: string[];
}

/** 组织机构增加用户对象 */
export interface OrganizationUnitAddUser {
  /** 用户标识列表 */
  userIds: string[];
}
