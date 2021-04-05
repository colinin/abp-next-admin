import { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@/api/types'

export const service = 'LocalizationManagement'
export const controller = 'Language'

export class Language extends AuditedEntityDto {
  id!: string
  enable: boolean = true
  cultureName!: string
  uiCultureName!: string
  displayName!: string
  flagIcon!: string
}

export class GetLanguagesInput extends PagedAndSortedResultRequestDto {
  filter = ''
}

export class CreateOrUpdateLanguageInput {
  enable = true
  cultureName = ''
  uiCultureName = ''
  displayName = ''
  flagIcon = ''
}
