import type {
  Dictionary,
  ExtensibleAuditedEntityDto,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

interface OpenIddictAuthorizationDto extends ExtensibleAuditedEntityDto<string> {
  applicationId?: string;
  creationDate?: string;
  /** 是否内置（静态） */
  isStatic?: boolean;
  properties?: Dictionary<string, string>;
  /** 是否必需 */
  required?: boolean;
  scopes?: string[];
  status?: string;
  subject?: string;
  type?: string;
}

interface OpenIddictAuthorizationGetListInput extends PagedAndSortedResultRequestDto {
  beginCreationTime?: string;
  clientId?: string;
  endCreationTime?: string;
  filter?: string;
  status?: string;
  subject?: string;
  type?: string;
}

export type { OpenIddictAuthorizationDto, OpenIddictAuthorizationGetListInput };
