export interface FileShareInput {
    name: string;
    path?: string;
    roles?: string[];
    users?: string[];
    expirationTime?: Date;
    maxAccessCount: number;
}

export interface FileShare {
    url: string;
    expirationTime?: Date;
    maxAccessCount: number;
}

export interface MyFileShare {
    name: string;
    path?: string;
    roles?: string[];
    users?: string[];
    md5: string;
    url: string;
    accessCount: number;
    expirationTime?: Date;
    maxAccessCount: number;
}