/**
 * 格式化适用于abp框架的分页请求
 * abp框架对于分页的定义如下：
 * skipCount： 从哪个偏移位置请求实体列表
 * maxResultCount： 返回实体列表数目
 */
export function formatPagedRequest(request: { skipCount: number; maxResultCount: number }) {
  if (request.skipCount > 0) {
    request.skipCount -= 1;
  }
  request.skipCount = request.skipCount * request.maxResultCount;
}
