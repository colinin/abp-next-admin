import { PagedAndSortedResultRequestDto } from '@/api/types'

export const service = 'LocalizationManagement'
export const controller = 'Text'

export class Text {
  id!: string
  key!: string
  value?: string = ''
  cultureName!: string
  resourceName!: string
}

export class GetTextsInput extends PagedAndSortedResultRequestDto {
  filter = ''
  cultureName = ''
  targetCultureName = ''
  resourceName = ''
  onlyNull = false
}

export class CreateOrUpdateTextInput {
  value?: string = ''
}

export class CreateTextInput extends CreateOrUpdateTextInput {
  key = ''
  resourceName = ''
  cultureName = ''
}

export class UpdateTextInput extends CreateOrUpdateTextInput {
}
