import ApiService from './serviceBase'
import { PagedAndSortedResultRequestDto, PagedResultDto } from './types'

const serviceUrl = process.env.VUE_APP_BASE_API
const baseUrl = '/api/file-management/file-system'
export const FileUploadUrl = serviceUrl + baseUrl + '/files'

export default class FileManagementService {
  public static getFileSystem(name: string, path: string | undefined) {
    let _url = baseUrl + '?name=' + name
    if (path) {
      _url += '&path=' + path
    }
    return ApiService.Get<FileSystem>(_url, serviceUrl)
  }

  public static getFileSystemList(payload: FileSystemGetByPaged) {
    let _url = baseUrl + '?skipCount=' + payload.skipCount
    _url += '&maxResultCount=' + payload.maxResultCount
    _url += '&sorting=' + payload.sorting
    if (payload.filter) {
      _url += '&filter=' + payload.filter
    }
    if (payload.parent) {
      _url += '&parent=' + payload.parent
    }
    return ApiService.Get<PagedResultDto<FileSystem>>(_url, serviceUrl)
  }

  public static editFileSystem(name: string, newName: string) {
    const _payload = { newName }
    return ApiService.Put<FileSystem>(baseUrl, _payload, serviceUrl)
  }

  public static createFolder(path: string, parent: string | undefined) {
    const _url = baseUrl + '/folders'
    const _payload = {
      path,
      parent
    }
    return ApiService.Post<void>(_url, _payload, serviceUrl)
  }

  public static deleteFolder(path: string) {
    const _url = baseUrl + '/folders?path=' + path
    return ApiService.Delete(_url, serviceUrl)
  }

  public static moveFolder(path: string, toPath: string) {
    const _url = baseUrl + '/folders/move?path=' + path
    const _payload = { toPath }
    return ApiService.Put<void>(_url, _payload, serviceUrl)
  }

  public static copyFolder(path: string, toPath: string) {
    const _url = baseUrl + '/folders/copy?path=' + path
    const _payload = { toPath }
    return ApiService.Put<void>(_url, _payload, serviceUrl)
  }

  public static deleteFile(path: string, name: string) {
    let _url = baseUrl + '/files?path=' + path
    _url += '&name=' + name
    return ApiService.Delete(_url, serviceUrl)
  }

  public static moveFile(payload: FileCopyOrMove) {
    const _url = baseUrl + '/files/move'
    return ApiService.Put<void>(_url, payload, serviceUrl)
  }

  public static copyFile(payload: FileCopyOrMove) {
    const _url = baseUrl + '/files/copy'
    return ApiService.Put<void>(_url, payload, serviceUrl)
  }

  public static downlodFle(name: string, path: string | undefined, currentByte: number | undefined) {
    let _url = baseUrl + '/files?name=' + name
    if (path) {
      _url += '&path=' + path
    }
    _url += '&currentByte=' + currentByte
    return ApiService.HttpRequestWithOriginResponse(({
      url: _url,
      method: 'GET',
      responseType: 'blob'
    }))
  }
}

export enum FileSystemType {
  Folder = 0,
  File = 1
}

export class FileSystem {
  type!: FileSystemType
  name!: string
  parent?: string
  size?: number
  extension?: string
  creationTime!: Date
  lastModificationTime?: Date
}

export class FileSystemGetByPaged extends PagedAndSortedResultRequestDto {
  parent?: string
  filter?: string
}

export class FileCopyOrMove {
  path!: string
  name!: string
  toPath!: string
  toName?: string
}
