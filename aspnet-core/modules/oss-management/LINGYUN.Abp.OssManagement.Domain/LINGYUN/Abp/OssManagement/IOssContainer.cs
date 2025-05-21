﻿using System.Threading.Tasks;

namespace LINGYUN.Abp.OssManagement;

/// <summary>
/// Oss容器
/// </summary>
public interface IOssContainer
{
    /// <summary>
    /// 创建容器
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<OssContainer> CreateAsync(string name);
    /// <summary>
    /// 创建Oss对象
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<OssObject> CreateObjectAsync(CreateOssObjectRequest request);
    /// <summary>
    /// 获取容器信息
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<OssContainer> GetAsync(string name);
    /// <summary>
    /// 获取Oss对象信息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<OssObject> GetObjectAsync(GetOssObjectRequest request);
    /// <summary>
    /// 删除容器
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="Volo.Abp.BusinessException">
    /// When the bucket does not exist, an exception is thrown with the error code is 
    /// <see cref="OssManagementErrorCodes.ContainerDeleteWithNotEmpty"/>
    /// </exception>
    Task DeleteAsync(string name);
    /// <summary>
    /// 删除Oss对象
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task DeleteObjectAsync(GetOssObjectRequest request);
    /// <summary>
    /// 批量删除Oss对象
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task BulkDeleteObjectsAsync(BulkDeleteObjectRequest request);
    /// <summary>
    /// 容器是否存在
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<bool> ExistsAsync(string name);
    /// <summary>
    /// Oss对象是否存在
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<bool> ObjectExistsAsync(GetOssObjectRequest request);
    /// <summary>
    /// 获取容器列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<GetOssContainersResponse> GetListAsync(GetOssContainersRequest request);
    /// <summary>
    /// 获取对象列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<GetOssObjectsResponse> GetObjectsAsync(GetOssObjectsRequest request);
    // Task<List<OssObject>> GetObjectsAsync(string name, string prefix = null, string marker = null, string delimiter = null, int maxResultCount = 10);
}
