import ApiService from './serviceBase'

const openIdConfigurationUrl = '/.well-known/openid-configuration'

export default class IdentityServer4Service {
  public static getOpenIdConfiguration() {
    return ApiService.Get<OpenIdConfiguration>(openIdConfigurationUrl)
  }
}

export class OpenIdConfiguration {
  issuer!: string
  jwks_uri!: string
  authorization_endpoint!: string
  token_endpoint!: string
  userinfo_endpoint!: string
  end_session_endpoint!: string
  check_session_iframe!: string
  revocation_endpoint!: string
  introspection_endpoint!: string
  device_authorization_endpoint!: string
  frontchannel_logout_supported!: boolean
  frontchannel_logout_session_supported!: boolean
  backchannel_logout_supported!: boolean
  backchannel_logout_session_supported!: boolean
  scopes_supported = new Array<string>()
  claims_supported = new Array<string>()
  grant_types_supported = new Array<string>()
  response_types_supported = new Array<string>()
  response_modes_supported = new Array<string>()
  token_endpoint_auth_methods_supported = new Array<string>()
  id_token_signing_alg_values_supported = new Array<string>()
  subject_types_supported = new Array<string>()
  code_challenge_methods_supported = new Array<string>()
  request_parameter_supported!: boolean
}

export class Secret {
  type!: string
  value!: string
  description?: string
  expiration?: Date
}

export class Scope {
  scope!: string
}

export class Property {
  key!: string
  value!: string
}

export class UserClaim {
  type!: string

  constructor(
    type: string
  ) {
    this.type = type
  }
}
