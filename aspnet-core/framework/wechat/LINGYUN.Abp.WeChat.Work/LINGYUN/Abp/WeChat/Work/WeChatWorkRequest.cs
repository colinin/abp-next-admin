using System;

namespace LINGYUN.Abp.WeChat.Work;

[Serializable]
public abstract class WeChatWorkRequest
{
    public virtual string SerializeToJson()
    {
        Validate();

        return WeChatObjectSerializeExtensions.SerializeToJson(this);
    }

    protected virtual void Validate()
    {

    }
}
