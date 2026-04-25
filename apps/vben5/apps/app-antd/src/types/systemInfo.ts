interface ComponentKey {
  /** 组件名称 */
  name: string;
  /** 组件显示名称 */
  displayName: string;
}

interface ComponentInfo {
  /** 组件名称 */
  name: string;
  /** 组件名称集合 */
  keys: ComponentKey[];
  /** 组件状态集合 */
  details: Record<string, any>;
}

interface SystemInfo {
  components: ComponentInfo[];
}

export type { SystemInfo };
