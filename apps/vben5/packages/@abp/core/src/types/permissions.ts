interface IPermissionChecker {
  authorize(name: string | string[]): void;
  isGranted(name: string | string[], requiresAll?: boolean): boolean;
}

export type { IPermissionChecker };
