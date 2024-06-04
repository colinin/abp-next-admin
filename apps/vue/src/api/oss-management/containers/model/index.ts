export interface OssContainer {
    name: string;
    size?: number;
    creationDate: Date;
    lastModifiedDate?: Date;
    metadata: { [key: string]: string };
}
  
export interface OssContainersResult {
    prefix: string;
    marker: string;
    nextMarker: string;
    maxKeys: number;
    containers: OssContainer[];
}

export interface GetOssContainerPagedRequest extends PagedAndSortedResultRequestDto {
    prefix?: string;
    marker?: string;
}