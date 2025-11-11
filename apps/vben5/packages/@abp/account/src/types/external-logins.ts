interface UserLoginInfoDto {
  loginProvider: string;
  providerDisplayName: string;
  providerKey: string;
}

interface ExternalLoginInfoDto {
  displayName: string;
  name: string;
}

interface WorkWeixinLoginBindInput {
  code: string;
}

interface ExternalLoginResultDto {
  externalLogins: ExternalLoginInfoDto[];
  userLogins: UserLoginInfoDto[];
}

interface RemoveExternalLoginInput {
  loginProvider: string;
  providerKey: string;
}

export type {
  ExternalLoginInfoDto,
  ExternalLoginResultDto,
  RemoveExternalLoginInput,
  UserLoginInfoDto,
  WorkWeixinLoginBindInput,
};
