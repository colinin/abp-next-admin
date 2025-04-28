import type { PagedAndSortedResultRequestDto } from '@abp/core';

interface OssContainerDto {
  creationDate: Date;
  lastModifiedDate?: Date;
  metadata: Record<string, string>;
  name: string;
  size: number;
}

interface OssContainersResultDto {
  containers: OssContainerDto[];
  marker?: string;
  maxKeys?: number;
  nextMarker?: string;
  prefix?: string;
}

interface GetOssContainersInput extends PagedAndSortedResultRequestDto {
  marker?: string;
  prefix?: string;
}

interface GetOssObjectsInput extends PagedAndSortedResultRequestDto {
  bucket?: string;
  delimiter?: string;
  encodingType?: string;
  marker?: string;
  mD5?: string;
  prefix?: string;
}

export type {
  GetOssContainersInput,
  GetOssObjectsInput,
  OssContainerDto,
  OssContainersResultDto,
};
