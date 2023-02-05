export interface Language extends AuditedEntityDto<string> {
  enable: boolean;
  cultureName: string;
  uiCultureName: string;
  displayName: string;
  flagIcon: string;
}

export interface LanguageCreateOrUpdate {
  enable: boolean;
  displayName: string;
  flagIcon: string;
}

export interface LanguageCreate extends LanguageCreateOrUpdate {
  cultureName: string;
  uiCultureName: string;
}

export interface LanguageUpdate extends LanguageCreateOrUpdate {}

export interface LanguageListResult extends ListResultDto<Language> {}

export interface LanguagePagedResult extends PagedResultDto<Language> {}

export interface GetLanguagePagedRequest extends PagedAndSortedResultRequestDto {
  filter?: string;
}
