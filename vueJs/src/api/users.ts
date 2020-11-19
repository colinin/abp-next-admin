import qs from 'querystring'
import { PagedAndSortedResultRequestDto, FullAuditedEntityDto, PagedResultDto, ListResultDto, ExtensibleObject } from '@/api/types'
import { OrganizationUnit } from './organizationunit'
import ApiService from './serviceBase'

const IdentityServiceUrl = process.env.VUE_APP_BASE_API
const IdentityServerUrl = process.env.VUE_APP_BASE_IDENTITY_SERVER

export default class UserApiService {
  public static getUsers(input: UsersGetPagedDto) {
    let _url = '/api/identity/users'
    _url += '?skipCount=' + input.skipCount
    _url += '&maxResultCount=' + input.maxResultCount
    if (input.sorting) {
      _url += '&sorting=' + input.sorting
    }
    if (input.filter) {
      _url += '&filter=' + input.filter
    }
    return ApiService.Get<PagedResultDto<User>>(_url, IdentityServiceUrl)
  }

  public static getUserById(userId: string) {
    let _url = '/api/identity/users/'
    _url += userId
    return ApiService.Get<User>(_url, IdentityServiceUrl)
  }

  public static getUserByName(userName: string) {
    let _url = '/api/identity/users/by-username/'
    _url += userName
    return ApiService.Get<User>(_url, IdentityServiceUrl)
  }

  public static updateUser(userId: string, userData: UserUpdate) {
    let _url = '/api/identity/users/'
    _url += userId
    return ApiService.Put<User>(_url, userData, IdentityServiceUrl)
  }

  public static deleteUser(userId: string | undefined) {
    let _url = '/api/identity/users/'
    _url += userId
    return ApiService.Delete(_url, IdentityServiceUrl)
  }

  public static createUser(userData: UserCreate) {
    const _url = '/api/identity/users'
    return ApiService.Post<User>(_url, userData, IdentityServiceUrl)
  }

  public static getUserRoles(userId: string) {
    let _url = '/api/identity/users'
    _url += '/' + userId
    _url += '/roles'
    return ApiService.Get<UserRoleDto>(_url, IdentityServiceUrl)
  }

  public static getUserClaims(userId: string) {
    const _url = '/api/identity/users/' + userId + '/claims'
    return ApiService.Get<ListResultDto<UserClaim>>(_url, IdentityServiceUrl)
  }

  public static addUserClaim(userId: string, payload: UserClaimCreateOrUpdate) {
    const _url = '/api/identity/users/' + userId + '/claims'
    return ApiService.Post<void>(_url, payload, IdentityServiceUrl)
  }

  public static updateUserClaim(userId: string, payload: UserClaimCreateOrUpdate) {
    const _url = '/api/identity/users/' + userId + '/claims'
    return ApiService.Put<void>(_url, payload, IdentityServiceUrl)
  }

  public static deleteUserClaim(userId: string, payload: UserClaimDelete) {
    let _url = '/api/identity/users/' + userId + '/claims'
    _url += '?claimType=' + payload.claimType
    _url += '&claimValue=' + payload.claimValue
    return ApiService.Delete(_url, IdentityServiceUrl)
  }

  public static getOrganizationUnits(userId: string, payload: UsersGetPagedDto) {
    let _url = '/api/identity/users/' + userId
    _url += '/organization-units'
    _url += '?filter=' + payload.filter
    _url += '&sorting=' + payload.sorting
    _url += '&skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    return ApiService.Get<PagedResultDto<OrganizationUnit>>(_url, IdentityServiceUrl)
  }

  public static removeOrganizationUnit(userId: string, ouId: string) {
    const _url = '/api/identity/users/' + userId + '/organization-units/' + ouId
    return ApiService.Delete(_url, IdentityServiceUrl)
  }

  public static changeUserOrganizationUnits(userId: string, payload: ChangeUserOrganizationUnitDto) {
    const _url = '/api/identity/users/organization-units/' + userId
    return ApiService.Put<void>(_url, payload, IdentityServiceUrl)
  }

  public static setUserRoles(userId: string, roles: string[]) {
    let _url = '/api/identity/users'
    _url += '/' + userId
    _url += '/roles'
    return ApiService.HttpRequest({
      baseURL: IdentityServiceUrl,
      url: _url,
      data: { RoleNames: roles },
      method: 'PUT'
    })
  }

  public static changePassword(input: UserChangePasswordDto) {
    const _url = '/api/identity/my-profile/change-password'
    return ApiService.HttpRequest({
      baseURL: IdentityServiceUrl,
      url: _url,
      data: input,
      method: 'POST'
    })
  }

