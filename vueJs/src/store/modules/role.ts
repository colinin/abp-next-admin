import { VuexModule, Module, Mutation, Action, getModule } from 'vuex-module-decorators'
import store from '@/store'
import { IRoleData } from '@/api/types'

export interface IRoleState {
  roles: IRoleData[]
}

@Module({ dynamic: true, store, name: 'role' })
class Role extends VuexModule implements IRoleState {
  public roles = new Array<IRoleData>()

  @Mutation
  private SET_ROLES(roles: IRoleData[]) {
    this.roles = roles
  }

  @Action
  public SetRoles(roles: IRoleData[]) {
    this.SET_ROLES(roles)
  }
}

export const RoleModule = getModule(Role)
