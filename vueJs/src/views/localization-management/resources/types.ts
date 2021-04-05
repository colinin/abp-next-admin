import { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@/api/types'

export const service = 'LocalizationManagement'
export const controller = 'Resource'

export class Resource extends AuditedEntityDto {
  id!: string
  enable: boolean = true
  name!: string
  displayName!: string
  description!: string
}

export class GetResourcesInput extends PagedAndSortedResultRequestDto {
  filter = ''
}

export class CreateOrUpdateResourceInput {
  enable = true
  name = ''
  displayName = ''
  description = ''
}
