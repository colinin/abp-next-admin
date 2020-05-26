import { VuexModule, Module, Action, Mutation, getModule } from 'vuex-module-decorators'
import UserApiService, { UserLoginData, UserLoginPhoneData } from '@/api/users'
import TenantService from '@/api/tenant'
import { getToken, setToken, removeToken, getRefreshToken, setRefreshToken } from '@/utils/localStorage'
import { resetRouter } from '@/router'
import { TagsViewModule } from './tags-view'
import { removeTenant, setTenant } from '@/utils/sessions'
import { PermissionModule } from '@/store/modules/permission'
import { AbpConfigurationModule } from '@/store/modules/abp'
import store from '@/store'

export interface IUserState {
  token: string
  id: string
  name: string
  roles: string[]
  email: string
}

@Module({ dynamic: true, store, name: 'user' })
class User extends VuexModule implements IUserState {
  public token = getToken() || ''
  public id = ''
  public name = ''
  public roles: string[] = []
  public email = ''

  @Mutation
  private SET_TOKEN(token: string) {
    this.token = token
    setToken(token)
  }

  @Mutation
  private SET_ID(id: string) {
    this.id = id
  }

  @Mutation
  private SET_NAME(name: string) {
    this.name = name
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
    if (userInfo.tenantName) {
      await this.PreLogin(userInfo.tenantName)
    }
    const userLoginData = new UserLoginData()
    userLoginData.userName = userInfo.username
    userLoginData.password = userInfo.password
    const loginResult = await UserApiService.userLogin(userLoginData)
    const token = loginResult.token_type + ' ' + loginResult.access_token
    this.SET_TOKEN(token)
    setRefreshToken(loginResult.refresh_token)
    await this.PostLogin()
  }

  @Action({ rawError: true })
  public async PhoneLogin(userInfo: { tenantName: string | undefined, phoneNumber: string, verifyCode: string}) {
    if (userInfo.tenantName) {
      await this.PreLogin(userInfo.tenantName)
    }
    const userLoginData = new UserLoginPhoneData()
    userLoginData.phoneNumber = userInfo.phoneNumber
    userLoginData.verifyCode = userInfo.verifyCode
    const loginResult = await UserApiService.userLoginWithPhone(userLoginData)
    const token = loginResult.token_type + ' ' + loginResult.access_token
    this.SET_TOKEN(token)
    setRefreshToken(loginResult.refresh_token)
    await this.PostLogin()
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
    this.SET_ID(userInfo.sub)
    this.SET_NAME(userInfo.name)
    this.SET_EMAIL(userInfo.email)
    return userInfo
  }

  @Action
  public async LogOut() {
    if (this.token === '') {
      throw Error('LogOut: token is undefined!')
    }
    const token = getRefreshToken()
    if (token) {
      await UserApiService.userLogout(token)
    }
    this.SET_TOKEN('')
    this.SET_ROLES([])
    removeToken()
    removeTenant()
    resetRouter()
    // Reset visited views and cached views
    TagsViewModule.delAllViews()
    PermissionModule.ResetPermissions()
    PermissionModule.ResetRoutes()
  }

  @Action
  public RefreshSession() {
    return new Promise((resolve, reject) => {
      const token = getToken()
      if (token) {
        UserApiService.refreshToken(token).then(result => {
          const token = result.token_type + ' ' + result.access_token
          setRefreshToken(result.refresh_token)
          this.SET_TOKEN(token)
          return resolve(result)
        }).catch(error => {
          return reject(error)
        })
      } else {
        return resolve('')
      }
    })
  }

  @Action
  private async PreLogin(tenantName: string) {
    const tenantResult = await TenantService.getTenantByName(tenantName)
    setTenant(tenantResult.tenantId)
  }

  @Action
  private async PostLogin() {
    await AbpConfigurationModule.GetAbpConfiguration()
  }
}

export const UserModule = getModule(User)
