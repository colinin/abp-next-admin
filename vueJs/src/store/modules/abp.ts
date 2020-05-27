import store from '@/store'
import { getItemJson, setItem } from '@/utils/localStorage'
import AbpConfigurationService, { IAbpConfiguration, AbpConfiguration as AbpConfig } from '@/api/abpconfiguration'
import { VuexModule, Module, Mutation, Action, getModule } from 'vuex-module-decorators'

export interface IAbpConfigurationState {
  configuration: IAbpConfiguration
}

const abpConfigurationKey = 'vue_admin_abp_configuration'

@Module({ dynamic: true, store, name: 'abpconfiguration' })
class AbpConfiguration extends VuexModule implements IAbpConfigurationState {
  configuration = getItemJson(abpConfigurationKey) || new AbpConfig()

  @Mutation
  private SET_ABPCONFIGURATION(configuration: IAbpConfiguration) {
    this.configuration = configuration
    setItem(abpConfigurationKey, JSON.stringify(configuration))
  }

  @Action({ rawError: true })
  public async GetAbpConfiguration() {
    const config = await AbpConfigurationService.getAbpConfiguration()
    this.SET_ABPCONFIGURATION(config)
  }
}

export const AbpConfigurationModule = getModule(AbpConfiguration)
