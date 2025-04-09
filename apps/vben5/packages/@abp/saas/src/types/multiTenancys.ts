interface FindTenantResultDto {
  isActive: boolean;
  name?: string;
  normalizedName?: string;
  success: boolean;
  tenantId?: string;
}

export type { FindTenantResultDto };
