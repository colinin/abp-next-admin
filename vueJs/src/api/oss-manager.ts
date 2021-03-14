import ApiService from './serviceBase'
import { PagedAndSortedResultRequestDto } from './types'
import { urlStringify } from '@/utils/index'

const serviceUrl = process.env.VUE_APP_BASE_API
const containerUrl = '/api/oss-management/containes'
const objectUrl = '/api/oss-management/objects'
export const objectUploadUrl = serviceUrl + objectUrl + '/upload'
export const staticUrl = '/api/files/static/'

export default class OssManager {
  public static createBucket(name: string) {
    const _url = containerUrl + '/' + name
    return ApiService.Post<OssContainer>(_url, null, serviceUrl)
  }

  public static getBucket(name: string) {
    const _url = containerUrl + '/'  + name
    return ApiService.Get<OssContainer>(_url, serviceUrl)
  }

  public static getBuckets(input: GetOssContainerRequest) {
    const _url = containerUrl + '?' + urlStringify(input)
    return ApiService.Get<OssContainerResultList>(_url, serviceUrl)
  }

  public static deleteBucket(name: string) {
    const _url = containerUrl + '/'  + name
    return ApiService.Delete(_url, serviceUrl)
  }

  public static createFolder(bucket: string, name: string, path: string = '') {
    const input = {
      bucket: bucket,
      object: name,
      path: path
    }
    return ApiService.Post<OssObject>(objectUrl, input, serviceUrl)
  }

  public static getObjects(input: GetOssObjectRequest) {
    const _url = containerUrl + '/objects?' + urlStringify(input)
    return ApiService.Get<OssObjectResultList>(_url, serviceUrl)
  }

  public static getObject(bucket: string, object: string, path: string = '') {
    let _url = objectUrl + '?bucket=' + bucket + '&object=' + object
    if (path) {
      _url += '&path=' + path
    }
    return ApiService.Get<OssObject>(_url, serviceUrl)
  }

  public static getObjectData(bucket: string, object: string, path: string = '') {
    return ApiService.HttpRequest<Blob>({
      url: this.generateOssUrl(bucket, object, path),
      baseURL: serviceUrl,
      method: 'GET',
      responseType: 'blob'
    })
  }

  public static deleteObject(bucket: string, object: string, path: string = '') {
    let _url = objectUrl + '?bucket=' + bucket + '&object=' + object
    if (path) {
      _url += '&path=' + path
    }
    return ApiService.Delete(_url, serviceUrl)
  }

  public static bulkDeleteObject(bucket: string, objects: string[], path: string = '') {
    const _url = objectUrl + '/bulk-delete'
    return ApiService
      .HttpRequest({
        url: _url,
        baseURL: serviceUrl,
        method: 'DELETE',
        data: {
          bucket: bucket,
          path: path,
          objects: objects
        }
      })
  }

  public static generateOssUrl(bucket: string, object: string, path: string = '', prefix: string = '') {
    let _url = staticUrl + bucket + '/'
    _url += bucket.endsWith('/') ? '' : '/'
    if (path) {
      // 某些情况下要对 / 编码
      _url += 'p/' + path.replace('/', '%2F')
      if (_url.endsWith('%2F')) {
        _url = _url.substring(0, _url.length - 3) + '/'
      }
    }
    _url += object
    if (prefix) {
      _url = prefix + _url
    }
    return _url
  }
}

export class GetOssContainerRequest extends PagedAndSortedResultRequestDto {
  prefix?: string
  marker?: string
}

export class GetOssObjectRequest extends PagedAndSortedResultRequestDto {
  bucket!: string
  prefix?: string
  delimiter?: string = '/'
  marker?: string
  encodingType?: string
  maxResultCount = 100
}

export class OssContainer {
  name!: string
  size!: number
  creationDate?: Date
  lastModifiedDate?: Date
  metadata?: {[key: string]: string}
}

export class OssContainerResultList {
  prefix?: string
  marker?: string
  nextMarker?: string
  maxKeys?: number
  containers!: OssContainer[]
}

export class OssObject {
  name!: string
  path?: string
  size!: number
  isFolder!: boolean
  creationDate?: Date
  lastModifiedDate?: Date
  metadata?: {[key: string]: string}
}

export class OssObjectResultList {
  bucket!: string
  prefix?: string
  delimiter?: string
  marker?: string
  nextMarker?: string
  maxKeys?: number
  objects!: OssObject[]
}
