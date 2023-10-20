using System;

namespace LINGYUN.Abp.Idempotent;

[AttributeUsage(AttributeTargets.Method)]
public class IdempotentAttribute : Attribute
{
    /// <summary>
    /// 超时等待时间
    /// </summary>
    public int? Timeout { get; }
    /// <summary>
    /// 用作建立唯一标识的参数名称列表
    /// </summary>
    /// <remarks>
    /// 通过查找参数列表中的值定义序列化唯一md5值
    /// </remarks>
    public string[]? KeyMap { get; }
    /// <summary>
    /// 资源重定向路径模板
    /// </summary>
    /// <remarks>
    /// 定义一个可格式化的资源路径,当请求冲突时如果参数名称匹配成功则重定向目标路径
    /// <br />
    /// 例: /api/app/demo/{id}
    /// <br />
    /// 例: /api/app/demo/method?id={id}
    /// </remarks>
    public string? RedirectUrl { get; }
    /// <summary>
    /// 自定义的幂等key
    /// </summary>
    public string? IdempotentKey { get; set; }
    public IdempotentAttribute()
    {

    }

    public IdempotentAttribute(
        string? iodempotentKey = null,
        int? timeout = null, 
        string? redirectUrl = null,
        string[]? keyMap = null)
    {
        IdempotentKey = iodempotentKey;
        Timeout = timeout;
        KeyMap = keyMap;
        RedirectUrl = redirectUrl;
    }
}
