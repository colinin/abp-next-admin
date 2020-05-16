import axios from 'axios'
import { MessageBox, Message } from 'element-ui'
import { UserModule } from '@/store/modules/user'
import { getTenant } from '@/utils/sessions'
import { getLanguage } from '@/utils/cookies'

const service = axios.create({
  baseURL: process.env.VUE_APP_BASE_API, // url = base url + request url
  timeout: 30000
  // withCredentials: true // send cookies when cross-domain requests
})

// Request interceptors
service.interceptors.request.use(
  (config) => {
    // Add X-Access-Token header to every request, you can add other custom headers here
    if (UserModule.token) {
      config.headers.Authorization = UserModule.token
    }
    const tenantId = getTenant()
    if (tenantId) {
      config.headers.__tenant = tenantId
    }
    // abp官方类库用的 zh-Hans 的简体中文包 这里直接粗暴一点
    if (getLanguage()?.indexOf('zh') !== -1) {
      config.headers['Accept-Language'] = 'zh-Hans'
    }
    return config
  },
  (error) => {
    Promise.reject(error)
  }
)

// Response interceptors
service.interceptors.response.use(
  (response) => {
    return response
  },
  (error) => {
    console.log(error.response)
    if (error.response.status === 401) {
      MessageBox.confirm(
        '身份令牌已过期,请重新登录!',
        '确定登出',
        {
          confirmButtonText: '重新登录',
          cancelButtonText: '取消',
          type: 'error'
        }).then(() => {
        UserModule.ResetToken()
        location.reload() // To prevent bugs from vue-router
        return Promise.reject(error)
      })
    }
    if (error.response.status === 429) {
      Message({
        message: '您的操作过快,请稍后再试!',
        type: 'warning',
        duration: 5 * 1000
      })
      return Promise.reject(error)
    }
    if (error.response.status === 400 && error.response.data.error_description) {
      Message({
        message: error.response.data.error_description,
        type: 'error',
        duration: 5 * 1000
      })
      return Promise.reject(error)
    }
    let message = error.message
    if (error.response.headers._abperrorformat) {
      message = error.response.data.error.message + error.response.data.error.details
    } else if (error.response.headers._abperrorformat) {
      message = error.response.data.error_description
    }

    Message({
      message: message,
      type: 'error',
      duration: 5 * 1000
    })
    return Promise.reject(error)
  }
)

export default service
