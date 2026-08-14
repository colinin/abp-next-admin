interface StringLocalizer {
  L(key: string, args?: any[] | Record<string, any>  ): string;
  Lr(
    resource: string,
    key: string,
    args?: any[] | Record<string, any>  ,
  ): string;
}

export type { StringLocalizer };
