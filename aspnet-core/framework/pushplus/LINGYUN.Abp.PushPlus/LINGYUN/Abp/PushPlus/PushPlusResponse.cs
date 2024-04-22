using Newtonsoft.Json;

namespace LINGYUN.Abp.PushPlus;

public class PushPlusResponse<T>
{
    /// <summary>
    /// 状态码
    /// </summary>
    [JsonProperty("code")]
    public string Code { get; set; }
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
    public bool IsSuccessed => string.Equals("200", Code);

    public PushPlusResponse()
    {
    }

    public PushPlusResponse(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public PushPlusResponse(string code, string message, T data)
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
        if (!IsSuccessed)
        {
            throw new PushPlusRequestException(Code, Message);
        }
    }
}
