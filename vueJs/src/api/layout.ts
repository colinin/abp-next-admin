import ApiService from './serviceBase'
import { urlStringify } from '@/utils/index'
import { ListResultDto, PagedResultDto, PagedAndSortedResultRequestDto } from './types'

const sourceUrl = '/api/platform/layouts'
/** 远程服务地址 */
const serviceUrl = process.env.VUE_APP_BASE_API

/** 路由相关pi接口 */
export default class LayoutService {
  public static get(id: string) {
    const _url = sourceUrl + '/' + id
    return ApiService.Get<Layout>(_url, serviceUrl)
  }

  public static getAllList() {
    const _url = sourceUrl + '/all'
    return ApiService.Get<ListResultDto<Layout >>(_url, serviceUrl)
  }

  public static getList(payload: GetLayoutByPaged) {
    const _url = sourceUrl + '?' + urlStringify(payload)
    return ApiService.Get<PagedResultDto<Layout >>(_url, serviceUrl)
  }

  public static create(payload: LayoutCreate) {
    return ApiService.Post<Layout>(sourceUrl, payload, serviceUrl)
  }

  public static update(id: string, payload: LayoutUpdate) {
    const _url = sourceUrl + '/' + id
    return ApiService.Put<Layout>(_url, payload, serviceUrl)
  }

  public static delete(id: string) {
    const _url = sourceUrl + '/' + id
    return ApiService.Delete(_url, serviceUrl)
  }
}

export enum PlatformType {
  None = 0,
  WinCe = 2,
  WinForm = 4,
  Desktop = WinCe | WinForm,
  WebForm = 8,
  WebMvc = 16,
  WebMvvm = 32,
  Web = WebForm | WebMvc | WebMvvm,
  Android = 64,
  iOS = 128,
  Mobile = Android | iOS,
  MiniProgram = 256,
  All = Desktop | Web | Mobile | MiniProgram
}

export const PlatformTypes = [
  { key: 'None', value: PlatformType.None },
  { key: 'WinCe', value: PlatformType.WinCe },
  { key: 'WinForm', value: PlatformType.WinForm },
  { key: 'Desktop', value: PlatformType.Desktop },
  { key: 'WebForm', value: PlatformType.WebForm },
  { key: 'WebMvc', value: PlatformType.WebMvc },
  { key: 'WebMvvm', value: PlatformType.WebMvvm },
  { key: 'Web', value: PlatformType.Web },
  { key: 'Android', value: PlatformType.Android },
  { key: 'iOS', value: PlatformType.iOS },
  { key: 'Mobile', value: PlatformType.Mobile },
  { key: 'MiniProgram', value: PlatformType.MiniProgram },
  { key: 'All', value: PlatformType.All }
]

export class Route {
  id!: string
  name!: string
  path!: string
  displayName!: string
  description?: string
  redirect?: string
  meta: {[key: string]: any} = {}
}

export class Layout extends Route {
  platformType!: PlatformType
  dataId!: string
}

export class LayoutCreateOrUpdate {
  name!: string
  path!: string
  displayName!: string
  description?: string
  redirect?: string
  platformType!: PlatformType
}

export class LayoutCreate extends LayoutCreateOrUpdate {
  dataId!: string
}

export class LayoutUpdate extends LayoutCreateOrUpdate {

}

export class GetLayoutByPaged extends PagedAndSortedResultRequestDto {
  filter = ''
  reverse = false
  platformType?: PlatformType | null
}
