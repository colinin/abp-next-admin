import type { Dictionary } from '@abp/core';

interface PropertyInfo {
  key: string;
  value: string;
}
interface PropertyProps {
  data?: Dictionary<string, string>;
}

export type { PropertyInfo, PropertyProps };
