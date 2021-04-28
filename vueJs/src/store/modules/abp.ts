import store from '@/store'
import { setLanguage } from '@/lang/index'
import { getOrDefault, setItem } from '@/utils/localStorage'
import AbpConfigurationService, { IAbpConfiguration, AbpConfiguration as AbpConfig, CurrentUser } from '@/api/abpconfiguration'
import { VuexModule, Module, Mutation, Action, getModule } from 'vuex-module-decorators'


export interface IAbpState {
  configuration: IAbpConfiguration
}

const abpConfigurationKey = 'vue_admin_abp_configuration'

@Module({ dynamic: true, store, name: 'abp' })
class AbpConfiguration extends VuexModule implements IAbpState {
  configuration = getOrDefault(abpConfigurationKey, new AbpConfig())

  @Mutation
  private SET_ABPCONFIGURATION(configuration: IAbpConfiguration) {
    this.configuration = configuration
    setItem(abpConfigurationKey, JSON.stringify(configuration))
  }

  @Mutation
  private SET_ABPLOCALIZER(configuration: IAbpConfiguration) {
    const { currentCulture, values } = configuration.localization
    setLanguage(currentCulture.cultureName, values)
  }

  @Action({ rawError: true })
  public Initialize() {
    return new Promise<AbpConfig>((resolve, reject) => {
      AbpConfigurationService
        .getAbpConfiguration()
        .then(config => {
          this.SET_ABPCONFIGURATION(config)
          this.SET_ABPLOCALIZER(config)
          this.context.dispatch('setCurrentUserInfo', config.currentUser ?? new CurrentUser())
          return resolve(config)
        })
        .catch(error => {
          return reject(error)
        })
    })
  }
}

export const AbpModule = getModule(AbpConfiguration)
