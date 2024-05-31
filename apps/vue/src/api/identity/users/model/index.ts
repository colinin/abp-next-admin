import { IdentityClaim } from "../../claims/model";

/** 用户对象接口 */
export interface IUser {
  /** 用户名 */
  name: string;
  /** 用户账户 */
  userName: string;
  /** 用户简称 */
  surname?: string;
  /** 邮件地址 */
  email: string;
  /** 联系方式 */
  phoneNumber?: string;
  /** 双因素验证 */
  twoFactorEnabled: boolean;
  /** 登录锁定 */
  lockoutEnabled: boolean;
}

export interface ChangePassword {
  currentPassword?: string;
  newPassword: string;
}

export interface SetPassword {
  password: string;
}

export interface IdentityUserOrganizationUnitUpdateDto {
  organizationUnitIds: string[]
}

/** 用户对象 */
export interface User extends FullAuditedEntityDto<string>, IUser, IHasConcurrencyStamp {
  /** 租户标识 */
  tenentId?: string;
  /** 邮箱已验证 */
  emailConfirmed: boolean;
  /** 联系方式已验证 */
  phoneNumberConfirmed: boolean;
  /** 锁定截止时间 */
  lockoutEnd?: Date;
  /** 已激活的用户 */
  isActive: boolean;
  /** 角色列表 */
  roleNames: string[];
}

export interface CreateOrUpdateUser extends ExtensibleObject {
   /** 用户名 */
   name: string;
   /** 用户账户 */
   userName: string;
   /** 用户简称 */
   surname?: string;
   /** 邮件地址 */
   email: string;
   /** 联系方式 */
   phoneNumber?: string;
   /** 登录锁定 */
   lockoutEnabled: boolean;
  /** 角色列表 */
  roleNames?: string[];
  /** 密码 */
  password?: string;
}

/** 变更用户对象 */
export interface UpdateUser extends CreateOrUpdateUser {}

export interface CreateUser extends CreateOrUpdateUser {}

export interface GetUserPagedRequest extends PagedAndSortedResultRequestDto {
  filter?: string;
}

export interface UserClaim extends IdentityClaim {
  id: string;
}
