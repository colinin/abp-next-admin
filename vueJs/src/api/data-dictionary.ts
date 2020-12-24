import ApiService from './serviceBase'
import { urlStringify } from '@/utils/index'
import { ListResultDto, PagedResultDto, PagedAndSortedResultRequestDto } from './types'

const sourceUrl = '/api/platform/datas'
/** 远程服务地址 */
const serviceUrl = process.env.VUE_APP_BASE_API

/** 数据字典api接口 */
export default class DataDictionaryService {
  public static get(id: string) {
    const _url = sourceUrl + '/' + id
    return ApiService.Get<Data>(_url, serviceUrl)
  }

  public static getList(payload: GetDataByPaged) {
    const _url = sourceUrl + '?' + urlStringify(payload)
    return ApiService.Get<PagedResultDto<Data >>(_url, serviceUrl)
  }

  public static getAll() {
    const _url = sourceUrl + '/all'
    return ApiService.Get<ListResultDto<Data >>(_url, serviceUrl)
  }

  public static create(payload: DataCreate) {
    return ApiService.Post<Data>(sourceUrl, payload, serviceUrl)
  }

  public static update(id: string, payload: DataUpdate) {
    const _url = sourceUrl + '/' + id
    return ApiService.Put<Data>(_url, payload, serviceUrl)
  }

  public static delete(id: string) {
    const _url = sourceUrl + '/' + id
    return ApiService.Delete(_url, serviceUrl)
  }

  public static appendItem(id: string, payload: DataItemCreate) {
    const _url = sourceUrl + '/' + id + '/items'
    return ApiService.Post<Data>(_url, payload, serviceUrl)
  }

  public static updateItem(id: string, name: string, payload: DataItemUpdate) {
    const _url = sourceUrl + '/' + id + '/items/' + name
    return ApiService.Put<Data>(_url, payload, serviceUrl)
  }

  public static removeItem(id: string, name: string) {
    const _url = sourceUrl + '/' + id + '/items/' + name
    return ApiService.Delete(_url, serviceUrl)
  }
}

export enum ValueType {
  String = 0,
  Numeic = 1,
  Boolean = 2,
  Date = 3,
  DateTime = 4,
  Array = 5,
  Object = 6
}

export class DataItem {
  id!: string
  name!: string
  defaultValue!: string
  displayName!: string
  description?: string
  allowBeNull!: boolean
  valueType!: ValueType
}

export class Data {
  id!: string
  name!: string
  code!: string
  displayName!: string
  description?: string
  parentId?: string
  items = new Array<DataItem>()
}

export class GetDataByPaged extends PagedAndSortedResultRequestDto {
  filter = ''
}

export class DataCreateOrUpdate {
  name = ''
  displayName = ''
  description? = ''
}

export class DataCreate extends DataCreateOrUpdate {
  parentId?: string
}

export class DataUpdate extends DataCreateOrUpdate {
}

export class DataItemCreateOrUpdate {
  defaultValue = ''
  displayName = ''
  description? = ''
  allowBeNull = true
  valueType = ValueType.String
}

export class DataItemUpdate extends DataItemCreateOrUpdate {
}

export class DataItemCreate extends DataItemCreateOrUpdate {
  name = ''
}
