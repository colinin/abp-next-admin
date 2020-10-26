import ApiService from './serviceBase'
import { ListResultDto } from './types'
import { User } from './users'

const IdentityServiceUrl = process.env.VUE_APP_BASE_API

export default class UserLookupApiService {
  public static searchUsers(filter = '', sorting = 'UserName', skipCount = 0, maxResultCount = 10) {
    let _url = '/api/identity/users/lookup/search'
    _url += '?filter=' + filter
    _url += '&sorting=' + sorting
    _url += '&skipCount=' + skipCount
    _url += '&maxResultCount=' + maxResultCount
    return ApiService.Get<ListResultDto<User>>(_url, IdentityServiceUrl)
  }
}
