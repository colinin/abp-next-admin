namespace LINGYUN.Abp.OpenObserve.Serialization;

/// <summary>
/// OpenObserve 序列化器
/// </summary>
/// <remarks>
/// 统一负责请求体序列化与响应反序列化, 客户端不直接依赖具体的序列化实现。
/// 默认实现见 <see cref="DefaultOpenObserveSerializer"/>,
/// 需要自定义(如替换为 Newtonsoft 或其它策略)时实现该接口, 并参考 <see cref="DefaultOpenObserveSerializer"/> 的注册方式覆盖默认实现。
/// </remarks>
public interface IOpenObserveSerializer
{
    /// <summary>
    /// 序列化请求体
    /// </summary>
    string Serialize<T>(T value);

    /// <summary>
    /// 反序列化响应
    /// </summary>
    /// <param name="jsonString">响应内容</param>
    T? Deserialize<T>(string jsonString);

    /// <summary>
    /// 尝试反序列化响应
    /// </summary>
    /// <remarks>
    /// 用于错误响应等允许解析失败的场景, 解析失败时返回 false 而不抛出异常
    /// </remarks>
    /// <param name="jsonString">响应内容</param>
    /// <param name="value">反序列化结果</param>
    /// <returns>解析成功返回 true</returns>
    bool TryDeserialize<T>(string jsonString, out T? value);
}
