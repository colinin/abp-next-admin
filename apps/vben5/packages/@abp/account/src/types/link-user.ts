interface LinkUserDto {
  directlyLinked: boolean;
  linkTenantId?: string;
  linkTenantName?: string;
  linkUserId: string;
  linkUserName: string;
}

interface UnLinkUserInput {
  tenantId?: string;
  userId: string;
}

export type { LinkUserDto, UnLinkUserInput };
