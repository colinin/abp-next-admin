export interface Folder {
  key: string,
  title: string,
  path?: string,
  name: string,
  isLeaf?: boolean,
  children?: Folder[],
}
