/**
 * 弹窗/抽屉的数据访问器，仅要求提供 `getData` 方法。
 */
interface ModalDataApi<TData> {
  getData(): TData | undefined;
}

/**
 * 获取弹窗/抽屉打开时传入的数据。
 *
 * 调用方应在 `open()` 之前通过 `setData()` 传入数据（新增场景传空对象），
 * 若未传入则抛出异常，避免后续出现难以定位的空指针错误。
 *
 * @param api 弹窗或抽屉的 Api 实例
 * @returns 打开时传入的数据
 */
export function getModalData<TData>(api: ModalDataApi<TData>): TData {
  const data = api.getData();
  if (data === undefined) {
    throw new Error(
      'Modal or drawer data is not set, please call setData() before open().',
    );
  }
  return data;
}
