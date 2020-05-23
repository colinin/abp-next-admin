import qs from 'querystring'
import { pagerFormat } from '@/utils/index'
import { PagedAndSortedResultRequestDto, FullAuditedEntityDto, PagedResultDto } from '@/api/types'
import ApiService from './serviceBase'

const IdentityServiceUrl = process.env.VUE_APP_BASE_API
const IdentityServerUrl = process.env.VUE_APP_BASE_IDENTITY_SERVER

export default class UserApiService {

  public static getUsers(input: UsersGetPagedDto) {
    let _url = '/api/identity/users'
    _url += '?skipCount=' + pagerFormat(input.skipCount)
    _url += '&maxResultCount=' + input.maxResultCount
    if (input.sorting) {
      _url += '&sorting=' + input.sorting
    }
    if (input.filter) {
      _url += '&filter=' + input.filter
    }
    return ApiService.Get<PagedResultDto<UserDataDto>>(_url, IdentityServiceUrl)
  }

  public static getUserById(userId: string) {
    let _url = '/api/identity/users/'
    _url += userId
    return ApiService.Get<UserDataDto>(_url, IdentityServiceUrl)
  }

  public static getUserByName(userName: string) {
    let _url = '/api/identity/users/by-username/'
    _url += userName
    return ApiService.Get<UserDataDto>(_url, IdentityServiceUrl)
  }

  public static updateUser(userId: string | undefined, userData: UserUpdateDto) {
    let _url = '/api/identity/users/'
    _url += userId
    return ApiService.Put<UserDataDto>(_url, userData, IdentityServiceUrl)
  }

  public static deleteUser(userId: string | undefined) {
    let _url = '/api/identity/users/'
    _url += userId
    return ApiService.Delete(_url, IdentityServiceUrl)
  }

  public static createUser(userData: UserCreateDto) {
    const _url = '/api/identity/users'
    return ApiService.Post<UserDataDto>(_url, userData, IdentityServiceUrl)
  }

  public static getUserRoles(userId: string) {
    let _url = '/api/identity/users'
    _url += '/' + userId
    _url += '/roles'
    return ApiService.Get<UserRoleDto>(_url, IdentityServiceUrl)
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

  public static userRegister(registerData: UserRegisterData) {
    const _url = '/api/account/register'
    return ApiService.HttpRequest<UserDataDto>({
      baseURL: IdentityServerUrl,
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
  filter: string | undefined

  constructor() {
    super()
    this.sorting = 'name'
  }
}

/** 用户注册对象 */
export class UserRegisterData {
  /** 应用名称 */
  appName!: string
  /** 用户名 */
  userName!: string
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

/** 创建用户对象 */
export class UserCreateDto {
  /** 用户名 */
  name!: string
  /** 用户账户 */
  userName!: string
  /** 用户密码 */
  password!: string
  /** 用户简称 */
  surname?: string
  /** 邮件地址 */
  email!: string
  /** 联系方式 */
  phoneNumber: number | undefined
  /** 双因素验证 */
  twoFactorEnabled!: boolean
  /** 登录失败锁定 */
  lockoutEnabled!: boolean
  /** 用户列表 */
  roleNames?: string[]

  constructor() {
    this.twoFactorEnabled = false
    this.lockoutEnabled = true
    this.roleNames = new Array<string>()
  }
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
  /** 并发令牌 */
  concurrencyStamp: string | undefined
}

/** 变更用户对象 */
export class UserUpdateDto implements IUserData {
  /** 用户名 */
  name!: string;
  /** 用户账户 */
  userName!: string;
  /** 用户简称 */
  surname?: string;
  /** 邮件地址 */
  email!: string;
  /** 联系方式 */
  phoneNumber?: number;
  /** 双因素验证 */
  twoFactorEnabled!: boolean;
  /** 登录锁定 */
  lockoutEnabled!: boolean;
  /** 并发令牌 */
  concurrencyStamp!: string;
  /** 用户角色列表 */
  roles: string[]

  constructor() {
    this.roles = new Array<string>()
  }
}

/** 用户对象 */
export class UserDataDto extends FullAuditedEntityDto implements IUserData {
  /** 用户名 */
  name!: string
  /** 用户账户 */
  userName!: string
  /** 用户简称 */
  surname!: string
  /** 邮件地址 */
  email!: string
  /** 联系方式 */
  phoneNumber?: number
  /** 双因素验证 */
  twoFactorEnabled!: boolean
  /** 登录锁定 */
  lockoutEnabled!: boolean
  /** 并发令牌 */
  concurrencyStamp!: string
  /** 用户标识 */
  id!: string
  /** 租户标识 */
  tenentId: string | undefined
  /** 邮箱已验证 */
  emailConfirmed!: boolean
  /** 联系方式已验证 */
  phoneNumberConfirmed!: boolean
  /** 锁定截止时间 */
  lockoutEnd: Date | undefined
}

/** 用户对象接口 */
export interface IUserData {
  /** 用户名 */
  name: string
  /** 用户账户 */
  userName: string
  /** 用户简称 */
  surname?: string
  /** 邮件地址 */
  email: string
  /** 联系方式 */
  phoneNumber?: number
  /** 双因素验证 */
  twoFactorEnabled: boolean
  /** 登录锁定 */
  lockoutEnabled: boolean
  /** 锁定截止时间 */
  concurrencyStamp: string | undefined
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
  /** 并发令牌 */
  concurrencyStamp: string | undefined
}
