using Newtonsoft.Json;
using System;

namespace LINGYUN.Abp.WxPusher;

[Serializable]
public class WxPusherResult<T>
{
    /// <summary>
    /// 状态码
    /// </summary>
    [JsonProperty("code")]
    public int Code { get; set; }
    /// <summary>
    /// 错误消息
    /// </summary>
    [JsonProperty("msg")]
    public string Message { get; set; }
    /// <summary>
    /// 返回数据
    /// </summary>
    [JsonProperty("data")]
    public T Data { get; set; }
    /// <summary>
    /// 是否调用成功
    /// </summary>
    [JsonProperty("success")] 
    public bool Success { get; set; }

    public WxPusherResult()
    {
    }

    public WxPusherResult(int code, string message)
    {
        Code = code;
        Message = message;
    }

    public WxPusherResult(int code, string message, T data)
    {
        Code = code;
        Message = message;
        Data = data;
    }

    public T GetData()
    {
        ThrowOfFailed();

        return Data;
    }

    public void ThrowOfFailed()
    {
        if (!Success)
        {
            throw new WxPusherRemoteCallException(Code.ToString(), Message);
        }
    }
}