  public static resetPassword(input: UserResetPasswordData) {
    const _url = '/api/account/phone/reset-password'
    return ApiService.HttpRequest({
      baseURL: IdentityServiceUrl,
      url: _url,
      data: input,
      method: 'PUT'
    })
  }

  public static userRegister(registerData: UserRegisterData) {
    const _url = '/api/account/phone/register'
    return ApiService.HttpRequest<User>({
      baseURL: IdentityServiceUrl,
      url: _url,
      method: 'POST',
      data: registerData
    })
  }

  public static getUserInfo() {
    const _url = '/connect/userinfo'
    return ApiService.HttpRequest<UserInfo>({
      baseURL: IdentityServerUrl,
      url: _url,
      method: 'GET'
    })
  }

  public static userLogin(loginData: UserLoginData) {
    const _url = '/connect/token'
    const login = {
      grant_type: 'password',
      username: loginData.userName,
      password: loginData.password,
      client_id: process.env.VUE_APP_CLIENT_ID,
      client_secret: process.env.VUE_APP_CLIENT_SECRET
    }
    return ApiService.HttpRequest<UserLoginResult>({
      baseURL: IdentityServerUrl,
      url: _url,
      method: 'POST',
      data: qs.stringify(login),
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      }
    })
  }

  /** 发送短信登录验证码 */
  public static sendSmsSigninCode(phoneNumber: string) {
    const _url = '/api/account/phone/send-signin-code'
    return ApiService.HttpRequest<void>({
      baseURL: IdentityServiceUrl,
      url: _url,
      method: 'POST',
      data: {
        phoneNumber: phoneNumber
      }
    })
  }

  public static sendSmsResetPasswordCode(phoneNumber: string) {
    const _url = '/api/account/phone/send-password-reset-code'
    return ApiService.HttpRequest<void>({
      baseURL: IdentityServiceUrl,
      url: _url,
      method: 'POST',
      data: {
        phoneNumber: phoneNumber
      }
    })
  }

  public static sendSmsRegisterCode(phoneNumber: string) {
    const _url = '/api/account/phone/send-register-code'
    return ApiService.HttpRequest<void>({
      baseURL: IdentityServiceUrl,
      url: _url,
      method: 'POST',
      data: {
        phoneNumber: phoneNumber
      }
    })
  }

  public static userLoginWithPhone(loginData: UserLoginPhoneData) {
    const _url = '/connect/token'
    const login = {
      grant_type: 'phone_verify',
      phone_number: loginData.phoneNumber,
      phone_verify_code: loginData.verifyCode,
      client_id: process.env.VUE_APP_CLIENT_ID,
      client_secret: process.env.VUE_APP_CLIENT_SECRET
    }
    return ApiService.HttpRequest<UserLoginResult>({
      baseURL: IdentityServerUrl,
      url: _url,
      method: 'POST',
      data: qs.stringify(login),
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      }
    })
  }

  public static refreshToken(token: string, refreshToken: string) {
    const _url = '/connect/token'
    const refresh = {
      grant_type: 'refresh_token',
      refresh_token: refreshToken,
      client_id: process.env.VUE_APP_CLIENT_ID,
      client_secret: process.env.VUE_APP_CLIENT_SECRET
    }
    return ApiService.HttpRequest<UserLoginResult>({
      baseURL: IdentityServerUrl,
      url: _url,
      method: 'POST',
      data: qs.stringify(refresh),
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
        Authorization: token
      }
    })
  }

  public static userLogout(token: string | undefined) {
    if (token) {
      const _url = '/connect/revocation'
      const loginOut = {
        token: token,
        client_id: process.env.VUE_APP_CLIENT_ID,
        client_secret: process.env.VUE_APP_CLIENT_SECRET
      }
      return ApiService.HttpRequestWithOriginResponse({
        baseURL: IdentityServerUrl,
        url: _url,
        method: 'post',
        data: qs.stringify(loginOut),
        headers: {
          'Content-Type': 'application/x-www-form-urlencoded'
        }
      })
    }
  }
}

/** 用户列表查询对象 */
export class UsersGetPagedDto extends PagedAndSortedResultRequestDto {
  /** 查询过滤字段 */
  filter = ''

  constructor() {
    super()
    this.sorting = 'name'
  }
}

/** 用户注册对象 */
export class UserRegisterData {
  /** 手机号码 */
  phoneNumber!: string
  /** 手机验证码 */
  verifyCode!: string
  /** 名称 */
  name?: string
  /** 用户名 */
  userName?: string
  /** 密码 */
  password!: string
  /** 邮件地址 */
  emailAddress!: string
}

/** 用户登录对象 */
export class UserLoginData {
  /** 用户名 */
  userName!: string
  /** 用户密码 */
  password!: string
}

