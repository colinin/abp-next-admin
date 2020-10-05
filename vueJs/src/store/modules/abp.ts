import store from '@/store'
import i18n from '@/lang/index'
import { getOrDefault, setItem } from '@/utils/localStorage'
import AbpConfigurationService, { IAbpConfiguration, AbpConfiguration as AbpConfig } from '@/api/abpconfiguration'
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
    const { cultureName } = configuration.localization.currentCulture
    const localeMessage = i18n.getLocaleMessage(cultureName)
    // const resources: { [key: string]: any} = {}
    Object.keys(configuration.localization.values).forEach(key => {
      const resource = configuration.localization.values[key]
      if (typeof resource !== 'object') return
      Object.keys(resource).forEach(key2 => {
        if (/'{|{/g.test(resource[key2])) {
          resource[key2] = resource[key2].replace(/'{|{/g, '{').replace(/}'|}/g, '}')
        }
      })
      localeMessage[key] = resource
      // resources[key] = resource
    })
    i18n.setLocaleMessage(cultureName, localeMessage)
    // i18n.mergeLocaleMessage(cultureName, resources)
  }

  @Action({ rawError: true })
  public LoadAbpConfiguration() {
    return new Promise<AbpConfig>((resolve, reject) => {
      AbpConfigurationService.getAbpConfiguration().then(config => {
        this.SET_ABPCONFIGURATION(config)
        this.SET_ABPLOCALIZER(config)
        return resolve(config)
      }).catch(error => {
        return reject(error)
      })
    })
  }
}

export const AbpModule = getModule(AbpConfiguration)
