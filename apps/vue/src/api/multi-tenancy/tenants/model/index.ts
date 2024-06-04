/** 租户查询过滤对象 */
export interface TenantGetByPaged extends PagedAndSortedResultRequestDto {
    /** 查询过滤字段 */
    filter?: string;
  }
  
  /** 租户创建对象 */
  export interface TenantCreateOrEdit {
    /** 管理员邮件地址 */
    adminEmailAddress: string;
    /** 管理员密码 */
    adminPassword: string;
    /** 租户名称 */
    name: string;
    /** 使用共享数据库 */
    useSharedDatabase: boolean;
    /** 默认连接字符串 */
    defaultConnectionString?: string;
  }
  
  /** 租户对象 */
  export interface TenantDto extends FullAuditedEntityDto<string> {
    /** 租户名称 */
    name: string;
  }
  
  /** 租户连接字符串 */
  export interface TenantConnectionString {
    /** 名称 */
    name: string;
    /** 值 */
    value: string;
  }
  
  export interface FindTenantResult {
    name: string;
    tenantId: string;
    success: string;
    isActive: boolean;
  }
  