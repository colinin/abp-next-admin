declare interface ExtensibleObject {
  extraProperties: ExtraPropertyDictionary;
}

declare interface EntityDto<TPrimaryKey> {
  id: TPrimaryKey;
}

declare interface CreationAuditedEntityDto<TPrimaryKey> extends EntityDto<TPrimaryKey> {
  creationTime: Date;
  creatorId?: string;
}

declare interface CreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto>
  extends CreationAuditedEntityDto<TPrimaryKey> {
  creator: TUserDto;
}

declare interface AuditedEntityDto<TPrimaryKey> extends CreationAuditedEntityDto<TPrimaryKey> {
  lastModificationTime?: Date;
  lastModifierId?: string;
}

declare interface AuditedEntityWithUserDto<TPrimaryKey, TUserDto>
  extends AuditedEntityDto<TPrimaryKey> {
  creator: TUserDto;
  lastModifier: TUserDto;
}

declare interface ExtensibleEntityDto<TKey> extends ExtensibleObject {
  id: TKey;
}

declare interface ExtensibleCreationAuditedEntityDto<TPrimaryKey>
  extends ExtensibleEntityDto<TPrimaryKey> {
  creationTime: Date;
  creatorId?: string;
}

declare interface ExtensibleCreationAuditedEntityWithUserDto<TPrimaryKey, TUserDto>
  extends ExtensibleCreationAuditedEntityDto<TPrimaryKey> {
  creator: TUserDto;
}

declare interface ExtensibleAuditedEntityDto<TPrimaryKey>
  extends ExtensibleCreationAuditedEntityDto<TPrimaryKey> {
  lastModificationTime?: Date;
  lastModifierId?: string;
}

declare interface ExtensibleAuditedEntityWithUserDto<TPrimaryKey, TUserDto>
  extends ExtensibleAuditedEntityDto<TPrimaryKey> {
  creator: TUserDto;
  lastModifier: TUserDto;
}

declare interface ExtensibleFullAuditedEntityDto<TPrimaryKey>
  extends ExtensibleAuditedEntityDto<TPrimaryKey> {
  isDeleted: boolean;
  deleterId?: string;
  deletionTime?: Date;
}

declare interface ExtensibleFullAuditedEntityWithUserDto<TPrimaryKey, TUserDto>
  extends ExtensibleFullAuditedEntityDto<TPrimaryKey> {
  creator: TUserDto;
  lastModifier: TUserDto;
  deleter: TUserDto;
}

declare interface FullAuditedEntityDto<TPrimaryKey> extends AuditedEntityDto<TPrimaryKey> {
  isDeleted: boolean;
  deleterId?: string;
  deletionTime?: Date;
}

declare interface FullAuditedEntityWithUserDto<TUserDto> extends FullAuditedEntityDto<TPrimaryKey> {
  creator: TUserDto;
  lastModifier: TUserDto;
  deleter: TUserDto;
}

declare interface LimitedResultRequestDto {
  maxResultCount?: number;
}

declare interface ExtensibleLimitedResultRequestDto
  extends LimitedResultRequestDto,
    ExtensibleObject {}

declare interface ListResultDto<T> {
  items: T[];
}

declare interface ExtensibleListResultDto<T> extends ListResultDto<T>, ExtensibleObject {}

declare interface PagedResultDto<T> extends ListResultDto<T> {
  totalCount: number;
}

declare interface ExtensiblePagedResultDto<T> extends PagedResultDto<T>, ExtensibleObject {}

declare interface PagedAndSortedResultRequestDto extends PagedResultRequestDto {
  sorting?: string;
}

declare interface ExtensiblePagedAndSortedResultRequestDto
  extends PagedAndSortedResultRequestDto,
    ExtensibleObject {}

declare interface PagedResultRequestDto extends LimitedResultRequestDto {
  skipCount?: number;
}

declare interface ExtensiblePagedResultRequestDto extends PagedResultRequestDto, ExtensibleObject {}
