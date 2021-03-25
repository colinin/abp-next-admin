import { VuexModule, Module, Mutation, Action, getModule } from 'vuex-module-decorators'
import store from '@/store'
import { getItem, setItem } from '@/utils/localStorage'
import DynamicApiService, { ApplicationApiDescriptionModel } from '@/api/dynamic-api'

export interface IHttpProxy {
  applicationApiDescriptionModel: ApplicationApiDescriptionModel
}

const dynamicApiKey = 'abp-api-definition'

@Module({ dynamic: true, store, name: 'http-proxy' })
class HttpProxy extends VuexModule implements IHttpProxy {
  public applicationApiDescriptionModel = new ApplicationApiDescriptionModel()

  @Mutation
  private SET_API_DESCRIPTOR(apiDescriptor: ApplicationApiDescriptionModel) {
    this.applicationApiDescriptionModel = apiDescriptor
  }

  @Action({ rawError: true })
  public async Initialize() {
    const dynamicApiJson = getItem(dynamicApiKey)
    if (dynamicApiJson) {
      const apiDescriptor = JSON.parse(dynamicApiJson) as ApplicationApiDescriptionModel
      if (apiDescriptor) {
        this.SET_API_DESCRIPTOR(apiDescriptor)
        return
      }
    }
    const dynamicApi = await DynamicApiService.get()
    this.SET_API_DESCRIPTOR(dynamicApi)
    setItem(dynamicApiKey, JSON.stringify(dynamicApi))
  }
}

export const HttpProxyModule = getModule(HttpProxy)
