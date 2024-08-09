export interface PackageCreateDto extends PackageCreateOrUpdateDto {
  name: string;
  version: string;
}

export interface PackageBlobUploadDto {
  name: string;
  size?: number;
  summary?: string;
  contentType?: string;
  createdAt?: string;
  updatedAt?: string;
  license?: string;
  authors?: string;
  file: Blob;
}

export interface PackageBlobRemoveDto {
  name: string;
}

export interface PackageBlobDownloadInput {
  name: string;
}

export interface PackageGetLatestInput {
  name: string;
  version: string;
}

export interface PackageGetPagedListInput extends PagedAndSortedResultRequestDto {
  filter?: string;
  name?: string;
  note?: string;
  version?: string;
  description?: string;
  forceUpdate?: boolean;
  authors?: string;
}

export interface PackageUpdateDto extends PackageCreateOrUpdateDto {
  concurrencyStamp?: string;
}

export interface PackageCreateOrUpdateDto {
  note: string;
  description?: string;
  forceUpdate?: boolean;
  authors?: string;
  level?: PackageLevel;
}

export enum PackageLevel {
  Resource = 0,
  Full = 1,
  None = -1,
}

export interface PackageBlobDto extends CreationAuditedEntityDto<number> {
  name?: string;
  url?: string;
  size?: number;
  summary?: string;
  createdAt?: string;
  updatedAt?: string;
  license?: string;
  authors?: string;
  sHA256?: string;
  contentType?: string;
  downloadCount?: number;
  extraProperties?: Dictionary<string, any>;
}

export interface PackageDto extends ExtensibleAuditedEntityDto<string> {
  concurrencyStamp?: string;
  name?: string;
  note?: string;
  version?: string;
  description?: string;
  forceUpdate?: boolean;
  authors?: string;
  level?: PackageLevel;
  blobs?: PackageBlobDto[];
}

