import { FullAuditedEntityDto, PagedAndSortedResultRequestDto } from '/@/api/model/baseModel';

/** 租户查询过滤对象 */
export class TenantGetByPaged extends PagedAndSortedResultRequestDto {
  /** 查询过滤字段 */
  filter = '';
}

/** 租户创建对象 */
export class TenantCreateOrEdit {
  /** 管理员邮件地址 */
  adminEmailAddress = '';
  /** 管理员密码 */
  adminPassword = '';
  /** 租户名称 */
  name = '';
  /** 使用共享数据库 */
  useSharedDatabase = true;
  /** 默认连接字符串 */
  defaultConnectionString = '';
}

/** 租户对象 */
export class TenantDto extends FullAuditedEntityDto {
  /** 租户标识 */
  id!: string;
  /** 租户名称 */
  name!: string;
}

/** 租户连接字符串 */
export class TenantConnectionString {
  /** 名称 */
  name = '';
  /** 值 */
  value = '';
}

export class FindTenantResult {
  name = '';
  tenantId = '';
  success = '';
}
