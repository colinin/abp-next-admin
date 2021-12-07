export interface UserClaim {
  type: string;
}

export interface Property {
  key: string;
  value: string;
}

export interface OpenIdConfiguration {
  issuer: string;
  jwks_uri: string;
  authorization_endpoint: string;
  token_endpoint: string;
  userinfo_endpoint: string;
  end_session_endpoint: string;
  check_session_iframe: string;
  revocation_endpoint: string;
  introspection_endpoint: string;
  device_authorization_endpoint: string;
  frontchannel_logout_supported: boolean;
  frontchannel_logout_session_supported: boolean;
  backchannel_logout_supported: boolean;
  backchannel_logout_session_supported: boolean;
  scopes_supported: string[];
  claims_supported: string[];
  grant_types_supported: string[];
  response_types_supported: string[];
  response_modes_supported: string[];
  token_endpoint_auth_methods_supported: string[];
  id_token_signing_alg_values_supported: string[];
  subject_types_supported: string[];
  code_challenge_methods_supported: string[];
  request_parameter_supported: boolean;
}
