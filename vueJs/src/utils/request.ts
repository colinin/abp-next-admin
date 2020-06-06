import axios from 'axios'
import i18n from 'vue-i18n'
import { MessageBox, Notification } from 'element-ui'
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
    if (config.url === '/connect/token') {
      return config
    }
    // Add X-Access-Token header to every request, you can add other custom headers here
    if (UserModule.token) {
      config.headers.Authorization = UserModule.token
    }
    const tenantId = getTenant()
    if (tenantId) {
      config.headers.__tenant = tenantId
    }
    // abp官方类库用的 zh-Hans 的简体中文包 这里直接粗暴一点
    const language = getLanguage()
    if (language?.indexOf('zh') !== -1) {
      config.headers['Accept-Language'] = 'zh-Hans'
    } else {
      config.headers['Accept-Language'] = language
    }
    return config
  },
  (error) => {
    Promise.reject(error)
  }
)

function l(name: string) {
  return i18n.prototype.t(name).toString()
}

function showError(error: any, status: number) {
  let message = ''
  let title = ''
  if (typeof error === 'string') {
    message = error
  } else if (error.error.details) {
    message = error.error.details
    title = error.error.message
  } else if (error.error.message) {
    message = error.error.message
  } else {
    switch (status) {
      case 400:
        title = error.error
        message = error.error_description
        break
      case 401:
        title = l('AbpAccount.DefaultErrorMessage401')
        message = l('AbpAccount:DefaultErrorMessage401Detail')
        break
      case 403:
        title = l('AbpAccount:DefaultErrorMessage403')
        message = l('AbpAccount.DefaultErrorMessage403Detail')
        break
      case 404:
        title = l('AbpAccount.DefaultErrorMessage404')
        message = l('AbpAccount.DefaultErrorMessage404Detail')
        break
      case 429:
        message = l('global.operatingFast')
        break
      case 500:
        title = l('AbpAccount.500Message')
        message = l('AbpAccount.InternalServerErrorMessage')
        break
      default:
        break
    }
  }
  Notification({
    title: title,
    message: message,
    type: 'error',
    duration: 5 * 1000
  })
}

// Response interceptors
service.interceptors.response.use(
  (response) => {
    return response
  },
  (error) => {
    showError(error.response.data, error.response.status)
    MessageBox.confirm(
      l('login.tokenExprition'),
      l('login.confirmLogout'),
      {
        confirmButtonText: l('login.relogin'),
        cancelButtonText: l('global.cancel'),
        type: 'error'
      }).then(() => {
      UserModule.ResetToken()
      location.reload() // To prevent bugs from vue-router
      return Promise.reject(error)
    })
  }
)

export default service
