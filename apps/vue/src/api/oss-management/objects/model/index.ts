export enum FileType {
    Folder = 0,
    File = 1,
  }

export interface OssObject {
    isFolder: boolean;
    name: string;
    path: string;
    size?: number;
    extension: string;
    creationTime: Date;
    lastModifiedDate?: Date;
    metadata: { [key: string]: string };
  }
  
  export interface OssObjectCreate {
    bucket: string;
    path: string;
    object: string;
    overwrite: boolean;
    expirationTime?: number;
  }
  
  export interface OssObjectsResult {
    bucket: string;
    prefix: string;
    delimiter: string;
    marker: string;
    nextMarker: string;
    maxKeys: number;
    objects: OssObject[];
  }
  
  export interface OssCopyOrMove {
    path: string;
    name: string;
    toPath: string;
    toName?: string;
  }
  
  export interface OssObjectBulkDelete {
    bucket: string;
    path?: string;
    objects: string[];
  }

  export interface GetOssObjectPagedRequest extends PagedAndSortedResultRequestDto {
    bucket?: string;
    prefix?: string;
    delimiter?: string;
    marker?: string;
    encodingType?: string;
  }

  export interface GetOssObjectRequest {
    bucket: string;
    path: string;
    object: string;
  }
