import type { Dictionary } from '@abp/core';

import type { VNode } from 'vue';

interface PropertyInfo {
  key: string;
  value: string;
}
interface PropertyProps {
  allowDelete?: boolean;
  allowEdit?: boolean;
  data?: Dictionary<string, string>;
  disabled?: boolean;
  renderKey?: (key: string) => string | VNode;
}

export type { PropertyInfo, PropertyProps };
