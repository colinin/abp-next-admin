interface StringLocalizer {
  L(key: string, args?: any[] | Record<string, any> | undefined): string;
  Lr(
    resource: string,
    key: string,
    args?: any[] | Record<string, any> | undefined,
  ): string;
}

export type { StringLocalizer };
