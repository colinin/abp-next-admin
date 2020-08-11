import Vue from 'vue'
import Vuex from 'vuex'
import { IAppState } from './modules/app'
import { IUserState } from './modules/user'
import { ITagsViewState } from './modules/tags-view'
import { IErrorLogState } from './modules/error-log'
import { IPermissionState } from './modules/permission'
import { ISettingsState } from './modules/settings'
import { IRoleState } from './modules/role'
import { IAbpState } from './modules/abp'

Vue.use(Vuex)

export interface IRootState {
  abp: IAbpState
  app: IAppState
  user: IUserState
  tagsView: ITagsViewState
  errorLog: IErrorLogState
  permission: IPermissionState
  settings: ISettingsState
  role: IRoleState
}

// Declare empty store first, dynamically register all modules later.
export default new Vuex.Store<IRootState>({})
