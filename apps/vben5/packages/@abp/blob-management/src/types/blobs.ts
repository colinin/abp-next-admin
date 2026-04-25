import type {
  ExtensibleAuditedEntityDto,
  PagedAndSortedResultRequestDto,
} from '@abp/core';

export enum BlobType {
  File = 1,
  Folder = 0,
}

interface BlobDto extends ExtensibleAuditedEntityDto<string> {
  contentType?: string;
  downloadCount: number;
  expirationTime?: Date;
  name: string;
  path: string;
  size: number;
  type: BlobType;
}

interface BlobFolderCreateBaseDto {
  name: string;
  parentId?: string;
}

interface BlobFileCreateBaseDto {
  compareMd5?: string;
  file: Blob | File;
  name: string;
  parentId?: string;
}

interface BlobFileChunkCreateBaseDto extends BlobFileCreateBaseDto {
  chunkNumber: number;
  chunkSize: number;
  currentChunkSize: number;
  totalChunks: number;
}

interface BlobFileCreateDto extends BlobFileCreateBaseDto {
  containerId: string;
}

interface BlobFileChunkCreateDto extends BlobFileChunkCreateBaseDto {
  containerId: string;
}

interface BlobFolderCreateDto extends BlobFolderCreateBaseDto {
  containerId: string;
}

interface BlobDownloadByNameInput {
  blobName: string;
  containerName: string;
}

interface BlobGetPagedListInputBase extends PagedAndSortedResultRequestDto {
  contentType?: string;
  parentId?: string;
  type?: BlobType;
}

interface BlobGetPagedListInput extends BlobGetPagedListInputBase {
  containerId: string;
}

export type {
  BlobDownloadByNameInput,
  BlobDto,
  BlobFileChunkCreateBaseDto,
  BlobFileChunkCreateDto,
  BlobFileCreateBaseDto,
  BlobFileCreateDto,
  BlobFolderCreateBaseDto,
  BlobFolderCreateDto,
  BlobGetPagedListInput,
  BlobGetPagedListInputBase,
};
