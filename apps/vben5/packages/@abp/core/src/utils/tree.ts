interface TreeHelperConfig {
  children: string;
  id: string;
  pid: string;
}
const DEFAULT_CONFIG: TreeHelperConfig = {
  id: 'id',
  pid: 'pid',
  children: 'children',
};

const getConfig = (config: Partial<TreeHelperConfig>) =>
  Object.assign({}, DEFAULT_CONFIG, config);

// tree from list
export function listToTree<T = any>(
  list: any[],
  config: Partial<TreeHelperConfig> = {},
): T[] {
  const conf = getConfig(config) as TreeHelperConfig;
  const map: { [key: string]: any } = {};
  const roots: any[] = [];
  const { id, pid, children } = conf;

  // 将每个元素放入map中，方便通过id查找
  list.forEach((item) => {
    map[item[id]] = { ...item, [children]: [] };
  });

  // 构建树形结构
  list.forEach((item) => {
    const parentId = item[pid];
    if (parentId === null || parentId === undefined) {
      // 根节点
      roots.push(map[item[id]]);
    } else {
      // 非根节点，将其添加到父节点的children数组中
      if (map[parentId]) {
        map[parentId][children].push(map[item[id]]);
      }
    }
  });

  return roots;
}
