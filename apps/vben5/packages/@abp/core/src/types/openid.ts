interface OpenIdConfiguration {
  authorization_endpoint: string;
  backchannel_logout_session_supported: boolean;
  backchannel_logout_supported: boolean;
  check_session_iframe: string;
  claims_supported: string[];
  code_challenge_methods_supported: string[];
  device_authorization_endpoint: string;
  end_session_endpoint: string;
  frontchannel_logout_session_supported: boolean;
  frontchannel_logout_supported: boolean;
  grant_types_supported: string[];
  id_token_signing_alg_values_supported: string[];
  introspection_endpoint: string;
  issuer: string;
  jwks_uri: string;
  request_parameter_supported: boolean;
  response_modes_supported: string[];
  response_types_supported: string[];
  revocation_endpoint: string;
  scopes_supported: string[];
  subject_types_supported: string[];
  token_endpoint: string;
  token_endpoint_auth_methods_supported: string[];
  userinfo_endpoint: string;
}

export type { OpenIdConfiguration };
