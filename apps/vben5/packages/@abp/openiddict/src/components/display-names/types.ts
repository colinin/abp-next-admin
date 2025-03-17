import type { Dictionary } from '@abp/core';

interface DisplayNameInfo {
  culture: string;
  displayName: string;
}
interface DisplayNameProps {
  data?: Dictionary<string, string>;
}

export type { DisplayNameInfo, DisplayNameProps };
