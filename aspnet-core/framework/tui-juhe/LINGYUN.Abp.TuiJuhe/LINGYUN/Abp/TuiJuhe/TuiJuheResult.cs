using System;
using System.Text.Json.Serialization;

namespace LINGYUN.Abp.TuiJuhe;

[Serializable]
public class TuiJuheResult<TParam, TResult>
{
    /// <summary>
    /// 状态码
    /// </summary>
    [JsonPropertyName("code")]
    public int Code { get; set; }
    /// <summary>
    /// 错误消息
    /// </summary>
    [JsonPropertyName("reason")]
    public string Reason { get; set; }
    /// <summary>
    /// 响应数据
    /// </summary>
    [JsonPropertyName("result")]
    public TResult Result { get; set; }
    /// <summary>
    /// 响应参数
    /// </summary>
    [JsonPropertyName("params")]
    public TParam Params { get; set; }

    [JsonIgnore]
    public bool Success => 200 == Code;

    public TuiJuheResult()
    {
    }

    public TuiJuheResult(int code, string reason)
    {
        Code = code;
        Reason = reason;
    }

    public TuiJuheResult(int code, string reason, TResult result)
    {
        Code = code;
        Reason = reason;
        Result = result;
    }

    public TResult GetData()
    {
        ThrowOfFailed();

        return Result;
    }

    public void ThrowOfFailed()
    {
        if (!Success)
        {
            throw new TuiJuheRemoteCallException(Code.ToString(), Reason);
        }
    }
}
