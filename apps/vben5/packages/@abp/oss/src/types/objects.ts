interface OssObjectDto {
  creationDate: Date;
  isFolder: boolean;
  lastModifiedDate?: Date;
  mD5?: string;
  metadata: Record<string, string>;
  name: string;
  path: string;
  size: number;
}

interface CreateOssObjectInput {
  bucket: string;
  expirationTime?: string;
  file?: File;
  fileName: string;
  overwrite: boolean;
  path?: string;
}

interface GetOssObjectInput {
  bucket: string;
  mD5: boolean;
  object: string;
  path?: string;
}

interface BulkDeleteOssObjectInput {
  bucket: string;
  object: string;
  path?: string;
}

interface OssObjectsResultDto {
  bucket: string;
  delimiter?: string;
  marker?: string;
  maxKeys: number;
  nextMarker?: string;
  objects: OssObjectDto[];
  prefix?: string;
}

export type {
  BulkDeleteOssObjectInput,
  CreateOssObjectInput,
  GetOssObjectInput,
  OssObjectDto,
  OssObjectsResultDto,
};
