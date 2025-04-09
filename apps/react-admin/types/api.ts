export interface Result<T = any> {
	code: string;
	/** 错误详情 */
	details?: string;
	message: string;
	result: T;
}
