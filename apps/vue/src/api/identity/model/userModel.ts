import {
  ExtensibleObject,
  FullAuditedEntityDto,
  ListResultDto,
  PagedAndSortedResultRequestDto,
  PagedResultDto,
} from '../../model/baseModel';

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

/** 用户对象 */
export class User extends FullAuditedEntityDto implements IUser {
  /** 用户名 */
  name = '';
  /** 用户账户 */
  userName = '';
  /** 用户简称 */
  surname = '';
  /** 邮件地址 */
  email = '';
  /** 联系方式 */
  phoneNumber = '';
  /** 双因素验证 */
  twoFactorEnabled = false;
  /** 登录锁定 */
  lockoutEnabled = false;
  /** 用户标识 */
  id = '';
  /** 租户标识 */
  tenentId? = '';
  /** 邮箱已验证 */
  emailConfirmed = false;
  /** 联系方式已验证 */
  phoneNumberConfirmed = false;
  /** 锁定截止时间 */
  lockoutEnd?: Date = undefined;
  /** 并发令牌 */
  concurrencyStamp = '';
  /** 已激活的用户 */
  isActive = true;
  /** 角色列表 */
  roleNames: string[] = [];
}

export class CreateOrUpdateUser extends ExtensibleObject {
  /** 用户名 */
  name = '';
  /** 用户账户 */
  userName = '';
  /** 用户简称 */
  surname = '';
  /** 邮件地址 */
  email = '';
  /** 联系方式 */
  phoneNumber = '';
  /** 登录锁定 */
  lockoutEnabled = false;
  /** 角色列表 */
  roleNames: string[] | null = null;
  /** 密码 */
  password: string | null = null;
}

/** 变更用户对象 */
export class UpdateUser extends CreateOrUpdateUser {
  /** 并发令牌 */
  concurrencyStamp = '';
}

export class CreateUser extends CreateOrUpdateUser {}

export class GetUserPagedRequest extends PagedAndSortedResultRequestDto {
  filter = '';
}

export class UserPagedResult extends PagedResultDto<User> {}

export class UserClaimBase {
  claimType = '';
  claimValue = '';
}

export class CreateUserClaim extends UserClaimBase {}

export class UpdateUserClaim extends UserClaimBase {
  newClaimValue!: string;
}

export class UserClaim extends UserClaimBase {
  id!: string;
}

export class UserClaimListResult extends ListResultDto<UserClaim> {}
