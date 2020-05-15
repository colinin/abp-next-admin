import request from '@/utils/request'
import { AxiosRequestConfig, AxiosPromise } from 'axios'

export default class ApiService {
  public static Get<T>(url: string, baseUrl = process.env.VUE_APP_BASE_API): Promise<T> {
    return this.HttpRequest<T>({
      baseURL: baseUrl,
      url: url,
      method: 'GET'
    })
  }

  public static Post<T>(url: string, payload: any, baseUrl = process.env.VUE_APP_BASE_API): Promise<T> {
    return this.HttpRequest<T>({
      baseURL: baseUrl,
      url: url,
      method: 'POST',
      data: payload
    })
  }

  public static Patch<T>(url: string, payload: any, baseUrl = process.env.VUE_APP_BASE_API): Promise<T> {
    return this.HttpRequest<T>({
      baseURL: baseUrl,
      url: url,
      method: 'PATCH',
      data: payload
    })
  }

  public static Put<T>(url: string, payload: any, baseUrl = process.env.VUE_APP_BASE_API): Promise<T> {
    return this.HttpRequest<T>({
      baseURL: baseUrl,
      url: url,
      method: 'PUT',
      data: payload
    })
  }

  public static Delete(url: string, baseUrl = process.env.VUE_APP_BASE_API) {
    return request({
      baseURL: baseUrl,
      url: url,
      method: 'DELETE'
    })
  }

  public static HttpRequestWithOriginResponse(options: AxiosRequestConfig): AxiosPromise<any> {
    return request(options)
  }

  public static HttpRequest<T>(options: AxiosRequestConfig): Promise<T> {
    return new Promise<T>((resolve, reject) => {
      request(options).then(res => {
        resolve(res.data)
      }).catch(error => {
        reject(error)
      })
    })
  }
}
