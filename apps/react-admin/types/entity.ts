import type { BasicStatus, PermissionType } from "./enum";

export interface UserToken {
	accessToken?: string;
	refreshToken?: string;
}

export interface UserInfo {
	[key: string]: any;
	id: string;
	email: string;
	password?: string;
	role?: Role;
	status?: BasicStatus;
	permissions?: Permission[];
	//TODO 清理无用的信息
	/**
	 * 用户描述
	 */
	desc: string;
	/**
	 * 首页地址
	 */
	homePath: string;

	/**
	 * accessToken
	 */
	token: string;
	/**
	 * 头像
	 */
	avatar: string;
	/**
	 * 用户昵称
	 */
	realName: string;
	/**
	 * 用户角色
	 */
	roles?: string[];
	/**
	 * 用户id
	 */
	userId: string;
	/**
	 * 用户名
	 */
	username: string;
}

export interface Organization {
	id: string;
	name: string;
	status: "enable" | "disable";
	desc?: string;
	order?: number;
	children?: Organization[];
}

export interface Permission {
	id: string;
	parentId: string;
	name: string;
	label: string;
	type: PermissionType;
	route: string;
	status?: BasicStatus;
	order?: number;
	icon?: string;
	component?: string;
	hide?: boolean;
	hideTab?: boolean;
	frameSrc?: string;
	newFeature?: boolean;
	children?: Permission[];
}

export interface Role {
	id: string;
	name: string;
	label: string;
	status: BasicStatus;
	order?: number;
	desc?: string;
	permission?: Permission[];
}
