export class Route {
  id!: string;
  name!: string;
  path!: string;
  displayName!: string;
  description?: string;
  redirect?: string;
  meta: { [key: string]: any } = {};
}
