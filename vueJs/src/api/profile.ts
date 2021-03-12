import ApiService from './serviceBase'

const IdentityServiceUrl = process.env.VUE_APP_BASE_API

export default class MyProfileService {
  public static getMyProfile() {
    const _url = '/api/identity/my-profile'
    return ApiService.Get<MyProfile>(_url, IdentityServiceUrl)
  }

  public static updateMyProfile(payload: UpdateMyProfile) {
    const _url = '/api/identity/my-profile'
    return ApiService.Put<MyProfile>(_url, payload, IdentityServiceUrl)
  }

  public static changePassword(payload: ChangePassword) {
    const _url = '/api/identity/my-profile/change-password'
    return ApiService.Post<void>(_url, payload, IdentityServiceUrl)
  }
}

export class MyProfileBase {
  userName?: string
  email?: string
  name?: string
  surname?: string
  phoneNumber?: string

  constructor(
    name = '',
    email = '',
    userName = '',
    surname = '',
    phoneNumber = ''
  ) {
    this.name = name
    this.email = email
    this.userName = userName
    this.surname = surname
    this.phoneNumber = phoneNumber
  }
}

export class MyProfile extends MyProfileBase {
  isExternal!: boolean
  hasPassword!: boolean
  extraProperties?: {[key: string]: any }
}

export class UpdateMyProfile extends MyProfileBase {
  extraProperties?: {[key: string]: any }
}

export class ChangePassword {
  currentPassword!: string
  newPassword!: string
}
