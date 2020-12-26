import { ExtensibleEntity, PagedAndSortedResultRequestDto } from './types'

export class PersistedGrant extends ExtensibleEntity<string> {
  key!: string
  type!: string
  subjectId?: string
  sessionId!: string
  description?: string
  consumedTime?: Date
  clientId!: string
  creationTime!: Date
  expiration?: Date
  data!: string
}

export class GetGrantByPaged extends PagedAndSortedResultRequestDto {
  filter = ''
  subjectId = ''
}

