interface IdentityClaimDto {
  claimType: string;
  claimValue: string;
  id: string;
}

interface IdentityClaimDeleteDto {
  claimType: string;
  claimValue: string;
}

interface IdentityClaimUpdateDto {
  claimType: string;
  claimValue: string;
  newClaimValue: string;
}

interface IdentityClaimCreateDto {
  claimType: string;
  claimValue: string;
}

export type {
  IdentityClaimCreateDto,
  IdentityClaimDeleteDto,
  IdentityClaimDto,
  IdentityClaimUpdateDto,
};