export enum VerifyType {
  Register = 0,
  Signin = 10,
  ResetPassword = 20
}

export class PhoneVerify {
  phoneNumber!: string
  verifyType!: VerifyType
}

export class UserResetPasswordData {
  /** 手机号码 */
  phoneNumber!: string
  /** 手机验证码 */
  verifyCode!: string
  /** 新密码 */
  newPassword!: string
}

/** 用户手机登录对象 */
export class UserLoginPhoneData {
  /** 手机号码 */
  phoneNumber!: string
  /** 手机验证码 */
  verifyCode!: string
}

/** 用户信息对象 由IdentityServer提供 */
export class UserInfo {
  /** 标识 */
  sub!: string
  /** 名称 */
  name!: string
  /** 邮件地址 */
  email!: string
  /** 联系方式 */
  phone_number!: number
}

/** 用户登录返回对象 */
export class UserLoginResult {
  /** 访问令牌 */
  access_token!: string
  /** 过期时间 */
  expires_in!: number
  /** 令牌类型 */
  token_type!: string
  /** 刷新令牌 */
  refresh_token!: string
}

/** 用户密码变更对象 */
export class UserChangePasswordDto {
  /** 当前密码 */
  currentPassword!: string
  /** 新密码 */
  newPassword!: string
}

/** 用户角色对象 */
export class UserRoleDto {
  /** 角色列表 */
  items: IUserRole[]

  constructor() {
    this.items = new Array<IUserRole>()
  }
}

/** 用户角色接口 */
export class UserRole implements IUserRole {
  /** 标识 */
  id!: string
  /** 名称 */
  name!: string
  /** 是否默认角色 */
  isDefault!: boolean
  /** 是否静态角色 */
  isStatic!: boolean
  /** 是否公共角色 */
  isPublic!: boolean
}

export class UserCreateOrUpdate extends ExtensibleObject {
  /** 用户名 */
  name = ''
  /** 用户账户 */
  userName = ''
  /** 用户简称 */
  surname = ''
  /** 邮件地址 */
  email = ''
  /** 联系方式 */
  phoneNumber = ''
  /** 登录锁定 */
  lockoutEnabled = false
  /** 角色列表 */
  roleNames: string[] | null = null
  /** 密码 */
  password: string | null = null
}

/** 变更用户对象 */
export class UserUpdate extends UserCreateOrUpdate {
  /** 并发令牌 */
  concurrencyStamp = ''
}

export class UserCreate extends UserCreateOrUpdate {
}

/** 用户对象 */
export class User extends FullAuditedEntityDto implements IUser {
  /** 用户名 */
  name = ''
  /** 用户账户 */
  userName = ''
  /** 用户简称 */
  surname = ''
  /** 邮件地址 */
  email = ''
  /** 联系方式 */
  phoneNumber = ''
  /** 双因素验证 */
  twoFactorEnabled = false
  /** 登录锁定 */
  lockoutEnabled = false
  /** 用户标识 */
  id = ''
  /** 租户标识 */
  tenentId? = ''
  /** 邮箱已验证 */
  emailConfirmed = false
  /** 联系方式已验证 */
  phoneNumberConfirmed = false
  /** 锁定截止时间 */
  lockoutEnd?: Date = undefined
  /** 并发令牌 */
  concurrencyStamp = ''
}

/** 用户对象接口 */
export interface IUser {
  /** 用户名 */
  name: string
  /** 用户账户 */
  userName: string
  /** 用户简称 */
  surname?: string
  /** 邮件地址 */
  email: string
  /** 联系方式 */
  phoneNumber?: string
  /** 双因素验证 */
  twoFactorEnabled: boolean
  /** 登录锁定 */
  lockoutEnabled: boolean
}

/** 用户角色接口 */
export interface IUserRole {
  /** 角色标识 */
  id: string
  /** 角色名称 */
  name: string
  /** 默认角色 */
  isDefault: boolean
  /** 静态角色 */
  isStatic: boolean
  /** 公共角色 */
  isPublic: boolean
}

export class ChangeUserOrganizationUnitDto {
  organizationUnitIds = new Array<string>()

  public addOrganizationUnit(id: string) {
    this.organizationUnitIds.push(id)
  }

  public removeOrganizationUnit(id: string) {
    const index = this.organizationUnitIds.findIndex(ouId => ouId === id)
    this.organizationUnitIds.splice(index, 1)
  }
}

export class UserClaimCreateOrUpdate {
  claimType = ''
  claimValue = ''
}

export class UserClaimDelete {
  claimType = ''
  claimValue = ''
}

export class UserClaim extends UserClaimCreateOrUpdate {
  id!: string
}
