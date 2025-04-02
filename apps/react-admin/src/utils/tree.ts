import { chain } from "ramda";

/**
 * Flatten an array containing a tree structure
 * @param {T[]} trees - An array containing a tree structure
 * @returns {T[]} - Flattened array
 */
export function flattenTrees<T extends { children?: T[] }>(trees: T[] = []): T[] {
	return chain((node) => {
		const children = node.children || [];
		return [node, ...flattenTrees(children)];
	}, trees);
}

//将平铺的节点列表转换为树形结构

interface TreeHelperConfig {
	children: string;
	id: string;
	pid: string;
}
const DEFAULT_CONFIG: TreeHelperConfig = {
	id: "id",
	pid: "pid",
	children: "children",
};

const getConfig = (config: Partial<TreeHelperConfig>) => Object.assign({}, DEFAULT_CONFIG, config);

//
// /**
//  * 注意此函数会在list原始数据上操作，会改变原始数据
//  * @param list
//  * @param config
//  * @returns
//  */
// export function listToTree<T = any>(list: any[], config: Partial<TreeHelperConfig> = {}): T[] {
// 	const conf = getConfig(config) as TreeHelperConfig;
// 	const nodeMap = new Map();
// 	const result: T[] = [];
// 	const { id, pid, children } = conf;

// 	for (const node of list) {
// 		node[children] = node[children] || [];
// 		nodeMap.set(node[id], node);
// 	}
// 	for (const node of list) {
// 		const parent = nodeMap.get(node[pid]);
// 		(parent ? parent[children] : result).push(node);
// 		if (parent) {
// 			parent.hasChildren = true;
// 		}
// 	}
// 	return result;
// }

export function listToTree<T = any>(list: any[], config: Partial<TreeHelperConfig> = {}): T[] {
	const conf = getConfig(config) as TreeHelperConfig;
	const nodeMap = new Map();
	const result: T[] = [];
	const { id, pid, children } = conf;

	// 创建 list 的深拷贝，避免修改原始数据
	const clonedList = list.map((node) => ({ ...node }));

	for (const node of clonedList) {
		node[children] = node[children] || [];
		nodeMap.set(node[id], node);
	}
	for (const node of clonedList) {
		const parent = nodeMap.get(node[pid]);
		(parent ? parent[children] : result).push(node);
		if (parent) {
			parent.hasChildren = true;
		}
	}
	return result;
}
