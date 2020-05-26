import store from '@/store'
import { getAbpConfig, setAbpConfig } from '@/utils/localStorage'
import AbpConfigurationService, { IAbpConfiguration } from '@/api/abpconfiguration'
import { VuexModule, Module, Mutation, Action, getModule } from 'vuex-module-decorators'

export interface IAbpConfigurationState {
  configuration: IAbpConfiguration
}

@Module({ dynamic: true, store, name: 'abpconfiguration' })
class AbpConfiguration extends VuexModule implements IAbpConfigurationState {
  configuration = getAbpConfig()

  @Mutation
  private SET_ABPCONFIGURATION(configuration: IAbpConfiguration) {
    this.configuration = configuration
    setAbpConfig(configuration)
  }

  @Action({ rawError: true })
  public async GetAbpConfiguration() {
    const config = await AbpConfigurationService.getAbpConfiguration()
    this.SET_ABPCONFIGURATION(config)
  }
}

export const AbpConfigurationModule = getModule(AbpConfiguration)