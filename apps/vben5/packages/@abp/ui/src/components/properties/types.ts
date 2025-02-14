import type { Dictionary } from '@abp/core';

interface PropertyInfo {
  key: string;
  value: string;
}
interface PropertyProps {
  allowDelete?: boolean;
  allowEdit?: boolean;
  data?: Dictionary<string, string>;
  disabled?: boolean;
}

export type { PropertyInfo, PropertyProps };
