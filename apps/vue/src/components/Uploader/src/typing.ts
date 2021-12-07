export interface FileSystem {
  name: string;
  averageSpeed: number;
  currentSpeed: number;
  paused: boolean;
  error: boolean;
  isFolder: boolean;
}

export interface Chunk {
  chunkNumber: number;
  totalChunks: number;
  chunkSize: number;
  currentChunkSize: number;
  totalSize: number;
  identifier: string;
  filename: string;
  relativePath: string;
}

export interface File extends FileSystem {
  file: File;
  relativePath: string;
  size: number;
  uniqueIdentifier: string;
  chunks: Chunk[];
}
