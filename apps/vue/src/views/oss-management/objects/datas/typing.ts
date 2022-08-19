export interface Folder {
  key: string,
  title: string,
  path?: string,
  name: string,
  children?: Folder[],
}
