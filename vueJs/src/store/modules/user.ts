import { VuexModule, Module, Action, Mutation, getModule } from 'vuex-module-decorators'
import UserApiService, { UserLoginData } from '@/api/users'
import TenantService from '@/api/tenant'
import { getToken, setToken, removeToken, getRefreshToken, setRefreshToken } from '@/utils/cookies'
import { resetRouter } from '@/router'
import { TagsViewModule } from './tags-view'
import { removeTenant, setTenant } from '@/utils/sessions'
import { PermissionModule } from '@/store/modules/permission'
import store from '@/store'

export interface IUserState {
  token: string
  name: string
  avatar: string
  introduction: string
  roles: string[]
  email: string
}

@Module({ dynamic: true, store, name: 'user' })
class User extends VuexModule implements IUserState {
  public token = getToken() || ''
  public refreshToken = getRefreshToken() || ''
  public name = ''
  public avatar = ''
  public introduction = ''
  public roles: string[] = []
  public email = ''

  @Mutation
  private SET_TOKEN(token: string) {
    this.token = token
  }

  @Mutation
  private SET_NAME(name: string) {
    this.name = name
  }

  @Mutation
  private SET_REFRESH_TOKEN(token: string) {
    this.refreshToken = token
  }

  @Mutation
  private SET_ROLES(roles: string[]) {
    this.roles = roles
  }

  @Mutation
  private SET_EMAIL(email: string) {
    this.email = email
  }

  @Action({ rawError: true })
  public async Login(userInfo: { tenantName: string | undefined, username: string, password: string}) {
    const userLoginData = new UserLoginData()
    userLoginData.userName = userInfo.username
    userLoginData.password = userInfo.password
    if (userInfo.tenantName) {
      const tenantResult = await TenantService.getTenantByName(userInfo.tenantName)
      if (tenantResult.success) {
        setTenant(tenantResult.tenantId)
      } else {
        throw new Error('给定的租户不可用: ' + userInfo.tenantName)
      }
    }
    const loginResult = await UserApiService.userLogin(userLoginData)
    const token = loginResult.token_type + ' ' + loginResult.access_token
    setToken(token)
    this.SET_TOKEN(token)
    setRefreshToken(loginResult.refresh_token)
    this.SET_REFRESH_TOKEN(loginResult.refresh_token)
  }

  @Action
  public ResetToken() {
    removeToken()
    removeTenant()
    this.SET_TOKEN('')
    this.SET_ROLES([])
  }

  @Action
  public async GetUserInfo() {
    if (this.token === '') {
      throw Error('GetUserInfo: token is undefined!')
    }
    const userInfo = await UserApiService.getUserInfo()
    this.SET_NAME(userInfo.name)
    this.SET_EMAIL(userInfo.email)
    return userInfo
  }

  // @Action
  // public async ChangeRoles(role: string) {
  //   // Dynamically modify permissions
  //   const token = role + '-token'
  //   this.SET_TOKEN(token)
  //   setToken(token)
  //   await this.GetUserInfo()
  //   resetRouter()
  //   // Generate dynamic accessible routes based on roles
  //   // PermissionModule.GenerateRoutes(this.roles)
  //   // Add generated routes
  //   router.addRoutes(PermissionModule.dynamicRoutes)
  //   // Reset visited views and cached views
  //   TagsViewModule.delAllViews()
  // }

  @Action
  public async LogOut() {
    if (this.token === '') {
      throw Error('LogOut: token is undefined!')
    }
    await UserApiService.userLogout(getRefreshToken())
    removeToken()
    resetRouter()
    // Reset visited views and cached views
    TagsViewModule.delAllViews()
    PermissionModule.ResetPermissions()
    this.SET_TOKEN('')
    this.SET_ROLES([])
  }
}

export const UserModule = getModule(User)
